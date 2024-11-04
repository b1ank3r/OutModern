using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Client.Comment
{
    public partial class Comment : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private int customerId;
        private string productDetailId;
        protected void Page_Load(object sender, EventArgs e)
        {
            productDetailId = Request.QueryString["ProductDetailId"];
            if (productDetailId == null)
            {
                Response.Redirect("~/src/ErrorPages/404.aspx");
            }
            if (Session["CUSTID"] != null)
            {
                customerId = (int)Session["CUSTID"];
            }
            else
            {
                customerId = 0;
            }
            if (!IsPostBack)
            {
                GetProductInfo();
            }
        }

        private void GetProductInfo()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = $"SELECT TOP 1 p.ProductId, p.ProductName, p.UnitPrice, p.ProductCategory, c.ColorName, s.SizeName, pi.Path AS ImagePath FROM ProductDetail pd INNER JOIN Product p ON pd.ProductId = p.ProductId INNER JOIN Color c ON pd.ColorId = c.ColorId INNER JOIN Size s ON pd.SizeId = s.SizeId LEFT JOIN ProductImage pi ON pd.ProductDetailId = pi.ProductDetailId WHERE pd.ProductDetailId = @ProductDetailId";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProductDetailId", productDetailId);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblProductName.Text = reader["ProductName"].ToString();
                            lblProductPrice.Text = "RM " + ((decimal)reader["UnitPrice"]).ToString();
                            lblProductColour.Text = "Color: " + reader["ColorName"].ToString();
                            lblProductSize.Text = "Size: " + reader["SizeName"].ToString();
                            imgProduct.ImageUrl = reader["ImagePath"] != DBNull.Value ? reader["ImagePath"].ToString() : null;
                        }
                    }
                }
            }
        }

        protected void btnSubmitComment_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            lblStarErrorMessage.Visible = false;
            decimal selectedRating = string.IsNullOrEmpty(hdnSelectedRating.Value.ToString()) ? 0 : decimal.Parse(hdnSelectedRating.Value.ToString());
            string commentText = txtComment.Text.Trim();

            if(selectedRating == 0)
            {
                lblStarErrorMessage.Visible = true;
            }

            if(string.IsNullOrEmpty(commentText))
            {
                lblMessage.Visible = true;
            }

            if (!string.IsNullOrEmpty(commentText) && selectedRating != 0)
            {
                decimal rating = Convert.ToDecimal(selectedRating);
                // Insert the review into the database
                string sqlQuery = @"INSERT INTO Review (CustomerId, ProductDetailId, Rating, ReviewDateTime, ReviewDescription)
                            VALUES (@CustomerId, @ProductDetailId, @Rating, @ReviewDateTime, @ReviewDescription)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", customerId);
                        command.Parameters.AddWithValue("@ProductDetailId", productDetailId);
                        command.Parameters.AddWithValue("@Rating", rating);
                        command.Parameters.AddWithValue("@ReviewDateTime", DateTime.Now);
                        command.Parameters.AddWithValue("@ReviewDescription", commentText);
                        int rowsAffected = command.ExecuteNonQuery();
                        if(rowsAffected > 0)
                        {
                            txtComment.Text = "";
                            lblMessage.Visible = false;
                            lblStarErrorMessage.Visible = false;
                            commentMessage.Visible = true;
                        }
                    }
                }
            }
        }

        protected void BtnOk_Click(object sender, EventArgs e)
        {
            commentMessage.Visible = false;
            Response.Redirect("~/src/Client/UserProfile/UserReivew.aspx");
        }
    }
}
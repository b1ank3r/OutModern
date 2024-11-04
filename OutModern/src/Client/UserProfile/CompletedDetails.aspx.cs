using OutModern.src.Admin.CustomerDetails;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Client.UserProfile
{
    public partial class CompletedDetails : System.Web.UI.Page
    {
        protected static readonly string Comment = "Comment";

        // Side menu urls
        protected Dictionary<string, string> urls = new Dictionary<string, string>()
        {
            { Comment , "~/src/Client/Comment/Comment.aspx" }
        };

        private string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private string orderId;
        protected void Page_Load(object sender, EventArgs e)
        {
            orderId = Request.QueryString["OrderId"];
            if (orderId == null)
            {
                Response.Redirect("~/src/ErrorPages/404.aspx");
            }
            if (!IsPostBack)
            {
                initPageData();

                Page.DataBind();
            }
        }

        //store each column sorting state into viewstate
        protected Dictionary<string, string> SortDirections
        {
            get
            {
                if (ViewState["SortDirections"] == null)
                {
                    ViewState["SortDirections"] = new Dictionary<string, string>();
                }
                return (Dictionary<string, string>)ViewState["SortDirections"];
            }
            set
            {
                ViewState["SortDirections"] = value;
            }
        }

        private void initPageData()
        {
            // All Product ordered
            DataTable productOrdered = getProductOrdered();
            if (productOrdered.Rows.Count == 0)
            {
                Response.Redirect("~/src/ErrorPages/404.aspx");
            }
            lvProductOrder.DataSource = productOrdered;
            lvProductOrder.DataBind();

            double subTotal = 0; // Calculate subtotal
            foreach (DataRow row in productOrdered.Rows)
            {
                subTotal += Convert.ToDouble(row["Subtotal"]);
            }
            Label lblSubTotal = (Label)lvProductOrder.FindControl("lblSubTotal");
            lblSubTotal.Text = subTotal.ToString();

        }

        private DataTable getProductOrdered()
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlQuery =
                    "Select ProductDetail.ProductDetailId, Product.ProductName, Size.SizeName, " +
                    "Color.ColorName, Product.UnitPrice, OrderItem.Quantity, " +
                    "UnitPrice*OrderItem.Quantity as Subtotal " +
                    "FROM [Order], OrderItem , ProductDetail, Size, Color, Product " +
                    "WHERE [Order].OrderId = OrderItem.OrderId " +
                    "AND OrderItem.ProductDetailId = ProductDetail.ProductDetailId " +
                    "AND Color.ColorId = ProductDetail.ColorId " +
                    "AND Size.SizeId = ProductDetail.SizeId " +
                    "AND Product.ProductId = ProductDetail.ProductId " +
                    "AND [Order].OrderId = @orderId;";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@orderId", orderId);
                    data.Load(command.ExecuteReader());
                }
            }

            data.Columns.Add("Path", typeof(string));
            foreach (DataRow row in data.Rows)
            {
                row["Path"] = getProductDetailImagePath(row["ProductDetailId"].ToString());
            }

            return data;
        }

        private string getProductDetailImagePath(string productDetailId)
        {
            string data = "";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlQuery =
                    "Select Top 1 [path] " +
                    "FROM ProductImage " +
                    "WHERE ProductDetailId = @productDetailId;";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@productDetailId", productDetailId);
                    data = command.ExecuteScalar()?.ToString();

                }
            }
            return data == null ? "" : data;

        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Client.ClientMaster
{
    public partial class Client : System.Web.UI.MasterPage
    {
        protected string homeUrl = "~/src/Client/Home/Home.aspx";
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private int customerId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string currentUrl = Request.Url.PathAndQuery;

                if (currentUrl == homeUrl)
                {
                    hyperlinkHome.CssClass = hyperlinkHome.CssClass.Replace("top-nav-item", "top-nav-item-active");
                }
            }

            if (Session["CUSTID"] != null)
            {
                customerId = (int)Session["CUSTID"];
            }
            else
            {
                customerId = 0;
            }


            int cartItemCount = GetCartItemCount();
            numberLabel.Text = cartItemCount.ToString();
            LoadPromoCode();

            Page.DataBind();
        }

        private int GetCartItemCount()
        {
            int count = 0;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM CartItem WHERE CartId = (SELECT CartId FROM Cart WHERE CustomerId = @CustomerId)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                con.Open();
                count = (int)cmd.ExecuteScalar();
            }

            return count;
        }

        protected void lBtnSearch_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();
            // Check for SQL Injection
            if (DetectSQLInjection(searchQuery))
            {
                LockUserAccount(); // if detected, update user status to "Locked" and clear the login session
                Response.Redirect("~/src/ErrorPages/ErrorSQLInjection.aspx", false);
                return;
            }

            // Construct the URL with the search query as a query string parameter
            string redirectUrl = $"~/src/Client/Products/Products.aspx?search={searchQuery}";

            // Redirect to the Products page with the search query
            Response.Redirect(redirectUrl, false);
        }

        private void LockUserAccount()
        {
            if (customerId != 0)
            {
                // Define the update query
                string updateQuery = "UPDATE Customer SET CustomerStatusId = 2 WHERE CustomerId = @customerId;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@customerId", customerId);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0) // Customer status updated successfully
                        {
                            HttpContext.Current.Session.Clear(); // Remove session and redirect to home page
                            HttpContext.Current.Session.Abandon();
                        }
                    }
                }
            }
        }

        private bool DetectSQLInjection(string input)
        {
            // List of suspicious SQL keywords
            string[] sqlKeywords = { "SELECT", "INSERT", "UPDATE", "DELETE", "DROP", "ALTER", "TRUNCATE" };

            // Check if the input contains any of the suspicious SQL keywords
            foreach (string keyword in sqlKeywords)
            {
                if (input.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // SQL injection attempt detected
                    return true;
                }
            }

            // No suspicious SQL keywords found
            return false;
        }

        //load random promo code
        private void LoadPromoCode()
        {
            DataTable data = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT TOP 1 PromoCode, EndDate, DiscountRate " +
                               "FROM PromoCode " +
                               "WHERE GETDATE() BETWEEN StartDate AND EndDate " +
                               "ORDER BY NEWID();";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);
            }

            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];
                string promoCode = row["PromoCode"].ToString();
                DateTime endDate = (DateTime)row["EndDate"];
                int discountRate = (int)row["DiscountRate"];

                lblPromoCode.Text = $"Use promo code {promoCode} to get {discountRate}% off. Hurry, offer ends on {endDate:dd MMM yyyy}!";
            }
            else
            {
                lblPromoCode.Text = "No promo code available";
            }
        }


        protected void btnTest_Click(object sender, EventArgs e)
        {

        }

        protected void btn_logout_Click(object sender, EventArgs e)
        {
            Session.Remove("LoggedIn");
            Session.Remove("CUSTID");
            Session.Remove("CustStatus");

            Response.Redirect("~/src/Client/Login/Login.aspx");
        }
    }
}
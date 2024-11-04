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
    public partial class Completed : System.Web.UI.Page
    {
        protected static readonly string CompletedDetails = "CompletedDetails";

        // Side menu urls
        protected Dictionary<string, string> urls = new Dictionary<string, string>()
        {
            { CompletedDetails , "~/src/Client/UserProfile/CompletedDetails.aspx" }
        };


        private string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            int custID;
            //if (Request.Cookies["CustID"] == null)
            if (Session["CUSTID"] == null)
            {
                Response.Redirect("~/src/Client/Login/Login.aspx");
            }
            else
            {
                //custID = int.Parse(Request.Cookies["CustID"].Value);
                custID = (int)Session["CUSTID"];

                if (!IsPostBack)
                {
                    lvOrders.DataSource = getOrders(); // Bind the ListView on first load
                    lvOrders.DataBind();
                }
                else
                {
                    // Re-bind during postbacks to ensure data persists
                    lvOrders.DataSource = getOrders();
                    lvOrders.DataBind();
                }

                try
                {

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                    {
                        conn.Open();
                        //use parameterized query to prevent sql injection
                        string query = "SELECT * FROM [Customer] WHERE CustomerId = @custId";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@custId", custID);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows) // Check if there are any results
                        {
                            reader.Read(); // Read the first row

                            //left box display
                            lbl_username.Text = reader["CustomerUsername"].ToString();

                            string profileImagePath = reader["ProfileImagePath"].ToString();
                            img_profile.ImageUrl = profileImagePath;

                            reader.Close();
                        }




                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
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

        //Get all orders
        protected DataTable getOrders(string sortExpression = null, string sortDirection = "ASC")
        {
            //int custID = int.Parse(Request.Cookies["CustID"].Value);
            int custID = (int)Session["CUSTID"];

            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "SELECT [Order].OrderId, Customer.CustomerFullName AS CustomerName, [Order].OrderDatetime, [Order].Total, OrderStatus.OrderStatusName " +
                    "FROM [Order] " +
                    "JOIN OrderStatus ON [Order].OrderStatusId = OrderStatus.OrderStatusId " +
                    "JOIN Customer ON [Order].CustomerId = Customer.CustomerId " +
                    "WHERE Customer.CustomerId = @custId AND [Order].OrderStatusId = @orderStatusId";

                if (!string.IsNullOrEmpty(sortExpression))
                {
                    sqlQuery += "ORDER BY " + sortExpression + " " + sortDirection;
                }

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@custId", custID);
                    command.Parameters.AddWithValue("@orderStatusId", 4);
                    data.Load(command.ExecuteReader());
                }

                data.Columns.Add("ProductDetails", typeof(DataTable));
                foreach (DataRow row in data.Rows)
                {
                    row["ProductDetails"] = getProductOrdered(row["OrderId"].ToString());
                }
            }

            return data;
        }

        //get the product Ordered for a particular order
        private DataTable getProductOrdered(string orderId)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "Select Product.ProductName, OrderItem.Quantity " +
                    "FROM [Order], OrderItem, ProductDetail, Product " +
                    "WHERE [Order].OrderId = OrderItem.OrderId " +
                    "AND OrderItem.ProductDetailId = ProductDetail.ProductDetailId " +
                    "AND ProductDetail.ProductId = Product.ProductId " +
                    "AND [Order].OrderId = @orderId;";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@orderId", orderId);
                    data.Load(command.ExecuteReader());
                }
            }

            return data;
        }

        protected void btn_togo_my_order_Click(object sender, EventArgs e)
        {
            // Redirect to Edit User Profile page
            Response.Redirect("ToShip.aspx");
        }

        protected void btn_to_ship_Click(object sender, EventArgs e)
        {
            // Redirect to Edit User Profile page
            Response.Redirect("ToShip.aspx");
        }

        protected void btn_to_receive_Click(object sender, EventArgs e)
        {
            // Redirect to Edit User Profile page
            Response.Redirect("ToReceive.aspx");
        }

        protected void btn_completed_Click(object sender, EventArgs e)
        {
            // Redirect to Edit User Profile page
            Response.Redirect("Completed.aspx");
        }

        protected void btn_cancelled_Click(object sender, EventArgs e)
        {
            // Redirect to Edit User Profile page
            Response.Redirect("Cancelled.aspx");
        }

        protected void btn_togo_profile_Click(object sender, EventArgs e)
        {
            // Redirect to User Profile page
            Response.Redirect("UserProfile.aspx");
        }

        protected void btnComment_Click(object sender, EventArgs e)
        {
            string productDetailId = "1"; // Replace "123" with the actual ProductDetailId value

            // Construct the URL with the query string parameter
            string url = $"~/src/Client/Comment/Comment.aspx?ProductDetailId={productDetailId}";

            Response.Redirect(url);
        }

        protected void btn_rw_his_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/src/Client/UserProfile/UserReivew.aspx");
        }

        protected void btn_rw_his_Click1(object sender, EventArgs e)
        {
            Response.Redirect("~/src/Client/UserProfile/UserReivew.aspx");
        }
    }
}
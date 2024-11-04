using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Admin.Dashboard
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected string lineData;
        protected string salesByCategoryData;
        private string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            string adminRole = Session["AdminRole"]?.ToString();
            if (adminRole != "Manager")
            {
                Response.Redirect("~/src/ErrorPages/403.aspx");
            }

            if (!IsPostBack)
            {
                Session["MenuCategory"] = "Dashboard";
                initPageData();
            }

        }

        private void initPageData()
        {
            string formatIntegerString = "# ##0";

            lblTotalCustomer.Text = getTotalCustomer().ToString(formatIntegerString);
            lblTotalStaff.Text = getTotalStaff().ToString(formatIntegerString);
            lblMonthOrders.Text = getMonthOrders().ToString(formatIntegerString);
            lblNewOrders.Text = getTodayOrders().ToString(formatIntegerString);
            //lblMonthCancelled.Text = getMonthCancelled().ToString(formatIntegerString);
            //lblTodayCancelled.Text = getTodayCancelled().ToString(formatIntegerString);
            lblTodayReviews.Text = getTodayReviews().ToString(formatIntegerString);

            lblOverallRating.Text = getOverallRating().ToString("0.0");

            populateSalesChart();
            populateSalesByCategoryChart();
        }

        private void populateSalesByCategoryChart()
        {

            DataTable data = getCategorySales();

            //prepare data for highchart
            salesByCategoryData = "[";
            foreach (DataRow row in data.Rows)
            {
                salesByCategoryData += "{name: '" + row["ProductCategory"] + "', y: " + row["Total"] + "},";
            }
            salesByCategoryData = salesByCategoryData.TrimEnd(',') + "]";
        }

        private void populateSalesChart()
        {

            DataTable data = getSalesData();

            //prepare data for highchart
            lineData = "[";
            foreach (DataRow row in data.Rows)
            {
                string dateString = row["Month"].ToString() + "/" + row["Year"].ToString();
                // Parse the string to a DateTime object
                DateTime dateTime = DateTime.ParseExact(dateString, "M/yyyy", CultureInfo.InvariantCulture);

                // Convert to the beginning of the month
                dateTime = new DateTime(dateTime.Year, dateTime.Month, 1);

                // Convert DateTime to Unix timestamp in milliseconds
                DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime);
                long unixDateTime = dateTimeOffset.ToUnixTimeMilliseconds();

                lineData += "[" + unixDateTime + "," + row["Total"] + "],";

            }
            lineData = lineData.TrimEnd(',') + "]";
        }

        //
        //db operation
        //

        //get total customer registered
        private int getTotalCustomer()
        {
            int total = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "Select COUNT(CustomerId) as Total " +
                    "FROM Customer, UserStatus " +
                    "WHERE UserStatus.UserStatusId = Customer.CustomerStatusId " +
                    "AND UserStatusName = 'Activated'";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    total = int.Parse(command.ExecuteScalar().ToString());
                }

            }

            return total;
        }

        //get total staffs which is activated
        private int getTotalStaff()
        {
            int total = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                connection.Open();
                string sqlQuery =
                    "Select COUNT(AdminId) as Total " +
                    "FROM Admin, UserStatus " +
                    "WHERE UserStatus.UserStatusId = Admin.AdminStatusId " +
                    "AND UserStatusName = 'Activated'";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    total = int.Parse(command.ExecuteScalar().ToString());
                }

            }

            return total;
        }

        // get this month orders, and it is not cancelled 
        private int getMonthOrders()
        {
            int total = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "Select COUNT(OrderId) as Total " +
                    "FROM [Order], OrderStatus " +
                    "WHERE MONTH(OrderDatetime) = MONTH(GETDATE()) " +
                    "AND YEAR(OrderDatetime) = YEAR(GETDATE()) " +
                    "AND OrderStatus.OrderStatusId = [Order].OrderStatusId " +
                    "AND OrderStatusName != 'Cancelled' ";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    total = int.Parse(command.ExecuteScalar().ToString());
                }

            }

            return total;
        }

        // get today orders, and it is not cancelled 
        private int getTodayOrders()
        {
            int total = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "Select COUNT(OrderId) as Total " +
                    "FROM [Order], OrderStatus " +
                    "WHERE CONVERT(date, OrderDateTime) = CONVERT(date, GETDATE()) " +
                    "AND OrderStatus.OrderStatusId = [Order].OrderStatusId " +
                    "AND OrderStatusName != 'Cancelled' ";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    total = int.Parse(command.ExecuteScalar().ToString());
                }
            }

            return total;
        }

        //get total reviews given to all product
        private int getTodayReviews()
        {
            int total = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                connection.Open();
                string sqlQuery =
                    "Select COUNT(ReviewId) as Total " +
                    "FROM Review " +
                    "WHERE CONVERT(date, ReviewDateTime) = CONVERT(date, GETDATE()) ";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    total = int.Parse(command.ExecuteScalar().ToString());
                }

            }

            return total;
        }

        //get and calculate overall rating for all products
        private double getOverallRating()
        {
            double average = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "Select AVG(Rating) as Average " +
                    "FROM Review ";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                        average = double.Parse(command.ExecuteScalar().ToString());
                }
            }

            return average;
        }

        // get sales data for the last 12 months from db, calculate the sum of total price for each month
        private DataTable getSalesData()
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "Select TOP 12 MONTH(OrderDateTime) as Month, YEAR(OrderDateTime) as Year, SUM(Total) as Total " +
                    "FROM [Order], OrderStatus " +
                    "WHERE OrderStatusName = 'Received' AND OrderStatus.OrderStatusID = [Order].OrderStatusID " +
                    "GROUP BY MONTH(OrderDateTime), YEAR(OrderDateTime) " +
                    "ORDER BY YEAR(OrderDateTime) desc, MONTH(OrderDateTime) desc;";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    data.Load(command.ExecuteReader());
                }
            }

            return data;
        }

        // total sales for each of the category
        private DataTable getCategorySales()
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "Select ProductCategory, Sum(Total) As Total " +
                    "FROM [Order], OrderItem, Product, OrderStatus, ProductDetail " +
                    "WHERE OrderItem.OrderID = [Order].OrderID " +
                    "AND OrderStatus.OrderStatusID = [Order].OrderStatusID " +
                    "AND ProductDetail.ProductDetailID = OrderItem.ProductDetailID " +
                    "AND Product.ProductID = ProductDetail.ProductID " +
                    "AND OrderStatusName = 'Received' " +
                    "Group By ProductCategory " +
                    "Order By ProductCategory ";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    data.Load(command.ExecuteReader());
                }
            }

            return data;
        }


    }
}
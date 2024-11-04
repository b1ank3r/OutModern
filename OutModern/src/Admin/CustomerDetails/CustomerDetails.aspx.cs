using OutModern.src.Admin.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace OutModern.src.Admin.CustomerDetails
{
    public partial class CustomerDetails : System.Web.UI.Page
    {

        protected static readonly string OrderDetails = "OrderDetails";
        protected static readonly string CustomerEdit = "CustomerEdit";
        private string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        // Side menu urls
        protected Dictionary<string, string> urls = new Dictionary<string, string>()
        {
            { OrderDetails , "~/src/Admin/OrderDetails/OrderDetails.aspx" },
            { CustomerEdit , "~/src/Admin/CustomerEdit/CustomerEdit.aspx" }
        };
        protected string customerId;

        protected void Page_Load(object sender, EventArgs e)
        {
            customerId = Request.QueryString["CustomerId"];
            if (customerId == null)
            {
                Response.Redirect("~/src/ErrorPages/404.aspx");
            }


            if (!IsPostBack)
            {
                Session["MenuCategory"] = "Customers";

                initCustomerDetails();

                lblOrderMade.Text = getTotalOrderMade().ToString();

                rptAddress.DataSource = getAddresses();
                rptAddress.DataBind();

                lvOrders.DataSource = orderDataSource();
                lvOrders.DataBind();

                //bind the filter status
                ddlFilterOrderStatus.DataSource = getOrderStatus();
                ddlFilterOrderStatus.DataTextField = "OrderStatusName";
                ddlFilterOrderStatus.DataValueField = "OrderStatusId";
                ddlFilterOrderStatus.DataBind();

                Page.DataBind();
            }
        }

        private void initCustomerDetails()
        {
            DataTable data = getCustomer();
            if (data.Rows.Count == 0)
            {
                Response.Redirect("~/src/ErrorPages/404.aspx");
            }

            DataRow row = data.Rows[0];

            imgProfile.ImageUrl = row["ProfileImagePath"].ToString();

            lblCustomerId.Text = row["CustomerId"].ToString();
            lblFullName.Text = row["CustomerFullName"].ToString();
            lblUsername.Text = row["CustomerUsername"].ToString();
            lblEmail.Text = row["CustomerEmail"].ToString();
            lblPhoneNo.Text = row["CustomerPhoneNumber"].ToString();



            string status = row["UserStatusName"].ToString();
            lblStatus.Text = status;
            switch (status)
            {
                case "Activated":
                    lblStatus.CssClass += " bg-green-300";
                    break;
                case "Locked":
                    lblStatus.CssClass += " bg-amber-300";
                    break;
                case "Deleted":
                    lblStatus.CssClass += " bg-red-300";
                    break;
                default:
                    break;
            }

        }

        //choose to load data source
        private DataTable orderDataSource(string sortExpression = null, string sortDirection = "ASC")
        {
            //get the search term
            string searchTerms = Request.QueryString["q"];

            if (searchTerms != null)
            {
                ((TextBox)Master.FindControl("txtSearch")).Text = searchTerms;
                return FilterDataTable(getOrders(sortExpression, sortDirection), searchTerms);
            }

            else return getOrders(sortExpression, sortDirection);
        }


        //search logic
        private DataTable FilterDataTable(DataTable dataTable, string searchTerm)
        {
            // Escape single quotes for safety
            string safeSearchTerm = searchTerm.Replace("'", "''");

            // Build the filter expression with relevant fields
            string expression = string.Format(
                "Convert(OrderId, 'System.String') LIKE '%{0}%' OR " +
                "Convert(OrderDateTime, 'System.String') LIKE '%{0}%' OR " +
                "Convert(Total, 'System.String') LIKE '%{0}%' OR " +
                "OrderStatusName LIKE '%{0}%'",
                safeSearchTerm);

            // Filter the rows
            DataRow[] filteredRows = dataTable.Select(expression);

            // Create a new DataTable for the filtered results
            DataTable filteredDataTable = dataTable.Clone();

            // Import the filtered rows
            foreach (DataRow row in filteredRows)
            {
                filteredDataTable.ImportRow(row);
            }

            return filteredDataTable;
        }


        //store each column sorting state into viewstate
        private Dictionary<string, string> SortDirections
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

        // Toggle Sorting
        private void toggleSortDirection(string columnName)
        {
            if (!SortDirections.ContainsKey(columnName))
            {
                SortDirections[columnName] = "ASC";
            }
            else
            {
                SortDirections[columnName] = SortDirections[columnName] == "ASC" ? "DESC" : "ASC";
            }
        }

        //
        //DB operation
        //

        //Get customer details
        private DataTable getCustomer()
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "Select CustomerId, CustomerFullName, CustomerUsername,ProfileImagePath, CustomerEmail, CustomerPhoneNumber, UserStatusName " +
                    "FROM Customer, UserStatus " +
                    "Where Customer.CustomerStatusId = UserStatus.UserStatusId " +
                    "AND CustomerId = @customerId ";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@customerId", customerId);

                    data.Load(command.ExecuteReader());
                }
            }

            return data;
        }

        //Get address
        private DataTable getAddresses()
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "Select AddressName, AddressLine, State, PostalCode, Country " +
                    "From Address, Customer " +
                    "WHERE Address.CustomerId = Customer.CustomerId " +
                    "AND Customer.CustomerId = @customerId ";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@customerId", customerId);
                    data.Load(command.ExecuteReader());
                }
            }

            return data;
        }

        // get order data
        private DataTable getOrders(string sortExpression = null, string sortDirection = "ASC")
        {
            DataTable data = new DataTable();

            string statusId = ddlFilterOrderStatus.SelectedValue;
            string orderDateFrom = txtOrderDateFrom.Text.Trim();
            string orderDateTo = txtOrderDateTo.Text.Trim();

            //check if date is null
            if (!String.IsNullOrEmpty(orderDateFrom) && !String.IsNullOrEmpty(orderDateTo))
            {
                orderDateFrom += " 00:00:00";
                orderDateTo += " 23:59:59";
                if (!ValidationUtils.IsValidDateTimeRange(orderDateFrom, orderDateTo))
                {
                    Page.ClientScript.RegisterStartupScript(GetType(),
                        "Failed to Filter",
                        "document.addEventListener('DOMContentLoaded',  ()=> alert('End date must be greater than start date'));",
                        true);

                    orderDateFrom = "";
                    orderDateTo = "";
                }
            }

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "Select OrderId, OrderDateTime, Total, OrderStatusName " +
                    "From [Order], Customer, OrderStatus " +
                    "Where [Order].CustomerId = Customer.CustomerId " +
                    "AND [Order].OrderStatusId = OrderStatus.OrderStatusId " +
                    "AND Customer.CustomerId = @customerId ";

                // add status filter
                if (!string.IsNullOrEmpty(statusId) && statusId != "-1")
                {
                    sqlQuery += "AND [Order].OrderStatusId = @statusId ";
                }

                // add date filter
                if (!string.IsNullOrEmpty(orderDateFrom) && !string.IsNullOrEmpty(orderDateTo))
                {
                    sqlQuery += "AND OrderDateTime BETWEEN @orderDateFrom AND @orderDateTo ";
                }

                if (!string.IsNullOrEmpty(sortExpression))
                {
                    sqlQuery += "ORDER BY " + sortExpression + " " + sortDirection;
                }
                else
                {
                    sqlQuery += "ORDER BY OrderDateTime DESC ";
                }

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    if (!string.IsNullOrEmpty(statusId) && statusId != "-1")
                        command.Parameters.AddWithValue("@statusId", statusId);

                    if (!string.IsNullOrEmpty(orderDateFrom) && !string.IsNullOrEmpty(orderDateTo))
                    {
                        command.Parameters.AddWithValue("@orderDateFrom", orderDateFrom);
                        command.Parameters.AddWithValue("@orderDateTo", orderDateTo);
                    }

                    command.Parameters.AddWithValue("@customerId", customerId);
                    data.Load(command.ExecuteReader());
                }
            }

            // Data to product ordered in each row
            data.Columns.Add("ProductDetails", typeof(DataTable));
            foreach (DataRow row in data.Rows)
            {
                row["ProductDetails"] = getProductOrdered(row["OrderId"].ToString());
            }

            return data;
        }

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

        private int getTotalOrderMade()
        {
            int totalOrder = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "Select Count(OrderId) as TotalOrder " +
                    "From [Order] " +
                    "Where CustomerId = @customerId ";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@customerId", customerId);
                    if (command.ExecuteScalar() != DBNull.Value)
                    {
                        totalOrder = int.Parse(command.ExecuteScalar().ToString());
                    }
                }
            }

            return totalOrder;
        }

        // get all order status
        private DataTable getOrderStatus()
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "Select OrderStatusId, OrderStatusName " +
                    "FROM OrderStatus ";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    data.Load(command.ExecuteReader());
                }
            }

            return data;
        }

        //
        //Page Events
        //
        protected void lvOrders_PagePropertiesChanged(object sender, EventArgs e)
        {
            string sortExpression = ViewState["SortExpression"]?.ToString();
            lvOrders.DataSource =
                sortExpression == null ?
                orderDataSource() :
                orderDataSource(sortExpression, SortDirections[sortExpression]);
            lvOrders.DataBind();
        }

        protected void lvOrders_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType != ListViewItemType.DataItem) return;


            DataRowView rowView = (DataRowView)e.Item.DataItem;
            HtmlGenericControl statusSpan = (HtmlGenericControl)e.Item.FindControl("orderStatus");
            string status = rowView["OrderStatusName"].ToString();

            switch (status)
            {
                case "Order Placed":
                    statusSpan.Attributes["class"] += " order-placed";
                    break;
                case "Shipped":
                    statusSpan.Attributes["class"] += " shipped";
                    break;
                case "Cancelled":
                    statusSpan.Attributes["class"] += " cancelled";
                    break;
                case "Received":
                    statusSpan.Attributes["class"] += " received";
                    break;
                default:
                    // Handle cases where status doesn't match any of the above
                    break;
            }
        }

        protected void lvOrders_Sorting(object sender, ListViewSortEventArgs e)
        {
            toggleSortDirection(e.SortExpression); // Toggle sorting direction for the clicked column

            ViewState["SortExpression"] = e.SortExpression; // used for retain the sorting

            // Re-bind the ListView with sorted data
            lvOrders.DataSource = orderDataSource(e.SortExpression, SortDirections[e.SortExpression]);
            lvOrders.DataBind();
        }

        protected void ddlFilterOrderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sortExpression = ViewState["SortExpression"]?.ToString();
            lvOrders.DataSource = sortExpression == null ?
                orderDataSource() :
                orderDataSource(sortExpression, SortDirections[sortExpression]);
            lvOrders.DataBind();
        }

        protected void ddlFilterOrderStatus_DataBound(object sender, EventArgs e)
        {
            ddlFilterOrderStatus.Items.Insert(0, new ListItem("All Status", "-1"));
        }

        protected void txtOrderDateFrom_TextChanged(object sender, EventArgs e)
        {
            string sortExpression = ViewState["SortExpression"]?.ToString();
            lvOrders.DataSource = sortExpression == null ?
                orderDataSource() :
                orderDataSource(sortExpression, SortDirections[sortExpression]);
            lvOrders.DataBind();
        }
    }
}
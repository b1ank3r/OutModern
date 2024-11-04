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

namespace OutModern.src.Admin.CustomerEdit
{
    public partial class CustomerEdit : System.Web.UI.Page
    {
        protected static readonly string CustomerDetails = "CustomerDetails";

        // Side menu urls
        protected Dictionary<string, string> urls = new Dictionary<string, string>()
        {
            { CustomerDetails , "~/src/Admin/CustomerDetails/CustomerDetails.aspx" },
        };

        private string ConnectionStirng = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private string customerId;

        protected void Page_Load(object sender, EventArgs e)
        {
            customerId = Request.QueryString["CustomerId"];
            if (customerId == null)
            {
                Response.Redirect("~/src/ErrorPages/404.aspx");
            }

            if (!Page.IsPostBack)
            {
                Session["MenuCategory"] = "Customers";

                Page.DataBind();
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                initCustomerDetails();
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

            lblCustomerId.Text = row["CustomerId"].ToString();
            txtFullName.Text = row["CustomerFullName"].ToString();
            txtUsername.Text = row["CustomerUsername"].ToString();
            txtEmail.Text = row["CustomerEmail"].ToString();
            txtPhoneNo.Text = row["CustomerPhoneNumber"].ToString();

            ddlStatus.DataSource = getUserStatus();
            ddlStatus.DataValueField = "UserStatusId";
            ddlStatus.DataTextField = "UserStatusName";
            ddlStatus.DataBind();
            ListItem statusItem = ddlStatus.Items.FindByText(row["UserStatusName"].ToString());
            if (statusItem != null) statusItem.Selected = true; // auto select current value

            rptAddresses.DataSource = getAddresses();
            rptAddresses.DataBind();
        }

        //
        //db operation
        //
        private DataTable getCustomer()
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionStirng))
            {
                connection.Open();
                string sqlQuery =
                    "Select CustomerId, CustomerFullName, CustomerUsername, CustomerEmail, CustomerPhoneNumber, UserStatusName " +
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

        private DataTable getUserStatus()
        {

            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionStirng))
            {
                connection.Open();
                string sqlQuery =
                    "SELECT UserStatusId, UserStatusName " +
                    "FROM UserStatus ";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    data.Load(command.ExecuteReader());
                }
            }

            return data;
        }

        private DataTable getAddresses()
        {

            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionStirng))
            {
                connection.Open();
                string sqlQuery =
                    "SELECT AddressName, isDeleted, AddressLine, Country, State, PostalCode " +
                    "FROM Address, Customer " +
                    "WHERE Address.CustomerId = Customer.CustomerId " +
                    "AND Address.CustomerId = @customerId";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@customerId", customerId);
                    data.Load(command.ExecuteReader());
                }
            }

            return data;
        }

        // update customer
        private int updateCustomer(string customerFullName, string customerUsername, string customerEmail, string customerPhoneNo, string statusId)
        {
            int affectedRow = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionStirng))
            {
                connection.Open();
                string sqlQuery =
                    "Update Customer " +
                    "SET CustomerFullName = @customerFullName, CustomerUsername = @customerUsername, " +
                    "CustomerEmail = @customerEmail, CustomerStatusId = @statusId, " +
                    "CustomerPhoneNumber = @customerPhoneNo " +
                    "WHERE CustomerId = @customerId";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@customerId", customerId);
                    command.Parameters.AddWithValue("@customerFullName", customerFullName);
                    command.Parameters.AddWithValue("@customerUsername", customerUsername);
                    command.Parameters.AddWithValue("@customerEmail", customerEmail);
                    command.Parameters.AddWithValue("@customerPhoneNo", customerPhoneNo);
                    command.Parameters.AddWithValue("@statusId", statusId);

                    affectedRow = command.ExecuteNonQuery();
                }
            }

            return affectedRow;
        }

        //
        //page event
        //
        protected void lbUpdate_Click(object sender, EventArgs e)
        {

            string customerFullName = txtFullName.Text.Trim();
            string customerUsername = txtUsername.Text.Trim();
            string customerEmail = txtEmail.Text.Trim();
            string customerPhoneNo = txtPhoneNo.Text.Trim();
            string statusId = ddlStatus.SelectedValue;

            //Validation
            //Check null
            if (string.IsNullOrEmpty(customerFullName)
                || string.IsNullOrEmpty(customerUsername)
                || string.IsNullOrEmpty(customerEmail)
                || string.IsNullOrEmpty(customerPhoneNo))
            {
                lblUpdateStatus.Text = "**Please fill in all the fields";
                Page.ClientScript
                        .RegisterClientScriptBlock(GetType(), "Update Failed",
                        "document.addEventListener('DOMContentLoaded', ()=> alert('Please fill in all the fields'));", true);
                return;
            }

            //check email format
            if (!StringUtil.EmailUtil.IsValidEmail(customerEmail))
            {
                lblUpdateStatus.Text = "**Invalid Email Format";
                Page.ClientScript
                        .RegisterClientScriptBlock(GetType(), "Update Failed", "document.addEventListener('DOMContentLoaded', ()=>alert('Invalid Email Format'));", true);
                return;
            }

            //get original email
            DataTable data = getCustomer();
            string originalEmail = data.Rows[0]["CustomerEmail"].ToString();

            //check email duplication
            if (StringUtil.EmailUtil.IsDuplicateEmail(customerEmail) && customerEmail != originalEmail)
            {
                lblUpdateStatus.Text = "**Email already exists";
                Page.ClientScript
                        .RegisterClientScriptBlock(GetType(),
                        "Update Failed", "document.addEventListener('DOMContentLoaded', ()=>alert('Email already exists'));", true);
                return;
            }

            //check phone number format
            if (!StringUtil.PhoneUtil.IsValidPhoneNumber(customerPhoneNo))
            {
                lblUpdateStatus.Text = "**Invalid Phone Number Format";
                Page.ClientScript
                        .RegisterClientScriptBlock(GetType(), "Update Failed", "document.addEventListener('DOMContentLoaded', ()=> alert('Invalid Phone Number Format'));", true);
                return;
            }



            int affectedRow = updateCustomer(customerFullName, customerUsername, customerEmail, customerPhoneNo, statusId);

            if (affectedRow > 0)
            {
                lblUpdateStatus.Text = "**Update Successfully";
            }
            else
            {
                lblUpdateStatus.Text = "**Update Fail, please try again later...";

            }

        }
        protected void rptAddresses_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            // Find the span control
            Label lblIsDeleted = (Label)e.Item.FindControl("lblIsDeleted");
            string isDeleted = lblIsDeleted.Text;

            // Add CSS class based on status
            switch (isDeleted)
            {
                case "False":
                    lblIsDeleted.Text = "Active";
                    lblIsDeleted.CssClass += " bg-green-300";
                    break;
                case "True":
                    lblIsDeleted.Text = "Deleted";
                    lblIsDeleted.CssClass += " bg-red-300";
                    break;
            }


        }
    }
}
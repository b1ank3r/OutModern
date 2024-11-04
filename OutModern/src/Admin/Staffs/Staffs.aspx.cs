using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


namespace OutModern.src.Admin.Staffs
{
    public partial class Staffs : System.Web.UI.Page
    {

        protected static readonly string StaffEdit = "StaffEdit";
        protected static readonly string StaffAdd = "StaffAdd";
        protected static readonly string StaffDetails = "StaffDetails";
        protected Dictionary<string, string> urls = new Dictionary<string, string>()
        {
            { StaffEdit , "~/src/Admin/StaffEdit/StaffEdit.aspx" },
            { StaffAdd , "~/src/Admin/StaffAdd/StaffAdd.aspx" },
            { StaffDetails , "~/src/Admin/StaffDetails/StaffDetails.aspx" },
        };

        private string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                string adminRole = Session["AdminRole"]?.ToString();
                if (adminRole != "Manager")
                {
                    Response.Redirect("~/src/ErrorPages/403.aspx");
                }

                Session["MenuCategory"] = "Staffs";

                lvStaffs.DataSource = staffDataSource();
                lvStaffs.DataBind();

                //Filter Bind
                ddlFilterRole.DataSource = getAdminRoles();
                ddlFilterRole.DataValueField = "AdminRoleId";
                ddlFilterRole.DataTextField = "AdminRoleName";
                ddlFilterRole.DataBind();

                ddlFilterStatus.DataSource = getAdminStatus();
                ddlFilterStatus.DataValueField = "UserStatusId";
                ddlFilterStatus.DataTextField = "UserStatusName";
                ddlFilterStatus.DataBind();


                Page.DataBind();
            }
        }

        //choose to load data source
        private DataTable staffDataSource(string sortExpression = null, string sortDirection = "ASC")
        {
            //get the search term
            string searchTerms = Request.QueryString["q"];

            if (searchTerms != null)
            {
                ((TextBox)Master.FindControl("txtSearch")).Text = searchTerms;
                return FilterDataTable(getStaffs(sortExpression, sortDirection), searchTerms);
            }

            else return getStaffs(sortExpression, sortDirection);
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
        //DB opration
        //

        // Get Staffs
        private DataTable getStaffs(string sortExpression = null, string sortDirection = "ASC")
        {
            DataTable data = new DataTable();

            string roleId = ddlFilterRole.SelectedValue;
            string statusId = ddlFilterStatus.SelectedValue;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "SELECT Admin.AdminId, Admin.AdminFullName AS AdminName, Admin.AdminUsername, Admin.AdminEmail, Admin.AdminPhoneNo, UserStatus.UserStatusName AS AdminStatus, AdminRole.AdminRoleName AS AdminRole " +
                    "FROM Admin, UserStatus, AdminRole " +
                    "WHERE Admin.AdminStatusId = UserStatus.UserStatusId " +
                    "AND Admin.AdminRoleId = AdminRole.AdminRoleId ";

                if (!string.IsNullOrEmpty(roleId) && roleId != "-1")
                {
                    sqlQuery += "AND Admin.AdminRoleId = @roleId ";
                }

                if (!string.IsNullOrEmpty(statusId) && statusId != "-1")
                {
                    sqlQuery += "AND Admin.AdminStatusId = @statusId ";
                }

                if (!string.IsNullOrEmpty(sortExpression))
                {
                    sqlQuery += "ORDER BY " + sortExpression + " " + sortDirection;
                }

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    if (!string.IsNullOrEmpty(roleId) && roleId != "-1")
                    {
                        command.Parameters.AddWithValue("@roleId", roleId);
                    }

                    if (!string.IsNullOrEmpty(statusId) && statusId != "-1")
                    {
                        command.Parameters.AddWithValue("@statusId", statusId);
                    }

                    data.Load(command.ExecuteReader());
                }
            }

            return data;
        }





        //update staff
        private int updateStaff(
        string adminId,
        string adminName,
        string adminUsername,
        string adminRole,
        string adminEmail,
        string adminPhoneNo,
        string adminStatus)
        {
            int numberRowEffected = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                connection.Open();

                string sqlQuery =
                    "UPDATE Admin " +

                    "SET AdminFullName = @adminName, " +
                    "AdminUsername = @adminUsername, " +
                    "AdminEmail = @adminEmail, " +
                    "AdminPhoneNo = @adminPhoneNo, " +
                    "AdminStatusId = @adminStatus, " +
                    "AdminRoleId = @adminRole " +

                    "WHERE AdminId = @adminId";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@adminName", adminName);
                    command.Parameters.AddWithValue("@adminUsername", adminUsername);
                    command.Parameters.AddWithValue("@adminEmail", adminEmail);
                    command.Parameters.AddWithValue("@adminPhoneNo", adminPhoneNo);
                    command.Parameters.AddWithValue("@adminStatus", adminStatus);
                    command.Parameters.AddWithValue("@adminRole", adminRole);
                    command.Parameters.AddWithValue("@adminId", adminId);

                    numberRowEffected = command.ExecuteNonQuery();
                }
            }

            return numberRowEffected;
        }


        //insert staff
        private int insertStaff(
            string adminName,
            string adminUsername,
            string adminPassword,
            string adminRole,
            string adminEmail,
            string adminPhoneNo,
            string adminStatus)
        {
            int numberRowEffected = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                connection.Open();

                string sqlQuery =
                    "INSERT INTO Admin (AdminFullName, AdminUsername, AdminPassword, AdminEmail, AdminPhoneNo, AdminStatusId, AdminRoleId) " +
                    "VALUES (@adminName, @adminUsername, @adminPassword, @adminEmail, @adminPhoneNo, @adminStatus, @adminRole)";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@adminName", adminName);
                    command.Parameters.AddWithValue("@adminUsername", adminUsername);
                    command.Parameters.AddWithValue("@adminPassword", adminPassword);
                    command.Parameters.AddWithValue("@adminEmail", adminEmail);
                    command.Parameters.AddWithValue("@adminPhoneNo", adminPhoneNo);
                    command.Parameters.AddWithValue("@adminStatus", adminStatus);
                    command.Parameters.AddWithValue("@adminRole", adminRole);

                    numberRowEffected = command.ExecuteNonQuery();
                }
            }

            return numberRowEffected;
        }


        //get all admin role for ddl
        private DataTable getAdminRoles()
        {
            DataTable data = new DataTable();


            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "SELECT AdminRoleId, AdminRoleName " +
                    "FROM AdminRole";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    data.Load(command.ExecuteReader());
                }
            }

            return data;
        }


        //get all admin status (user status) for ddl
        private DataTable getAdminStatus()
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "SELECT UserStatusId, UserStatusName " +
                    "FROM UserStatus " +
                    "WHERE UserStatusName != 'Locked'";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    data.Load(command.ExecuteReader());
                }
            }

            return data;
        }


        //Search Bar Method
        private DataTable FilterDataTable(DataTable dataTable, string searchTerm)
        {
            // Escape single quotes for safety
            string safeSearchTerm = searchTerm.Replace("'", "''");

            // Build the filter expression with all relevant fields
            string expression = string.Format(
                "Convert(AdminId, 'System.String') LIKE '%{0}%' OR " +
                "AdminName LIKE '%{0}%' OR " +
                "AdminUsername LIKE '%{0}%' OR " +
                "AdminRole LIKE '%{0}%' OR " +
                "AdminEmail LIKE '%{0}%' OR " +
                "AdminPhoneNo LIKE '%{0}%' OR " +
                "AdminStatus LIKE '%{0}%'",
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


        //
        // Events in page
        //
        protected void lvStaffs_PagePropertiesChanged(object sender, EventArgs e)
        {
            lvStaffs.InsertItemPosition = InsertItemPosition.None;
            lvStaffs.EditIndex = -1;

            string sortExpression = ViewState["SortExpression"]?.ToString();
            lvStaffs.DataSource =
                sortExpression == null ?
                staffDataSource() :
                staffDataSource(sortExpression, SortDirections[sortExpression]);
            lvStaffs.DataBind();
        }

        protected void lbAddStaff_Click(object sender, EventArgs e)
        {
            lvStaffs.EditIndex = -1;
            lvStaffs.InsertItemPosition = InsertItemPosition.FirstItem;

            string sortExpression = ViewState["SortExpression"]?.ToString();
            lvStaffs.DataSource = sortExpression == null ?
                staffDataSource() :
                staffDataSource(sortExpression, SortDirections[sortExpression]);
            lvStaffs.DataBind();

            // bind data source for Role ddl
            DropDownList ddlAddRole = lvStaffs.InsertItem.FindControl("ddlAddRole") as DropDownList;
            ddlAddRole.DataSource = getAdminRoles();
            ddlAddRole.DataValueField = "AdminRoleId";
            ddlAddRole.DataTextField = "AdminRoleName";
            ddlAddRole.DataBind();

            // bind data source for Status ddl
            DropDownList ddlAddStatus = lvStaffs.InsertItem.FindControl("ddlAddStatus") as DropDownList;
            ddlAddStatus.DataSource = getAdminStatus();
            ddlAddStatus.DataValueField = "UserStatusId";
            ddlAddStatus.DataTextField = "UserStatusName";
            ddlAddStatus.DataBind();
        }

        protected void lvStaffs_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lvStaffs.InsertItemPosition = InsertItemPosition.None;
            lvStaffs.EditIndex = -1;
            string sortExpression = ViewState["SortExpression"]?.ToString();
            lvStaffs.DataSource =
                sortExpression == null ?
                staffDataSource() :
                staffDataSource(sortExpression, SortDirections[sortExpression]);
            lvStaffs.DataBind();
        }

        protected void lvStaffs_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            string sortExpression = ViewState["SortExpression"]?.ToString();
            DataTable staffs = sortExpression == null ?
                staffDataSource() :
                staffDataSource(sortExpression, SortDirections[sortExpression]);

            int currentRowIndex = e.NewEditIndex + dpBottomStaffs.StartRowIndex;
            DataRow oriRow = staffs.Rows[currentRowIndex]; // get ori value to show as default value in ddl

            lvStaffs.DataSource = staffs;
            lvStaffs.InsertItemPosition = InsertItemPosition.None;
            lvStaffs.EditIndex = e.NewEditIndex;
            lvStaffs.DataBind();

            // bind data source for Role ddl
            DropDownList ddlEditRole = lvStaffs.Items[e.NewEditIndex].FindControl("ddlEditRole") as DropDownList;
            ddlEditRole.DataSource = getAdminRoles();
            ddlEditRole.DataValueField = "AdminRoleId";
            ddlEditRole.DataTextField = "AdminRoleName";
            ddlEditRole.DataBind();

            // bind default value of admin role in ddl
            string defaultRole = oriRow["AdminRole"].ToString();
            ListItem roleItem = ddlEditRole.Items.FindByText(defaultRole);
            if (roleItem != null) roleItem.Selected = true;

            // bind data source for Status ddl
            DropDownList ddlEditStatus = lvStaffs.Items[e.NewEditIndex].FindControl("ddlEditStatus") as DropDownList;
            ddlEditStatus.DataSource = getAdminStatus();
            ddlEditStatus.DataValueField = "UserStatusId";
            ddlEditStatus.DataTextField = "UserStatusName";
            ddlEditStatus.DataBind();

            // bind default value of admin status in ddl
            string defaultStatus = oriRow["AdminStatus"].ToString();
            ListItem statusItem = ddlEditStatus.Items.FindByText(defaultStatus);
            if (statusItem != null) statusItem.Selected = true;

        }

        protected void lvStaffs_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            ListViewItem item = e.Item;

            // Find the TextBox controls within the item template
            TextBox txtAddAdminName = (TextBox)item.FindControl("txtAddAdminName");
            TextBox txtAddAdminUsername = (TextBox)item.FindControl("txtAddAdminUsername");
            DropDownList ddlAddRole = (DropDownList)item.FindControl("ddlAddRole");
            TextBox txtAddAdminEmail = (TextBox)item.FindControl("txtAddAdminEmail");
            TextBox txtAddAdminPhoneNo = (TextBox)item.FindControl("txtAddAdminPhoneNo");
            DropDownList ddlAddStatus = (DropDownList)item.FindControl("ddlAddStatus");

            // Get the values from the controls
            string addAdminName = txtAddAdminName.Text.Trim();
            string addAdminUsername = txtAddAdminUsername.Text.Trim();
            string addRole = ddlAddRole.SelectedValue;
            string addAdminEmail = txtAddAdminEmail.Text.Trim();
            string addAdminPhoneNo = txtAddAdminPhoneNo.Text.Trim();
            string addStatus = ddlAddStatus.SelectedValue;

            //check nulls
            if (string.IsNullOrEmpty(addAdminName) || string.IsNullOrEmpty(addAdminUsername) || string.IsNullOrEmpty(addRole) || string.IsNullOrEmpty(addAdminEmail) || string.IsNullOrEmpty(addAdminPhoneNo) || string.IsNullOrEmpty(addStatus))
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "Add Failed",
                        "document.addEventListener('DOMContentLoaded', ()=> alert('Please fill in all fields'));",
                        true);
                return;
            }

            //check email
            if (!StringUtil.EmailUtil.IsValidEmail(addAdminEmail))
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                    "Add Failed",
                    "document.addEventListener('DOMContentLoaded', ()=>alert('Please Enter Valid Email'));",
                    true);

                return;
            }

            //check email duplicate
            if (Utils.ValidationUtils.IsEmailExist(addAdminEmail))
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                            "Add Failed",
                            "document.addEventListener('DOMContentLoaded', ()=>alert('Email already exist'));",
                            true);

                return;
            }

            //check phone
            if (!StringUtil.PhoneUtil.IsValidPhoneNumber(addAdminPhoneNo))
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                        "Add Failed",
                        "document.addEventListener('DOMContentLoaded', ()=>alert('Please Enter Valid Phone Number'));",
                        true);

                return;
            }

            string tempPass = addAdminUsername + addAdminPhoneNo;
            string hashPass = StringUtil.PasswordUtil.HashPassword(tempPass);

            int noOfRowAffected = insertStaff(addAdminName, addAdminUsername, hashPass, addRole, addAdminEmail, addAdminPhoneNo, addStatus);

            if (noOfRowAffected > 0)
            {
                lvStaffs.InsertItemPosition = InsertItemPosition.None;
                lvStaffs.DataSource = staffDataSource();
                lvStaffs.DataBind();
            }
        }

        protected void lvStaffs_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            // Get the ListViewItem
            ListViewItem item = lvStaffs.Items[e.ItemIndex];
            if (item == null) return;

            // Find the TextBox controls within the edit item template
            Label lblAdminId = (Label)item.FindControl("lblAdminId");
            TextBox txtEditAdminName = (TextBox)item.FindControl("txtEditAdminName");
            TextBox txtEditAdminUsername = (TextBox)item.FindControl("txtEditAdminUsername");
            DropDownList ddlEditRole = (DropDownList)item.FindControl("ddlEditRole");
            TextBox txtEditAdminEmail = (TextBox)item.FindControl("txtEditAdminEmail");
            TextBox txtEditAdminPhoneNo = (TextBox)item.FindControl("txtEditAdminPhoneNo");
            DropDownList ddlEditStatus = (DropDownList)item.FindControl("ddlEditStatus");

            // Get the updated values from the controls
            string adminId = lblAdminId.Text;
            string updatedAdminName = txtEditAdminName.Text.Trim();
            string updatedAdminUsername = txtEditAdminUsername.Text.Trim();
            string updatedRole = ddlEditRole.SelectedValue;
            string updatedAdminEmail = txtEditAdminEmail.Text.Trim();
            string updatedAdminPhoneNo = txtEditAdminPhoneNo.Text.Trim();
            string updatedStatus = ddlEditStatus.SelectedValue;

            //check nulls
            if (string.IsNullOrEmpty(updatedAdminName)
                || string.IsNullOrEmpty(updatedAdminUsername)
                || string.IsNullOrEmpty(updatedRole)
                || string.IsNullOrEmpty(updatedAdminEmail)
                || string.IsNullOrEmpty(updatedAdminPhoneNo)
                || string.IsNullOrEmpty(updatedStatus))
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                    "Update Failed",
                    "document.addEventListener('DOMContentLoaded', ()=>alert('Please Fill in All Fields'));",
                    true);


                return;
            }

            //check email
            if (!StringUtil.EmailUtil.IsValidEmail(updatedAdminEmail))
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                    "Update Failed",
                    "document.addEventListener('DOMContentLoaded', ()=>alert('Please Enter Valid Email'));",
                    true);

                return;
            }

            //check email duplicate
            if (Utils.ValidationUtils.IsEmailExist(updatedAdminEmail, adminId))
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                        "Update Failed",
                        "document.addEventListener('DOMContentLoaded', ()=>alert('Email already exist'));",
                        true);

                return;
            }

            //check phone
            if (!StringUtil.PhoneUtil.IsValidPhoneNumber(updatedAdminPhoneNo))
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                        "Update Failed",
                        "document.addEventListener('DOMContentLoaded', ()=>alert('Please Enter Valid Phone Number'));",
                        true);

                return;
            }

            int noOfRowAffected = updateStaff(adminId, updatedAdminName, updatedAdminUsername, updatedRole, updatedAdminEmail, updatedAdminPhoneNo, updatedStatus);

            if (noOfRowAffected > 0)
            {
                lvStaffs.EditIndex = -1;
                lvStaffs.DataSource = staffDataSource();
                lvStaffs.DataBind();
            }
        }

        protected void lvStaffs_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType != ListViewItemType.DataItem) return;

            DataRowView rowView = (DataRowView)e.Item.DataItem;

            // Change the Status color based on txt
            HtmlGenericControl statusSpan = (HtmlGenericControl)e.Item.FindControl("userStatus");
            if (statusSpan != null)
            {
                string status = rowView["AdminStatus"].ToString();
                switch (status)
                {
                    case "Activated":
                        statusSpan.Attributes["class"] += " activated";
                        break;
                    case "Locked":
                        statusSpan.Attributes["class"] += " locked";
                        break;
                    case "Deleted":
                        statusSpan.Attributes["class"] += " deleted";
                        break;
                    default:
                        break;
                }
            }

        }

        protected void lvStaffs_Sorting(object sender, ListViewSortEventArgs e)
        {
            toggleSortDirection(e.SortExpression); // Toggle sorting direction for the clicked column

            ViewState["SortExpression"] = e.SortExpression; // used for retain the sorting

            // Re-bind the ListView with sorted data
            lvStaffs.DataSource = staffDataSource(e.SortExpression, SortDirections[e.SortExpression]);
            lvStaffs.DataBind();
        }

        protected void lvStaffs_ItemCommand(object sender, ListViewCommandEventArgs e)
        {

        }

        protected void ddlFilterRole_DataBound(object sender, EventArgs e)
        {
            // set first item as all
            ddlFilterRole.Items.Insert(0, new ListItem("All Roles", "-1"));
        }

        protected void ddlFilterStatus_DataBound(object sender, EventArgs e)
        {
            // set first item as all
            ddlFilterStatus.Items.Insert(0, new ListItem("All Status", "-1"));
        }

        protected void ddlFilterRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sortExpression = ViewState["SortExpression"]?.ToString();
            lvStaffs.DataSource =
                sortExpression == null ?
                staffDataSource() :
                staffDataSource(sortExpression, SortDirections[sortExpression]);
            lvStaffs.DataBind();
        }
    }
}
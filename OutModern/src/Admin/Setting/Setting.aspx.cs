using Microsoft.SqlServer.Server;
using OutModern.src.Admin.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Admin.Setting
{
    public partial class Setting : System.Web.UI.Page
    {
        private string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private string adminId;

        protected void Page_Load(object sender, EventArgs e)
        {

            //retrieve admin id from session
            adminId = Session["AdminId"]?.ToString();
            if (adminId == null)
            {
                Response.Redirect("~/src/ErrorPages/403.aspx");
            }

            Session["MenuCategory"] = null;
            if (!IsPostBack)
            {
                DataTable adminData = getAdminData();
                if (adminData.Rows.Count > 0)
                {
                    DataRow row = adminData.Rows[0];
                    lblId.Text = adminId.ToString();
                    txtFName.Text = row["AdminFullName"].ToString();
                    txtUsername.Text = row["AdminUsername"].ToString();
                    txtEmail.Text = row["AdminEmail"].ToString();
                    txtPhoneNo.Text = row["AdminPhoneNo"].ToString();
                }
            }
        }

        //
        //db
        //

        private DataTable getAdminData()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string sqlQuery =
                    "SELECT AdminFullName, AdminUsername, AdminEmail, AdminPhoneNo " +
                    "FROM Admin " +
                    "WHERE AdminId = @adminId";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@AdminId", adminId);
                    dt.Load(cmd.ExecuteReader());
                }
            }
            return dt;
        }

        // update admin password
        private int updateAdminPassword(string hashedPassword)
        {
            int affectedRow = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string sqlQuery =
                    "UPDATE Admin " +
                    "SET AdminPassword = @AdminPassword " +
                    "WHERE AdminId = @AdminId";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@AdminPassword", hashedPassword);
                    cmd.Parameters.AddWithValue("@AdminId", adminId);
                    affectedRow = cmd.ExecuteNonQuery();
                }
            }

            return affectedRow;
        }

        // get admin old password
        private string getAdminPassword()
        {
            string adminPassword = "";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string sqlQuery =
                    "SELECT AdminPassword " +
                    "FROM Admin " +
                    "WHERE AdminId = @AdminId";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@AdminId", adminId);
                    adminPassword = cmd.ExecuteScalar().ToString();
                }
            }
            return adminPassword;
        }


        //
        //page event
        //
        protected void lbUpdateInfo_Click(object sender, EventArgs e)
        {
            string adminFullName = txtFName.Text.Trim();
            string adminUsername = txtUsername.Text.Trim();
            string adminEmail = txtEmail.Text.Trim();
            string adminPhoneNo = txtPhoneNo.Text.Trim();

            //check nulls
            if (string.IsNullOrEmpty(adminFullName) || string.IsNullOrEmpty(adminUsername) || string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPhoneNo))
            {
                Page.Page.ClientScript
                    .RegisterStartupScript(GetType(),
                    "Update Info Failed",
                    "document.addEventListener('DOMContentLoaded', ()=>alert('Please Fill in All Fields in General Info'));",
                    true);
                return;
            }

            //check email format
            if (StringUtil.EmailUtil.IsValidEmail(adminEmail) == false)
            {
                Page.Page.ClientScript
                    .RegisterStartupScript(GetType(),
                    "Update Info Failed",
                    "document.addEventListener('DOMContentLoaded', ()=>alert('Invalid Email Format'));",
                     true);
                return;
            }

            //check duplicate email
            if (ValidationUtils.IsEmailExist(adminEmail, adminId.ToString()))
            {
                Page.Page.ClientScript
                        .RegisterStartupScript(GetType(),
                        "Update Info Failed",
                         "document.addEventListener('DOMContentLoaded', ()=>alert('Email Already Exists'));",
                        true);
                return;
            }

            //check phone no format
            if (StringUtil.PhoneUtil.IsValidPhoneNumber(adminPhoneNo) == false)
            {
                Page.Page.ClientScript
                    .RegisterStartupScript(GetType(),
                    "Update Info Failed",
                    "document.addEventListener('DOMContentLoaded', ()=>alert('Invalid Phone No Format'));",
                    true);
                return;
            }

            //update admin info
            int affectedRow = updateAdminInfo(adminFullName, adminUsername, adminEmail, adminPhoneNo);
            if (affectedRow > 0)
            {
                Page.Page.ClientScript
                    .RegisterStartupScript(GetType(),
                        "Update Info Success",
                        "document.addEventListener('DOMContentLoaded', ()=>alert('Update Success'));",
                        true);
            }
            else
            {
                Page.Page.ClientScript
                    .RegisterStartupScript(GetType(),
                        "Update Info Failed",
                        "document.addEventListener('DOMContentLoaded', ()=>alert('Update Failed'));",
                        true);
            }

        }

        private int updateAdminInfo(string adminFullName, string adminUsername, string adminEmail, string adminPhoneNo)
        {
            int affectedRow = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string sqlQuery =
                    "UPDATE Admin " +
                    "SET AdminFullName = @AdminFullName, AdminUsername = @AdminUsername, AdminEmail = @AdminEmail, AdminPhoneNo = @AdminPhoneNo " +
                    "WHERE AdminId = @AdminId";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@AdminFullName", adminFullName);
                    cmd.Parameters.AddWithValue("@AdminUsername", adminUsername);
                    cmd.Parameters.AddWithValue("@AdminEmail", adminEmail);
                    cmd.Parameters.AddWithValue("@AdminPhoneNo", adminPhoneNo);
                    cmd.Parameters.AddWithValue("@AdminId", adminId);
                    affectedRow = cmd.ExecuteNonQuery();
                }
            }
            return affectedRow;
        }

        protected void lbUpdatePassword_Click(object sender, EventArgs e)
        {
            string currPass = txtCurrPass.Text.Trim();
            string newPass = txtNewPass.Text.Trim();
            string confirmPass = txtConfirmPass.Text.Trim();

            //check nulls
            if (string.IsNullOrEmpty(currPass) || string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass))
            {
                Page.Page.ClientScript
                    .RegisterStartupScript(GetType(),
                        "Update Password Failed",
                        "document.addEventListener('DOMContentLoaded', ()=>alert('Please Fill in All Fields in Change Password'));",
                        true);
                return;
            }

            //check new password and confirm password
            if (newPass != confirmPass)
            {
                Page.Page.ClientScript
                    .RegisterStartupScript(GetType(),
                    "Update Password Failed",
                    "document.addEventListener('DOMContentLoaded', ()=>alert('New Password and Confirm Password Not Match'));",
                    true);
                return;
            }

            //check current password and old password
            string oldPass = getAdminPassword();
            if (StringUtil.PasswordUtil.VerifyPassword(currPass, oldPass) == false)
            {
                Page.Page.ClientScript
                    .RegisterStartupScript(GetType(),
                        "Update Password Failed",
                        "document.addEventListener('DOMContentLoaded', ()=>alert('Current Password Incorrect'));",
                        true);
                return;
            }

            //update password
            int affectedRow = updateAdminPassword(StringUtil.PasswordUtil.HashPassword(newPass));

            if (affectedRow > 0)
            {
                Page.Page.ClientScript
                    .RegisterStartupScript(GetType(),
                    "Update Password Success",
                    "document.addEventListener('DOMContentLoaded', ()=>alert('Update Success'));",
                    true);
            }
            else
            {
                Page.Page.ClientScript
                    .RegisterStartupScript(GetType(),
                    "Update Password Failed",
                    "document.addEventListener('DOMContentLoaded', ()=>alert('Update Failed'));",
                    true);
            }

        }


    }
}
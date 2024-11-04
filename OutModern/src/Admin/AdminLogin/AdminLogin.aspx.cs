using StringUtil;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Client.Login
{
    public partial class AdminLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if already logged in, redirect to dashboard
            string adminId = Session["AdminID"]?.ToString();
            string adminRole = Session["AdminRole"]?.ToString();
            if (adminId != null && adminRole == "Manager")
            {
                Response.Redirect("~/src/Admin/Dashboard/Dashboard.aspx");
            }
        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            string err = "";
            string email = txt_email.Text.Trim();
            string passwd = txt_password.Text.Trim();

            if (email == null || passwd == null || email == "" || passwd == null)
            {
                err = "Email and Password cannot be left empty.";
            }
            else
            {
                //email validation
                if (!EmailUtil.IsValidEmail(email))
                {
                    err = "Invalid Email Format.";
                }

                if (err == "")
                {
                    try
                    {

                        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                        {
                            conn.Open();
                            //use parameterized query to prevent sql injection
                            string query =
                                "SELECT AdminId, AdminPassword, UserStatusName, AdminRoleName " +
                                "FROM [Admin], AdminRole, UserStatus " +
                                "WHERE Admin.AdminRoleId = AdminRole.AdminRoleId " +
                                "AND UserStatus.UserStatusId = Admin.AdminStatusId " +
                                "AND AdminEmail = @email";

                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@email", email);

                            SqlDataReader reader = cmd.ExecuteReader();
                            if (!reader.HasRows)
                            {
                                err = "Invalid email or password.";
                            }
                            else
                            {
                                //get password from reader
                                reader.Read();
                                string hashedPassword = reader["AdminPassword"].ToString();
                                int adminID = (int)reader["AdminId"];
                                string adminRole = (string)reader["AdminRoleName"];
                                string adminStatus = (string)reader["UserStatusName"];

                                //compare password
                                if (!PasswordUtil.VerifyPassword(passwd, hashedPassword))  //if have hash password then use this
                                    err = "Invalid email or password.";
                                else
                                {
                                    //not activated customer cannot log in
                                    if (adminStatus != "Activated")
                                    {
                                        err = "Admin is not activated";
                                    }
                                    else
                                    {
                                        Session["AdminID"] = adminID;
                                        Session["AdminRole"] = adminRole;

                                        if (adminRole == "Manager")
                                            Response.Redirect("~/src/Admin/Dashboard/Dashboard.aspx");
                                        else if (adminRole == "Staff")
                                            Response.Redirect("~/src/Admin/Products/Products.aspx");
                                    }
                                }
                                conn.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        err = "An error occurred during login. Please try again.";
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                            "Error",
                            "document.addEventListener('DOMContentLoaded'," +
                            " function() { alert('" + err + "'); });", true);
                    }
                }
            }
            ErrMsg.Text = err;

        }

    }
}
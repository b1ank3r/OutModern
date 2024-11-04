using StringUtil;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Client.UserProfile
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Request.Cookies["CustID"] == null)
            if (Session["CUSTID"] == null)
            {
                Response.Redirect("~/src/Client/Login/Login.aspx");
            }
        }

        protected void btn_reset_password_Click(object sender, EventArgs e)
        {
            //Reset message
            string err = "";
            string newPassword = txt_new_password.Text;
            string retypePassword = txt_reenter_new_password.Text;

            string conn = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            //int custID = int.Parse(Request.Cookies["CustID"].Value);
            int custID = (int)Session["CUSTID"];



            //password validation

            if (newPassword == "")
            {
                err = "Password cannot be left empty.";
                lblMessage.Text = err;
            }
            else
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(newPassword, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\d\s]).{8,}$"))
                {
                    err = "Invalid Password Format.Password must be at least 8 characters long and contain at least 1 uppercase, 1 lowercase, 1 number and 1 special character.";
                }
                else
                {

                    if (newPassword == "" || retypePassword == "")
                    {
                        err = "Password cannot be left empty.";
                    }
                    else
                    {

                        if (err == "")
                        {
                            if (newPassword == retypePassword)
                            {
                                try
                                {
                                    using (SqlConnection sqlConnection = new SqlConnection(conn))
                                    {
                                        sqlConnection.Open();

                                        string updatePasswd = "UPDATE Customer SET CustomerPassword = @Password WHERE CustomerId = @CustId";

                                        using (SqlCommand updateCommand = new SqlCommand(updatePasswd, sqlConnection))
                                        {
                                            String hashedPassword = PasswordUtil.HashPassword(newPassword);

                                            updateCommand.Parameters.AddWithValue("@Password", hashedPassword);
                                            updateCommand.Parameters.AddWithValue("@CustId", custID);

                                            int rowsAffected = updateCommand.ExecuteNonQuery();

                                            if (rowsAffected > 0)
                                            {
                                                // Password updated successfully
                                                err = "Password updated successfully";
                                                //for show pop up message in log in
                                                Session["PasswordChanged"] = true;
                                                Response.Redirect("UserProfile.aspx");
                                            }
                                            else
                                            {
                                                err = "Failed to update password";
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    err = "An error occurred: " + ex.Message;
                                }
                            }
                            else
                            {
                                err = "New Password and Re-enter New Password are not same";
                            }
                        }
                    }

                }
                lblMessage.Text = err;
            }
        }
    }
}
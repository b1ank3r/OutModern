using StringUtil;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Client.Login
{
    public partial class ResetPassword1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_reset_password_Click(object sender, EventArgs e)
        {
            ////Reset message
            //lblMessage.Text = "";

            string err = "";
            string newPassword = txt_new_password.Text;
            string retypePassword = txt_reenter_new_password.Text;

            string conn = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string custId = Session["CustomerId"] as string;

            if (string.IsNullOrEmpty(newPassword))
            {
                err = "Password cannot be left empty.";
                lblMessage.Text = err;
            }
            else
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(newPassword, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\d\s]).{8,}$"))
                {
                    err = "Password must be at least 8 characters long and contain at least 1 uppercase, 1 lowercase, 1 number and 1 special character.";
                    lblMessage.Text = err;
                }
                else
                {
                    if (newPassword == "" || retypePassword == "")
                    {
                        err = "Password cannot be left empty.";
                        lblMessage.Text = err;
                    }
                    else
                    {
                        if (newPassword != retypePassword)
                        {
                            err = "Confirm Password does not match Password.";
                        }
                        else
                        {
                            if (custId != null)
                            {
                                //password validation
                                if (!System.Text.RegularExpressions.Regex.IsMatch(newPassword, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\d\s]).{8,}$"))
                                {
                                    err = "Invalid Password Format.";
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
                                                        updateCommand.Parameters.AddWithValue("@CustId", custId);

                                                        int rowsAffected = updateCommand.ExecuteNonQuery();

                                                        if (rowsAffected > 0)
                                                        {
                                                            // Password updated successfully
                                                            err = "Password updated successfully";
                                                            //for show pop up message in log in
                                                            Session["PasswordChange"] = true;
                                                            Response.Redirect("Login.aspx");
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
                            else
                            {
                                Console.WriteLine("Invalid to change bacause valid time had passed.");
                            }
                        }

                        lblMessage.Text = err;

                    }
                }


            }

        }

    }
}
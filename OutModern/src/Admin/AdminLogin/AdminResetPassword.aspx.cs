using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Client.Login
{
    public partial class AdminResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_reset_password_Click(object sender, EventArgs e)
        {
            string conn = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string admId = Session["AdminId"] as string;
            if (admId != null)
            {
                String newPassword = txt_new_password.Text;
                String retypePassword = txt_reenter_new_password.Text;
                if (newPassword == retypePassword)
                {
                    try
                    {
                        using (SqlConnection sqlConnection = new SqlConnection(conn))
                        {
                            sqlConnection.Open();

                            string updatePasswd = "UPDATE Admin SET AdminPassword = @Password WHERE AdminId = @AdmId";

                            using (SqlCommand updateCommand = new SqlCommand(updatePasswd, sqlConnection))
                            {
                                //String hashedPassword = StringUtil.PasswordHandler.hashingPassword(newPassword);

                                updateCommand.Parameters.AddWithValue("@Password", newPassword);
                                updateCommand.Parameters.AddWithValue("@AdmId", admId);

                                int rowsAffected = updateCommand.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // Password updated successfully
                                    lblMessage.Text = "Password updated successfully";
                                    //for show pop up message in log in
                                    Session["PasswordChanged"] = true;
                                    Response.Redirect("AdminLogin.aspx");
                                }
                                else
                                {
                                    lblMessage.Text = "Failed to update password";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "An error occurred: " + ex.Message;
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid to change bacause valid time had passed.");
            }

        }
    }
}
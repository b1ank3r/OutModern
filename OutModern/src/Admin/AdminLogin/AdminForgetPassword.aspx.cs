using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Client.Login
{
    public partial class AdminForgetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            string conn = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            if (!string.IsNullOrEmpty(txt_fp_email.Text))
            {
                string receiverEmail = txt_fp_email.Text;

                using (SqlConnection sqlConnection = new SqlConnection(conn))
                {
                    sqlConnection.Open();
                    string query = "SELECT AdminId FROM Admin WHERE AdminEmail = @Email";

                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@Email", receiverEmail);

                        object result = sqlCommand.ExecuteScalar();
                        if (result != null)
                        {
                            string admId = result.ToString();

                            Session["AdminId"] = admId;

                            try
                            {
                                string senderEmail = "magnetic636@gmail.com";

                                MailMessage verificationMail = new MailMessage(senderEmail, receiverEmail);

                                verificationMail.Subject = "Password Recovery from OutModern";
                                verificationMail.Body = "<h3>Please click the button link below to verify that you are the user and proceed to password reset page.</h3><br><br>" +
                                        "<a href=\"http://localhost:44338/src/Admin/AdminLogin/AdminResetPassword.aspx\" style=\"color:white;border:1px solid black;background-color:black;padding: 15px 10px;\">Reset Password</a>";
                                verificationMail.IsBodyHtml = true;

                                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                                smtpClient.EnableSsl = true;
                                smtpClient.UseDefaultCredentials = false;
                                smtpClient.Credentials = new NetworkCredential(senderEmail, "tqbp zars jlzu wpof");

                                smtpClient.Send(verificationMail);
                                lblMessage.Text = "Email sent successfully";
                            }
                            catch (Exception ex)
                            {
                                lblMessage.Text = "Email failed to send: " + ex.Message;
                            }
                        }
                        else
                        {
                            lblMessage.Text = "You are not part of our admin.";
                        }
                    }

                }
            }
            else if (string.IsNullOrEmpty(txt_fp_email.Text))
            {
                lblMessage.Text = "Email cannot be left empty.";
            }
            else if (IsValidEmail(txt_fp_email.Text))
            {
                lblMessage.Text = "Invalid Email Format.";
            }

        }

        protected bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}
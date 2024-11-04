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
    public partial class SignUp1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_sign_up_Click(object sender, EventArgs e)
        {
            //reset all error text
            FullnameErrMsg.Text = "";
            UsernameErrMsg.Text = "";
            EmailErrMsg.Text = "";
            PasswordErrMsg.Text = "";
            ReenterPasswordErrMsg.Text = "";
            PolicyErrMsg.Text = "";

            string fullname = txt_su_fullname.Text;
            string username = txt_su_username.Text;
            string email = txt_su_email.Text;
            string password = txt_su_password.Text;
            string reenter_passwword = txt_su_reenter_password.Text;
            string fullnameErr = "";
            string usernameErr = "";
            string emailErr = "";
            string passwordErr = "";
            string reenterpwdErr = "";

            //fullname validation
            if (string.IsNullOrEmpty(fullname))
            {
                fullnameErr = "Fullname cannot be left empty.";
            }

            //username validation
            if (string.IsNullOrEmpty(username))
            {
                usernameErr = "Username cannot be left empty.";
            }

            //email validation
            if (string.IsNullOrEmpty(email))
            {
                emailErr = "Email cannot be left empty.";
            }
            else if (!IsValidEmail(email))
            {
                emailErr = "Invalid Email Format.";
            }
            else if (IsDuplicateEmail(email))
            {
                emailErr = "Email already exists.";
            }

            //password validation
            if (string.IsNullOrEmpty(password))
            {
                passwordErr = "Password cannot be left empty.";
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\d\s]).{8,}$"))
            {
                passwordErr = "Password must be at least 8 characters long and contain at least 1 uppercase, 1 lowercase, 1 number and 1 special character.";
            }

            //reenter password  validation
            if (string.IsNullOrEmpty(reenter_passwword))
            {
                reenterpwdErr = "Confirm Password cannot be left empty.";
            }
            else if (reenter_passwword != password)
            {
                reenterpwdErr = "Confirm Password does not match Password.";
            }

            //policy checkbox validation
            if (!chkbox_policy.Checked)
            {
                PolicyErrMsg.Text = "Please agree to the terms and conditions.";
            }

            //register account
            if (fullnameErr == "" && usernameErr == "" && emailErr == "" && passwordErr == "" && reenterpwdErr == "" && PolicyErrMsg.Text == "")
            {
                try
                {
                    // Insert into the Customer table and retrieve the CustomerId
                    int customerId;
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                    {

                        string hashedPassword = PasswordUtil.HashPassword(password);

                        conn.Open();
                        //use parameterized query to prevent sql injection
                        string query = "INSERT INTO [Customer] (CustomerFullname, CustomerUsername, CustomerPassword, CustomerEmail, CustomerStatusId)"
                            + " VALUES (@fullname, @username, @password, @email, @status);SELECT SCOPE_IDENTITY();";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@fullname", fullname);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        cmd.Parameters.AddWithValue("@status", 1);
                        //string id = cmd.ExecuteScalar().ToString();
                        customerId = Convert.ToInt32(cmd.ExecuteScalar());
                        conn.Close();

                        // Insert into another table using the retrieved CustomerId
                        if (customerId > 0)
                        {
                            try
                            {
                                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                                {
                                    con.Open();
                                    // Use parameterized query to prevent SQL injection
                                    string cartQuery = "INSERT INTO [Cart] (CustomerId, Subtotal) VALUES (@customerId,@subtotal);";
                                    SqlCommand cartCmd = new SqlCommand(cartQuery, con);
                                    cartCmd.Parameters.AddWithValue("@customerId", customerId);
                                    cartCmd.Parameters.AddWithValue("@subtotal", 0.00);
                                    // Add other parameters for the Cart table as needed
                                    cartCmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                // Handle exception
                                Response.Write(ex.Message);
                            }
                        }

                        //for show pop up message
                        Session["registered"] = true;
                        Response.Redirect("Login.aspx");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
            else
            {
                //show error message
                if (fullnameErr != "")
                {
                    FullnameErrMsg.Text = fullnameErr;
                }

                if (usernameErr != "")
                {
                    UsernameErrMsg.Text = usernameErr;
                }

                if (emailErr != "")
                {
                    EmailErrMsg.Text = emailErr;
                }

                if (passwordErr != "")
                {
                    PasswordErrMsg.Text = passwordErr;
                }

                if (reenterpwdErr != "")
                {
                    ReenterPasswordErrMsg.Text = reenterpwdErr;
                }
            }

        }

        protected bool IsDuplicateEmail(string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM [Customer] WHERE CustomerEmail = @email";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@email", email);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        return true;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //Err
            }
            return false;
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
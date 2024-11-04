using StringUtil;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Client.Login
{
    public partial class Login2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //// Delete specific cookies on login page load
            //string[] cookieNamesToDelete = new string[] { "LoggedIn", "CustID", "CustStatus" };

            //foreach (string cookieName in cookieNamesToDelete)
            //{
            //    HttpCookie cookieToDelete = Request.Cookies[cookieName];
            //    if (cookieToDelete != null)
            //    {
            //        cookieToDelete.Expires = DateTime.Now.AddDays(-1); // Set expiration to past date
            //        Response.Cookies.Add(cookieToDelete); // Re-add cookie with expired date
            //    }
            //}

            //Session.Remove("LoggedIn");
            //Session.Remove("CUSTID");
            //Session.Remove("CustStatus");

            //// Check if there's a redirect parameter
            //string redirectParam = Request.QueryString["redirect"];

            //// If user is logged in
            //if (Session["CUSTID"] != null && Session["LoggedIn"] != null && (int)Session["CustStatus"] == 1)
            //{
            //    // If there's a "redirect=home" query, redirect to Home.aspx
            //    if (redirectParam == "home")
            //    {
            //        Response.Redirect("~/src/Client/Home/Home.aspx");
            //    }
            //    else
            //    {
            //        Session.Remove("LoggedIn");
            //        Session.Remove("CUSTID");
            //        Session.Remove("CustStatus");
            //    }
            //}

            //if already logged in, redirect to dashboard
            string loggedIn = Session["LoggedIn"]?.ToString();
            string custId = Session["CUSTID"]?.ToString();
            string custStatus = Session["CustStatus"]?.ToString();
            if (loggedIn != null && custId != null && custStatus == "1")
            {
                Response.Redirect("~/src/Client/Home/Home.aspx");
            }
        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            string err = "";
            string email = txt_email.Text;
            string passwd = txt_password.Text;

            if (email == null || passwd == null || email == "" || passwd == null)
            {
                err = "Email and Password cannot be left empty.";
            }
            else
            {
                //email validation
                if (!IsValidEmail(email))
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
                            string query = "SELECT * FROM [Customer] WHERE CustomerEmail = @email";
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
                                string hashedPassword = reader["CustomerPassword"].ToString();
                                int customerID = (int)reader["CustomerId"];
                                int customerStatus = (int)reader["CustomerStatusId"];

                                //compare password
                                if (!PasswordUtil.VerifyPassword(passwd, hashedPassword))
                                {
                                    err = "Invalid email or password.";

                                }
                                else
                                {
                                    //not activated customer cannot log in
                                    if (customerStatus != 1)
                                    {
                                        err = "Customer is not activated";
                                    }
                                    else
                                    {
                                        //HttpCookie loggedInCookie = new HttpCookie("LoggedIn", "true");
                                        //loggedInCookie.Expires = DateTime.Now.AddDays(30); // Set cookie expiration
                                        //Response.Cookies.Add(loggedInCookie);

                                        //HttpCookie custIDCookie = new HttpCookie("CustID", customerID.ToString());
                                        //custIDCookie.Expires = DateTime.Now.AddDays(30);
                                        //Response.Cookies.Add(custIDCookie);

                                        //HttpCookie custStatusCookie = new HttpCookie("CustStatus", customerStatus.ToString());
                                        //custStatusCookie.Expires = DateTime.Now.AddDays(30);
                                        //Response.Cookies.Add(custStatusCookie);

                                        // Set session variable for logged-in status
                                        Session["LoggedIn"] = true;

                                        // Set session variable for customer ID
                                        Session["CUSTID"] = customerID;

                                        // Set session variable for customer status
                                        Session["CustStatus"] = customerStatus;

                                        Response.Redirect("~/src/Client/Home/Home.aspx");
                                    }
                                }
                                conn.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        err = "An error occurred during login. Please try again.";
                        Response.Write(ex.Message);
                    }
                }
            }
            ErrMsg.Text = err;
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
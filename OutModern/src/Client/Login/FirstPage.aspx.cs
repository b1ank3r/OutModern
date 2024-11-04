using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Client.Login
{
    public partial class Login1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_first_page_login_Click(object sender, EventArgs e)
        {
            // Redirect to Login page
            Response.Redirect("Login.aspx");
        }

        protected void btn_first_page_signup_Click(object sender, EventArgs e)
        {
            // Redirect to Login page
            Response.Redirect("SignUp.aspx");
        }

        protected void btn_first_page_signup_for_free_Click(object sender, EventArgs e)
        {
            // Redirect to Login page
            Response.Redirect("SignUp.aspx");
        }

        protected void btn_admin_login_Click(object sender, EventArgs e)
        {
            // Redirect to Login page
            Response.Redirect("AdminLogin.aspx");
        }
    }
}
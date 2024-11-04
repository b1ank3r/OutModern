using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Client.Home
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Request.Cookies["LoggedIn"] != null)
            //if (Session["LoggedIn"] != null && (bool)Session["LoggedIn"] == true)
            //{
            //bool loggedIn = bool.Parse(Request.Cookies["LoggedIn"].Value);
            //int custID = int.Parse(Request.Cookies["CustID"].Value);
            //int custStatus = int.Parse(Request.Cookies["CustStatus"].Value);

            // Get the session variable for logged-in status
            bool loggedIn = Convert.ToBoolean(Session["LoggedIn"]);
            // Get the session variable for customer ID
            int custID = Convert.ToInt32(Session["CUSTID"]);
            // Get the session variable for customer status
            int custStatus = Convert.ToInt32(Session["CustStatus"]);

            if (loggedIn && custStatus == 1) // Check for active customer
            {
                Session["LoggedIn"] = loggedIn;
                Session["CustID"] = custID;
                Session["CustStatus"] = custStatus;

                //pop out message show CustID and CustStatus
                //lblCustInfo.Text = $"Hi {custID}!";
                //lblCustInfo.Visible = true;

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    conn.Open();
                    //use parameterized query to prevent sql injection
                    string query = "SELECT * FROM [Customer] WHERE CustomerId = @custId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@custId", custID);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows) // Check if there are any results
                    {
                        reader.Read(); // Read the first row


                        //left box display
                        lblCustInfo.Visible = true;
                        lblCustInfo.Text = "Hi ";
                        lblCustInfo.Text += reader["CustomerUsername"].ToString();
                        lblCustInfo.Text += "!";

                        reader.Close();
                    }

                }

            }
            //else
            //{
            //// Clear cookies if login failed or customer inactive
            //Response.Cookies["LoggedIn"].Expires = DateTime.Now.AddDays(-1);
            //Response.Cookies["CustID"].Expires = DateTime.Now.AddDays(-1);
            //Response.Cookies["CustStatus"].Expires = DateTime.Now.AddDays(-1);

            //Response.Redirect("~/src/Client/Login/Login.aspx");
            //}
            //}
            //else
            //{
            //Response.Redirect("~/src/Client/Login/Login.aspx");
            //}
        }


    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Admin.AdminMaster
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected static readonly string Dashboard = "Dashboard";
        protected static readonly string Staffs = "Staffs";
        protected static readonly string Products = "Products";
        protected static readonly string Orders = "Orders";
        protected static readonly string Customers = "Customers";
        protected static readonly string PromoCode = "PromoCode";
        protected static readonly string Setting = "Setting";

        // Side menu urls
        protected Dictionary<string, string> urls = new Dictionary<string, string>()
        {
            { Dashboard , "~/src/Admin/Dashboard/Dashboard.aspx" },
            { Staffs , "~/src/Admin/Staffs/Staffs.aspx" },
            { Products , "~/src/Admin/Products/Products.aspx" },
            { Orders , "~/src/Admin/Orders/Orders.aspx" },
            { Customers , "~/src/Admin/Customers/Customers.aspx" },
            { PromoCode , "~/src/Admin/PromoCode/PromoCode.aspx" },
            { Setting , "~/src/Admin/Setting/Setting.aspx" },
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            string adminRole = Session["AdminRole"]?.ToString();
            string adminId = Session["AdminId"]?.ToString();

            // Check if user is logged in
            if (adminRole == null || adminId == null)
            {
                Response.Redirect("~/src/ErrorPages/403.aspx");
            }

            if (!IsPostBack)
            {
                DataTable user = getUser(adminId);
                DataRow row = user.Rows[0];
                lblUsername.Text = row["AdminUsername"].ToString();

                setMenuActive();
                Page.DataBind(); // data bind
            }
        }

        private void setMenuActive()
        {
            string menuCategory = Session["MenuCategory"]?.ToString();

            if (menuCategory != null)
            {
                foreach (var item in urls)
                {
                    if (menuCategory == item.Key)
                    {
                        var menu = FindControl(("hyperlink" + item.Key)) as HyperLink;
                        if (menu != null)
                        {
                            menu.CssClass += " active";
                        }
                    }
                }
            }
        }

        //
        //db
        //
        private DataTable getUser(string adminId)
        {
            DataTable user = new DataTable();
            string conn = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection sqlConnection = new SqlConnection(conn))
            {
                sqlConnection.Open();
                string query =
                    "SELECT AdminFullName, AdminUsername, AdminRoleName " +
                    "FROM Admin, AdminRole " +
                    "WHERE Admin.AdminRoleId = AdminRole.AdminRoleId " +
                    "AND AdminId = @adminId";

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@adminId", adminId);
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        sqlDataAdapter.Fill(user);
                    }

                }
            }

            return user;
        }



        //
        //page
        //
        protected void lBtnSearch_Click(object sender, EventArgs e)
        {
            var searchTerm = txtSearch.Text.Trim();
            var currentUrl = HttpContext.Current.Request.Url;
            var query = HttpUtility.ParseQueryString(currentUrl.Query);

            // Update the 'q' query string parameter
            query["q"] = searchTerm;

            // Rebuild the URL
            var newUrl = currentUrl.AbsolutePath + "?" + query.ToString();

            // Redirect to the same page with new query string
            Response.Redirect(newUrl);

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            lblServerTime.Text = DateTime.Now.ToString();
        }

        // add query string to the sitemap url dynamically
        protected void sitemapAdmin_ItemDataBound(object sender, SiteMapNodeItemEventArgs e)
        {
            // Check if the current node is a SiteMapNode
            if (e.Item.ItemType == SiteMapNodeItemType.PathSeparator ||
                e.Item.ItemType == SiteMapNodeItemType.Current)
            {
                return;
            }

            // Retrieve the current page's query string
            string currentPageQueryString = HttpContext.Current.Request.QueryString.ToString();

            // If the current page has a query string
            if (!string.IsNullOrEmpty(currentPageQueryString))
            {
                // Generate the dynamic query string for the next page
                string dynamicQueryString = currentPageQueryString;

                // Find the hyperlink control in the current item and modify its NavigateUrl
                HyperLink link = (HyperLink)e.Item.Controls[0]; // Assuming the hyperlink control is the first control in the item
                link.NavigateUrl += "?" + dynamicQueryString;
            }
        }

        protected void linkBtnLogout_Click(object sender, EventArgs e)
        {
            Session.Remove("AdminRole");
            Session.Remove("AdminId");

            Response.Redirect("~/src/Admin/AdminLogin/AdminLogin.aspx");
        }
    }
}
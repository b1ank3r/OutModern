using StringUtil;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Client.Profile
{
    public partial class UserProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int custID;
            //if (Request.Cookies["CustID"] == null)
            if (Session["CUSTID"] == null)
            {
                Response.Redirect("~/src/Client/Login/Login.aspx");
            }
            else
            {
                //custID = int.Parse(Request.Cookies["CustID"].Value);
                custID = (int)Session["CUSTID"];

                if (!IsPostBack)
                {
                    try
                    {
                        ////For show the customer profile
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
                                lbl_username.Text = reader["CustomerUsername"].ToString();

                                lbl_up_username.Text = reader["CustomerUsername"].ToString();
                                lbl_up_fullname.Text = reader["CustomerFullname"].ToString();
                                lbl_up_email.Text = reader["CustomerEmail"].ToString();
                                //img_profile.ImageUrl = reader["ProfileImagePath"].ToString();

                                string profileImagePath = reader["ProfileImagePath"].ToString();
                                img_profile.ImageUrl = profileImagePath;

                                if (reader["CustomerPhoneNumber"].ToString() == "")
                                {
                                    lbl_up_phoneNumber.Text = "-";
                                }
                                else
                                {
                                    lbl_up_phoneNumber.Text = reader["CustomerPhoneNumber"].ToString();
                                }

                                reader.Close();
                            }


                            //// get address name for dropdownlist for customer
                            string addressQuery = "SELECT * FROM Address WHERE CustomerId = @custId And isDeleted = 0";
                            SqlCommand addressCmd = new SqlCommand(addressQuery, conn);
                            addressCmd.Parameters.AddWithValue("@custId", custID);

                            DataTable data = new DataTable();
                            data.Load(addressCmd.ExecuteReader());

                            if (data.Rows.Count == 0)
                            {
                                lbl_addressLine.Text = "N/A";
                                lbl_country.Text = "N/A";
                                lbl_state.Text = "N/A";
                                lbl_postaCode.Text = "N/A";
                            }
                            else
                            {
                                lbl_addressLine.Text = data.Rows[0]["AddressLine"].ToString();
                                lbl_country.Text = data.Rows[0]["Country"].ToString();
                                lbl_state.Text = data.Rows[0]["State"].ToString();
                                lbl_postaCode.Text = data.Rows[0]["PostalCode"].ToString();

                                //if no change selection
                                Session["AddressName"] = ddl_address_name.SelectedValue;

                                ddl_address_name.DataSource = data;
                                ddl_address_name.DataTextField = "AddressName";
                                ddl_address_name.DataValueField = "AddressName";
                                ddl_address_name.DataBind();
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message);
                    }
                }

            }
        }

        protected void btn_edit_profile_Click(object sender, EventArgs e)
        {
            int custID = (int)Session["CUSTID"];

            //if user dont have address, they need to add address before edit profile
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                conn.Open();
                //use parameterized query to prevent sql injection
                string query = "SELECT * FROM [Address] WHERE CustomerId = @custId And isDeleted = 0";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@custId", custID);

                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows) // Check if there are any results
                {
                    Session["ShowAddAddressPopup"] = true;
                }
                else
                {
                    // Redirect to Edit User Profile page
                    Response.Redirect("EditUserProfile.aspx");
                }

            }

        }

        protected void btn_togo_my_order_Click(object sender, EventArgs e)
        {
            // Redirect to Edit User Profile page
            Response.Redirect("ToShip.aspx");
        }

        protected void btn_togo_profile_Click(object sender, EventArgs e)
        {
            // Redirect to Edit User Profile page
            Response.Redirect("UserProfile.aspx");
        }

        protected void btn_chg_pwd_Click(object sender, EventArgs e)
        {
            // Redirect to login page
            Response.Redirect("~/src/Client/UserProfile/ChangePassword.aspx");
        }

        protected void btn_dlt_acc_Click(object sender, EventArgs e)
        {
            // Get CustID from the cookie
            //int custID = int.Parse(Request.Cookies["CustID"].Value);
            int custID = (int)Session["CUSTID"];

            // Connection string
            //string connectionString = "ConnectionString";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                // Update customer status to 3 (deleted)
                string updateSql = "UPDATE Customer SET CustomerStatusId = (SELECT UserStatusId FROM UserStatus WHERE UserStatusName = 'Deleted') WHERE CustomerId = @CustID";
                SqlCommand updateCommand = new SqlCommand(updateSql, conn);
                updateCommand.Parameters.AddWithValue("@CustID", custID);

                conn.Open();
                updateCommand.ExecuteNonQuery();
                conn.Close();
            }

            // Invalidate any existing session cookies
            Session.Abandon();

            // Redirect to login page
            Response.Redirect("~/src/Client/Login/Login.aspx");
        }

        protected void ddl_address_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                conn.Open();
                // Get selected address name from dropdown
                string selectedAddressName = ddl_address_name.SelectedValue;

                //string custID = Request.Cookies["CustID"].Value;
                string custID = Session["CUSTID"].ToString();

                // Get address data (assuming only one address per customer)
                string addressQuery = "SELECT * FROM Address WHERE CustomerId = @custId AND AddressName = @addressName And isDeleted = 0";
                SqlCommand addressCmd = new SqlCommand(addressQuery, conn);
                addressCmd.Parameters.AddWithValue("@custId", custID);
                addressCmd.Parameters.AddWithValue("@addressName", selectedAddressName);

                SqlDataReader addressReader = addressCmd.ExecuteReader();

                if (addressReader.HasRows)
                {
                    addressReader.Read(); // Read the first row (assuming only one address)

                    lbl_addressLine.Text = addressReader["AddressLine"].ToString();
                    lbl_country.Text = addressReader["Country"].ToString();
                    lbl_state.Text = addressReader["State"].ToString();
                    lbl_postaCode.Text = addressReader["PostalCode"].ToString();

                    //if change selection
                    Session["AddressName"] = ddl_address_name.SelectedValue;
                }
                else
                {
                    // Handle case where no address is found for the selected name
                    lbl_addressLine.Text = "N/A";
                    lbl_country.Text = "N/A";
                    lbl_state.Text = "N/A";
                    lbl_postaCode.Text = "N/A";
                }
                addressReader.Close();
            }
        }

        protected void btn_add_address_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/src/Client/UserProfile/AddAddress.aspx");
        }

        protected void btn_dlt_address_Click(object sender, EventArgs e)
        {
            int custID = (int)Session["CUSTID"];

            //if user dont have address, they need to add address before delete address
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                conn.Open();
                //use parameterized query to prevent sql injection
                string query = "SELECT * FROM [Address] WHERE CustomerId = @custId And isDeleted = 0";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@custId", custID);

                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows) // Check if there are any results
                {
                    Session["ShowAddAddressPopup2"] = true;
                }
                else
                {
                    // Redirect to Edit User Profile page
                    Response.Redirect("~/src/Client/UserProfile/DeleteAddress.aspx");
                }

            }

            
        }

        protected void btn_rw_his_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/src/Client/UserProfile/UserReivew.aspx");
        }
    }
}
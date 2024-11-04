using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Client.UserProfile
{
    public partial class DeleteAddress : System.Web.UI.Page
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

                    ////For show the customer address
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                    {
                        conn.Open();

                        //// get address name for dropdownlist for customer
                        string addressQuery = "SELECT * FROM Address WHERE CustomerId = @custId And isDeleted = 0";
                        SqlCommand addressCmd = new SqlCommand(addressQuery, conn);
                        addressCmd.Parameters.AddWithValue("@custId", custID);

                        DataTable data = new DataTable();
                        data.Load(addressCmd.ExecuteReader());

                        if (data.Rows.Count == 0)
                        {
                            txt_address_line.Text = "N/A";
                            txt_country.Text = "N/A";
                            txt_state.Text = "N/A";
                            txt_postal_code.Text = "N/A";
                        }
                        else
                        {
                            txt_address_line.Text = data.Rows[0]["AddressLine"].ToString();
                            txt_country.Text = data.Rows[0]["Country"].ToString();
                            txt_state.Text = data.Rows[0]["State"].ToString();
                            txt_postal_code.Text = data.Rows[0]["PostalCode"].ToString();

                            //if no change selection
                            //Session["AddressNameForDlt"] = ddl_address_name.SelectedValue;
                            Session["AddressNameForDlt"] = data.Rows[0]["AddressName"].ToString();

                            ddl_address_name.DataSource = data;
                            ddl_address_name.DataTextField = "AddressName";
                            ddl_address_name.DataValueField = "AddressName";
                            ddl_address_name.DataBind();
                        }

                    }
                }
            }
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

                    txt_address_line.Text = addressReader["AddressLine"].ToString();
                    txt_country.Text = addressReader["Country"].ToString();
                    txt_state.Text = addressReader["State"].ToString();
                    txt_postal_code.Text = addressReader["PostalCode"].ToString();

                    //if change selection
                    Session["AddressNameForDlt"] = ddl_address_name.SelectedValue;
                }
                else
                {
                    // Handle case where no address is found for the selected name
                    txt_address_line.Text = "N/A";
                    txt_country.Text = "N/A";
                    txt_state.Text = "N/A";
                    txt_postal_code.Text = "N/A";
                }
                addressReader.Close();
            }
        }

        protected void btn_add_address_Click(object sender, EventArgs e)
        {
            //int custID = int.Parse(Request.Cookies["CustID"].Value);
            int custID = (int)Session["CUSTID"];

            string addressName = Session["AddressNameForDlt"] as string;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    conn.Open();

                    string query1 = "UPDATE Address SET isDeleted = @isDeleted WHERE CustomerId = @CustID And AddressName = @addressName";
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    cmd1.Parameters.AddWithValue("@CustID", custID);
                    cmd1.Parameters.AddWithValue("@addressName", addressName);
                    cmd1.Parameters.AddWithValue("@isDeleted", 1);

                    int customerId = Convert.ToInt32(cmd1.ExecuteScalar());
                    conn.Close();

                    // Profile updated successfully
                    lblMessage.Text = "Address delete successfully";
                    //for show pop up message
                    Session["AddressDelete"] = true;
                    Response.Redirect("UserProfile.aspx");

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: You cannot delete an address when there is none. An error occurred while deleting address.";
            }
        }
    }
}
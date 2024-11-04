using OutModern.src.Admin.Customers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Client.UserProfile
{
    public partial class AddAddress : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Request.Cookies["CustID"] == null)
            if (Session["CUSTID"] == null)
            {
                Response.Redirect("~/src/Client/Login/Login.aspx");
            }
        }

        protected void btn_add_address_Click(object sender, EventArgs e)
        {
            //reset all error text
            addressNameErrMsg.Text = "";
            addressLineErrMsg.Text = "";
            countryErrMsg.Text = "";
            stateErrMsg.Text = "";
            postalCodeLineErrMsg.Text = "";

            lblMessage.Text = "";

            string adName = txt_address_name.Text;
            string adLine = txt_address_line.Text;
            string country = txt_country.Text;
            string state = txt_state.Text;
            string ptCode = txt_postal_code.Text;
            string addressNameErr = "";
            string addressLineErr = "";
            string countryErr = "";
            string stateErr = "";
            string postalCodeLineErr = "";

            //int custID = int.Parse(Request.Cookies["CustID"].Value);
            int custID = (int)Session["CUSTID"];

            //Validation
            //Address Line Validation
            if (string.IsNullOrEmpty(adName))
            {
                addressNameErr = "Address Name cannot be left empty.";
            }
            else if (StringUtil.AddressUtil.IsDuplicateAddressName(adName))
            {
                addressNameErr = "Address Name cannot be duplicate.";
            }

            //Address Line Validation
            if (string.IsNullOrEmpty(adLine))
            {
                addressLineErr = "Address cannot be left empty.";
            }

            //Country Validation
            if (string.IsNullOrEmpty(country))
            {
                countryErr = "Country cannot be left empty.";
            }
            else if (country.All(char.IsDigit)) // Attempt to parse as integer
            {
                countryErr = "Country cannot be a number. Please enter a valid country name.";
            }

            //State Validation
            if (string.IsNullOrEmpty(state))
            {
                stateErr = "State cannot be left empty.";
            }
            else if (state.All(char.IsDigit)) // Attempt to parse as integer
            {
                stateErr = "State cannot be a number. Please enter a valid State name.";
            }

            //Postal Code Validation
            if (string.IsNullOrEmpty(ptCode))
            {
                postalCodeLineErr = "Postal Code cannot be left empty.";
            }
            else if (!ptCode.All(char.IsDigit)) // Check if all characters are digits
            {
                postalCodeLineErr = "Postal Code can only contain numbers.";
            }

            if (addressNameErr == "" && addressLineErr == "" && countryErr == "" && stateErr == "" && postalCodeLineErr == "")
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                    {
                        conn.Open();

                        string query1 = "INSERT INTO [Address] (CustomerId, AddressName, AddressLine, Country, State, PostalCode, isDeleted)"
                            + " VALUES (@custId, @addressName, @addressLine, @country, @state, @postalCode, @isDeleted);SELECT SCOPE_IDENTITY();";
                        SqlCommand cmd1 = new SqlCommand(query1, conn);
                        cmd1.Parameters.AddWithValue("@custId", custID);
                        cmd1.Parameters.AddWithValue("@addressName", adName);
                        cmd1.Parameters.AddWithValue("@addressLine", adLine);
                        cmd1.Parameters.AddWithValue("@country", country);
                        cmd1.Parameters.AddWithValue("@state", state);
                        cmd1.Parameters.AddWithValue("@postalCode", ptCode);
                        cmd1.Parameters.AddWithValue("@isDeleted", 0);
                        
                        int customerId = Convert.ToInt32(cmd1.ExecuteScalar());
                        conn.Close();

                        // Profile updated successfully
                        lblMessage.Text = "Address added successfully";
                        //for show pop up message
                        Session["AddressAdded"] = true;
                        Response.Redirect("UserProfile.aspx");

                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    lblMessage.Text = "An error occurred while updating profile.";
                }
            }
            else
            {
                //show error message
                if (addressNameErr != "")
                {
                    addressNameErrMsg.Text = addressNameErr;
                }

                if (addressLineErr != "")
                {
                    addressLineErrMsg.Text = addressLineErr;
                }

                if (countryErr != "")
                {
                    countryErrMsg.Text = countryErr;
                }

                if (stateErr != "")
                {
                    stateErrMsg.Text = stateErr;
                }

                if (postalCodeLineErr != "")
                {
                    postalCodeLineErrMsg.Text = postalCodeLineErr;
                }

            }

        }
    }
}
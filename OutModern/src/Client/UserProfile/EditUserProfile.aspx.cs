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
    public partial class EditUserProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //int custID = int.Parse(Request.Cookies["CustID"].Value);
            int custID = (int)Session["CUSTID"];

            string addressName = Session["AddressName"] as string;

            if (!IsPostBack)
            {

                try
                {
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

                            txt_edit_username.Text = reader["CustomerUsername"].ToString();
                            txt_edit_fullname.Text = reader["CustomerFullname"].ToString();

                            string profileImagePath = reader["ProfileImagePath"].ToString();
                            img_profile.ImageUrl = profileImagePath;

                            if (reader["CustomerPhoneNumber"].ToString() == "")
                            {
                                txt_edit_phone_number.Text = "-";
                            }
                            else
                            {
                                txt_edit_phone_number.Text = reader["CustomerPhoneNumber"].ToString();
                            }
                            reader.Close();
                        }

                        ////
                        //if user no select the address name
                        if (addressName == null || addressName == "")
                        {
                            //// get address name for dropdownlist for customer
                            string addressQuery = "SELECT * FROM Address WHERE CustomerId = @custId And isDeleted = 0";
                            SqlCommand addressCmd = new SqlCommand(addressQuery, conn);
                            addressCmd.Parameters.AddWithValue("@custId", custID);

                            DataTable data = new DataTable();
                            data.Load(addressCmd.ExecuteReader());

                            if (data.Rows.Count != 0)
                            {
                                txt_edit_address_name.Text = data.Rows[0]["AddressName"].ToString();
                                txt_edit_address.Text = data.Rows[0]["AddressLine"].ToString();
                                txt_edit_country.Text = data.Rows[0]["Country"].ToString();
                                txt_edit_state.Text = data.Rows[0]["State"].ToString();
                                txt_edit_postal_code.Text = data.Rows[0]["PostalCode"].ToString();
                            }

                        }
                        else
                        {
                            //// get address name for dropdownlist for customer
                            string addressQuery = "SELECT * FROM Address WHERE CustomerId = @custId And isDeleted = 0 And AddressName = @addresssName";
                            SqlCommand addressCmd = new SqlCommand(addressQuery, conn);
                            addressCmd.Parameters.AddWithValue("@custId", custID);
                            addressCmd.Parameters.AddWithValue("@addresssName", addressName);

                            DataTable data = new DataTable();
                            data.Load(addressCmd.ExecuteReader());

                            if (data.Rows.Count == 0)
                            {
                                txt_edit_address_name.Text = "-";
                                txt_edit_address.Text = "-";
                                txt_edit_country.Text = "-";
                                txt_edit_state.Text = "-";
                                txt_edit_postal_code.Text = "-";
                            }
                            else
                            {
                                txt_edit_address_name.Text = data.Rows[0]["AddressName"].ToString();
                                txt_edit_address.Text = data.Rows[0]["AddressLine"].ToString();
                                txt_edit_country.Text = data.Rows[0]["Country"].ToString();
                                txt_edit_state.Text = data.Rows[0]["State"].ToString();
                                txt_edit_postal_code.Text = data.Rows[0]["PostalCode"].ToString();

                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }

            }
        }

        protected void btn_save_Click(object sender, EventArgs e)
        {
            //reset all error text
            usernameErrMsg.Text = "";
            fullnameErrMsg.Text = "";
            phoneNumErrMsg.Text = "";
            addressNameErrMsg.Text = "";
            addressLineErrMsg.Text = "";
            countryErrMsg.Text = "";
            stateErrMsg.Text = "";
            postalCodeErrMsg.Text = "";
            lblMessage.Text = "";

            //int custID = int.Parse(Request.Cookies["CustID"].Value);
            int custID = (int)Session["CUSTID"];

            string addressName = Session["AddressName"] as string;

            if (addressName == null || addressName == "")
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                    {
                        conn.Open();
                        //// get address name for dropdownlist for customer
                        string addressQuery = "SELECT * FROM Address WHERE CustomerId = @custId And isDeleted = 0";
                        SqlCommand addressCmd = new SqlCommand(addressQuery, conn);
                        addressCmd.Parameters.AddWithValue("@custId", custID);

                        DataTable data = new DataTable();
                        data.Load(addressCmd.ExecuteReader());

                        if (data.Rows.Count != 0)
                        {
                            addressName = data.Rows[0]["AddressName"].ToString();
                        }
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }

            string username = txt_edit_username.Text;
            string fullname = txt_edit_fullname.Text;
            string phone = txt_edit_phone_number.Text;
            string addressName1 = txt_edit_address_name.Text;
            string addressLine = txt_edit_address.Text;
            string country = txt_edit_country.Text;
            string state = txt_edit_state.Text;
            string postalCode = txt_edit_postal_code.Text;

            string usernameErr = "";
            string fullnameErr = "";
            string phoneNumErr = "";
            string addressNameErr = "";
            string addressLineErr = "";
            string countryErr = "";
            string stateErr = "";
            string postalCodeErr = "";

            //validation
            //username validation
            if (string.IsNullOrEmpty(username))
            {
                usernameErr = "Username cannot be left empty.";
            }

            //fullname validation
            if (string.IsNullOrEmpty(fullname))
            {
                fullnameErr = "Fullname cannot be left empty.";
            }

            //phone validation
            if (!StringUtil.PhoneUtil.IsValidPhoneNumber(phone))
            {
                phoneNumErr = "Invalid Phone Number Format.";
            }

            //Address Line Validation
            if (string.IsNullOrEmpty(addressName1))
            {
                addressNameErr = "Address cannot be left empty.";
            }

            //Address Line Validation
            if (string.IsNullOrEmpty(addressLine))
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
            if (string.IsNullOrEmpty(postalCode))
            {
                postalCodeErr = "Postal Code cannot be left empty.";
            }
            else if (!postalCode.All(char.IsDigit)) // Check if all characters are digits
            {
                postalCodeErr = "Postal Code can only contain numbers.";
            }

            if (usernameErr == "" && fullnameErr == "" && phoneNumErr == "" && addressNameErr == "" && addressLineErr == "" && countryErr == "" && stateErr == "" && postalCodeErr == "")
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                    {
                        conn.Open();

                        string query1 = "UPDATE Customer SET CustomerFullname = @custFullname, CustomerUsername = @custUsername, CustomerPhoneNumber = @custPhone WHERE CustomerId = @custId";
                        SqlCommand cmd1 = new SqlCommand(query1, conn);
                        cmd1.Parameters.AddWithValue("@custFullname", fullname);
                        cmd1.Parameters.AddWithValue("@custUsername", username);
                        cmd1.Parameters.AddWithValue("@custPhone", phone);
                        cmd1.Parameters.AddWithValue("@custId", custID);

                        int rowsAffected1 = cmd1.ExecuteNonQuery();

                        if (rowsAffected1 > 0)
                        {
                            // Profile updated successfully
                            lblMessage.Text = "Profile updated successfully";
                        }
                        else
                        {
                            lblMessage.Text = "Failed to update profile";
                        }

                        conn.Close();

                        conn.Open();
                        string query = "UPDATE Address SET AddressName = @addressName1, AddressLine = @addressLine, Country = @country, State = @state, PostalCode = @postalCode WHERE CustomerId = @custId And AddressName = @addressname And isDeleted = 0";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@addressName1", addressName1);
                        cmd.Parameters.AddWithValue("@addressLine", addressLine);
                        cmd.Parameters.AddWithValue("@country", country);
                        cmd.Parameters.AddWithValue("@state", state);
                        cmd.Parameters.AddWithValue("@postalCode", postalCode);
                        cmd.Parameters.AddWithValue("@custId", custID);
                        cmd.Parameters.AddWithValue("@addressname", addressName);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Profile updated successfully
                            lblMessage.Text += ", Address updated successfully";
                            //for show pop up message
                            Session["ProfileChanged"] = true;
                            Response.Redirect("UserProfile.aspx");
                        }
                        else
                        {
                            lblMessage.Text += ", Failed to update address";
                        }

                        conn.Close();

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
                if (usernameErr != "")
                {
                    usernameErrMsg.Text = usernameErr;
                }

                if (fullnameErr != "")
                {
                    fullnameErrMsg.Text = fullnameErr;
                }

                if (phoneNumErr != "")
                {
                    phoneNumErrMsg.Text = phoneNumErr;
                }

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

                if (postalCodeErr != "")
                {
                    postalCodeErrMsg.Text = postalCodeErr;
                }
            }

        }

        protected void btn_togo_profile_Click(object sender, EventArgs e)
        {
            // Redirect to User Profile page
            Response.Redirect("UserProfile.aspx");
        }

        protected void btn_togo_my_order_Click(object sender, EventArgs e)
        {
            // Redirect to User Profile page
            Response.Redirect("ToShip.aspx");
        }

        protected void btnUploadImage_Click(object sender, EventArgs e)
        {
            if (imgUpload.HasFile)
            {
                try
                {
                    // Get the uploaded file
                    HttpPostedFile uploadedFile = imgUpload.PostedFile;

                    // Define the file name and path
                    string fileName = System.IO.Path.GetFileName(uploadedFile.FileName);

                    // Create a directory for storing images, if not exists
                    string savePath = Server.MapPath("~/images/profile-pics/"); // Ensure this directory exists
                    if (!System.IO.Directory.Exists(savePath))
                    {
                        System.IO.Directory.CreateDirectory(savePath);
                    }

                    // Set the complete file path
                    string filePath = System.IO.Path.Combine(savePath, fileName);

                    // Save the file to the server
                    uploadedFile.SaveAs(filePath);

                    ////
                    // Cache-buster to ensure the image is reloaded
                    string imageUrlWithCacheBuster = "~/images/profile-pics/" + fileName + "?v=" + Guid.NewGuid().ToString();

                    // Update the ProfileImagePath in the database
                    //int custID = int.Parse(Request.Cookies["CustID"].Value);
                    int custID = (int)Session["CUSTID"];

                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                    {
                        conn.Open();
                        string updateQuery = "UPDATE Customer SET ProfileImagePath = @filePath WHERE CustomerId = @custId";
                        SqlCommand cmd = new SqlCommand(updateQuery, conn);
                        cmd.Parameters.AddWithValue("@filePath", "~/images/profile-pics/" + fileName); // Use a relative path for ASP.NET
                        cmd.Parameters.AddWithValue("@custId", custID);

                        cmd.ExecuteNonQuery(); // Execute the update
                        conn.Close();
                    }
                    // Update the image to show the new uploaded one
                    img_profile.ImageUrl = imageUrlWithCacheBuster;

                    lblMessage.Text = "Image uploaded and profile updated successfully.";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error uploading image: " + ex.Message;
                }
            }
            else
            {
                lblMessage.Text = "Please select an image to upload.";
            }
        }

        protected void btn_rw_his_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/src/Client/UserProfile/UserReivew.aspx");

        }

        protected void btn_rw_his_Click1(object sender, EventArgs e)
        {
            Response.Redirect("~/src/Client/UserProfile/UserReivew.aspx");
        }
    }
}
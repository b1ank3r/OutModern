using OutModern.src.Admin.Customers;
using OutModern.src.Admin.PromoCode;
using OutModern.src.Client.Cart;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace OutModern.src.Client.Shipping
{
    public class Address
    {
        public int AddressId { get; set; }
        public int CustomerId { get; set; }
        public string AddressName { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }

    public partial class Shipping : System.Web.UI.Page
    {

        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private int customerId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CUSTID"] != null)
            {
                customerId = (int)Session["CUSTID"];
                if (IsCartEmpty(customerId))
                {
                    Response.Redirect("~/src/Client/Cart/Cart.aspx");
                }
            }
            else
            {
                Response.Redirect("~/src/Client/Login/Login.aspx");
            }

            if (!IsPostBack)
            {
                Session["SelectedAddress"] = null;
            }

            BindCartItems();
            BindAddresses();
            UpdateSubtotalandGrandTotalLabel();
        }

        private bool IsCartEmpty(int custId)
        {
            bool cartIsEmpty = true;

            // Check if the cart has any items
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM CartItem WHERE CartId = (SELECT CartId FROM Cart WHERE CustomerId = @CustomerId)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CustomerId", custId);

                con.Open();
                int itemCount = (int)cmd.ExecuteScalar();
                con.Close();

                // If item count is greater than 0, cart is not empty
                if (itemCount > 0)
                {
                    cartIsEmpty = false;
                }
            }

            return cartIsEmpty;
        }

        private void BindAddresses()
        {
            // Get the list of addresses for the current customer
            List<Address> addresses = GetAddresses();

            // Bind the addresses to the ListView control
            AddressListView.DataSource = addresses;
            AddressListView.DataBind();
        }

        private void BindCartItems()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT CI.ProductDetailId, CI.Quantity, PD.ColorId, PD.SizeId, PD.ProductId, " +
                               "P.ProductName, P.UnitPrice, (CI.Quantity * P.UnitPrice) AS Subtotal, " +
                               "(SELECT TOP 1 PI.Path FROM ProductImage PI WHERE PI.ProductDetailId = PD.ProductDetailId) AS ProductImageUrl, " +
                               "S.SizeName, C.ColorName " +
                               "FROM CartItem CI " +
                               "INNER JOIN ProductDetail PD ON CI.ProductDetailId = PD.ProductDetailId " +
                               "INNER JOIN Product P ON PD.ProductId = P.ProductId " +
                               "INNER JOIN Size S ON PD.SizeId = S.SizeId " +
                               "INNER JOIN Color C ON PD.ColorId = C.ColorId " +
                               "WHERE CI.CartId = (SELECT CartId FROM Cart WHERE CustomerId = @CustomerId)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                con.Open();
                da.Fill(dt);
                con.Close();

                ProductListView.DataSource = dt;
                ProductListView.DataBind();
            }
        }

        private void UpdateSubtotalandGrandTotalLabel()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT Subtotal FROM Cart WHERE CustomerId = @CustomerId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                con.Open();
                object result = cmd.ExecuteScalar();
                con.Close();

                // Check if the result is not null
                if (result != null)
                {
                    decimal discountAmount = 0;
                    decimal subtotal = Convert.ToDecimal(result);
                    lblItemPrice.Text = $"RM{subtotal.ToString("N2")}";

                    decimal deliveryCost = decimal.Parse(lblDeliveryCost.Text.Replace("RM", ""));

                    PromoTable promoCode = Session["PromoCode"] as PromoTable;

                    if (promoCode != null)
                    {
                        // Update the UI with the discount rate
                        lblDiscountRate.Text = $"({promoCode.DiscountRate}%)";

                        // Calculate the discount amount
                        discountAmount = subtotal * ((decimal)promoCode.DiscountRate / 100);

                        // Update the UI with the discount amount
                        lblDiscount.Text = $"RM{discountAmount.ToString("N2")}";
                    }
                    else
                    {
                        lblDiscountRate.Text = "(- 0%)";
                        lblDiscount.Text = "RM0.00";
                    }

                    decimal grandTotal = subtotal + deliveryCost - discountAmount;

                    lblTotal.Text = $"RM{grandTotal.ToString("N2")}";
                }
                else
                {
                    lblTotal.Text = "RM0.00";
                }
            }
        }


        private List<Address> GetAddresses()
        {
            List<Address> addresses = new List<Address>();

            // Establish connection to your database (assuming SQL Server)


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Write SQL query to select addresses based on customer ID
                string query = "SELECT * FROM Address WHERE CustomerId = @CustomerId AND isDeleted = 0";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameter for the customer ID
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Create a new Address object for each record and add it to the list
                            Address address = new Address
                            {
                                AddressId = Convert.ToInt32(reader["AddressId"]),
                                CustomerId = Convert.ToInt32(reader["CustomerId"]),
                                AddressName = reader["AddressName"].ToString(),
                                AddressLine = reader["AddressLine"].ToString(),
                                Country = reader["Country"].ToString(),
                                State = reader["State"].ToString(),
                                PostalCode = reader["PostalCode"].ToString()
                            };

                            addresses.Add(address);
                        }
                    }
                }
            }

            // Return the list of addresses
            return addresses;
        }


        //REMEMBER CHANGE CUSTOMER ID 
        protected void btnAddAddress_Click(object sender, EventArgs e)
        {

            if (ValidateAndHighlight())
            {
                // If all textboxes are filled, proceed to add the address to the database
                Address address = new Address
                {
                    CustomerId = customerId, // Replace with the actual customer ID
                    AddressName = txtNickname.Text,
                    AddressLine = txtAddr.Text,
                    Country = ddlCountryOrigin.SelectedItem.Text,
                    State = txtState.Text,
                    PostalCode = txtPostal.Text
                };

                txtNickname.Text = "";
                txtAddr.Text = "";
                txtState.Text = "";
                txtPostal.Text = "";
                ddlCountryOrigin.SelectedIndex = 0;

                // Add the address to the database
                AddAddressToDatabase(address);

                BindAddresses();

                string script = $"window.onload = function() {{ showSnackbar(); }};";
                lblSnackbar.Text = "Addess added successfully!";
                ScriptManager.RegisterStartupScript(this, GetType(), "showSnackbar", script, true);
            }

        }

        private void AddAddressToDatabase(Address address)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string insertQuery = @"INSERT INTO [dbo].[Address] (CustomerId, AddressName, AddressLine, Country, State, PostalCode) 
                        VALUES (@CustomerId, @AddressName, @AddressLine, @Country, @State, @PostalCode)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", address.CustomerId);
                    command.Parameters.AddWithValue("@AddressName", address.AddressName);
                    command.Parameters.AddWithValue("@AddressLine", address.AddressLine);
                    command.Parameters.AddWithValue("@Country", address.Country);
                    command.Parameters.AddWithValue("@State", address.State);
                    command.Parameters.AddWithValue("@PostalCode", address.PostalCode);

                    command.ExecuteNonQuery();
                }

            }

        }

        protected void btnProceed_Click(object sender, EventArgs e)
        {
            // Retrieve the selected address details from session variables
            Address selectedAddress = Session["SelectedAddress"] as Address;



            // Check if an address is selected
            if (selectedAddress != null)
            {
                // Store the selected address in a session variable
                Session["SelectedAddressPayment"] = selectedAddress;
                Session["ShippingToPayment"] = true;

                // Redirect to the Payment.aspx page
                Response.Redirect("../Payment/Payment.aspx");
            }
            else
            {
                // Display a message if no address is selected
                lblMessage.Visible = true;
            }

        }

        protected void AddressListView_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                lblMessage.Visible = false;

                ViewState["selectedItem"] = Convert.ToInt32(e.CommandArgument);  // Store the selected index

                // Retrieve the index of the selected item
                int index = Convert.ToInt32(e.CommandArgument);

                // Retrieve the primary key of the selected item
                int addressId = Convert.ToInt32(AddressListView.DataKeys[index].Value);

                // Access the selected data item using the primary key
                Address selectedAddress = GetAddressById(addressId);

                // Save the selected address details to session
                Session["SelectedAddress"] = selectedAddress;

                BindAddresses();

            }
        }

        protected void AddressListView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var dataItem = e.Item as ListViewDataItem;

                // Check if the current row is the selected row
                if (ViewState["selectedItem"] != null && ViewState["selectedItem"].ToString() == e.Item.DataItemIndex.ToString())
                {
                    // Find the LinkButton in the current item
                    LinkButton linkButton = e.Item.FindControl("LinkButton1") as LinkButton;
                    if (linkButton != null)
                    {
                        // Apply the highlight CSS class or any other desired styling
                        linkButton.Style["box-shadow"] += "0 0 10px #000000;";
                    }
                }
            }
        }


        private Address GetAddressById(int addressId)
        {
            Address address = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Write SQL query to select address details based on addressId
                string query = "SELECT * FROM Address WHERE AddressId = @AddressId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameter for the addressId
                    command.Parameters.AddWithValue("@AddressId", addressId);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Create a new Address object with the retrieved details
                            address = new Address
                            {
                                AddressId = Convert.ToInt32(reader["AddressId"]),
                                CustomerId = Convert.ToInt32(reader["CustomerId"]),
                                AddressName = reader["AddressName"].ToString(),
                                AddressLine = reader["AddressLine"].ToString(),
                                Country = reader["Country"].ToString(),
                                State = reader["State"].ToString(),
                                PostalCode = reader["PostalCode"].ToString()
                            };
                        }
                    }
                }
            }

            return address;
        }

        protected void AddressListView_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {

        }
        protected void AddressListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected bool ValidateAndHighlight()
        {
            try
            {
                string nickname = txtNickname.Text.Trim();
                string addr = txtAddr.Text.Trim();
                string postal = txtPostal.Text.Trim();
                string state = txtState.Text.Trim();
                string country = ddlCountryOrigin.SelectedValue;

                List<Control> errorControls = new List<Control>();

                if (string.IsNullOrEmpty(nickname))
                {
                    errorControls.Add(txtNickname);
                    lblNameError.Text = "Nickname cannot be left empty.";
                }
                else
                {
                    RemoveErrorBorder(txtNickname);
                    lblNameError.Text = "";
                }

                if (string.IsNullOrEmpty(addr))
                {
                    errorControls.Add(txtAddr);
                    lblAddrError.Text = "Address cannot be left empty.";
                }
                else
                {
                    RemoveErrorBorder(txtAddr);
                    lblAddrError.Text = "";
                }

                if (string.IsNullOrEmpty(postal))
                {
                    errorControls.Add(txtPostal);
                    lblPostalError.Text = "Postal cannot be left empty.";
                }
                else
                {
                    RemoveErrorBorder(txtPostal);
                    lblPostalError.Text = "";
                }

                if (string.IsNullOrEmpty(state))
                {
                    errorControls.Add(txtState);
                    lblStateError.Text = "State cannot be left empty.";
                }
                else
                {
                    RemoveErrorBorder(txtState);
                    lblStateError.Text = "";
                }

                if (country == "default")
                {
                    errorControls.Add(ddlCountryOrigin);
                    lblCountryError.Text = "Please select a country.";
                }
                else
                {
                    RemoveErrorBorder(ddlCountryOrigin);
                    lblCountryError.Text = "";
                }

                // Apply red border only to controls with errors
                foreach (Control control in errorControls)
                {
                    if (control is WebControl webControl)
                    {
                        webControl.CssClass += " error-border";
                    }
                }

                return errorControls.Count == 0; // Return true if no errors, false otherwise
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in ValidateAndHighlight: " + ex.Message);
                return false;
            }
        }

        protected void RemoveErrorBorder(WebControl control)
        {
            // Remove error border CSS class from the control
            control.CssClass = control.CssClass.Replace("error-border", "").Trim();
        }




    }

}
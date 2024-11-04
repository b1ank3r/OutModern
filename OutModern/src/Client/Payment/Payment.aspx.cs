using OutModern.src.Admin.Customers;
using OutModern.src.Admin.PromoCode;
using OutModern.src.Client.Cart;
using OutModern.src.Client.Shipping;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Client.Payment
{
    public class PaymentInfo
    {
        public string PaymentMethod { get; set; }
        public string CardNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string CVV { get; set; }
    }

    public partial class Payment : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private int customerId; // REMEMBER TO CHANGE ID

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (!(bool)Session["ShippingToPayment"] || Session["ShippingToPayment"] == null)
                    {
                        Response.Redirect("~/src/Client/Cart/Cart.aspx");
                    }
                }
                catch (NullReferenceException)
                {
                    // Handle the NullReferenceException by redirecting back to the cart page
                    Response.Redirect("~/src/Client/Cart/Cart.aspx");
                }


                Session["ShippingToPayment"] = false;

                // Check if the selected address is stored in the session
                if (Session["SelectedAddressPayment"] != null)
                {
                    // Retrieve the selected address from the session
                    Address selectedAddress = Session["SelectedAddressPayment"] as Address;
                    Session["SelectedAddressPayment2"] = Session["SelectedAddressPayment"];

                    Session.Remove("SelectedAddressPayment");

                }

            }


            if (Session["CUSTID"] != null)
            {
                customerId = (int)Session["CUSTID"];
            }
            else
            {
                Response.Redirect("~/src/Client/Login/Login.aspx");
            }

            BindCartItems();
            UpdateSubtotalandGrandTotalLabel();
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session.IsNewSession)
            {

                Session.Remove("PromoCode2");
            }


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
                    // If subtotal is null, display 0.00
                    lblTotal.Text = "RM0.00";
                }
            }
        }

        protected void UpdateDiscount(PromoTable promoCode)
        {

            if (promoCode != null)
            {

                // Update the UI with the discount rate
                lblDiscountRate.Text = $"({promoCode.DiscountRate}%)";

                decimal subtotal = GetCartSubtotal(customerId);
                // Calculate the discount amount
                decimal discountAmount = subtotal * ((decimal)promoCode.DiscountRate / 100);

                // Update the UI with the discount amount
                lblDiscount.Text = $"RM{discountAmount.ToString("N2")}";
            }
            else
            {
                lblDiscountRate.Text = "(Invalid code)";
                // lblDiscount.Text = "RM0.00";
            }

        }

        private decimal GetCartSubtotal(int customerId)
        {
            decimal subtotal = 0;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT Subtotal FROM Cart WHERE CustomerId = @CustomerId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                con.Open();
                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    subtotal = Convert.ToDecimal(result);
                }

                con.Close();
            }

            return subtotal;
        }

        private decimal GetTotal()
        {
            decimal grandTotal = decimal.Parse(lblTotal.Text.Replace("RM", ""));
            Session["GrandTotal"] = grandTotal;

            return grandTotal;
        }

        protected void btnSubmitOrder_Click(object sender, EventArgs e)
        {
            string paymentMethod;

            if (paypal.Checked)
            {

                paymentMethod = "Paypal";
                // total;
                decimal totalAmount = GetTotal();

                //payment request
                string returnURL = "http://localhost:44338/src/Client/Payment/Success.aspx?status=success";
                string cancelURL = "http://localhost:44338/src/Client/Cart/Cart.aspx";
                string currency = "MYR";
                string paypalSandboxEmail = "sb-olaow30210165@business.example.com";

                string paypalSandboxURL = $"https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick&business={paypalSandboxEmail}&currency_code={currency}&amount={totalAmount}&return={returnURL}&cancel_return={cancelURL}";

                PaymentInfo creditCardInfo = new PaymentInfo
                {
                    PaymentMethod = paymentMethod,
                    CardNumber = null,
                    ExpirationDate = null,
                    CVV = null
                };
                Session["PaymentInfo"] = creditCardInfo;
                Session["PaymentToSuccess"] = true;

                // Redirect the user to PayPal
                Response.Redirect(paypalSandboxURL);

            }
            else
            {
                //creditcard

                // Validate and highlight inputs
                ValidateAndHighlightInputs();

                // Check if any input has validation errors
                if (HasValidationErrors())
                {
                    return; // Prevent further processing if there are validation errors
                }

                decimal totalAmount = GetTotal();

                // Retrieve credit card information from form fields
                paymentMethod = "Credit Card";
                string cardNumber = txtCardNumber.Text.Trim();
                string expirationDateText = txtExpirationDate.Text.Trim();
                string cvv = txtCvv.Text.Trim();



                DateTime expirationDate = DateTime.ParseExact(expirationDateText, "MM/yy", CultureInfo.InvariantCulture);

                // Create an instance of CreditCardInfo and store the card information
                PaymentInfo creditCardInfo = new PaymentInfo
                {
                    PaymentMethod = paymentMethod,
                    CardNumber = cardNumber,
                    ExpirationDate = expirationDate,
                    CVV = cvv
                };

                // Store credit card information into session variable
                Session["PaymentInfo"] = creditCardInfo;
                Session["PaymentToSuccess"] = true;
                // Redirect or perform any other action after storing the payment information
                Response.Redirect("../Payment/Success.aspx?status=success");

            }
        }

        protected void PaymentMethod_CheckedChanged(object sender, EventArgs e)
        {

            lblCreditCardContainer.CssClass = lblCreditCardContainer.CssClass.Replace(" selected", "");
            lblPaypalContainer.CssClass = lblPaypalContainer.CssClass.Replace(" selected", "");

            // Handle the CheckedChanged event as before
            if (creditCard.Checked)
            {
                // Show credit card payment details
                paymentDetails.Style["display"] = "block";
                lblCreditCardContainer.CssClass += " selected";
            }
            else
            {
                // Hide payment details for Paypal
                paymentDetails.Style["display"] = "none";
                lblPaypalContainer.CssClass += " selected";
            }
        }


        private void ValidateAndHighlightInputs()
        {
            // Validate card number input
            if (string.IsNullOrWhiteSpace(txtCardNumber.Text) || !ValidateCreditCardNumber(txtCardNumber.Text))
            {
                txtCardNumber.Attributes["style"] = "border-color: red;";
                lblCreditError.Text = "Please enter Credit Card number in correct format!";
            }
            else
            {
                txtCardNumber.Attributes["style"] = "";
                lblCreditError.Text = "";
            }

            // Validate expiration date input
            if (string.IsNullOrWhiteSpace(txtExpirationDate.Text) || txtExpirationDate.Text.Length < 5)
            {
                txtExpirationDate.Attributes["style"] = "border-color: red;";
                lblDateError.Text = "Please enter Expiration Date in correct format.";
            }
            else
            {
                txtExpirationDate.Attributes["style"] = "";
                lblDateError.Text = "";
            }

            // Validate CVV input
            if (string.IsNullOrWhiteSpace(txtCvv.Text) || txtCvv.Text.Length < 3)
            {
                txtCvv.Attributes["style"] = "border-color: red;";
                lblCvvError.Text = "Please Enter CVV in correct format.";
            }
            else
            {
                txtCvv.Attributes["style"] = "";
                lblCvvError.Text = "";
            }
        }

        private bool ValidateCreditCardNumber(string cardNumber)
        {
            // Remove non-numeric characters
            cardNumber = Regex.Replace(cardNumber, @"\D", "");

            // Validate credit card number using Luhn algorithm
            int sum = 0;
            bool doubleDigit = false;

            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int digit = int.Parse(cardNumber[i].ToString());

                if (doubleDigit)
                {
                    digit *= 2;
                    if (digit > 9)
                    {
                        digit -= 9;
                    }
                }

                sum += digit;
                doubleDigit = !doubleDigit;
            }

            bool isValid = (sum % 10) == 0;

            return isValid;
        }


        private bool HasValidationErrors()
        {
            // Check if any input has validation errors
            return string.IsNullOrWhiteSpace(txtCardNumber.Text) || !ValidateCreditCardNumber(txtCardNumber.Text)
                || (string.IsNullOrWhiteSpace(txtExpirationDate.Text) || txtExpirationDate.Text.Length < 5)
                || (string.IsNullOrWhiteSpace(txtCvv.Text) || txtCvv.Text.Length < 3);
        }

    }
}
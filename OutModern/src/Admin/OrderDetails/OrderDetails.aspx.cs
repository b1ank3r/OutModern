using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Admin.OrderDetails
{
    public partial class OrderDetails : System.Web.UI.Page
    {

        protected static readonly string OrderEdit = "OrderEdit";
        protected static readonly string CustomerDetails = "CustomerDetails";


        // Side menu urls
        protected Dictionary<string, string> urls = new Dictionary<string, string>()
        {
            { OrderEdit , "~/src/Admin/OrderEdit/OrderEdit.aspx" },
            { CustomerDetails , "~/src/Admin/CustomerDetails/CustomerDetails.aspx" }
        };

        private string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private string orderId;

        protected void Page_Load(object sender, EventArgs e)
        {
            orderId = Request.QueryString["OrderId"];
            if (orderId == null)
            {
                Response.Redirect("~/src/ErrorPages/404.aspx");
            }
            if (!IsPostBack)
            {
                Session["MenuCategory"] = "Orders";

                initPageData();

                Page.DataBind();
            }
        }

        private void initPageData()
        {
            // All Product ordered
            DataTable productOrdered = getProductOrdered();
            if (productOrdered.Rows.Count == 0)
            {
                Response.Redirect("~/src/ErrorPages/404.aspx");
            }
            lvProductOrder.DataSource = productOrdered;
            lvProductOrder.DataBind();

            double subTotal = 0; // Calculate subtotal
            foreach (DataRow row in productOrdered.Rows)
            {
                subTotal += Convert.ToDouble(row["Subtotal"]);
            }
            Label lblSubTotal = (Label)lvProductOrder.FindControl("lblSubTotal");
            lblSubTotal.Text = subTotal.ToString("0.00");

            Label lblDeliveryFee = (Label)lvProductOrder.FindControl("lblDeliveryFee");
            lblDeliveryFee.Text = "5.00"; //hardcoded delivery fee

            // Payment info
            DataTable paymentInfo = getPaymentInfo();
            if (paymentInfo.Rows.Count > 0)
            {
                lblPaymentMethod.Text = paymentInfo.Rows[0]["PaymentMethodName"].ToString();
                lblPaymentDate.Text = paymentInfo.Rows[0]["PaymentDateTime"].ToString();

                Label lblTotal = (Label)lvProductOrder.FindControl("lblTotal");
                lblTotal.Text = paymentInfo.Rows[0]["Total"].ToString();

                Label lblPromoCode = (Label)lvProductOrder.FindControl("lblPromoCode");
                Label lblDiscount = (Label)lvProductOrder.FindControl("lblDiscount");

                if (paymentInfo.Rows[0].IsNull("PromoCode"))
                {
                    lblPromoCode.Text = "none";
                    lblDiscount.Text = "0";
                }
                else
                {
                    lblPromoCode.Text = paymentInfo.Rows[0]["PromoCode"].ToString();
                    lblDiscount.Text = paymentInfo.Rows[0]["DiscountRate"].ToString();

                    //calculate total discount
                    double discount = double.Parse(paymentInfo.Rows[0]["DiscountRate"].ToString()) / 100;
                    double totalDiscount = subTotal * discount;

                    Label lblDiscountPrice = (Label)lvProductOrder.FindControl("lblDiscountPrice");
                    lblDiscountPrice.Text = totalDiscount.ToString("0.00");
                }
            }

            // Order info
            lblOrderID.Text = orderId;
            DataTable orderDetail = getOrderDetail();
            if (orderDetail.Rows.Count > 0)
            {
                string orderStatus = orderDetail.Rows[0]["OrderStatusName"].ToString();
                lblOrderDate.Text = orderDetail.Rows[0]["OrderDateTime"].ToString();
                lblOrderStatus.Text = orderStatus;

                // show each button based on order status
                if (orderStatus == "Received")
                {
                    // show no button
                    btnCancel.Visible = false;
                    btnReturnToPlaced.Visible = false;
                    btnShipped.Visible = false;
                }
                else if (orderStatus == "Order Placed")
                {
                    //show shipped and cancel button
                    btnCancel.Visible = true;
                    btnReturnToPlaced.Visible = false;
                    btnShipped.Visible = true;
                }
                else if (orderStatus == "Shipped" || orderStatus == "Cancelled")
                {
                    //show return to placed button
                    btnCancel.Visible = false;
                    btnReturnToPlaced.Visible = true;
                    btnShipped.Visible = false;
                }

            }

            // Customer info
            DataTable customerInfo = getCustomerInfo();
            if (customerInfo.Rows.Count > 0)
            {
                lblCusName.Text = customerInfo.Rows[0]["CustomerFullName"].ToString();
                lblCusEmail.Text = customerInfo.Rows[0]["CustomerEmail"].ToString();
                lblCusPhoneNo.Text = customerInfo.Rows[0]["CustomerPhoneNumber"].ToString();
                hlCustomerDetail.NavigateUrl = urls[CustomerDetails] + "?CustomerId=" + customerInfo.Rows[0]["CustomerId"].ToString();

                lblAddressLine.Text = customerInfo.Rows[0]["AddressLine"].ToString();
                lblPostalCode.Text = customerInfo.Rows[0]["Country"].ToString();
                lblState.Text = customerInfo.Rows[0]["State"].ToString();
                lblCountry.Text = customerInfo.Rows[0]["PostalCode"].ToString();
            }

        }


        //
        // db operation
        //

        // get all products ordered
        private DataTable getProductOrdered()
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlQuery =
                    "Select ProductDetail.ProductDetailId, Product.ProductName, Size.SizeName, " +
                    "Color.ColorName, Product.UnitPrice, OrderItem.Quantity, " +
                    "UnitPrice*OrderItem.Quantity as Subtotal " +
                    "FROM [Order], OrderItem , ProductDetail, Size, Color, Product " +
                    "WHERE [Order].OrderId = OrderItem.OrderId " +
                    "AND OrderItem.ProductDetailId = ProductDetail.ProductDetailId " +
                    "AND Color.ColorId = ProductDetail.ColorId " +
                    "AND Size.SizeId = ProductDetail.SizeId " +
                    "AND Product.ProductId = ProductDetail.ProductId " +
                    "AND [Order].OrderId = @orderId;";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@orderId", orderId);
                    data.Load(command.ExecuteReader());
                }
            }

            data.Columns.Add("Path", typeof(string));
            foreach (DataRow row in data.Rows)
            {
                row["Path"] = getProductDetailImagePath(row["ProductDetailId"].ToString());
            }

            return data;
        }

        // get a image path of a product detail
        private string getProductDetailImagePath(string productDetailId)
        {
            string data = "";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlQuery =
                    "Select Top 1 [path] " +
                    "FROM ProductImage " +
                    "WHERE ProductDetailId = @productDetailId;";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@productDetailId", productDetailId);
                    data = command.ExecuteScalar()?.ToString();

                }
            }
            return data == null ? "" : data;

        }

        // get payment info of the order
        private DataTable getPaymentInfo()
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlQuery =
                    "Select PaymentMethod.PaymentMethodName, [Order].PaymentDateTime,[Order].Total, PromoCode.PromoCode, PromoCode.DiscountRate " +
                    "FROM [Order] Left Join PromoCode On [Order].PromoId = PromoCode.PromoId, " +
                    "PaymentMethod " +
                    "WHERE [Order].PaymentMethodId = PaymentMethod.PaymentMethodId " +
                    "AND [Order].OrderId = @orderId;";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@orderId", orderId);
                    data.Load(command.ExecuteReader());
                }
            }
            return data;
        }

        // get order detail
        private DataTable getOrderDetail()
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlQuery =
                    "Select OrderDateTime, OrderStatusName " +
                    "FROM [Order], OrderStatus " +
                    "WHERE [Order].OrderStatusId = OrderStatus.OrderStatusId " +
                    "AND [Order].OrderId = @orderId;";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@orderId", orderId);
                    data.Load(command.ExecuteReader());
                }
            }
            return data;
        }

        // get customer info
        private DataTable getCustomerInfo()
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlQuery =
                    "Select Customer.CustomerId, CustomerFullName, CustomerEmail, CustomerPhoneNumber, " +
                    "AddressLine, Country, State, PostalCode " +
                    "FROM Customer, [Order], Address " +
                    "WHERE Customer.CustomerId = [Order].CustomerId " +
                    "AND [Order].AddressId = Address.AddressId " +
                    "AND OrderId = @orderId";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@orderId", orderId);
                    data.Load(command.ExecuteReader());
                }
            }

            return data;
        }

        //
        // page event
        //

        protected void lvProductOrder_ItemCommand(object sender, ListViewCommandEventArgs e)
        {

        }

        protected void lvProductOrder_PagePropertiesChanged(object sender, EventArgs e)
        {
            lvProductOrder.DataSource = getProductOrdered();
            lvProductOrder.DataBind();
        }

        protected void lvProductOrder_ItemDataBound(object sender, ListViewItemEventArgs e)
        {

        }

        protected void btnReturnToPlaced_Click(object sender, EventArgs e)
        {
            int affectedRows = updateOrderStatus(orderId, "Order Placed");
            if (affectedRows > 0)
            {
                lblUpdateStatusMsg.Text = "**Updated to \"Order Placed\"";

                //show shipped and cancel button
                btnCancel.Visible = true;
                btnReturnToPlaced.Visible = false;
                btnShipped.Visible = true;

                lblOrderStatus.Text = "Order Placed";
            }
            else
            {
                lblUpdateStatusMsg.Text = "**Failed to update to \"Order Placed\"";
            }
        }

        protected void btnShipped_Click(object sender, EventArgs e)
        {
            int affectedRows = updateOrderStatus(orderId, "Shipped");
            if (affectedRows > 0)
            {
                lblUpdateStatusMsg.Text = "**Updated to \"Shipped\"";

                //show return to placed button
                btnCancel.Visible = false;
                btnReturnToPlaced.Visible = true;
                btnShipped.Visible = false;

                lblOrderStatus.Text = "Shipped";
            }
            else
            {
                lblUpdateStatusMsg.Text = "**Failed to update to \"Shipped\"";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            int affectedRows = updateOrderStatus(orderId, "Cancelled");
            if (affectedRows > 0)
            {
                lblUpdateStatusMsg.Text = "**Updated to \"Cancelled\"";

                //show return to placed button
                btnCancel.Visible = false;
                btnReturnToPlaced.Visible = true;
                btnShipped.Visible = false;

                lblOrderStatus.Text = "Cancelled";
            }
            else
            {
                lblUpdateStatusMsg.Text = "**Failed to update to \"Cancelled\"";
            }
        }


        // update the order status based on the order id
        private int updateOrderStatus(string orderId, string orderStatusName)
        {
            int affectedRows = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery = "UPDATE [Order] " +
                    "SET [Order].OrderStatusId = OrderStatus.OrderStatusId " +
                    "FROM [Order], OrderStatus " +
                    "WHERE OrderId = @orderId AND OrderStatus.OrderStatusName = @orderStatusName";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@orderId", orderId);
                    command.Parameters.AddWithValue("@orderStatusName", orderStatusName);
                    affectedRows = command.ExecuteNonQuery();
                }
            }
            return affectedRows;
        }

    }
}
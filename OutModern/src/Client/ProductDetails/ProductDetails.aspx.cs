using OutModern.src.Admin.Customers;
using OutModern.src.Client.Products;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace OutModern.src.Client.ProductDetails
{
    public partial class ProductDetails : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private int customerId; // REMEMBER TO CHANGE ID
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ProductId"] == null)
            {
                Response.Redirect("~/src/ErrorPages/404.aspx");
            }
            if (Session["CUSTID"] != null)
            {
                customerId = (int)Session["CUSTID"];
            }
            else
            {
                customerId = 0;
            }

            if (!IsPostBack)
            {
                GetProductInfo();
                ColorRepeater.DataBind();
                SizeRepeater.DataBind();
                initColorSize();
                calculateOverallRating();
            }
            bindImageRepeater(GetImages(ViewState["ColorId"].ToString()));
            bindReviews();
        }

        private bool IsButtonEnabled(string colorId)
        {
            string productId = Request.QueryString["ProductId"];
            int quantity = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT SUM(Quantity) AS TotalQuantity " +
                                  "FROM ProductDetail " +
                                  "WHERE ColorId = @colorId AND ProductId = @productId";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@colorId", colorId);
                    command.Parameters.AddWithValue("@productId", productId);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            quantity = (int)reader["TotalQuantity"];
                        }
                    }
                }
            }
            return quantity == 0;
        }

        private DataTable getReviewList()
        {
            string productId = Request.QueryString["ProductId"];
            DataTable data = new DataTable();
            string sortingCriteria = ViewState["SortingCriteria"] as string;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlQuery =
                     "Select ReviewId, ProfileImagePath as CustomerPicture, CustomerFullname as CustomerName, ReviewDateTime as ReviewTime, Rating as ReviewRating, ColorName, SizeName, Quantity as ReviewQuantity, ReviewDescription as ReviewText " +
                     "From Review r, Customer c, ProductDetail pd, Product p, Color co, Size s " +
                     "Where r.CustomerId = c.CustomerId AND pd.ProductDetailId = r.ProductDetailId " +
                     "AND pd.ColorId = co.ColorId AND pd.ProductId = p.ProductId AND s.SizeId = pd.SizeId " +
                     "AND p.ProductId = @productId ";

                if (sortingCriteria == "topRated")
                {
                    sqlQuery += " ORDER BY ReviewRating DESC, ReviewTime DESC";
                }
                else
                {
                    sqlQuery += " ORDER BY ReviewTime DESC";
                }

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);
                    data.Load(command.ExecuteReader());
                }
            }
            data.Columns.Add("Replies", typeof(DataTable));
            foreach (DataRow row in data.Rows)
            {
                row["Replies"] = getReviewReplies(row["ReviewId"].ToString());
            }
            return data;
        }

        private DataTable getReviewReplies(string reviewId)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlQuery =
                    "Select Reply as ReplyText, DateTime as ReplyTime " +
                    "From  ReviewReply rr, Review r " +
                    "Where r.ReviewId = rr.ReviewId " +
                    "AND r.ReviewId = @reviewId " +
                    "ORDER BY ReplyTime DESC;";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("reviewId", reviewId);
                    data.Load(command.ExecuteReader());
                }
            }
            return data;
        }

        private void calculateOverallRating()
        {
            DataTable reviewsTable = getReviewList();
            int totalReview = 0;
            double totalRating = 0;
            double avgRating = 0.0;
            int oneStar = 0, twoStar = 0, threeStar = 0, fourStar = 0, fiveStar = 0;
            double onePer = 0, twoPer = 0, threePer = 0, fourPer = 0, fivePer = 0;
            foreach (DataRow row in reviewsTable.Rows)
            {
                double rating;
                if (double.TryParse(row["ReviewRating"].ToString(), out rating))
                {
                    totalRating += rating;
                    totalReview++;
                    switch (rating)
                    {
                        case 1.0:
                            oneStar++;
                            break;
                        case 2.0:
                            twoStar++;
                            break;
                        case 3.0:
                            threeStar++;
                            break;
                        case 4.0:
                            fourStar++;
                            break;
                        case 5.0:
                            fiveStar++;
                            break;
                    }
                    avgRating = totalRating / (double)totalReview;
                    onePer = oneStar / (double)totalReview * 100;
                    twoPer = twoStar / (double)totalReview * 100;
                    threePer = threeStar / (double)totalReview * 100;
                    fourPer = fourStar / (double)totalReview * 100;
                    fivePer = fiveStar / (double)totalReview * 100;
                }
            }
            if (totalReview > 0)
            {
                avgRating = totalRating / totalReview;
            }
            starBar1.Attributes["style"] = "width: calc((" + onePer + ") / 100 * 100%)";
            starBar2.Attributes["style"] = "width: calc((" + twoPer + ") / 100 * 100%)";
            starBar3.Attributes["style"] = "width: calc((" + threePer + ") / 100 * 100%)";
            starBar4.Attributes["style"] = "width: calc((" + fourPer + ") / 100 * 100%)";
            starBar5.Attributes["style"] = "width: calc((" + fivePer + ") / 100 * 100%)";
            lbl1star.Text = onePer.ToString("F0");
            lbl2star.Text = twoPer.ToString("F0");
            lbl3star.Text = threePer.ToString("F0");
            lbl4star.Text = fourPer.ToString("F0");
            lbl5star.Text = fivePer.ToString("F0");
            avgRating = Math.Round(avgRating, 1);
            lblAvgRatings.Text = avgRating.ToString("F1");
            lblReviews.Text = "(" + totalReview.ToString() + " Reviews)";
            lblTotalReview.Text = totalReview.ToString();
            ratingStar2.InnerHtml = GenerateStars(avgRating);
            ratingStars.InnerHtml = GenerateStars(avgRating);
        }

        private void initColorSize()
        {
            bool quantityGreaterThanZero = false;
            foreach (RepeaterItem colorItem in ColorRepeater.Items)
            {
                LinkButton colorBtn = colorItem.FindControl("lbtnColor") as LinkButton;
                string colorId = colorBtn.Attributes["data-colorId"];
                ViewState["ColorId"] = colorId;
                foreach (RepeaterItem sizeItem in SizeRepeater.Items)
                {
                    LinkButton sizeBtn = sizeItem.FindControl("lbtnSize") as LinkButton;
                    string sizeId = sizeBtn.Attributes["data-sizeId"];
                    ViewState["SizeId"] = sizeId;
                    if (GetQuantity() > 0)
                    {
                        quantityGreaterThanZero = true;
                        lblSize.Visible = true;
                        lblSize.Text = sizeBtn.Attributes["value"].ToString();
                        break;
                    }
                }
                if (quantityGreaterThanZero)
                {
                    lblColor.Visible = true;
                    lblColor.Text = colorBtn.Attributes["value"].ToString();
                    GetImages(colorId);
                    break;
                }
            }  
        }

        protected void lvReviews_PagePropertiesChanged(object sender, EventArgs e)
        {
            bindReviews();
        }

        protected string GenerateStars(double rating)
        {
            int fullStars = (int)rating;
            double remainder = rating - fullStars;
            int grayStars = 5 - fullStars - (remainder >= 0.5 ? 1 : 0);
            StringBuilder stars = new StringBuilder();
            for (int i = 0; i < fullStars; i++)
            {
                stars.Append("<i class='fas fa-star text-yellow-400 text-lg'></i>");
            }
            if (remainder >= 0.5)
            {
                stars.Append("<i class='fas fa-star-half-alt text-yellow-400 text-lg'></i>");
            }
            for (int i = 0; i < grayStars; i++)
            {
                stars.Append("<i class='far fa-star text-gray-400 text-lg'></i>");
            }
            return stars.ToString();
        }

        private int GetQuantity()
        {
            int quantity = 0;
            string productId = Request.QueryString["ProductId"];
            string colorId = ViewState["ColorId"] as string;
            string sizeId = ViewState["SizeId"] as string;
            if (colorId != null && sizeId != null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = "SELECT Quantity AS TotalQuantity FROM ProductDetail WHERE ProductId = @productId AND SizeId = @sizeId AND ColorId = @colorId";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@productId", productId);  // Replace with your ProductId
                        command.Parameters.AddWithValue("@sizeId", sizeId);
                        command.Parameters.AddWithValue("@colorId", colorId);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader["TotalQuantity"] != DBNull.Value)
                                {
                                    lblQuantity.Text = reader["TotalQuantity"].ToString();
                                    quantity = Convert.ToInt32(reader["TotalQuantity"]);
                                    txtQuantity.Attributes["Max"] = quantity.ToString();
                                }
                            }
                        }
                    }
                }
            }
            return quantity;
        }

        protected List<string> GetImages(string colorId)
        {
            string productId = Request.QueryString["ProductId"];
            List<string> imageUrls = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string sqlQuery = "SELECT DISTINCT Path " +
    "FROM Product " +
    "INNER JOIN ProductDetail ON Product.ProductId = ProductDetail.ProductId " +
    "INNER JOIN ProductImage ON ProductDetail.ProductDetailId = ProductImage.ProductDetailId " +
    "WHERE ProductDetail.ColorId = @colorId AND Product.ProductId = @productId";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@colorId", colorId);  // Replace with your ColorId
                    command.Parameters.AddWithValue("@productId", productId);  // Replace with your ProductId
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string imageUrl = reader["Path"].ToString();
                            imageUrls.Add(imageUrl);
                        }
                    }
                    bindImageRepeater(imageUrls);
                }
            }
            return imageUrls;
        }

        private void bindImageRepeater(List<string> imageUrls)
        {
            ImageRepeater.DataSource = imageUrls.Select(url => new { ImageUrl = url }); ;
            ImageRepeater.DataBind();
            MainImageRepeater.DataSource = imageUrls.Select(url => new { ImageUrl = url });
            MainImageRepeater.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "InitializeSlider", "initializeSlider();", true);
        }

        private void GetProductInfo()
        {
            string productId = Request.QueryString["ProductId"];
            if (productId != null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "Select ProductId, ProductName, ProductDescription, ProductCategory, UnitPrice " +
                    "FROM Product " +
                    "WHERE ProductId = @ProductId;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", productId);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            lblProdName.Text = reader["ProductName"].ToString();
                            lblDescription.Text = reader["ProductDescription"].ToString();
                            lblPrice.Text = "RM " + reader["UnitPrice"].ToString();
                            updateSizeChart(reader["ProductCategory"].ToString());
                        }
                        connection.Close();
                    }
                }
            }
        }

        private void updateSizeChart(string category)
        {
            if(category == "Hoodies")
            {
                ImgSizeChart.ImageUrl = "~/images/product-img/hoodie-size-guide.png";
            } else if (category == "Sweaters")
            {
                ImgSizeChart.ImageUrl = "~/images/product-img/sweater-size-guide.png";
            } else if (category == "Tee Shirts")
            {
                ImgSizeChart.ImageUrl = "~/images/product-img/tee-size-guide.png";
            } else if (category == "Trousers")
            {
                ImgSizeChart.ImageUrl = "~/images/product-img/trouser-size-guide.png";
            } else if (category == "Shorts")
            {
                ImgSizeChart.ImageUrl = "~/images/product-img/short-size-guide.png";
            }
        }

        private void selectColor(string colorId)
        {
            foreach (RepeaterItem item in ColorRepeater.Items)
            {
                LinkButton colorBtn = item.FindControl("lbtnColor") as LinkButton; // Replace with your button ID
                if (colorBtn != null)
                {
                    colorBtn.CssClass.Replace(" selectedColor", "");
                    if (colorBtn.Attributes["data-colorId"] == colorId)
                    {
                        colorBtn.CssClass += " selectedColor";
                        lblColor.Text = colorBtn.Attributes["value"].ToString();
                    }
                }
            }
            GetQuantity();
            GetImages(colorId);
            txtQuantity.Text = "1";
        }

        protected void ColorRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SelectColor")
            {
                string previousColorId = ViewState["ColorId"] as string;
                string newColorId = e.CommandArgument.ToString();
                foreach (RepeaterItem item in ColorRepeater.Items)
                {
                    LinkButton colorBtn = item.FindControl("lbtnColor") as LinkButton;
                    if (colorBtn != null && colorBtn.Attributes["data-colorId"] == previousColorId)
                    {
                        colorBtn.CssClass = colorBtn.CssClass.Replace(" selectedColor", "").Trim();
                        break;
                    }
                }
                ViewState["ColorId"] = newColorId;
                selectColor(newColorId);
                checkQuantity(newColorId);
            }
        }

        private void checkQuantity(string colorId)
        {
            string firstAvailableSizeId = null;
            foreach (RepeaterItem item in SizeRepeater.Items)
            {
                LinkButton sizeBtn = item.FindControl("lbtnSize") as LinkButton;
                string sizeId = sizeBtn.Attributes["data-sizeId"];
                if (IsButtonEnabled(colorId, sizeId))
                {
                    sizeBtn.Enabled = false;
                    sizeBtn.CssClass = " buttonDisabled";
                }
                else
                {
                    sizeBtn.Enabled = true;
                    sizeBtn.CssClass = "flex items-center justify-center rounded-md px-4 py-3 hover:bg-gray-300";
                    if (sizeId == ViewState["SizeId"].ToString() && GetQuantity() > 0)
                    {
                        sizeBtn.CssClass += " selectedSize";
                    }
                    else if (firstAvailableSizeId == null)
                    {
                        firstAvailableSizeId = sizeId;
                    }
                }
            }
            if (GetQuantity() == 0)
            {
                ViewState["SizeId"] = firstAvailableSizeId;
                selectSize(firstAvailableSizeId);
            }
        }

        private void selectSize(string sizeId)
        {
            foreach (RepeaterItem item in SizeRepeater.Items)
            {
                LinkButton sizeBtn = item.FindControl("lbtnSize") as LinkButton; // Replace with your button ID
                if (sizeBtn != null)
                {
                    sizeBtn.CssClass.Replace(" selectedSize", "").Trim(); ;
                    if (sizeBtn.Attributes["data-sizeId"] == sizeId)
                    {
                        sizeBtn.CssClass += " selectedSize";
                        lblSize.Text = sizeBtn.Attributes["value"].ToString();
                    }
                }
            }
            GetQuantity();
            txtQuantity.Text = "1";
        }

        protected void SizeRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SelectSize")
            {
                string newSizeId = e.CommandArgument.ToString();
                string previousSizeId = ViewState["SizeId"] as string;
                foreach (RepeaterItem item in SizeRepeater.Items)
                {
                    LinkButton sizeBtn = item.FindControl("lbtnSize") as LinkButton;
                    if (sizeBtn != null && sizeBtn.Attributes["data-sizeId"] == previousSizeId)
                    {
                        sizeBtn.CssClass = sizeBtn.CssClass.Replace(" selectedSize", "").Trim();
                        break;
                    }
                }
                ViewState["SizeId"] = newSizeId;
                selectSize(newSizeId);
            }
        }

        private bool IsButtonEnabled(string colorId, string sizeId)
        {
            string productId = Request.QueryString["ProductId"];
            int quantity = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT Quantity " +
                                  "FROM ProductDetail " +
                                  "WHERE ColorId = @colorId AND SizeId = @sizeId AND ProductId = @productId";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@colorId", colorId);
                    command.Parameters.AddWithValue("@sizeId", sizeId);
                    command.Parameters.AddWithValue("@productId", productId);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            quantity = (int)reader["Quantity"];
                        }
                    }
                    connection.Close();
                }
            }
            return quantity == 0;
        }

        protected void SizeRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton sizeBtn = e.Item.FindControl("lbtnSize") as LinkButton;
                string sizeId = sizeBtn.Attributes["data-sizeId"];
                string colorId = ViewState["ColorId"] as string;
                if (colorId != null && sizeId != null)
                {
                    if (IsButtonEnabled(colorId, sizeId))
                    {
                        sizeBtn.Enabled = false;
                        sizeBtn.CssClass = "buttonDisabled"; // Make sure the CSS class exists in your stylesheet
                    }
                }
            }
        }

        protected void ColorRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton colorBtn = e.Item.FindControl("lbtnColor") as LinkButton;
                string colorId = colorBtn.Attributes["data-colorId"];
                if (IsButtonEnabled(colorId))
                {
                    colorBtn.Enabled = false;
                    colorBtn.CssClass = "buttonColorDisabled"; // Make sure the CSS class exists in your stylesheet
                }
            }
        }

        protected void btnDecrease_Click(object sender, EventArgs e)
        {
            int quantity = Convert.ToInt32(txtQuantity.Text);
            if (quantity > 1)  // Ensure quantity doesn't go negative
            {
                quantity--;
                txtQuantity.Text = quantity.ToString();
            }
        }

        protected void btnIncrease_Click(object sender, EventArgs e)
        {
            int quantity = Convert.ToInt32(txtQuantity.Text);
            int totalQuantity = Convert.ToInt32(lblQuantity.Text);
            if (quantity < totalQuantity)
            {
                quantity++;
                txtQuantity.Text = quantity.ToString();
            }
        }

        protected void btnLatest_Click(object sender, EventArgs e)
        {
            ViewState["SortingCriteria"] = "latest";
            ddpReviews.SetPageProperties(0, ddpReviews.PageSize, true);
            btnLatest.CssClass = "btnSorting clicked";
            btnTopRated.CssClass = "btnSorting not-clicked";
            bindReviews();
        }

        protected void btnTopRated_Click(object sender, EventArgs e)
        {
            ViewState["SortingCriteria"] = "topRated";
            ddpReviews.SetPageProperties(0, ddpReviews.PageSize, true);
            btnTopRated.CssClass = "btnSorting clicked";
            btnLatest.CssClass = "btnSorting not-clicked";
            bindReviews();
        }

        private void bindReviews()
        {
            lvReviews.DataSource = getReviewList();
            lvReviews.DataBind();
        }

        protected void AddToCart_Click(object sender, EventArgs e)
        {
            // Get the selected product details
            int productId = Convert.ToInt32(Request.QueryString["ProductId"]);
            int colorId = Convert.ToInt32(ViewState["ColorId"]);
            int sizeId = Convert.ToInt32(ViewState["SizeId"]);
            int quantity = Convert.ToInt32(txtQuantity.Text);

            // Check if the customer ID is valid
            if (customerId > 0)
            {
                // Check if the cart item already exists for the same customer and product details
                int existingCartItemQuantity = GetExistingCartItemQuantity(customerId, productId, colorId, sizeId);

                if (existingCartItemQuantity > 0)
                {
                    // Update the quantity for the existing cart item
                    UpdateCartItemQuantity(customerId, productId, colorId, sizeId, existingCartItemQuantity + quantity);
                }
                else
                {
                    // Insert the data into the CartItem table
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string sqlQuery = "INSERT INTO CartItem (CartId, ProductDetailId, Quantity) VALUES (@CartId, @ProductDetailId, @Quantity)";
                        using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                        {
                            connection.Open();
                            command.Parameters.AddWithValue("@CartId", GetCartId(customerId)); // You need to implement this method to retrieve the cart ID based on the customer ID
                            command.Parameters.AddWithValue("@ProductDetailId", GetProductDetailId(productId, colorId, sizeId)); // You need to implement this method to retrieve the ProductDetailId based on the product ID, color ID, and size ID
                            command.Parameters.AddWithValue("@Quantity", quantity);
                            command.ExecuteNonQuery();

                            UpdateCartSubtotal(customerId);
                        }
                    }
                }

                Response.Redirect(Request.RawUrl);
            }
            else
            {
                Response.Redirect("~/src/Client/Login/Login.aspx");
            }
        }

        private void UpdateCartSubtotal(int customerId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE Cart " +
                               "SET Subtotal = (SELECT SUM(P.UnitPrice * CI.Quantity) " +
                                              "FROM CartItem CI " +
                                              "INNER JOIN ProductDetail PD ON CI.ProductDetailId = PD.ProductDetailId " +
                                              "INNER JOIN Product P ON PD.ProductId = P.ProductId " +
                                              "WHERE CI.CartId = Cart.CartId) " +
                               "WHERE CustomerId = @CustomerId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        private int GetExistingCartItemQuantity(int customerId, int productId, int colorId, int sizeId)
        {
            int existingQuantity = 0;

            // Query to check if the cart item already exists for the provided customer ID and product details
            string query = "SELECT Quantity FROM CartItem WHERE CartId = @CartId AND ProductDetailId = @ProductDetailId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CartId", GetCartId(customerId));
                    command.Parameters.AddWithValue("@ProductDetailId", GetProductDetailId(productId, colorId, sizeId));

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        existingQuantity = Convert.ToInt32(result);
                    }
                }
            }

            return existingQuantity;
        }

        private void UpdateCartItemQuantity(int customerId, int productId, int colorId, int sizeId, int newQuantity)
        {
            // Query to update the quantity of the existing cart item
            string query = "UPDATE CartItem SET Quantity = @Quantity WHERE CartId = @CartId AND ProductDetailId = @ProductDetailId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Quantity", newQuantity);
                    command.Parameters.AddWithValue("@CartId", GetCartId(customerId));
                    command.Parameters.AddWithValue("@ProductDetailId", GetProductDetailId(productId, colorId, sizeId));

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        private int GetCartId(int customerId)
        {
            int cartId = 0; // Initialize cartId to 0
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT CartId FROM Cart WHERE CustomerId = @CustomerId";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        cartId = Convert.ToInt32(result);
                    }
                }
            }
            return cartId;
        }

        private int GetProductDetailId(int productId, int colorId, int sizeId)
        {
            int productDetailId = 0; // Initialize productDetailId to 0
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT ProductDetailId FROM ProductDetail WHERE ProductId = @ProductId AND ColorId = @ColorId AND SizeId = @SizeId";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);
                    command.Parameters.AddWithValue("@ColorId", colorId);
                    command.Parameters.AddWithValue("@SizeId", sizeId);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        productDetailId = Convert.ToInt32(result);
                    }
                }
            }
            return productDetailId;
        }
    }

}
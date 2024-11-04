using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using OutModern.src.Admin.Utils;

namespace OutModern.src.Admin.ProductEdit
{
    public partial class ProductEdit : System.Web.UI.Page
    {
        protected static readonly string ProductDetails = "ProductDetails";
        protected Dictionary<string, string> urls = new Dictionary<string, string>()
        {
            { ProductDetails , "~/src/Admin/ProductDetails/ProductDetails.aspx" }
        };

        private string productId;
        private string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            productId = Request.QueryString["ProductId"];
            if (productId == null)
            {
                Response.Redirect("~/src/ErrorPages/404.aspx");
            }

            if (!IsPostBack)
            {
                Page.DataBind();
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["MenuCategory"] = "Products";

                initProductInfo(); //bind data in loadcomplete, else the ddl will rebind in page load
            }
        }

        private void initProductInfo()
        {
            DataTable productData = getProductInfo();
            if (productData.Rows.Count == 0)
            {
                Response.Redirect("~/src/ErrorPages/404.aspx");
            }

            DataRow data = productData.Rows[0];
            lblProdId.Text = data["ProductId"].ToString();
            txtProdName.Text = data["ProductName"].ToString();
            txtPrice.Text = data["UnitPrice"].ToString();
            txtProdDescription.Text = data["ProductDescription"].ToString();

            // bind status drop down list
            ddlStatus.DataSource = getProductStatus();
            ddlStatus.DataValueField = "ProductStatusId";
            ddlStatus.DataTextField = "ProductStatusName";
            ddlStatus.DataBind();
            ListItem statusItem = ddlStatus.Items.FindByText(data["ProductStatusName"].ToString());
            if (statusItem != null) statusItem.Selected = true; // auto select current value

            // Bind category drop down list
            ddlCategory.DataSource = getProductCategory();
            ddlCategory.DataValueField = "ProductCategory";
            ddlCategory.DataTextField = "ProductCategory";
            ddlCategory.DataBind();
            ListItem categoryItem = ddlCategory.Items.FindByText(data["ProductCategory"].ToString());
            if (categoryItem != null) categoryItem.Selected = true; // auto select current value

            //bind size ddl
            ddlSize.DataSource = getAllSizes();
            ddlSize.DataValueField = "SizeId";
            ddlSize.DataTextField = "SizeName";
            ddlSize.DataBind();

            //bind color to add color in with ddl
            bindColorDropDownList();

            //bind color available for product
            repeaterColors.DataSource = getProdColors();
            repeaterColors.DataBind();

            //init quantity for the particular size and product
            setQuantityText();

            //Bind repeater of images
            repeaterImages.DataSource = ViewState["ColorId"] != null ?
                getImages(ViewState["ColorId"].ToString()) :
                new DataTable();

            repeaterImages.DataBind();
        }

        //set quantity of the text
        private void setQuantityText()
        {
            if (ViewState["ColorId"] == null) return;


            string sizeId = ddlSize.SelectedValue.ToString();
            string colorId = ViewState["ColorId"].ToString();

            int quantity = getQuantity(sizeId, colorId);
            txtProdQuantity.Text = quantity.ToString();
        }

        // clear all the status
        private void clearStatusText()
        {
            lblSetQuantityStatus.Text = "";
            lblAddColorStatus.Text = "";
            lblAddImgStatus.Text = "";
            lblDeleteColorStatus.Text = "";
        }

        //select color style changing
        private void selectColor(string colorId)
        {
            // Remove active class
            foreach (RepeaterItem item in repeaterColors.Items)
            {
                LinkButton lbColor = item.FindControl("lbColor") as LinkButton;
                if (lbColor != null)
                {
                    lbColor.CssClass = lbColor.CssClass.Replace(" active", ""); // Remove "active"
                    if (lbColor.Attributes["data-colorId"] == colorId)
                    {
                        lbColor.CssClass += " active";
                    }
                }
            }

            //rebind image
            repeaterImages.DataSource = getImages(ViewState["ColorId"].ToString());
            repeaterImages.DataBind();
        }
        private void bindColorDropDownList()
        {
            //bind color to add color in with ddl
            DataTable colorNotAdded = getNotAddedColors();
            ddlColorAdd.DataSource = colorNotAdded;
            ddlColorAdd.DataValueField = "ColorId";
            ddlColorAdd.DataTextField = "ColorName";
            ddlColorAdd.DataBind();

            ListItemCollection itemCollection = ddlColorAdd.Items;
            for (int i = 0; i < itemCollection.Count; i++)
            {
                itemCollection[i].Attributes
                    .Add("data-hex", colorNotAdded.Rows[i]["HexColor"].ToString());
            }
        }

        //
        //DB operation
        //

        // get imageas
        private DataTable getImages(string colorId)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlQuery =
                    "select distinct [path] " +
                    "From Product, ProductDetail, ProductImage " +
                    "WHERE Product.ProductId = ProductDetail.ProductId " +
                    "AND ProductDetail.ProductDetailId = ProductImage.ProductDetailId " +
                    "AND ProductDetail.ColorId = @colorId " +
                    "AND Product.ProductId = @productId " +
                    "AND ProductDetail.isDeleted = 0";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@colorId", colorId);
                    command.Parameters.AddWithValue("@productId", productId);

                    data.Load(command.ExecuteReader());
                }
            }

            return data;
        }

        // Get the products info
        private DataTable getProductInfo()
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlQuery =
                    "Select ProductId, ProductName,ProductDescription, ProductCategory, UnitPrice, ProductStatusName " +
                    "FROM Product " +
                    "Join ProductStatus on Product.ProductStatusId = ProductStatus.ProductStatusId " +
                    "WHERE ProductId = @productId;";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);
                    data.Load(command.ExecuteReader());
                }
            }
            return data;
        }

        //Get Categories
        private DataTable getProductCategory()
        {
            DataTable data = new DataTable();
            data.Columns.Add("ProductCategory");

            // Add product categories to the table
            data.Rows.Add("Hoodies");
            data.Rows.Add("Tee Shirts");
            data.Rows.Add("Sweaters");
            data.Rows.Add("Shorts and Pants");
            data.Rows.Add("Trousers");

            return data;
        }

        //Get All Product Status Available
        private DataTable getProductStatus()
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "Select ProductStatusId, ProductStatusName " +
                    "From ProductStatus;";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    data.Load(command.ExecuteReader());
                }
            }

            return data;
        }

        // return all the sizes available (not specifically for 1 prod)
        private DataTable getAllSizes()
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "Select SizeId, SizeName " +
                    "From Size;";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    data.Load(command.ExecuteReader());
                }
            }

            return data;
        }

        // return all the colors not added
        private DataTable getNotAddedColors()
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "Select c.ColorId, c.ColorName, c.HexColor " +
                    "From Color c " +
                    "left Join (" +
                        "Select distinct c.ColorId, ColorName, HexColor " +
                        "From Color c, Product p, ProductDetail pd " +
                        "Where p.ProductId = pd.ProductId AND c.ColorId = pd.ColorId " +
                        "AND p.ProductId = @productId AND isDeleted = 0" +
                    ") as t " +
                    "ON c.ColorId = t.ColorId " +
                    "WHERE t.ColorId is null;";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);
                    data.Load(command.ExecuteReader());
                }
            }

            return data;
        }

        // return all the colors available for each specific prod
        private DataTable getProdColors()
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                 "Select distinct pd.ColorId, c.HexColor " +
                 "From Product p, Color c, ProductDetail pd " +
                 "Where p.ProductId = @productId AND pd.ColorId = c.ColorId " +
                 "AND p.ProductId = pd.ProductId AND isDeleted = 0";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);
                    data.Load(command.ExecuteReader());
                }
            }

            // store in viewstate to maintain the selection of color
            if (ViewState["ColorId"] == null && data.Rows.Count > 0)
            {
                ViewState["ColorId"] = data.Rows[0]["ColorId"].ToString();
            }

            return data;
        }

        //get quantity based on size and color
        private int getQuantity(string sizeId, string colorId)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlQuery =
                  "select quantity " +
                  "From ProductDetail " +
                  "Where ProductId = @productId " +
                  "AND SizeId = @sizeId " +
                  "AND ColorId = @colorId; ";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);
                    command.Parameters.AddWithValue("@sizeId", sizeId);
                    command.Parameters.AddWithValue("@colorId", colorId);

                    data.Load(command.ExecuteReader());
                }
            }


            return data.Rows.Count == 0 ? 0 : (int)data.Rows[0]["quantity"];
        }

        // update the quantity
        // return the number of row affected
        private int updateQuantity(string sizeId, string colorId, string quantity)
        {
            int affectedRow = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "UPDATE ProductDetail " +
                    "SET quantity = @quantity " +
                    "WHERE ProductId = @productId AND sizeId = @sizeId AND colorId = @colorId";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@quantity", quantity);
                    command.Parameters.AddWithValue("@productId", productId);
                    command.Parameters.AddWithValue("@sizeId", sizeId);
                    command.Parameters.AddWithValue("@colorId", colorId);

                    affectedRow = command.ExecuteNonQuery();
                }
            }
            return affectedRow;
        }

        // update product info
        // return the number of row affected
        private int updateProductInfo(string productName, string productDescription, string productCategory, string unitPrice, string statusId)
        {
            int affectedRow = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlQuery =
                    "Update Product " +
                    "SET ProductName = @productName, " +
                    "ProductDescription = @productDescription, " +
                    "ProductCategory = @productCategory, " +
                    "UnitPrice = @unitPrice, " +
                    "ProductStatusId = @StatusId " +
                    "Where ProductId = @productId";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@productName", productName);
                    command.Parameters.AddWithValue("@productDescription", productDescription);
                    command.Parameters.AddWithValue("@productCategory", productCategory);
                    command.Parameters.AddWithValue("@unitPrice", unitPrice);
                    command.Parameters.AddWithValue("@statusId", statusId);
                    command.Parameters.AddWithValue("@productId", productId);

                    affectedRow = command.ExecuteNonQuery();
                }
            }

            return affectedRow;
        }

        // update or insert the product with the selected color
        // if available (isDeleted = true), update the value to false, quantity to 0
        // if not available, insert it
        private int upsertProdColor(string colorId)
        {
            int affectedRow = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                int noOfProdInDB = 0;

                string queryCheckProdAvailability =
                    "Select Count(ProductDetailId) " +
                    "FROM ProductDetail " +
                    "WHERE ColorId = @colorId " +
                    "AND ProductDetail.ProductId = @productId AND isDeleted = 1";

                using (SqlCommand command = new SqlCommand(queryCheckProdAvailability, connection))
                {
                    command.Parameters.AddWithValue("@colorId", colorId);
                    command.Parameters.AddWithValue("@productId", productId);

                    noOfProdInDB = int.Parse(command.ExecuteScalar().ToString());
                }

                if (noOfProdInDB > 0)
                {
                    string sqlQuery =
                        "UPDATE ProductDetail " +
                        "SET isDeleted = 0, " +
                        "Quantity = 0 " +
                        "WHERE ProductId = @productId " +
                        "AND ColorId = @colorId;";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@colorId", colorId);
                        command.Parameters.AddWithValue("@productId", productId);

                        affectedRow = command.ExecuteNonQuery();
                    }
                }
                else
                {
                    string sqlQuery =
                        "INSERT INTO ProductDetail (Quantity, isDeleted, ColorId, SizeId, ProductId) " +
                        "SELECT 0 as Quantity, 0 as isDeleted, @colorId as ColorId, SizeId, @productId as ProductId " +
                        "FROM Size";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@colorId", colorId);
                        command.Parameters.AddWithValue("@productId", productId);

                        affectedRow = command.ExecuteNonQuery();
                    }

                }
            }

            return affectedRow;
        }

        // delete the product with the colorid given
        // set isDeleted = 1
        private int deleteProdColor(string colorId)
        {
            int affectedRow = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlQuery =
                    "Update ProductDetail " +
                    "SET isDeleted = 1 , Quantity = 0" +
                    "WHERE ColorId = @colorId AND ProductId = @productId";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@colorId", colorId);
                    command.Parameters.AddWithValue("@productId", productId);

                    affectedRow = command.ExecuteNonQuery();
                }

            }
            return affectedRow;
        }

        //save image path into db
        private int saveImagePath(string colorId, string filePath)
        {
            int affectedRow = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "INSERT INTO ProductImage ([Path], ProductDetailId) " +
                    "Select @path as [Path], ProductDetailId " +
                    "FROM ProductDetail " +
                    "WHERE ColorId = @colorId AND ProductId = @productId;";

                // save into db
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@colorId", colorId);
                    command.Parameters.AddWithValue("@path", filePath);
                    command.Parameters.AddWithValue("@productId", productId);

                    affectedRow = command.ExecuteNonQuery();
                }

            }
            return affectedRow;
        }

        // delete the image in db based on path
        private int deleteImage(string path)
        {
            int affectedRow = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "DELETE FROM ProductImage " +
                    "WHERE [path] = @path; " +
                    "DBCC CHECKIDENT ('ProductImage', RESEED,0); " +
                    "DBCC CHECKIDENT ('ProductImage', RESEED);";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@path", path);
                    affectedRow = command.ExecuteNonQuery();
                }
            }

            return affectedRow;
        }

        //update the product status to unavailable
        private int updateProductStatusToUnavailable()
        {
            int affectedRow = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "Update Product " +
                    "SET ProductStatusId = (Select ProductStatusId From ProductStatus Where ProductStatusName = 'Unavailable') " +
                    "Where ProductId = @productId";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);
                    affectedRow = command.ExecuteNonQuery();
                }
            }

            return affectedRow;
        }

        //
        //Page Events
        //
        protected void lbUpdate_Click(object sender, EventArgs e)
        {
            string productName = txtProdName.Text.Trim();
            string productCategory = ddlCategory.SelectedValue.ToString();
            string unitPrice = txtPrice.Text.Trim();
            string statusId = ddlStatus.SelectedValue.ToString();
            string productDescription = txtProdDescription.Text.Trim();

            // TODO : validation
            //check nulls
            if (string.IsNullOrEmpty(productName)
                || string.IsNullOrEmpty(productDescription)
                || string.IsNullOrEmpty(productCategory)
                || string.IsNullOrEmpty(unitPrice))
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                    "Update Failed",
                    "document.addEventListener('DOMContentLoaded', ()=>alert('Please Fill in All Fields'));",
                    true);
                return;
            }

            // check if price is decimal, with only 2 decimal places
            // restrict to 2 decimal places
            if (!ValidationUtils.IsValidPrice(unitPrice))
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                    "Update Failed",
                    "document.addEventListener('DOMContentLoaded', ()=>alert('Please Enter a Valid Price, with at most 2 decimal places'));",
                    true);
                return;
            }

            // check product added color and status
            // no color added should make status remain unavailable
            DataTable colorData = getProdColors();
            string selectedStatusName = ddlStatus.SelectedItem.Text.Trim();
            if (colorData.Rows.Count == 0 && selectedStatusName == "In Stock")
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                    "Update Failed",
                    "document.addEventListener('DOMContentLoaded', ()=>alert('Please Add a Color to Update the " +
                    "product Status to Available or In Stock'));",
                        true);

                //select back to "unavailable"
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Unavailable"));

                return;
            }

            int affectedRow = updateProductInfo(productName, productDescription, productCategory, unitPrice, statusId);

            if (affectedRow <= 0)
            {
                //register page js
                Page.ClientScript.RegisterStartupScript(GetType(),
                    "Update Failed",
                    $"document.addEventListener('DOMContentLoaded', ()=>alert('Failed to Update Product Info'));", true);
            }

            bindColorDropDownList();
        }

        protected void repeaterColors_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ChangeColor")
            {
                string colorId = e.CommandArgument.ToString();
                ViewState["ColorId"] = colorId;
                selectColor(colorId);

                bindColorDropDownList();
                setQuantityText();
            }
            else if (e.CommandName == "DeleteColor")
            {
                string colorId = e.CommandArgument.ToString();
                int affectedRow = deleteProdColor(colorId);

                clearStatusText();
                lblDeleteColorStatus.Text = affectedRow > 0 ?
                    "*Deleted Successfully" : "*Failed to Delete, Please Try Again Later";
                string vsColorId = ViewState["ColorId"].ToString();

                bindColorDropDownList();

                //rebind color
                repeaterColors.DataSource = getProdColors();
                repeaterColors.DataBind();

                // if current selected color is deleted
                // and there is color available
                // make it select the first item
                if (vsColorId == colorId && repeaterColors.Items.Count > 0)
                {
                    RepeaterItem item = repeaterColors.Items[0];
                    LinkButton lbColor = item.FindControl("lbColor") as LinkButton;
                    lbColor.CssClass += " active";

                    string firstColorId = lbColor.Attributes["data-colorId"].ToString();
                    ViewState["ColorId"] = firstColorId;

                    selectColor(firstColorId);
                    setQuantityText();
                }
                else
                {
                    // update the product status to unavailable in database
                    updateProductStatusToUnavailable();

                    // select ddl back to "unavailable"
                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText("Unavailable"));

                    // clear the color id
                    ViewState["ColorId"] = null;

                    //rebind image
                    repeaterImages.DataBind();
                }
            }
        }

        protected void repeaterImages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteImage")
            {
                string path = e.CommandArgument.ToString();

                // remove from db
                int affectedRow = deleteImage(path);

                clearStatusText();
                if (affectedRow > 0)
                {
                    // remove from server folder
                    string serverFilePath = Server.MapPath(path);
                    if (File.Exists(serverFilePath)) File.Delete(serverFilePath);

                    lblAddImgStatus.Text = "*Image Deleted Successfully";
                }
                else
                {
                    lblAddImgStatus.Text = "*Image Failed to Delete, Please Try Again later...";
                    Page.ClientScript
                        .RegisterStartupScript(GetType(),
                            "Delete Failed",
                            "document.addEventListener('DOMContentLoaded', ()=>alert('Failed to Delete Image, Please Try Again Later'));",
                            true);
                }


                //reBind repeater of images to show latest img
                repeaterImages.DataSource = getImages(ViewState["ColorId"].ToString());
                repeaterImages.DataBind();

                //bind color to add color in with ddl
                bindColorDropDownList();
            }
        }

        protected void ddlSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            setQuantityText();
            bindColorDropDownList();
        }

        protected void btnUpdateQuantity_Click(object sender, EventArgs e)
        {
            bindColorDropDownList();

            if (ViewState["ColorId"] == null)
            {
                lblSetQuantityStatus.Text = "*Please Add and Select a Color to Set Quantity!";
                return;
            }

            string sizeId = ddlSize.SelectedValue.ToString();
            string colorId = ViewState["ColorId"].ToString();
            string quantity = txtProdQuantity.Text;

            //check nulls
            if (string.IsNullOrEmpty(quantity))
            {
                lblSetQuantityStatus.Text = "**Please Enter a Quantity!";
                return;
            }

            // TODO : validation
            // check if quantity is integer
            if (!int.TryParse(quantity, out int _))
            {
                lblSetQuantityStatus.Text = "**Please Enter a Valid Quantity!";
                return;
            }

            int affectedRow = updateQuantity(sizeId, colorId, quantity);
            clearStatusText();
            if (affectedRow > 0)
            {
                lblSetQuantityStatus.Text = "**Quantity Updated Successfully";
            }
            else
            {
                lblSetQuantityStatus.Text = "**Failed To Update Quantity, Please Try Again..";
                Page.ClientScript
                    .RegisterClientScriptBlock(GetType(),
                    "Set Quantity",
                    "document.addEventListener('DOMContentLoaded', ()=>alert('Failed to Set Quantity, Please Try Again Later'));",
                                 true);
            }

        }

        protected void btnAddColor_Click(object sender, EventArgs e)
        {
            int affectedRow = upsertProdColor(ddlColorAdd.SelectedValue);

            clearStatusText();
            lblAddColorStatus.Text = affectedRow > 0 ? "*Insert Successfully" : "*Failed To Insert, Please Try Again..";

            //bind color to add color in with ddl
            bindColorDropDownList();

            //rebind color
            repeaterColors.DataSource = getProdColors();
            repeaterColors.DataBind();
            selectColor(ViewState["ColorId"].ToString());
        }

        protected void btnAddImage_Click(object sender, EventArgs e)
        {
            clearStatusText();

            if (!fileImgUpload.HasFile)
            {
                lblAddImgStatus.Text = "*No file Added..";
            }

            string colorId = ViewState["ColorId"].ToString();

            foreach (HttpPostedFile file in fileImgUpload.PostedFiles)
            {
                string newFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                string serverFolderPath = $"~/images/product_img/{colorId}/";
                string newFilePath = Path.Combine(serverFolderPath, newFileName);

                // save into db first
                int affectedRow = saveImagePath(colorId, newFilePath);

                // save into folder
                if (affectedRow > 0)
                {
                    string serverPhysicalPath = Server.MapPath(newFilePath);

                    string directory = Path.GetDirectoryName(serverPhysicalPath);

                    //create the folder if not exist
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    file.SaveAs(serverPhysicalPath); //convert into physical path then store

                    lblAddImgStatus.Text += $"*{Path.GetFileName(file.FileName)} added Successfully" + "</br>";
                }
                else
                {
                    lblAddImgStatus.Text += $"{file.FileName} FAILED to add </ br> ";
                }
            }

            //reBind repeater of images to show latest img
            repeaterImages.DataSource = getImages(ViewState["ColorId"].ToString());
            repeaterImages.DataBind();

            //bind color to add color in with ddl
            bindColorDropDownList();
        }


    }
}
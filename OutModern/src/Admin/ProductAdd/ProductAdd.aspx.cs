using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Configuration;
using OutModern.src.Admin.Utils;

namespace OutModern.src.Admin.ProductAdd
{
    public partial class ProductAdd : System.Web.UI.Page
    {
        protected static readonly string ProductDetails = "ProductDetails";
        protected static readonly string Products = "Products";
        protected Dictionary<string, string> urls = new Dictionary<string, string>()
        {
            { ProductDetails , "~/src/Admin/ProductDetails/ProductDetails.aspx" },
            { Products , "~/src/Admin/Products/Products.aspx" }

        };

        private string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["MenuCategory"] = "Products";

                ddlCategory.DataSource = getProductCategory();
                ddlCategory.DataValueField = "ProductCategory";
                ddlCategory.DataTextField = "ProductCategory";
                ddlCategory.DataBind();

            }
        }

        //
        //db operation
        //

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

        private int insertProduct(string productName, string productDescription, string category, string unitPrice)
        {
            int prodId = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "INSERT into Product (ProductName, ProductDescription, ProductCategory, UnitPrice, ProductStatusId) " +
                    "OUTPUT INSERTED.ProductId " +
                    "values (@productName, @productDescription, @category, @unitPrice, " +
                    "(SELECT ProductStatusId FROM ProductStatus WHERE ProductStatusName = 'Unavailable'))";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {

                    command.Parameters.AddWithValue("@productName", productName);
                    command.Parameters.AddWithValue("@productDescription", productDescription);
                    command.Parameters.AddWithValue("@category", category);
                    command.Parameters.AddWithValue("@unitPrice", unitPrice);

                    prodId = int.Parse(command.ExecuteScalar().ToString());
                }
            }

            return prodId;
        }

        //
        // Page Events
        //
        protected void lbAdd_Click(object sender, EventArgs e)
        {
            string productName = txtProdName.Text.Trim();
            string productDescription = txtProdDescription.Text.Trim();
            string category = ddlCategory.SelectedValue;
            string price = txtPrice.Text.Trim();
            //string statusId = ddlStatus.SelectedValue;

            // TODO : validation
            //check nulls
            if (string.IsNullOrEmpty(productName)
                || string.IsNullOrEmpty(productDescription)
                || string.IsNullOrEmpty(category)
                || string.IsNullOrEmpty(price))
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                            "Failed to Add",
                            $"document.addEventListener('DOMContentLoaded', ()=> alert('Please fill in all Fields to Proceed'));",
                            true);

                return;
            }

            if (!ValidationUtils.IsValidPrice(price))
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                            "Failed to Add",
                        $"document.addEventListener('DOMContentLoaded', ()=> alert('Please Enter a Valid Price, with at most 2 decimal places'));",
                        true);
                return;
            };

            int id = insertProduct(productName, productDescription, category, price);

            if (id > 0)
            {
                Response.Redirect($"~/src/Admin/ProductEdit/ProductEdit.aspx?ProductId={id}");
            }
        }

    }
}
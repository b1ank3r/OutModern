using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using OutModern.src.Admin.Staffs;


namespace OutModern.src.Admin.Products
{
    public partial class Products : System.Web.UI.Page
    {
        protected static readonly string ProductEdit = "ProductEdit";
        protected static readonly string ProductAdd = "ProductAdd";
        protected static readonly string ProductDetails = "ProductDetails";

        protected Dictionary<string, string> urls = new Dictionary<string, string>()
        {
            { ProductEdit , "~/src/Admin/ProductEdit/ProductEdit.aspx" },
            { ProductAdd , "~/src/Admin/ProductAdd/ProductAdd.aspx" },
            { ProductDetails , "~/src/Admin/ProductDetails/ProductDetails.aspx" },
        };

        private string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;


        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                Session["MenuCategory"] = "Products";

                lvProducts.DataSource = staffDataSource();
                lvProducts.DataBind();

                //bind filter category list
                ddlFilterCategory.DataSource = getProductCategory();
                ddlFilterCategory.DataTextField = "ProductCategory";
                ddlFilterCategory.DataValueField = "ProductCategory";
                ddlFilterCategory.DataBind();

                //bind filter status list
                ddlFilterStatus.DataSource = getProductStatus();
                ddlFilterStatus.DataTextField = "ProductStatusName";
                ddlFilterStatus.DataValueField = "ProductStatusId";
                ddlFilterStatus.DataBind();

                Page.DataBind();

            }
        }

        //choose to load data source
        private DataTable staffDataSource(string sortExpression = null, string sortDirection = "ASC")
        {
            //get the search term
            string searchTerms = Request.QueryString["q"];

            if (searchTerms != null)
            {
                ((TextBox)Master.FindControl("txtSearch")).Text = searchTerms;
                return FilterDataTable(getProducts(sortExpression, sortDirection), searchTerms);
            }

            else return getProducts(sortExpression, sortDirection);
        }


        //store each column sorting state into viewstate
        private Dictionary<string, string> SortDirections
        {
            get
            {
                if (ViewState["SortDirections"] == null)
                {
                    ViewState["SortDirections"] = new Dictionary<string, string>();
                }
                return (Dictionary<string, string>)ViewState["SortDirections"];
            }
            set
            {
                ViewState["SortDirections"] = value;
            }
        }

        // Toggle Sorting
        private void toggleSortDirection(string columnName)
        {
            if (!SortDirections.ContainsKey(columnName))
            {
                SortDirections[columnName] = "ASC";
            }
            else
            {
                SortDirections[columnName] = SortDirections[columnName] == "ASC" ? "DESC" : "ASC";
            }
        }

        //search logic
        private DataTable FilterDataTable(DataTable dataTable, string searchTerm)
        {
            string safeSearchTerm = searchTerm.Replace("'", "''");

            // Build filter expression with product fields
            string expression = string.Format(
                "Convert(ProductId, 'System.String') LIKE '%{0}%' OR " +
                "ProductName LIKE '%{0}%' OR " +
                "ProductCategory LIKE '%{0}%' OR " +
                "Convert(UnitPrice, 'System.String') LIKE '%{0}%' OR " +
                "ProductStatusName LIKE '%{0}%' ",
                safeSearchTerm);

            // Filter the rows
            DataRow[] filteredRows = dataTable.Select(expression);

            // Create a new DataTable for the filtered results
            DataTable filteredDataTable = dataTable.Clone();

            // Import the filtered rows
            foreach (DataRow row in filteredRows)
            {
                filteredDataTable.ImportRow(row);
            }

            return filteredDataTable;
        }

        //
        //db operation
        //

        // get all products
        private DataTable getProducts(string sortExpression = null, string sortDirection = "ASC")
        {
            DataTable data = new DataTable();

            string category = ddlFilterCategory.SelectedValue;
            string status = ddlFilterStatus.SelectedValue;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sqlQuery =
                    "Select ProductId, ProductName, ProductCategory, UnitPrice, ProductStatusName " +
                    "From Product, ProductStatus " +
                    "Where Product.ProductStatusId = ProductStatus.ProductStatusId ";

                if (!string.IsNullOrEmpty(category) && category != "-1")
                {
                    sqlQuery += "AND ProductCategory = @category ";
                }

                if (!string.IsNullOrEmpty(status) && status != "-1")
                {
                    sqlQuery += "AND Product.ProductStatusId = @status ";
                }

                if (!string.IsNullOrEmpty(sortExpression))
                {
                    sqlQuery += "ORDER BY " + sortExpression + " " + sortDirection;
                }


                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    if (!string.IsNullOrEmpty(category) && category != "-1")
                    {
                        command.Parameters.AddWithValue("@category", category);
                    }

                    if (!string.IsNullOrEmpty(status) && status != "-1")
                    {
                        command.Parameters.AddWithValue("@status", status);
                    }

                    data.Load(command.ExecuteReader());
                }
            }

            data.Columns.Add("Path", typeof(string));
            data.Columns.Add("Colors", typeof(DataTable));
            foreach (DataRow row in data.Rows)
            {
                string productId = row["productId"].ToString();
                row["Path"] = getImagePath(productId);
                row["Colors"] = getColors(productId);
            }

            return data;
        }

        //get a image path of a product
        private string getImagePath(string productId)
        {
            string path = "";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlQuery =
                    "Select Top 1 [path] " +
                    "FROM Product, ProductDetail, ProductImage " +
                    "WHERE Product.ProductId = ProductDetail.ProductId " +
                    "AND ProductDetail.ProductDetailId = ProductImage.ProductDetailId " +
                    "AND Product.ProductId = @productId " +
                    "Order by ProductDetail.ProductDetailId";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);
                    path = command.ExecuteScalar()?.ToString();
                }
            }

            return path == null ? "" : path;


        }

        //get All colors of a product
        private DataTable getColors(string productId)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlQuery =
                    "Select Distinct c.HexColor as color " +
                    "From Product p, Color c, ProductDetail pd " +
                    "Where p.ProductId = @productId AND pd.ColorId = c.ColorId " +
                    "AND p.ProductId = pd.ProductId AND isDeleted = 0";

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
            data.Rows.Add("Accessories");

            return data;
        }

        //Get All Product Status
        private DataTable getProductStatus()
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlQuery = "Select ProductStatusId, ProductStatusName From ProductStatus";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    data.Load(command.ExecuteReader());
                }
            }

            return data;
        }



        //
        //Page event
        //
        protected void lvProducts_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                // Retrieve the data item
                //Product rowView = (Product)e.Item.DataItem;
                DataRowView rowView = (DataRowView)e.Item.DataItem;

                // Find the span control
                HtmlGenericControl statusSpan = (HtmlGenericControl)e.Item.FindControl("productStatus");
                string status = rowView["ProductStatusName"].ToString();

                // Add CSS class based on status
                if (status == "In Stock")
                {
                    statusSpan.Attributes["class"] += " in-stock";
                }
                else if (status == "Out of Stock")
                {
                    statusSpan.Attributes["class"] += " out-of-stock";
                }
                else if (status == "Unavailable")
                {
                    statusSpan.Attributes["class"] += " temp-unavailable";
                }
            }

        }


        protected void lvProducts_PagePropertiesChanged(object sender, EventArgs e)
        {
            string sortExpression = ViewState["SortExpression"]?.ToString();
            lvProducts.DataSource = sortExpression == null ? staffDataSource() : staffDataSource(sortExpression, SortDirections[sortExpression]);
            lvProducts.DataBind();
        }

        protected void lvProducts_ItemCommand(object sender, ListViewCommandEventArgs e)
        {

        }



        protected void lvProducts_Sorting(object sender, ListViewSortEventArgs e)
        {
            toggleSortDirection(e.SortExpression); // Toggle sorting direction for the clicked column


            ViewState["SortExpression"] = e.SortExpression; // used for retain the sorting

            // Re-bind the ListView with sorted data
            lvProducts.DataSource = staffDataSource(e.SortExpression, SortDirections[e.SortExpression]);
            lvProducts.DataBind();
        }

        protected void ddlFilterCategory_DataBound(object sender, EventArgs e)
        {
            ddlFilterCategory.Items.Insert(0, new ListItem("All Categories", "-1"));
        }


        protected void ddlFilterStatus_DataBound(object sender, EventArgs e)
        {
            ddlFilterStatus.Items.Insert(0, new ListItem("All Status", "-1"));
        }

        protected void ddlFilterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sortExpression = ViewState["SortExpression"]?.ToString();
            lvProducts.DataSource = sortExpression == null ? staffDataSource() : staffDataSource(sortExpression, SortDirections[sortExpression]);
            lvProducts.DataBind();
        }

    }
}
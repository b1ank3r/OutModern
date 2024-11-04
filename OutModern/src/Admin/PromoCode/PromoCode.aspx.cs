using OutModern.src.Admin.Staffs;
using OutModern.src.Admin.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Admin.PromoCode
{
    public partial class PromoCode : System.Web.UI.Page
    {
        private string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["MenuCategory"] = "PromoCode";
                lvPromoCodes.DataSource = promoDataSource();
                lvPromoCodes.DataBind();
            }
        }

        //choose to load data source
        private DataTable promoDataSource(string sortExpression = null, string sortDirection = "ASC")
        {
            //get the search term
            string searchTerms = Request.QueryString["q"];

            if (searchTerms != null)
            {
                ((TextBox)Master.FindControl("txtSearch")).Text = searchTerms;
                return FilterDataTable(getPromoCodes(sortExpression, sortDirection), searchTerms);
            }

            else return getPromoCodes(sortExpression, sortDirection);
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

        private DataTable FilterDataTable(DataTable dataTable, string searchTerm)
        {
            // Escape single quotes for safety
            string safeSearchTerm = searchTerm.Replace("'", "''");

            // Build the filter expression with relevant fields
            string expression = string.Format(
                "Convert(PromoId, 'System.String') LIKE '%{0}%' OR " +
                "PromoCode LIKE '%{0}%' OR " +
                "Convert(DiscountRate, 'System.String') LIKE '%{0}%' OR " +
                "Convert(StartDate, 'System.String') LIKE '%{0}%' OR " +
                "Convert(EndDate, 'System.String') LIKE '%{0}%' OR " +
                "Convert(Quantity, 'System.String') LIKE '%{0}%' ",
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
        //DB operation
        //

        //get all promo code available
        protected DataTable getPromoCodes(string sortExpression = null, string sortDirection = "ASC")
        {
            DataTable data = new DataTable();

            string startDateFrom = txtFilterStartDateFrom.Text.Trim();
            string startDateTo = txtFilterStartDateTo.Text.Trim();
            string endDateFrom = txtFilterEndDateFrom.Text.Trim();
            string endDateTo = txtFilterEndDateTo.Text.Trim();

            //validate start date range
            if (!string.IsNullOrEmpty(startDateFrom) && !string.IsNullOrEmpty(startDateTo))
            {
                startDateFrom += " 00:00:00";
                startDateTo += " 23:59:59";

                if (!ValidationUtils.IsValidDateTimeRange(startDateFrom, startDateTo))
                {
                    Page.ClientScript
                        .RegisterStartupScript(GetType(),
                        "Failed to Filter Start Date",
                        $"document.addEventListener('DOMContentLoaded', ()=> alert('Start Date To must after Start Date From'))",
                        true);
                    startDateFrom = "";
                    startDateTo = "";
                }
            }
            //validate end date range
            if (!string.IsNullOrEmpty(endDateFrom) && !string.IsNullOrEmpty(endDateTo))
            {
                endDateFrom += " 00:00:00";
                endDateTo += " 23:59:59";

                if (!ValidationUtils.IsValidDateTimeRange(endDateFrom, endDateTo))
                {
                    Page.ClientScript.RegisterStartupScript(GetType(),
                                            "Failed to Filter End Date",
                                           $"document.addEventListener('DOMContentLoaded', ()=> alert('End Date To must after End Date From'))",
                                           true);
                    endDateFrom = "";
                    endDateTo = "";
                }
            }

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlQuery =
                    "Select * " +
                    "From PromoCode " +
                    "WHERE PromoId = PromoId ";

                if (!string.IsNullOrEmpty(startDateFrom) && !string.IsNullOrEmpty(startDateTo))
                {
                    sqlQuery += "AND StartDate >= @startDateFrom AND StartDate <= @startDateTo ";
                }

                if (!string.IsNullOrEmpty(endDateFrom) && !string.IsNullOrEmpty(endDateTo))
                {
                    sqlQuery += "AND EndDate >= @endDateFrom AND EndDate <= @endDateTo ";
                }

                if (!string.IsNullOrEmpty(sortExpression))
                {
                    sqlQuery += "ORDER BY " + sortExpression + " " + sortDirection;
                }
                else
                {
                    sqlQuery += "ORDER BY EndDate DESC";
                }

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    if (!string.IsNullOrEmpty(startDateFrom) && !string.IsNullOrEmpty(startDateTo))
                    {
                        command.Parameters.AddWithValue("@startDateFrom", startDateFrom);
                        command.Parameters.AddWithValue("@startDateTo", startDateTo);
                    }

                    if (!string.IsNullOrEmpty(endDateFrom) && !string.IsNullOrEmpty(endDateTo))
                    {
                        command.Parameters.AddWithValue("@endDateFrom", endDateFrom);
                        command.Parameters.AddWithValue("@endDateTo", endDateTo);
                    }

                    data.Load(command.ExecuteReader());
                }

            }

            return data;
        }

        // insert new Promo code
        private int insertPromoCode(string promoCode, string discountRate, string startDate, string endDate, string quantity)
        {
            int affectedRow = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlQuery =
                    "Insert into PromoCode (PromoCode, DiscountRate, StartDate, EndDate, Quantity) " +
                    "Values (@promoCode, @discountRate, @startDate, @endDate, @quantity) ";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@promoCode", promoCode);
                    command.Parameters.AddWithValue("@discountRate", discountRate);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
                    command.Parameters.AddWithValue("@quantity", quantity);

                    affectedRow = command.ExecuteNonQuery();
                }
            }

            return affectedRow;
        }

        // update promo code
        private int updatePromoCode(string promoId, string promoCode, string discountRate, string startDate, string endDate, string quantity)
        {
            int affectedRow = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlQuery =
                    "Update PromoCode " +
                    "Set PromoCode = @promoCode, DiscountRate = @discountRate, " +
                    "StartDate = @startDate, EndDate = @endDate, Quantity = @quantity " +
                    "WHERE PromoId = @promoId ";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@promoId", promoId);
                    command.Parameters.AddWithValue("@promoCode", promoCode);
                    command.Parameters.AddWithValue("@discountRate", discountRate);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
                    command.Parameters.AddWithValue("@quantity", quantity);

                    affectedRow = command.ExecuteNonQuery();
                }
            }

            return affectedRow;
        }

        // check promo code exist
        private bool isPromoCodeExist(string promoCode)
        {
            int row = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sqlQuery = "Select Count(*) From PromoCode Where PromoCode = @promoCode";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@promoCode", promoCode);
                    row = int.Parse(command.ExecuteScalar().ToString());
                }
            }

            return row > 0;
        }
        //
        //Page event
        //
        protected void lbAddPromoCode_Click(object sender, EventArgs e)
        {
            string sortExpression = ViewState["SortExpression"]?.ToString();
            lvPromoCodes.DataSource =
                sortExpression == null ?
                promoDataSource() :
                promoDataSource(sortExpression, SortDirections[sortExpression]);
            lvPromoCodes.InsertItemPosition = InsertItemPosition.FirstItem;
            lvPromoCodes.EditIndex = -1;
            lvPromoCodes.DataBind();
        }

        protected void lvPromoCodes_PagePropertiesChanged(object sender, EventArgs e)
        {
            lvPromoCodes.InsertItemPosition = InsertItemPosition.None;
            lvPromoCodes.EditIndex = -1;
            string sortExpression = ViewState["SortExpression"]?.ToString();
            lvPromoCodes.DataSource =
                sortExpression == null ?
                promoDataSource() :
                promoDataSource(sortExpression, SortDirections[sortExpression]);
            lvPromoCodes.DataBind();
        }

        protected void lvPromoCodes_ItemCommand(object sender, ListViewCommandEventArgs e)
        {

        }

        protected void lvPromoCodes_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lvPromoCodes.EditIndex = e.NewEditIndex;
            lvPromoCodes.InsertItemPosition = InsertItemPosition.None;

            string sortExpression = ViewState["SortExpression"]?.ToString();

            lvPromoCodes.DataSource =
                sortExpression == null ?
                promoDataSource() :
                promoDataSource(sortExpression, SortDirections[sortExpression]);
            lvPromoCodes.DataBind();
        }

        protected void lvPromoCodes_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lvPromoCodes.InsertItemPosition = InsertItemPosition.None;
            lvPromoCodes.EditIndex = -1;
            string sortExpression = ViewState["SortExpression"]?.ToString();
            lvPromoCodes.DataSource =
                sortExpression == null ?
                promoDataSource() :
                promoDataSource(sortExpression, SortDirections[sortExpression]);
            lvPromoCodes.DataBind();
        }

        protected void lvPromoCodes_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            ListViewItem item = e.Item;

            // Find the TextBox controls within the item template
            TextBox txtAddPromoCode = (TextBox)item.FindControl("txtAddPromoCode");
            TextBox txtAddDiscountRate = (TextBox)item.FindControl("txtAddDiscountRate");
            TextBox txtStartDate = (TextBox)item.FindControl("txtStartDate");
            TextBox txtEndDate = (TextBox)item.FindControl("txtEndDate");
            TextBox txtAddQuantity = (TextBox)item.FindControl("txtAddQuantity");

            // Get the values from the controls
            string promoCode = txtAddPromoCode.Text.Trim();
            string discountRate = txtAddDiscountRate.Text.Trim();
            string startDate = txtStartDate.Text;
            string endDate = txtEndDate.Text;
            string quantity = txtAddQuantity.Text.Trim();

            // Validation
            //check null
            if (string.IsNullOrEmpty(promoCode)
                || string.IsNullOrEmpty(discountRate)
                || string.IsNullOrEmpty(startDate)
                || string.IsNullOrEmpty(endDate)
                || string.IsNullOrEmpty(quantity))
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                        "alert",
                        $"document.addEventListener('DOMContentLoaded', ()=> alert('Please fill in all fields'))",
                        true);
                return;
            }

            //check date range
            if (!ValidationUtils.IsValidDateTimeRange(startDate, endDate))
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                    "alert", $"document.addEventListener('DOMContentLoaded', ()=> alert('Start Date must be before End Date'))",
                    true);
                return;
            }

            //check discount rate is integer
            if (!int.TryParse(discountRate, out int _))
            {
                Page.ClientScript
                    .RegisterStartupScript(
                    GetType(),
                    "alert",
                    $"document.addEventListener('DOMContentLoaded', ()=> alert('Discount Rate must be an integer'))",
                    true);
                return;
            }

            //check discount rate
            if (!ValidationUtils.IsValidDiscount(int.Parse(discountRate)))
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                    "alert",
                    $"document.addEventListener('DOMContentLoaded', ()=> alert('Discount Rate must be between 0 and 100'))",
                    true);
                return;
            }

            //check quantity is integer
            if (!int.TryParse(quantity, out int _))
            {
                Page.ClientScript
                    .RegisterStartupScript(
                        GetType(),
                        "alert",
                        $"document.addEventListener('DOMContentLoaded', ()=> alert('Quantity must be an integer'))",
                        true);
                return;
            }

            // check the promo code exist
            if (isPromoCodeExist(promoCode))
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                    "alert",
                    $"document.addEventListener('DOMContentLoaded', ()=> alert('Promo Code {promoCode} already exist'))",
                    true);
                return;
            }

            int noOfRowAffected = insertPromoCode(promoCode, discountRate, DateTime.Parse(startDate).ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Parse(endDate).ToString("yyyy-MM-dd HH:mm:ss"), quantity);

            if (noOfRowAffected > 0)
            {

                lblMsg.Text = "**Promo Code added successfully";
                lvPromoCodes.InsertItemPosition = InsertItemPosition.None;
                string sortExpression = ViewState["SortExpression"]?.ToString();
                lvPromoCodes.DataSource =
                    sortExpression == null ?
                    promoDataSource() :
                    promoDataSource(sortExpression, SortDirections[sortExpression]);
                lvPromoCodes.DataBind();
            }
            else
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                                           "alert",
                                    $"document.addEventListener('DOMContentLoaded', ()=> alert('Failed to add Promo Code'))",
                                                                             true);
            }
        }

        protected void lvPromoCodes_Sorting(object sender, ListViewSortEventArgs e)
        {
            toggleSortDirection(e.SortExpression); // Toggle sorting direction for the clicked column

            ViewState["SortExpression"] = e.SortExpression; // used for retain the sorting

            // Re-bind the ListView with sorted data
            lvPromoCodes.DataSource = promoDataSource(e.SortExpression, SortDirections[e.SortExpression]);
            lvPromoCodes.DataBind();
        }

        protected void lvPromoCodes_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            ListViewItem item = lvPromoCodes.Items[e.ItemIndex];

            // Find the TextBox controls within the item template
            Label lblPromoId = (Label)item.FindControl("lblPromoId");
            TextBox txtAddPromoCode = (TextBox)item.FindControl("txtAddPromoCode");
            TextBox txtAddDiscountRate = (TextBox)item.FindControl("txtAddDiscountRate");
            TextBox txtStartDate = (TextBox)item.FindControl("txtStartDate");
            TextBox txtEndDate = (TextBox)item.FindControl("txtEndDate");
            TextBox txtAddQuantity = (TextBox)item.FindControl("txtAddQuantity");

            // Get the values from the controls
            string promoId = lblPromoId.Text;
            string promoCode = txtAddPromoCode.Text.Trim();
            string discountRate = txtAddDiscountRate.Text.Trim();
            string startDate = txtStartDate.Text;
            string endDate = txtEndDate.Text;
            string quantity = txtAddQuantity.Text.Trim();

            // Validation
            //check null
            if (string.IsNullOrEmpty(promoCode)
                || string.IsNullOrEmpty(discountRate)
                || string.IsNullOrEmpty(startDate)
                || string.IsNullOrEmpty(endDate)
                || string.IsNullOrEmpty(quantity))
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                        "alert",
                        $"document.addEventListener('DOMContentLoaded', ()=> alert('Please fill in all fields'))",
                        true);
                return;
            }

            //check date range
            if (!ValidationUtils.IsValidDateTimeRange(startDate, endDate))
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                    "alert", $"document.addEventListener('DOMContentLoaded', ()=> alert('Start Date must be before End Date'))",
                    true);
                return;
            }

            //check discount rate is integer
            if (!int.TryParse(discountRate, out int _))
            {
                Page.ClientScript
                    .RegisterStartupScript(
                    GetType(),
                    "alert",
                    $"document.addEventListener('DOMContentLoaded', ()=> alert('Discount Rate must be an integer'))",
                    true);
                return;
            }

            //check discount rate value
            if (!ValidationUtils.IsValidDiscount(int.Parse(discountRate)))
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                    "alert",
                    $"document.addEventListener('DOMContentLoaded', ()=> alert('Discount Rate must be between 0 and 100'))",
                    true);
                return;
            }

            //check quantity is integer
            if (!int.TryParse(quantity, out int _))
            {
                Page.ClientScript
                    .RegisterStartupScript(
                        GetType(),
                        "alert",
                        $"document.addEventListener('DOMContentLoaded', ()=> alert('Quantity must be an integer'))",
                        true);
                return;
            }

            //get previous promo code
            string previousPromoCode = lvPromoCodes.DataKeys[e.ItemIndex].Values["PromoCode"].ToString();

            // check the promo code exist
            if (previousPromoCode != promoCode && isPromoCodeExist(promoCode))
            {
                Page.ClientScript
                    .RegisterStartupScript(GetType(),
                    "alert",
                    $"document.addEventListener('DOMContentLoaded', ()=> alert('Promo Code {promoCode} already exist'))",
                    true);

                return;
            }

            int noOfRowAffected = updatePromoCode(promoId, promoCode, discountRate, DateTime.Parse(startDate).ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Parse(endDate).ToString("yyyy-MM-dd HH:mm:ss"), quantity);

            if (noOfRowAffected > 0)
            {
                lvPromoCodes.EditIndex = -1;
                string sortExpression = ViewState["SortExpression"]?.ToString();
                lvPromoCodes.DataSource =
                    sortExpression == null ?
                    promoDataSource() :
                    promoDataSource(sortExpression, SortDirections[sortExpression]);
                lvPromoCodes.DataBind();
            }
            else
            {
                Page.Page.ClientScript
                    .RegisterStartupScript(GetType(),
                                "alert",
                    $"document.addEventListener('DOMContentLoaded', ()=> alert('Failed to update Promo Code'))",
                         true);
            }
        }

        protected void txtFilterStartDateFrom_TextChanged(object sender, EventArgs e)
        {
            string sortExpression = ViewState["SortExpression"]?.ToString();
            lvPromoCodes.DataSource = sortExpression == null ?
                promoDataSource() :
                promoDataSource(sortExpression, SortDirections[sortExpression]);
            lvPromoCodes.DataBind();

        }
    }
}
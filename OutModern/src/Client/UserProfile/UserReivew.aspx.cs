using OutModern.src.Admin.Customers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OutModern.src.Client.UserProfile
{
    public partial class UserReivew : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        int customerId;
        protected void Page_Load(object sender, EventArgs e)
        {
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
                BindReviewsToListView();
            }

        }

        private DataTable GetUserReviewHistory()
        {
            DataTable reviewDataTable = new DataTable();
            string filterCriteria = ViewState["FilterCriteria"] as string;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sqlQuery = @"SELECT r.ReviewId, p.ProductId, p.ProductName, p.UnitPrice, c.ColorName, s.SizeName, r.Rating, (SELECT TOP 1 PI.Path FROM ProductImage PI WHERE PI.ProductDetailId = PD.ProductDetailId) AS ProductImageUrl, r.ReviewDescription, r.ReviewDateTime, pd.ProductDetailId, r.CustomerId FROM ProductDetail pd INNER JOIN Product p ON pd.ProductId = p.ProductId INNER JOIN Color c ON pd.ColorId = c.ColorId INNER JOIN Size s ON pd.SizeId = s.SizeId LEFT JOIN Review r ON pd.ProductDetailId = r.ProductDetailId WHERE r.CustomerId = @CustomerId";

                if (filterCriteria == "seller")
                {
                    sqlQuery += " AND EXISTS (SELECT 1 FROM ReviewReply rr WHERE rr.ReviewId = r.ReviewId)";
                }

                sqlQuery += " ORDER BY r.ReviewDateTime DESC";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Load the data into the DataTable
                        reviewDataTable.Load(reader);
                    }
                }
            }
            reviewDataTable.Columns.Add("Replies", typeof(DataTable));
            foreach (DataRow row in reviewDataTable.Rows)
            {
                row["Replies"] = getReviewReplies(row["ReviewId"].ToString());
            }
            return reviewDataTable;
        }

        //get replies for particular id
        private DataTable getReviewReplies(string reviewId)
        {
            // Create a new DataTable to hold the dummy data
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //ReplyTextReplyTimeAdminRoleAdminName
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

        protected void lvReviews_PagePropertiesChanged(object sender, EventArgs e)
        {
            BindReviewsToListView();
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

        protected void btnShowAllReviews_Click(object sender, EventArgs e)
        {
            ViewState["FilterCriteria"] = "all";
            ddpReviews.SetPageProperties(0, ddpReviews.PageSize, true);
            BindReviewsToListView();
        }

        protected void btnShowSellerReplies_Click(object sender, EventArgs e)
        {
            ViewState["FilterCriteria"] = "seller";
            ddpReviews.SetPageProperties(0, ddpReviews.PageSize, true);
            BindReviewsToListView();
        }

        private void BindReviewsToListView()
        {
            lvReviews.DataSource = GetUserReviewHistory();
            lvReviews.DataBind();
        }
    }

}
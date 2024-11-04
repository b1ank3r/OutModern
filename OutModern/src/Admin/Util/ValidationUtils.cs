using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace OutModern.src.Admin.Utils
{
    public static class ValidationUtils
    {
        // check discount value range (int from 0 to 100)
        public static bool IsValidDiscount(int discount)
        {
            return discount >= 0 && discount <= 100;
        }

        // check 2 input dateTime which is string, where end date must be greater than start date
        public static bool IsValidDateTimeRange(string startDate, string endDate)
        {
            return DateTime.Parse(endDate) > DateTime.Parse(startDate);
        }

        //check 2 input date which is string, where end date must be greater than start date
        public static bool IsValidDateRange(string startDate, string endDate)
        {
            return DateTime.Parse(endDate) >= DateTime.Parse(startDate);
        }

        public static bool IsValidPrice(string price)
        {
            Regex priceRegrex = new Regex(@"^[0-9]+(\.[0-9]{0,2})?$");

            return decimal.TryParse(price, out decimal _) && priceRegrex.IsMatch(price);
        }

        public static bool IsEmailExist(string email)
        {
            int exist = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                con.Open();
                string sqlQuery =
                    "SELECT AdminId " +
                    "FROM Admin " +
                    "WHERE AdminEmail = @AdminEmail ";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@AdminEmail", email);
                    if (cmd.ExecuteScalar() != null)
                    {
                        exist = 1;
                    }
                }
            }

            return exist != 0;
        }
        public static bool IsEmailExist(string email, string adminId)
        {
            int exist = 0;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                con.Open();
                string sqlQuery =
                    "SELECT AdminId " +
                    "FROM Admin " +
                    "WHERE AdminEmail = @AdminEmail AND AdminId != @AdminId";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@AdminEmail", email);
                    cmd.Parameters.AddWithValue("@AdminId", adminId);
                    if (cmd.ExecuteScalar() != null)
                    {
                        exist = 1;
                    }
                }
            }

            return exist != 0;
        }
    }
}
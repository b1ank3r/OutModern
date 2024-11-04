using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net;
using System.Net.Mail;

//For Hashing
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace StringUtil
{
    public static class PasswordUtil
    {
        // Hashes the password using SHA256 algorithm
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Verifies if the plain password matches the hashed password
        public static bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            string hashedPlainPassword = HashPassword(plainPassword);
            return hashedPlainPassword == hashedPassword;
        }

        
    }

    public static class EmailUtil
    {
        // Checks if the email already exists in the database
        public static bool IsDuplicateEmail(string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM [Customer] WHERE CustomerEmail = @email";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@email", email);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        return true;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle error
            }
            return false;
        }

        // Checks if the email address is valid
        public static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }

    public static class PhoneUtil
    {
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            // Regular expression pattern for validating phone numbers
            string pattern = @"^[0-9]{10,11}$";

            // Check if the phone number matches the pattern
            return Regex.IsMatch(phoneNumber, pattern);
        }
    }

    public static class AddressUtil
    {
        public static bool IsDuplicateAddressName(string addressName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM [Address] WHERE AddressName = @addressname";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@addressname", addressName);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        return true;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle error
            }
            return false;
        }
    }

}
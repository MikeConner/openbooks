using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Security.Cryptography;

namespace OpenBookPgh
{
	/// <summary>
	/// Summary description for Auth
	/// </summary>
	public class Auth
	{
		public Auth()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		/// <summary>
		/// Checks the password and creates an authentication cookie
		/// </summary>
		/// <param name="page">a reference to the Page where the method was called from</param>
		/// <param name="username">username</param>
		/// <param name="password">password</param>
		/// <returns>True if login successful, otherwise false</returns>
		public static bool Login(Page page, string username, string password)
		{
			bool passwordVerified = false;

			try
			{
				passwordVerified = CheckPassword(username, password);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			if (passwordVerified == true)
			{
				string roles = GetUserRoles(username);

				// Create the authentication ticket
				FormsAuthenticationTicket authTicket = new
					FormsAuthenticationTicket(1,		                    // version
											  username,        				// user name
											  DateTime.Now,					// creation
											  DateTime.Now.AddMinutes(60),	// Expiration
											  false,						// Persistent
											  roles							// User data
											 );

				string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

				HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
				page.Response.Cookies.Add(authCookie);

				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Logs the user out by abandoning session & removing forms authentication cookies
		/// </summary>
		/// <param name="page">page</param>
		public static void Logout(Page page)
		{
			page.Session.Abandon();
			page.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
			FormsAuthentication.SignOut();
			page.Response.Redirect("~/Default.aspx");
		}
		/// <summary>
		/// Checks that supplied password matches value in database. Uses a hashed password & salt.
		/// </summary>
		/// <param name="username">supplied username</param>
		/// <param name="password">supplied password</param>
		/// <returns>True if password matches, False if password does not match</returns>
		public static bool CheckPassword(string username, string password)
		{
			bool passwordMatch = false;

			SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString);
			SqlCommand cmd = new SqlCommand("CheckPassword", conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 255).Value = username;

			try
			{
				conn.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				reader.Read(); 
					string dbPasswordHash = reader.GetString(0);
					string salt = reader.GetString(1);
				reader.Close();

				// Generate hashed password from inputed password and salt
				string hashedPasswordAndSalt = CreatePasswordHash(password, salt);
				
				// Check the hashed password and salt against the value in DB
				passwordMatch = hashedPasswordAndSalt.Equals(dbPasswordHash);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				conn.Close();
			}
			return passwordMatch;
		}
		public static void ResetPassword(int userID, string passwordHash, string salt)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("ResetPassword", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
					cmd.Parameters.Add("@PasswordHash", SqlDbType.VarChar, 50).Value = passwordHash;
					cmd.Parameters.Add("@Salt", SqlDbType.VarChar, 10).Value = salt;
					conn.Open();
					cmd.ExecuteNonQuery();
				}
			}
		}
		
		public static string GetUserRoles(string username)
		{
			string strRoles = string.Empty;
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("GetUserRoles", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.Add("@PermissionGroup", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
					cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = username;
					conn.Open();
					cmd.ExecuteNonQuery();
					strRoles = cmd.Parameters["@PermissionGroup"].Value.ToString();
				}
			}
			return strRoles;
		}
		
		
		public static void AddUser(string firstName, string lastName, string initials, string email, string userName, string passwordHash, string salt)
		{
			using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CityControllerConnectionString"].ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand("AddUser", conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;

					cmd.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = firstName;
					cmd.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = lastName;
					cmd.Parameters.Add("@Email", SqlDbType.VarChar, 250).Value = email;
					cmd.Parameters.Add("@Initials", SqlDbType.VarChar, 50).Value = initials;
					cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 50).Value = userName;
					cmd.Parameters.Add("@PasswordHash", SqlDbType.VarChar, 50).Value = passwordHash;
					cmd.Parameters.Add("@Salt", SqlDbType.VarChar, 10).Value = salt;
					conn.Open();
					cmd.ExecuteNonQuery();
				}
			}
		}
		public static string CreateSalt(int size)
		{
			// Generate a cryptographic random number using the cryptographic
			// service provider
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			byte[] buff = new byte[size];
			rng.GetBytes(buff);
			// Return a Base64 string representation of the random number
			return Convert.ToBase64String(buff);
		}
		public static string CreatePasswordHash(string pwd, string salt)
		{
			string saltAndPwd = String.Concat(pwd, salt);
			string hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "SHA1");
			return hashedPwd;
		}





	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

/// <summary>
/// Summary description for DataAccess
/// </summary>
public class DataAccess
{
    private static String CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;


    public DataAccess()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static bool UserAuthenitcation(string username, string password)
    {
        using (SqlConnection con = new SqlConnection(CS))
        {
            SqlCommand cmd = new SqlCommand("sploginUser", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramusername = new SqlParameter("@Username", username);
            SqlParameter parampassword = new SqlParameter("@Password", password);

            cmd.Parameters.Add(paramusername);
            cmd.Parameters.Add(parampassword);

            con.Open();
            int ReturnCode = (int)cmd.ExecuteScalar();
            return ReturnCode == 1;

        }
    }

    public static string register(string username, string password, string email)
    {
        if (username == "" || password == "" || email == "")
            return "Please fill all fields";

        using (SqlConnection con = new SqlConnection(CS))
        {
            SqlCommand cmd = new SqlCommand("spRegister", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter paramusername = new SqlParameter("@Username", username);
            SqlParameter parampassword = new SqlParameter("@Password", password);
            SqlParameter paramemail = new SqlParameter("@Email", email);

            cmd.Parameters.Add(paramusername);
            cmd.Parameters.Add(parampassword);
            cmd.Parameters.Add(paramemail);

            con.Open();
            int ReturnCode = (int)cmd.ExecuteScalar();
            if (ReturnCode == -1)
            {
                return "Username is already in user please chooose another username";
            }
            else
            {
                return ""; // success
            }
        }
    }

    public static string Upload(string username, HttpPostedFile file)
    {
            string fname = Path.GetFileName(file.FileName);
            string contentType = file.ContentType;
            if (file.ContentLength > 1048576)
            {
                return "File size is greater than 1MB. Please select file less than or equal to 1MB";
            }

            using (Stream fs = file.InputStream)
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    using (SqlConnection con = new SqlConnection(CS))
                    {
                        SqlCommand cmd = new SqlCommand("Insert into tbldata values(@Name,@content,@Data,@Username)", con);

                        cmd.Parameters.AddWithValue("@Name", fname);
                        cmd.Parameters.AddWithValue("@content", contentType);
                        cmd.Parameters.AddWithValue("@Data", bytes);
                        cmd.Parameters.AddWithValue("@Username", username);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                return "";
            }

        }

    public class File
    {
        public string fileName;
        public string contentType;
        public byte[] content;
    }

    public static File GetFileById(int id)
    {
        using (SqlConnection con = new SqlConnection(CS))
        {
            SqlCommand cmd = new SqlCommand("Select * from tbldata where id=@Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                reader.Read();
                File file = new File();
                file.contentType = reader["FileType"].ToString();
                file.fileName = reader["Name"].ToString();
                file.content = (byte[])reader["Data"];
                return file;
            }
        }
    }

    public static string RemoveFile()
    {
        return "Not implemented";
    }

    public static void ListFilesByUser(string username, Action <SqlDataReader> callback)
    {
        using (SqlConnection con = new SqlConnection(CS))
        {
            SqlCommand cmd = new SqlCommand("Select * from tbldata where Username='" + username + "'", con);
            con.Open();
            callback(cmd.ExecuteReader());
        }
    }
}
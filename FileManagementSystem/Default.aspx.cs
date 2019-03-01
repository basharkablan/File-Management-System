using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;

public partial class _Default : System.Web.UI.Page
{
    String CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["username"] != null)
        {
            LinkButton1.Visible = true;
            Panel1.Visible = false;
            Panel2.Visible = true;
        }
        else
        {
            LinkButton1.Visible = false;
            Panel1.Visible = true;
            Panel2.Visible = false;
        }
        if (!IsPostBack)
        {
            GetData();
        }
    }

    protected void btnlogin_Click(object sender, EventArgs e)
    {
        if (UserAuthenitcation(txtusername.Text, txtpassword.Text))
        {
            Session["username"] = txtusername.Text;
            Response.Redirect("Default.aspx");
        }
        else
        {
            lblMessage.ForeColor = Color.Red;
            lblMessage.Text = "Invalid Username and/or Password";
        }
    }

    private bool UserAuthenitcation(string username, string password)
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


    protected void btnRegister_Click(object sender, EventArgs e)
    {
        if (txtrusrename.Text != "" && txtrpasswrod.Text != "" && txtrcpassword.Text != "" && txtemail.Text != "")
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spRegister", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramusername = new SqlParameter("@Username", txtrusrename.Text);
                SqlParameter parampassword = new SqlParameter("@Password", txtrpasswrod.Text);
                SqlParameter paramemail = new SqlParameter("@Email", txtemail.Text);

                cmd.Parameters.Add(paramusername);
                cmd.Parameters.Add(parampassword);
                cmd.Parameters.Add(paramemail);

                con.Open();
                int ReturnCode = (int)cmd.ExecuteScalar();
                if (ReturnCode == -1)
                {
                    lblRegisterMessage.ForeColor = Color.Red;
                    lblRegisterMessage.Text = "Username is already in user please chooose another username";
                }
                else
                {
                    lblRegisterMessage.ForeColor = Color.Green;
                    lblRegisterMessage.Text = "User Register Successfully";
                    txtrusrename.Text = string.Empty;
                    txtemail.Text = string.Empty;
                }
            }
        }
        else
        {
            lblRegisterMessage.ForeColor = Color.Red;
            lblRegisterMessage.Text = "Please fill all fields";
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Session.Contents.RemoveAll();
        Response.Redirect("Default.aspx");
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            HttpPostedFile file = FileUpload1.PostedFile;
            string fname = Path.GetFileName(FileUpload1.PostedFile.FileName);
            string contentType = FileUpload1.PostedFile.ContentType;
            if (file.ContentLength > 1048576)
            {
                Response.Write("<script>alert('File size is greater than 1MB. Please select file less than or equal to 1MB');</script>");
            }
            else
            {
                using (Stream fs = FileUpload1.PostedFile.InputStream)
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
                            cmd.Parameters.AddWithValue("@Username", Session["username"]);
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                }
            }
            Response.Redirect(Request.Url.AbsoluteUri);
        }
        else
        {
            lblUploadMessage.ForeColor = Color.Red;
            lblUploadMessage.Text = "Please select a file to Upload";
        }

    }

    private void GetData()
    {
        using (SqlConnection con = new SqlConnection(CS))
        {
            SqlCommand cmd = new SqlCommand("Select * from tbldata where Username='" + Session["username"] + "'", con);
            con.Open();
            GridView1.DataSource = cmd.ExecuteReader();
            GridView1.DataBind();
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        int id = int.Parse((sender as LinkButton).CommandArgument);
        byte[] bytes;
        string fileName, contentType;
        using (SqlConnection con = new SqlConnection(CS))
        {
            SqlCommand cmd = new SqlCommand("Select * from tbldata where id=@Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                reader.Read();
                bytes = (byte[])reader["Data"];
                contentType = reader["FileType"].ToString();
                fileName = reader["Name"].ToString();
            }
        }
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = contentType;
        Response.AppendHeader("Content-Disposition", "attachment; fileName=" + fileName);
        Response.BinaryWrite(bytes);
        Response.Flush();
        Response.End();
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        int id = int.Parse((sender as LinkButton).CommandArgument);
        byte[] bytes;
        string fileName, contentType;
        using (SqlConnection con = new SqlConnection(CS))
        {
            SqlCommand cmd = new SqlCommand("Select * from tbldata where id=@Id", con);
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                reader.Read();
                bytes = (byte[])reader["Data"];
                contentType = reader["FileType"].ToString();
                fileName = reader["Name"].ToString();
            }
        }
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = contentType;
        Response.AppendHeader("Content-Disposition", "attachment; fileName=" + fileName);
        Response.BinaryWrite(bytes);
        Response.Flush();
        Response.End();
    }
}
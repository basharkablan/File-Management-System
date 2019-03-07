using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data.SqlClient;

public partial class _Default : System.Web.UI.Page
{
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
        if (DataAccess.UserAuthenitcation(txtusername.Text, txtpassword.Text))
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

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        string errorResult;
        Color color;

        if (txtrpasswrod.Text != txtrcpassword.Text)
        {
            color = Color.Red;
            errorResult = "The two passwords do not match";
        }
        else
        {
            errorResult = DataAccess.register(txtrusrename.Text, txtrpasswrod.Text, txtemail.Text);
            if (errorResult == "") // success
            {
                color = Color.Green;
                lblRegisterMessage.Text = "User Registered Successfully";
                txtrusrename.Text = string.Empty;
                txtemail.Text = string.Empty;
            }
            else
            {
                lblRegisterMessage.Text = errorResult;
                color = Color.Red;
            }
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Session.Contents.RemoveAll();
        Response.Redirect("Default.aspx");
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string errorResult;
        if (!FileUpload1.HasFile)
        {
            errorResult = "Please select a file to Upload";
        }
        else
        {
            errorResult = DataAccess.Upload(Session["username"].ToString(), FileUpload1.PostedFile);
        }
        
        if (errorResult == "")
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }
        else
        {
            lblUploadMessage.ForeColor = Color.Red;
            lblUploadMessage.Text = errorResult;
        }
    }

    private void GetData()
    {
        DataAccess.ListFilesByUser(Session["username"] + "", delegate(SqlDataReader data) {
            GridView1.DataSource = data;
            GridView1.DataBind();
        });
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        int id = int.Parse((sender as LinkButton).CommandArgument);
        DataAccess.File file = DataAccess.GetFileById(id);
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = file.contentType;
        Response.AppendHeader("Content-Disposition", "attachment; fileName=" + file.fileName);
        Response.BinaryWrite(file.content);
        Response.Flush();
        Response.End();
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        btnDownload_Click(sender, e);
    }
}
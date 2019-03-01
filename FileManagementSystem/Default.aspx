<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home Page</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" href="lib/bootstrap.min.css" />
    <script src="lib/jquery.min.js"></script>
    <script src="lib/bootstrap.min.js"></script>

    <style>
        * {
            font-family: Arial;
            font-size: 10pt;
        }

        table {
            border: 1px solid #ccc;
            border-collapse: collapse;
        }

        table th {
            background-color: #F7F7F7;
            color: #333;
            font-weight: bold;
        }

        table th, table td {
            padding: 5px;
            border: 1px solid #ccc;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-inverse">
            <div class="container-fluid">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#myNavbar">
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" href="Default.aspx">File Upload System</a>
                </div>
                <div class="collapse navbar-collapse" id="myNavbar">
                    <ul class="nav navbar-nav">
                        <li class="active"><a href="Default.aspx">Home</a></li>

                    </ul>
                    <ul class="nav navbar-nav navbar-right">
                        <li class="active">
                            <asp:LinkButton ID="LinkButton1" Visible="false" OnClick="LinkButton1_Click" runat="server">Log Out</asp:LinkButton></li>
                    </ul>
                </div>
            </div>
        </nav>
        <asp:Panel ID="Panel1" runat="server">
            <div class="container">
                <div class="row">
                    <div id="login-part" class="col-md-4 col-md-offset-1">
                        <h1>Login Here</h1>
                        <br />
                        <br />
                        <div class="form-group">
                            <label for="username">Username</label>
                            <asp:TextBox ID="txtusername" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="password">Password</label>
                            <asp:TextBox ID="txtpassword" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <asp:Button ID="btnlogin" OnClick="btnlogin_Click" CssClass="btn btn-default" runat="server" Text="Login" />
                        <br />
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </div>
                    <div id="register-part" class="col-md-4 col-md-offset-1">
                        <h1>Register Here</h1>
                        <br />
                        <br />
                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                        <div class="form-group">
                            <label for="username">Username:</label>
                            <asp:TextBox ID="txtrusrename" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="password">Password:</label>
                            <asp:TextBox ID="txtrpasswrod" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="cpassword">Confirm Password:</label>
                            <asp:TextBox ID="txtrcpassword" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="email">Email</label>
                            <asp:TextBox ID="txtemail" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <asp:Button ID="btnRegister" OnClick="btnRegister_Click" CssClass="btn btn-default" runat="server" Text="Regisgter" />
                        <br />
                        <asp:Label ID="lblRegisterMessage" runat="server" Text=""></asp:Label>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="Panel2" Visible="false" runat="server">
            <div class="container row">
                <div id="file-part" class="col-md-4 col-md-offset-4">
                    <div class="row">
                        <div class="col-md-8">
                            <asp:FileUpload ID="FileUpload1" CssClass="form-control" runat="server" />
                            <br />
                            <asp:Button ID="btnUpload" OnClick="btnUpload_Click" CssClass="btn btn-default" runat="server" Text="Upload" />
                            <br />
                            <asp:Label ID="lblUploadMessage" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    <br />
                    <br />
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="File Name" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnClick="btnDownload_Click"
                                        CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                                    <asp:LinkButton ID="lnkRemove" runat="server" Text="Remove" OnClick="btnRemove_Click"
                                        CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
    </form>
</body>
</html>

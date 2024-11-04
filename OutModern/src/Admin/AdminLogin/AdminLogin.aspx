<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminLogin.aspx.cs" Inherits="OutModern.src.Client.Login.AdminLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>OutModern | Login</title>

    <!-- import font type css-->
    <link href="<%=Page.ResolveClientUrl("~/lib/fonts/ChakraPetch/ChakraPetch.css") %>" rel="stylesheet" />

    <!-- import fontawesome css-->
    <link href="<%= Page.ResolveClientUrl("~/lib/fontawesome/css/all.min.css") %>" rel="stylesheet" />

    <link href="css/AdminLogin.css" rel="stylesheet" />

    <!-- import tailwind js-->
    <script src="<%= Page.ResolveClientUrl("~/lib/tailwind/tailwind.js") %>"></script>

    <script>
    window.onload = function () {
        // Check if the password has been changed successfully
        var passwordChanged = '<%= Session["PasswordChanged"] %>';
        if (passwordChanged === 'True') {
            alert("Password changed successfully! You can now login.");
            // Reset the session variable to avoid displaying the message again on subsequent page loads
            '<%= Session["PasswordChanged"] = null %>';
        }
    };
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <div class="login-container">
            <div class="column1">

                <div class="topRightBox">
                    <asp:HyperLink ID="hl_cust_login" runat="server" NavigateUrl="~/src/Client/Login/Login.aspx">
        <span class="hl_admin_login_text"><u>Login as Customer</u></span>
                    </asp:HyperLink>
                </div>

                <div class="rightBox">
                    <span class="title">Log In&nbsp</span>
                    <span class="title2">ADMIN</span>

                    <div class="loginBoxItem">
                        <asp:TextBox ID="txt_email" runat="server" class="loginTextBox" placeholder="Email" TextMode="Email"></asp:TextBox>
                    </div>
                    <div class="loginBoxItem">
                        <asp:TextBox ID="txt_password" runat="server" class="loginTextBox" TextMode="Password" placeholder="Password"></asp:TextBox>
                    </div>

                    <!-- Error Message-->
                    <asp:Label ID="ErrMsg" runat="server" Style="color: red"></asp:Label>

                    <div class="loginBoxButton">
                        <asp:Button ID="btn_login" runat="server" class="loginButton" 
                            Text="Log In" CssClass="bg-black hover:bg-gray-700" 
                            Style="width: 100%; border-radius: 10px; padding: 10px; font-family: sans-serif; color: white; font-weight: bold; border: 1px solid #f5f5f5; margin-top: 10px; cursor: pointer;" OnClick="btn_login_Click" />
                    </div>

                    <div style="padding-top: 0.312vw; padding-bottom: 2.604vw;">
                        <asp:HyperLink ID="hl_forget_password" runat="server" Style="font-family: sans-serif; font-size: 1.129rem; color: #94d4ca; cursor: pointer;" NavigateUrl="~/src/Admin/AdminLogin/AdminForgetPassword.aspx">Forget Password</asp:HyperLink>
                    </div>

                </div>
            </div>

            <div class="column2"></div>
        </div>
    </form>
</body>
</html>

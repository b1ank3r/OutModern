<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="OutModern.src.Client.Login.Login2" %>

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

    <link href="css/Login.css" rel="stylesheet" />

    <!-- import tailwind js-->
    <script src="<%= Page.ResolveClientUrl("~/lib/tailwind/tailwind.js") %>"></script>

    <!-- JavaScript code for displaying popup message -->
    <script>
        // Check if the password has been changed successfully
        var PasswordChange = '<%= Session["PasswordChange"] %>';
        if (PasswordChange === 'True') {
            alert("Password changed successfully! You can now login.");
        // Reset the session variable to avoid displaying the message again on subsequent page loads
            '<%= Session["PasswordChange"] = null %>';
        }


        // Check if the profile has been changed successfully
        var profileRegistered = '<%= Session["registered"] %>';
        if (profileRegistered === 'True') {
            alert("Sign Up successfully! Login Now!");
        // Reset the session variable to avoid displaying the message again on subsequent page loads
    '<%= Session["registered"] = null %>';
        }

            
      
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="column1"></div>
            <div class="column2">

                <div class="topRightBox">
                    <asp:HyperLink ID="hl_admin_login" runat="server" NavigateUrl="~/src/Admin/AdminLogin/AdminLogin.aspx">
                    <span class="hl_admin_login_text"><u>Login as Admin</u></span>
                    </asp:HyperLink>
                </div>

                <div class="rightBox">
                    <span class="title">Log In</span>

                    <div class="loginBoxItem">
                        <asp:TextBox ID="txt_email" runat="server" class="loginTextBox" placeholder="Email" TextMode="Email"></asp:TextBox>
                    </div>

                    <div class="loginBoxItem">
                        <asp:TextBox ID="txt_password" runat="server" class="loginTextBox" TextMode="Password" placeholder="Password"></asp:TextBox>
                    </div>

                    <!-- Error Message-->
                    <asp:Label ID="ErrMsg" runat="server" Style="color: red"></asp:Label>

                    <div class="loginBoxButton">
                        <asp:Button ID="btn_login" runat="server" Text="Log In" CssClass="bg-black hover:bg-gray-700" Style="width: 100%; border-radius: 10px; padding: 10px; font-family: sans-serif; color: white; font-weight: bold; border: 1px solid #f5f5f5; margin-top: 10px; cursor: pointer;" OnClick="btn_login_Click" />
                    </div>

                    <div style="padding-top: 0.156vw; padding-bottom: 1.563vw;">
                        <asp:HyperLink ID="hl_forget_password" runat="server" class="hl_forget_password" NavigateUrl="~/src/Client/Login/ForgetPassword.aspx">Forget Password</asp:HyperLink>
                    </div>

                    <div style="padding: 0.417vw; color: darkgrey;">
                        <hr />
                    </div>

                    <div style="padding-top: 0.417vw; padding-bottom: 2.083vw;" class="loginBoxBottom">
                        <span style="margin-top: 1.042vw; padding-left: 4.208vw; font-family: sans-serif; font-size: 1.042rem; color: #b3b3b3;">New to Out Modern?</span>
                        <asp:HyperLink ID="hl_signup" runat="server" class="hl_signup" NavigateUrl="~/src/Client/Login/SignUp.aspx">Sign Up</asp:HyperLink>
                    </div>

                </div>
            </div>
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminResetPassword.aspx.cs" Inherits="OutModern.src.Client.Login.AdminResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>OutModern | Reset Password</title>

    <!-- import font type css-->
    <link href="<%=Page.ResolveClientUrl("~/lib/fonts/ChakraPetch/ChakraPetch.css") %>" rel="stylesheet" />

    <!-- import fontawesome css-->
    <link href="<%= Page.ResolveClientUrl("~/lib/fontawesome/css/all.min.css") %>" rel="stylesheet" />

    <link href="css/ResetPassword.css" rel="stylesheet" />

    <!-- import tailwind js-->
    <script src="<%= Page.ResolveClientUrl("~/lib/tailwind/tailwind.js") %>"></script>
</head>
<body class="bg-cover bg-no-repeat bg-fixed" style="background-image: url('../../../../images/login-img/login-background6.jpg')">
    <form id="form1" runat="server">
        <div class="RPBox">
            <h1 class="RPBoxTitle">Reset Password</h1>

            <div class="RPBoxItem">
                <asp:TextBox ID="txt_new_password" runat="server" class="RPTextBox" TextMode="Password" placeholder="New Password"></asp:TextBox>
            </div>

            <div class="RPBoxItem">
                <asp:TextBox ID="txt_reenter_new_password" runat="server" class="RPTextBox" TextMode="Password" placeholder="Re-enter New Password"></asp:TextBox>
            </div>

            <div class="RPBoxButton">
                <asp:Button ID="btn_reset_password" runat="server" class="RPButton" Text="Reset" CssClass="bg-black hover:bg-gray-700" Style="width: 100%; border-radius: 10px; padding: 10px; font-family: sans-serif; color: white; font-weight: bold; border: 1px solid #f5f5f5; margin-top: 10px; cursor: pointer;" OnClick="btn_reset_password_Click"/>
            </div>

            <asp:Label ID="lblMessage" runat="server"></asp:Label>

        </div>
    </form>
</body>
</html>

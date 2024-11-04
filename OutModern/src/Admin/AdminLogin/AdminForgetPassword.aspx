<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminForgetPassword.aspx.cs" Inherits="OutModern.src.Client.Login.AdminForgetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>OutModern | Forget Password</title>

    <!-- import font type css-->
    <link href="<%=Page.ResolveClientUrl("~/lib/fonts/ChakraPetch/ChakraPetch.css") %>" rel="stylesheet" />

    <!-- import fontawesome css-->
    <link href="<%= Page.ResolveClientUrl("~/lib/fontawesome/css/all.min.css") %>" rel="stylesheet" />

    <link href="css/ForgetPassword.css" rel="stylesheet" />

    <!-- import tailwind js-->
    <script src="<%= Page.ResolveClientUrl("~/lib/tailwind/tailwind.js") %>"></script>
</head>
<body class="min-h-screen bg-cover bg-no-repeat bg-fixed" style="background-image: url('../../../../images/login-img/login-background6.jpg')">
    <form id="form1" runat="server">

        <div class="FPBox">


            <div class="FPBoxTitle">
                <asp:HyperLink ID="hl_back_login" runat="server" class="FPBack" NavigateUrl="~/src/Admin/AdminLogin/AdminLogin.aspx">< Back</asp:HyperLink>
                <div class="ml-0.5 mr-28 font-sans text-3xl text-black font-bold">Forget Password</div>
            </div>

            <div class="FPBoxItem">
                <spam class="text-center pb-4 text-black font-sans text-lg">We will send a link to your email that can reset your password if you have forgotten it.</spam>
            </div>

            <div class="FPBoxItem">
                <spam class="text-center pb-4 text-gray-400 font-sans text-lg">Please enter your email address below for recovery.</spam>
            </div>

            <div class="FPBoxItem">
                <asp:TextBox ID="txt_fp_email" runat="server" class="FPTextBox" placeholder="Email" TextMode="Email"></asp:TextBox>
            </div>

            <div class="FPConfirmButton">
                <asp:Button ID="btn_login" runat="server" class="FPButton" Text="Send Verification" CssClass="bg-black hover:bg-gray-700" Style="width: 100%; border-radius: 1.5625rem; padding: 0.5605vw 1.3417vw; font-family: sans-serif; color: whitesmoke; font-weight: bold; border: 0.052vw solid #f5f5f5; cursor: pointer;" OnClick="btn_login_Click" />
            </div>

            <asp:Label ID="lblMessage" runat="server" />

        </div>

    </form>
</body>
</html>

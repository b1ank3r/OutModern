<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FirstPage.aspx.cs" Inherits="OutModern.src.Client.Login.Login1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>OutModern</title>

    <!-- import font type css-->
    <link href="<%=Page.ResolveClientUrl("~/lib/fonts/ChakraPetch/ChakraPetch.css") %>" rel="stylesheet" />

    <!-- import fontawesome css-->
    <link href="<%= Page.ResolveClientUrl("~/lib/fontawesome/css/all.min.css") %>" rel="stylesheet" />

    <link href="css/FirstPage.css" rel="stylesheet" />

    <!-- import tailwind js-->
    <script src="<%= Page.ResolveClientUrl("~/lib/tailwind/tailwind.js") %>"></script>
</head>

<body>
    <form id="form1" runat="server">

        <!-- Header -->
        <header class="FirstPageHeader">
            <!-- Logo -->
            <div class="flex items-center">
                <h1 class="text-2xl font-bold text-gray-800 ml-20">OUTMODERN</h1>
            </div>
            <asp:Button ID="btn_first_page_signup" runat="server" class="signUpButton" Text="Sign Up" Style="background-color: #f5f5f5;" OnClick="btn_first_page_signup_Click" />
            <asp:Button ID="btn_first_page_login" runat="server" class="loginButton" Text="Log In" Style="background-color: #f5f5f5;" OnClick="btn_first_page_login_Click" />
            <asp:Button ID="btn_admin_login" runat="server" class="adminLoginButton" Text="Admin Login" Style="background-color: black;" OnClick="btn_admin_login_Click"/>
        </header>

        <div class="FirstPageContent">
            <div class="FirstPageWord">
                <span class="statement">Make your outfit the statement</span>
                <div class="nextLine">
                    <span class="statement2">Out Modern offers a curated selection that goes beyond trends, catering to those who want to express themselves through fashion. Browse our collection and discover hidden gems now. Sign up now and get ready to rewrite your style story!</span>
                </div>
                <div class="nextLine">
                    <asp:Button ID="btn_first_page_signup_for_free" runat="server" class="signUpFFButton" Text="Sign Up For Free" Style="background-color: whitesmoke;" OnClick="btn_first_page_signup_for_free_Click" />
                </div>
            </div>

        </div>

    </form>
</body>
</html>

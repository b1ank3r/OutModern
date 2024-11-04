<%@ Page Language="C#" MasterPageFile="~/src/Client/ClientMaster/Client.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="OutModern.src.Client.UserProfile.ChangePassword" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    <link href="css/ChangePassword.css" rel="stylesheet" />

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="RPBox">
    <h1 class="RPBoxTitle">Change Password</h1>

    <div class="RPBoxItem">
        <asp:TextBox ID="txt_new_password" runat="server" class="RPTextBox" TextMode="Password" placeholder="New Password"></asp:TextBox>
    </div>

    <div class="RPBoxItem">
        <asp:TextBox ID="txt_reenter_new_password" runat="server" class="RPTextBox" TextMode="Password" placeholder="Re-enter New Password"></asp:TextBox>
    </div>

    <div class="RPBoxButton">
        <asp:Button ID="btn_reset_password" runat="server" class="RPButton" Text="Reset" CssClass="bg-black hover:bg-gray-700" Style="width: 100%; border-radius: 10px; padding: 10px; font-family: sans-serif; color: white; font-weight: bold; border: 1px solid #f5f5f5; margin-top: 10px; cursor: pointer;" OnClick="btn_reset_password_Click"/>
    </div>

    <asp:Label ID="lblMessage" runat="server" style="color: red"></asp:Label>

</div>
</asp:Content>
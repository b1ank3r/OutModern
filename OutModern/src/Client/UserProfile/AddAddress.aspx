<%@ Page Language="C#" MasterPageFile="~/src/Client/ClientMaster/Client.Master" AutoEventWireup="true" CodeBehind="AddAddress.aspx.cs" Inherits="OutModern.src.Client.UserProfile.AddAddress" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    <link href="css/ChangePassword.css" rel="stylesheet" />

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="RPBox">
        <h1 class="RPBoxTitle">Add Address</h1>

        <div class="RPBoxItem">
            <span class="labelData">Address Name</span>
            <asp:TextBox ID="txt_address_name" runat="server" class="RPTextBox"></asp:TextBox>
            <!-- Error Message-->
            <asp:Label ID="addressNameErrMsg" runat="server" Style="color: red"></asp:Label>
        </div>



        <div class="RPBoxItem">
            <span class="labelData">Address</span>
            <asp:TextBox ID="txt_address_line" runat="server" class="RPTextBox"></asp:TextBox>
            <!-- Error Message-->
            <asp:Label ID="addressLineErrMsg" runat="server" Style="color: red"></asp:Label>
        </div>



        <div class="RPBoxItem">
            <span class="labelData">Country</span>
            <asp:TextBox ID="txt_country" runat="server" class="RPTextBox"></asp:TextBox>
            <!-- Error Message-->
            <asp:Label ID="countryErrMsg" runat="server" Style="color: red"></asp:Label>
        </div>



        <div class="RPBoxItem">
            <span class="labelData">State</span>
            <asp:TextBox ID="txt_state" runat="server" class="RPTextBox"></asp:TextBox>
            <!-- Error Message-->
            <asp:Label ID="stateErrMsg" runat="server" Style="color: red"></asp:Label>
        </div>



        <div class="RPBoxItem">
            <span class="labelData">Postal Code</span>
            <asp:TextBox ID="txt_postal_code" runat="server" class="RPTextBox"></asp:TextBox>
            <!-- Error Message-->
            <asp:Label ID="postalCodeLineErrMsg" runat="server" Style="color: red"></asp:Label>
        </div>



        <div class="RPBoxButton">
            <asp:Button ID="btn_add_address" runat="server" class="RPButton" Text="Add Address" CssClass="bg-black hover:bg-gray-700" Style="width: 100%; border-radius: 10px; padding: 10px; font-family: sans-serif; color: white; font-weight: bold; border: 1px solid #f5f5f5; margin-top: 10px; cursor: pointer;" OnClick="btn_add_address_Click" />
        </div>

        <asp:Label ID="lblMessage" runat="server"></asp:Label>

    </div>
</asp:Content>

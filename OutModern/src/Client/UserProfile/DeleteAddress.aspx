<%@ Page Language="C#" MasterPageFile="~/src/Client/ClientMaster/Client.Master" AutoEventWireup="true" CodeBehind="DeleteAddress.aspx.cs" Inherits="OutModern.src.Client.UserProfile.DeleteAddress" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    <link href="css/ChangePassword.css" rel="stylesheet" />

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="RPBox">
        <h1 class="RPBoxTitle">Delete Address</h1>

        <div class="boxItem">
            <span class="labelData">Address Name</span>
            <asp:DropDownList ID="ddl_address_name" runat="server"
                OnSelectedIndexChanged="ddl_address_name_SelectedIndexChanged"
                class="ddl_address"
                AutoPostBack="True"
                DataTextField="AddressName"
                DataValueField="AddressName">
            </asp:DropDownList>
        </div>

        <div class="boxItem">
            <span class="labelData">Address:</span>
            <asp:Label ID="txt_address_line" runat="server" class="data"></asp:Label>
        </div>

        <div class="boxItem">
            <span class="labelData">Country:</span>
            <asp:Label ID="txt_country" runat="server" class="data"></asp:Label>
        </div>

        <div class="boxItem">
            <span class="labelData">State:</span>
            <asp:Label ID="txt_state" runat="server" class="data"></asp:Label>
        </div>

        <div class="boxItem">
            <span class="labelData">Postal Code:</span>
            <asp:Label ID="txt_postal_code" runat="server" class="data"></asp:Label>
        </div>

        <div class="RPBoxButton">
            <asp:Button ID="btn_delete_address" runat="server" class="RPButton" Text="Delete Address" CssClass="bg-black hover:bg-gray-700" Style="width: 100%; border-radius: 10px; padding: 10px; font-family: sans-serif; color: white; font-weight: bold; border: 1px solid #f5f5f5; margin-top: 10px; cursor: pointer;" OnClick="btn_add_address_Click" />
        </div>

        <asp:Label ID="lblMessage" runat="server"></asp:Label>

    </div>
</asp:Content>

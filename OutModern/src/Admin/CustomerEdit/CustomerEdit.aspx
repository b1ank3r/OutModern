<%@ Page Title="" Language="C#" MasterPageFile="~/src/Admin/AdminMaster/Admin.Master" AutoEventWireup="true" CodeBehind="CustomerEdit.aspx.cs" Inherits="OutModern.src.Admin.CustomerEdit.CustomerEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/tailwindcss">
        /*Edit form*/
        #edit-form {
        }

            #edit-form .edit-section {
                @apply mt-2 ml-2;
            }

            #edit-form .edit-label {
                @apply font-[600];
            }

            #edit-form .edit-item {
                @apply block px-4 py-1 border border-gray-600 rounded w-56;
            }

        .disabled {
            @apply text-gray-400 border-gray-300;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mt-2 mb-10">

        <!-- Cus Desc -->
        <div class="border border-black rounded p-5 w-fit">
            <!--Update btn-->
            <div class="flex items-center justify-between">
                <div class="text-2xl font-bold">Edit Customer</div>
                <div>
                    <asp:Label ID="lblUpdateStatus" runat="server" 
                        CssClass="opacity-50"
                        Text=""></asp:Label>
                    <asp:LinkButton
                        OnClientClick="return confirm('Are you sure you want to update Customer?')"
                        OnClick="lbUpdate_Click"
                        CssClass="text-white p-2 rounded bg-green-500 hover:opacity-50"
                        ID="lbUpdate" runat="server">
                         Save and Update
                    </asp:LinkButton>
                </div>

            </div>
            <div id="edit-form" class="flex mt-4 gap-14">
                <!--Left-->
                <div class="">
                    <!--Customer ID-->
                    <div class="edit-section">
                        <div class="edit-label">Customer ID</div>
                        <div>
                            <asp:Label ID="lblCustomerId" CssClass="edit-item disabled" runat="server" Text="C12"></asp:Label>
                        </div>
                    </div>

                    <!--Customer NAme-->
                    <div class="edit-section">
                        <div class="edit-label">Customer FullName</div>
                        <div>
                            <asp:TextBox ID="txtFullName" CssClass="edit-item" runat="server" Text="CustomerA"></asp:TextBox>
                        </div>
                    </div>

                    <!--Customer Username-->
                    <div class="edit-section">
                        <div class="edit-label">Username</div>
                        <div>
                            <asp:TextBox ID="txtUsername" CssClass="edit-item" runat="server" Text="cus123"></asp:TextBox>

                        </div>
                    </div>


                    <!--Customer Status-->
                    <div class="edit-section">
                        <div class="edit-label">Status</div>
                        <div>
                            <asp:DropDownList ID="ddlStatus" CssClass="edit-item" runat="server">
                                <asp:ListItem>Activated</asp:ListItem>
                                <asp:ListItem>Locked</asp:ListItem>
                                <asp:ListItem>Deleted</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <!--Right-->
                <div>

                    <!-- Customer Email -->
                    <div class="edit-section">
                        <div class="edit-label">Email</div>
                        <div>
                            <asp:TextBox ID="txtEmail" CssClass="edit-item" runat="server" Text="cus123@mail.com"></asp:TextBox>
                        </div>
                    </div>

                    <!-- Customer Phone Num -->
                    <div class="edit-section">
                        <div class="edit-label">Phone No</div>
                        <div>
                            <asp:TextBox ID="txtPhoneNo" CssClass="edit-item" runat="server" Text="012-1234567"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <!--Customer Address-->
        <div class="mt-10 border border-black rounded p-5 w-fit">
            <div class="flex justify-between items-center  gap-5">
                <div class="text-2xl font-bold">Customer Addresses</div>
                <div class="opacity-50">
                    **You cannot edit this part
                </div>
            </div>

            <!--List of Addresses-->
            <div class="flex flex-wrap mt-4 ml-2 gap-4">
                <asp:Repeater ID="rptAddresses" runat="server" OnItemDataBound="rptAddresses_ItemDataBound">
                    <ItemTemplate>
                        <div class="border drop-shadow bg-white py-3 px-5">
                            <div class="flex justify-between">
                                <!-- Address Name -->
                                <div>
                                    <asp:Label CssClass="text-gray-500" ID="lblAddressName" runat="server"
                                        Text='<%# Eval("AddressName") %>'></asp:Label>
                                </div>
                                <!-- Address isDeleted? -->
                                <div>
                                    <asp:Label ID="lblIsDeleted" runat="server"
                                        CssClass="p-1 rounded "
                                        Text='<%# Eval("isDeleted") %>'></asp:Label>
                                </div>
                            </div>

                            <!--Address Line-->
                            <div class="ml-2 mt-3">
                                <div>Address Line</div>
                                <div>
                                    <asp:Label ID="lblAddressLine" runat="server"
                                        CssClass="px-2 py-1 block border-black border rounded text-gray-500"
                                        Text='<%# Eval("AddressLine") %>'></asp:Label>
                                </div>
                            </div>

                            <!--Postal code and State-->
                            <div class="ml-2 mt-3 flex gap-4">
                                <div>
                                    <div>PostalCode</div>
                                    <div>
                                        <asp:Label ID="lblPostalCode" runat="server"
                                            CssClass="px-2 py-1 block border-black border rounded text-gray-500"
                                            Text='<%# Eval("PostalCode") %>'></asp:Label>
                                    </div>
                                </div>
                                <div>
                                    <div>State</div>
                                    <div>
                                        <asp:Label ID="lblState" runat="server"
                                            CssClass="px-2 py-1 block border-black border rounded text-gray-500"
                                            Text='<%# Eval("State") %>'></asp:Label>

                                    </div>
                                </div>
                            </div>

                            <!-- Country-->
                            <div class="ml-2 mt-3 flex gap-4">
                                <div>
                                    <div>Country</div>
                                    <div>
                                        <asp:Label ID="lblCountry" runat="server"
                                            CssClass="px-2 py-1 block border-black border rounded text-gray-500"
                                            Text='<%# Eval("Country") %>'></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

            </div>
        </div>
    </div>
</asp:Content>

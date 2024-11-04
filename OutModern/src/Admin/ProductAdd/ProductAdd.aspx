<%@ Page Title="" Language="C#" MasterPageFile="~/src/Admin/AdminMaster/Admin.Master" AutoEventWireup="true" CodeBehind="ProductAdd.aspx.cs" Inherits="OutModern.src.Admin.ProductAdd.ProductAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/tailwindcss">
        .button {
            @apply text-white bg-green-500 text-center px-1 py-0 cursor-pointer hover:opacity-50 border border-gray-400;
        }

        /*Edit form*/

        #edit-form .edit-section {
            @apply mt-2 ml-2;
        }

        #edit-form .edit-label {
            @apply font-[600];
        }

        #edit-form .edit-item {
            @apply block px-4 py-1 border border-gray-600 rounded;
        }

            #edit-form .edit-item.disabled {
                @apply text-gray-400 border-gray-300;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mt-2">
        <div class="border p-5 border-black rounded w-full">
            <div class="">
                <!-- Prod Desc -->
                <div class="flex gap-10 justify-between">
                    <div class="text-2xl font-bold ">Add Product</div>
                    <asp:LinkButton
                        OnClick="lbAdd_Click"
                        OnClientClick="return confirm('Are you sure you want add new product?');"
                        CssClass="text-white p-2 rounded bg-green-500 hover:opacity-50"
                        ID="lbAdd" runat="server" NavigateUrl='#'>
                        Add Product
                    </asp:LinkButton>
                </div>
            </div>


            <div id="edit-form" class="mt-4 gap-10 flex">
                <div>
                    <!--Prod NAme-->
                    <div class="edit-section">
                        <div class="edit-label">Product Name</div>
                        <div>
                            <asp:TextBox ID="txtProdName" CssClass="edit-item" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <!--Prod Category-->
                    <div class="edit-section">
                        <div class="edit-label">Category</div>
                        <div>
                            <asp:DropDownList EnableViewState="true" ID="ddlCategory" CssClass="edit-item" runat="server"></asp:DropDownList>
                        </div>
                    </div>

                    <!--Prod Price-->
                    <div class="edit-section">
                        <div class="edit-label">Price (RM)</div>
                        <div>
                            <asp:TextBox ID="txtPrice" CssClass="edit-item" runat="server"></asp:TextBox>

                        </div>
                    </div>

                    <%--                    <!--Prod Status-->
                    <div class="edit-section">
                        <div class="edit-label">Status</div>
                        <div>
                            <asp:DropDownList ID="ddlStatus" CssClass="edit-item" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>--%>
                </div>

                <div class="flex-grow w-full">
                    <!--Prod Desc-->
                    <div class="edit-section">
                        <div class="edit-label">Product Description</div>
                        <div>
                            <asp:TextBox
                                ID="txtProdDescription"
                                TextMode="MultiLine"
                                CssClass="edit-item w-full resize-none h-36"
                                runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>

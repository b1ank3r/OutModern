<%@ Page Title="" Language="C#" MasterPageFile="~/src/Admin/AdminMaster/Admin.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="OutModern.src.Admin.Products.Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/tailwindcss">
        @layer components {

            .product-status {
                @apply rounded p-1;
            }

                .product-status.out-of-stock {
                    @apply bg-red-300;
                }

                .product-status.in-stock {
                    @apply bg-green-300;
                }

                .product-status.temp-unavailable {
                    @apply bg-amber-300;
                }
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="mt-2">
        <!--Add item-->
        <asp:HyperLink NavigateUrl="<%#urls[ProductAdd] %>" CssClass="inline-block rounded hover:opacity-60 p-2 box-border bg-green-600 text-white" ID="hyperlinkAddProduct" runat="server">
            <i class="fa-regular fa-plus"></i>
            New Product
        </asp:HyperLink>

        <!-- Filter and add item -->
        <div class="flex justify-between items-center mt-8">
            <div>
                <h2>Product List</h2>
            </div>

            <!-- Filter -->
            <div class="filter-item flex">
                <div class="item">
                    <asp:DropDownList ID="ddlFilterCategory" runat="server"
                        OnSelectedIndexChanged="ddlFilterCategory_SelectedIndexChanged"
                        OnDataBound="ddlFilterCategory_DataBound"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <i class="fa-regular fa-layer-group"></i>
                </div>
                <div class="item">
                    <asp:DropDownList ID="ddlFilterStatus" runat="server"
                        OnSelectedIndexChanged="ddlFilterCategory_SelectedIndexChanged"
                        OnDataBound="ddlFilterStatus_DataBound"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <i class="fa-regular fa-shelves-empty"></i>
                </div>
            </div>
        </div>

        <!-- Display Product -->
        <div class="mt-5">

            <!--Pagination-->
            <asp:DataPager PagedControlID="lvProducts" ID="dpTopProducts" class="pagination" runat="server" PageSize="2">
                <Fields>
                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="False" ShowNextPageButton="False" ShowPreviousPageButton="True" PreviousPageText="<" />
                    <asp:NumericPagerField CurrentPageLabelCssClass="active" ButtonCount="10" />
                    <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="False" ShowNextPageButton="True" ShowPreviousPageButton="False" NextPageText=">" />
                </Fields>
            </asp:DataPager>

            <asp:ListView OnSorting="lvProducts_Sorting" OnItemCommand="lvProducts_ItemCommand"
                OnPagePropertiesChanged="lvProducts_PagePropertiesChanged" ID="lvProducts" runat="server" DataKeyNames="ProductId" OnItemDataBound="lvProducts_ItemDataBound">
                <LayoutTemplate>

                    <table id="data-table" style="width: 100%; text-align: center;">
                        <thead>
                            <tr class="data-table-head">
                                <th class="active">
                                    <asp:LinkButton ID="lbId" runat="server" CommandName="Sort" CommandArgument="ProductId">
                                        ID
                                    </asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lbName" runat="server" CommandName="Sort" CommandArgument="ProductName">
                                        Name
                                    </asp:LinkButton>

                                </th>
                                <th>
                                    <asp:LinkButton ID="lbCategory" runat="server" CommandName="Sort" CommandArgument="ProductCategory">
                                        Category
                                    </asp:LinkButton>
                                </th>
                                <th>Colors
                                </th>
                                <th>
                                    <asp:LinkButton ID="lbPrice" runat="server" CommandName="Sort" CommandArgument="UnitPrice">
                                        Price (RM)
                                    </asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="ProductStatusName">
                                        Status
                                    </asp:LinkButton>
                                </th>
                                <th>Action</th>
                            </tr>
                        </thead>
                        <tr runat="server" id="itemPlaceholder" class="data-table-item"></tr>
                    </table>

                </LayoutTemplate>
                <ItemTemplate>
                    <tr onclick="window.location='<%# Page.ResolveClientUrl(urls[ProductDetails] + "?ProductId=" +  Eval("ProductId") )%>'">
                        <td><%# Eval("ProductId") %></td>
                        <td>
                            <asp:Image ID="imgPath" CssClass="mx-auto object-cover" runat="server" Width="5em" Height="5em" ImageUrl='<%# Eval("Path") %>' />
                            <%# Eval("ProductName") %>
                        </td>
                        <td><%# Eval("ProductCategory") %></td>
                        <td>
                            <div class="flex gap-2 justify-center items-center flex-wrap">
                                <asp:Repeater ID="rptColors" runat="server" DataSource='<%# Eval("Colors") %>'>
                                    <ItemTemplate>
                                        <div style='<%# "background-color: #" + Eval("Color") +";" %>'
                                            class="size-5 rounded-full border drop-shadow border-gray-300 ">
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>

                        </td>
                        <td><%# Eval("UnitPrice", "{0:0.00}") %></td>
                        <td>
                            <span runat="server" id="productStatus" class="product-status"><%# Eval("ProductStatusName") %></span>
                        </td>
                        <td>
                            <asp:HyperLink NavigateUrl='<%#urls[ProductEdit] + "?ProductId=" + Eval("ProductId") %>' runat="server" CssClass="button">
                                <i class="fa-regular fa-pen-to-square"></i>
                            </asp:HyperLink>
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    No data..
                </EmptyDataTemplate>
            </asp:ListView>


            <!--Pagination-->
            <asp:DataPager PagedControlID="lvProducts" ID="dpBottomProducts" class="pagination" runat="server" PageSize="5">
                <Fields>
                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="False" ShowNextPageButton="False" ShowPreviousPageButton="True" PreviousPageText="<" />
                    <asp:NumericPagerField CurrentPageLabelCssClass="active" ButtonCount="10" />
                    <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="False" ShowNextPageButton="True" ShowPreviousPageButton="False" NextPageText=">" />
                </Fields>
            </asp:DataPager>

        </div>
    </div>

</asp:Content>

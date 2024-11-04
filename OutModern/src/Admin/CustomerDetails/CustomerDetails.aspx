<%@ Page Title="" Language="C#" MasterPageFile="~/src/Admin/AdminMaster/Admin.Master" AutoEventWireup="true" CodeBehind="CustomerDetails.aspx.cs" Inherits="OutModern.src.Admin.CustomerDetails.CustomerDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/tailwindcss">
        @layer components {
            .desc-grp {
                @apply mb-2 leading-5;
            }

                .desc-grp .desc-title {
                    @apply text-black text-opacity-65;
                }

                .desc-grp .desc-desc {
                    @apply p-2 border border-gray-300 rounded;
                }

            /*Order status*/
            .status-list a {
                @apply p-2 border;
            }

            .order-status {
                @apply rounded p-1;
            }

                .order-status.order-placed {
                    @apply bg-amber-300;
                }

                .order-status.shipped {
                    @apply bg-green-100;
                }

                .order-status.cancelled {
                    @apply bg-red-300;
                }

                .order-status.received {
                    @apply bg-green-300;
                }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mt-2">

        <div class="mt-8">
            <!-- Customer Info-->
            <div class="">

                <!--title and edit button-->
                <div class="text-xl font-[600] flex items-center gap-2">
                    Customer Info            
                    <asp:HyperLink CssClass="inline-block text-white px-2 rounded bg-amber-500 hover:opacity-50" ID="hlEditOrder" runat="server"
                        NavigateUrl='<%#urls[CustomerEdit] + "?CustomerId=" + customerId %>'>
                    <i class="fa-regular fa-pen-to-square"></i>
                    </asp:HyperLink>
                </div>

                <!--Infomation-->
                <div class="mt-5 w-fit border border-black rounded p-2">
                    <!--Status-->
                    <div class="float-right ">
                        <asp:Label ID="lblStatus" runat="server" CssClass="p-1 rounded"></asp:Label>
                    </div>
                    <!--Profile pic and name-->
                    <div class="flex gap-5 items-center">
                        <div>
                            <asp:Image ID="imgProfile" runat="server"
                                CssClass="rounded-full size-20 object-cover border" />
                        </div>
                        <div>

                            <div>
                                <asp:Label CssClass="font-[600] text-xl" ID="lblFullName" runat="server"></asp:Label>
                            </div>
                            <div>
                                <asp:Label ID="lblUsername" CssClass="text-gray-700" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <!--Info-->
                    <div class="mt-3 flex gap-10">
                        <!--Left-->
                        <div>
                            <!--Id-->
                            <div>
                                <i class="fa-regular fa-id-badge fa-lg mr-2"></i>
                                <asp:Label ID="lblCustomerId" runat="server"></asp:Label>
                            </div>

                            <!--Email-->
                            <div class="mt-2">
                                <i class="fa-regular fa-envelope mr-2"></i>
                                <asp:Label ID="lblEmail" runat="server"></asp:Label>
                            </div>

                            <!--phone-->
                            <div class="mt-2">
                                <i class="fa-regular fa-phone mr-2"></i>
                                <asp:Label ID="lblPhoneNo" runat="server"></asp:Label>
                            </div>
                        </div>

                        <!--Right-->
                        <div class="flex gap-2">
                            <asp:Repeater ID="rptAddress" runat="server">
                                <ItemTemplate>
                                    <div class="flex">
                                        <div>
                                            <i class="fa-regular fa-location-dot mr-2"></i>
                                        </div>
                                        <div>
                                            <div class="text-gray-500"><%#Eval("AddressName") %></div>
                                            <div><%# Eval("AddressLine") %></div>
                                            <div><%# Eval("PostalCode") %>, <%# Eval("State") %> </div>
                                            <div><%# Eval("Country") %></div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblAddressEmpty" runat="server" CssClass="opacity-50"
                                        Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>'
                                        Text="No Address Added..." />
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>


            <!--Customer's Order details-->
            <div class="mt-10">
                <div class="flex items-center justify-between ">
                    <div class="text-xl font-[600] self-center">Orders Made (<asp:Label ID="lblOrderMade" CssClass="text-xl font-[600]" runat="server" Text="Label"></asp:Label>)</div>
                    <div class="filter-item flex mr-10">

                        <!-- Filter Date -->
                        <div class="item">
                            <div>
                                <div class="w-16 float-left">From :</div>
                                <asp:TextBox ID="txtOrderDateFrom"
                                    OnTextChanged="txtOrderDateFrom_TextChanged"
                                    AutoPostBack="true" TextMode="Date" runat="server"></asp:TextBox>
                            </div>
                            <div class="mt-3">
                                <div class="w-16 float-left">To :</div>
                                <asp:TextBox ID="txtOrderDateTo"
                                    OnTextChanged="txtOrderDateFrom_TextChanged"
                                    AutoPostBack="true" TextMode="Date" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <!-- Filter Order Status -->
                        <div class="item self-end">
                            <asp:DropDownList ID="ddlFilterOrderStatus" runat="server"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="ddlFilterOrderStatus_SelectedIndexChanged"
                                OnDataBound="ddlFilterOrderStatus_DataBound">
                            </asp:DropDownList>
                            <i class="fa-regular fa-cart-flatbed-boxes"></i>
                        </div>
                    </div>
                </div>

                <!-- Display Orders -->
                <div class="mt-2 px-10">
                    <!--Pagination-->
                    <asp:DataPager ID="dpTopOrders" class="pagination" runat="server" PagedControlID="lvOrders">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="False" ShowNextPageButton="False" ShowPreviousPageButton="True" PreviousPageText="<" />
                            <asp:NumericPagerField CurrentPageLabelCssClass="active" ButtonCount="10" />
                            <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="False" ShowNextPageButton="True" ShowPreviousPageButton="False" NextPageText=">" />
                        </Fields>
                    </asp:DataPager>

                    <asp:ListView
                        OnItemDataBound="lvOrders_ItemDataBound"
                        OnPagePropertiesChanged="lvOrders_PagePropertiesChanged"
                        OnSorting="lvOrders_Sorting"
                        ID="lvOrders" runat="server">
                        <LayoutTemplate>
                            <table id="data-table" style="width: 100%; text-align: center;">
                                <thead>
                                    <tr class="data-table-head">
                                        <th class="active">
                                            <asp:LinkButton ID="lbId" runat="server" CommandName="Sort" CommandArgument="OrderId">
                                             ID
                                            </asp:LinkButton>
                                        </th>
                                        <th>
                                            <asp:LinkButton ID="lbOrderDateTime" runat="server" CommandName="Sort" CommandArgument="OrderDateTime">
                                                  Order Date
                                            </asp:LinkButton>
                                        </th>
                                        <th>Product Ordered
                                        </th>
                                        <th>
                                            <asp:LinkButton ID="lblTotal" runat="server" CommandName="Sort" CommandArgument="Total">
                                             Total
                                            </asp:LinkButton>
                                        </th>
                                        <th>
                                            <asp:LinkButton ID="lbOrderStatus" runat="server" CommandName="Sort" CommandArgument="OrderStatusName">
                                             Order Status
                                            </asp:LinkButton>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr runat="server" id="itemPlaceholder" class="data-table-item"></tr>
                                </tbody>
                            </table>

                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr onclick="window.location='<%# Page.ResolveClientUrl(urls[OrderDetails] + "?OrderId=" +  Eval("OrderId") )%>'">
                                <td><%# Eval("OrderId") %></td>
                                <td><%# Eval("OrderDateTime", "{0:dd/MM/yyyy </br> h:mm tt}") %></td>
                                <td>
                                    <asp:Repeater ID="rptProducts" DataSource='<%# Eval("ProductDetails") %>' runat="server">
                                        <ItemTemplate>
                                            <div><%# Eval("ProductName")  %> -> <%# Eval("Quantity")  %></div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </td>
                                <td><%# Eval("Total") %></td>
                                <td><span runat="server" id="orderStatus" class="order-status"><%# Eval("OrderStatusName") %></span></td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            No data..
                        </EmptyDataTemplate>
                    </asp:ListView>

                    <!--Pagination-->
                    <asp:DataPager ID="dpBottomOrders" class="pagination" runat="server" PageSize="10" PagedControlID="lvOrders">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="False" ShowNextPageButton="False" ShowPreviousPageButton="True" PreviousPageText="<" />
                            <asp:NumericPagerField CurrentPageLabelCssClass="active" ButtonCount="10" />
                            <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="False" ShowNextPageButton="True" ShowPreviousPageButton="False" NextPageText=">" />
                        </Fields>
                    </asp:DataPager>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

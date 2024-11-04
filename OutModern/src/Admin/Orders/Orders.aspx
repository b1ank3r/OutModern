<%@ Page Title="" Language="C#" MasterPageFile="~/src/Admin/AdminMaster/Admin.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="OutModern.src.Admin.Orders.Orders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/tailwindcss">
        @layer components {
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

            .shipped-button {
                @apply inline-block bg-gray-100 text-black mt-2 rounded p-1 border border-black hover:opacity-50;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="mt-2">
        <!-- Filter and add item -->
        <div class="flex justify-between items-center mt-8">
            <div>
                <h2>Order List</h2>
            </div>

            <!-- Filter -->
            <div class="filter-item flex justify-center items-center">
                <!-- Filter Order Date -->
                <div class="item">
                    <div>
                        <div class="w-16 float-left">From :</div>
                        <asp:TextBox ID="txtFilterOrderDateFrom"
                            OnTextChanged="ddlFilterOrderStatus_SelectedIndexChanged"
                            AutoPostBack="true" TextMode="Date" runat="server"></asp:TextBox>
                    </div>
                    <div class="mt-3">
                        <div class="w-16 float-left">To :</div>
                        <asp:TextBox ID="txtFilterOrderDateTo"
                            OnTextChanged="ddlFilterOrderStatus_SelectedIndexChanged"
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
        <div class="mt-5">
            <!--Pagination-->
            <asp:DataPager ID="dpTopOrders" class="pagination" runat="server" PagedControlID="lvOrders">
                <Fields>
                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="False" ShowNextPageButton="False" ShowPreviousPageButton="True" PreviousPageText="<" />
                    <asp:NumericPagerField CurrentPageLabelCssClass="active" />
                    <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="False" ShowNextPageButton="True" ShowPreviousPageButton="False" NextPageText=">" />
                </Fields>
            </asp:DataPager>

            <asp:Label ID="lblStatusUpdataMsg" runat="server"
                CssClass="block my-5 text-green-600"></asp:Label>

            <asp:ListView
                OnItemDataBound="lvOrders_ItemDataBound"
                OnSorting="lvOrders_Sorting"
                OnPagePropertiesChanged="lvOrders_PagePropertiesChanged"
                DataKeyNames="OrderId"
                OnItemCommand="lvOrders_ItemCommand"
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
                                    <asp:LinkButton ID="lbCustomerName" runat="server" CommandName="Sort" CommandArgument="CustomerFullName">
                                     Customer Name
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
                                    <asp:LinkButton ID="lbTotal" runat="server" CommandName="Sort" CommandArgument="Total">
                                     Total
                                    </asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lbOrderStatus" runat="server" CommandName="Sort" CommandArgument="OrderStatusName">
                                     Order Status
                                    </asp:LinkButton>
                                </th>
                                <th>Update Status</th>
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
                        <td><%# Eval("CustomerName") %></td>
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
                        <td>

                            <asp:LinkButton
                                Visible='<%# Eval("OrderStatusName").ToString() == "Order Placed" %>'
                                CssClass="shipped-button bg-green-500 text-white"
                                OnClientClick="return confirm('Are you sure you want to change the status to Shipped?');"
                                CommandName="ToShipped"
                                CommandArgument='<%# Eval("OrderId") %>'
                                ID="lbStatusToShipped" runat="server">
                                <div>
                                    Shipped
                                </div>
                                        
                            </asp:LinkButton>

                            <asp:LinkButton
                                Visible='<%# Eval("OrderStatusName").ToString() == "Order Placed" %>'
                                CssClass="shipped-button bg-red-500 text-white"
                                OnClientClick="return confirm('Are you sure you want to cancel the order?'); event.stopPropagation();"
                                CommandName="ToCancel"
                                CommandArgument='<%# Eval("OrderId") %>'
                                ID="lbStatusCancel" runat="server">
                                    Cancel
                            </asp:LinkButton>

                            <asp:LinkButton
                                Visible='<%# Eval("OrderStatusName").ToString() == "Shipped" ||  Eval("OrderStatusName").ToString() == "Cancelled"%>'
                                CssClass="shipped-button"
                                OnClientClick="return confirm('Are you sure you want to change the status to Received?');"
                                CommandName="ToPlaced"
                                CommandArgument='<%# Eval("OrderId") %>'
                                ID="lbReturnToPlaced" runat="server">
                                    Return to Placed
                            </asp:LinkButton>


                        </td>
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

    <script>
</script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/src/Admin/AdminMaster/Admin.Master" AutoEventWireup="true" CodeBehind="OrderDetails.aspx.cs" Inherits="OutModern.src.Admin.OrderDetails.OrderDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/tailwindcss">
        @layer components {

            #data-table tbody tr {
                @apply cursor-auto;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mt-2">

        <!-- Order Details-->
        <div class="mt-8">
            <div class="flex gap-10">
                <!-- Display Product Ordered Details-->
                <div class="mt-2 w-full">
                    <!-- title-->
                    <div class="flex mb-5 gap-5 justify-between">
                        <div class="text-2xl font-bold">Order Info</div>
                        <div>
                            <div class="flex gap-2 flex-row-reverse">
                                <asp:Button ID="btnReturnToPlaced"
                                    CssClass="button bg-gray-100 text-black"
                                    OnClientClick="return confirm('Are you sure you want to return this order to placed?');"
                                    OnClick="btnReturnToPlaced_Click"
                                    runat="server" Text="Return to Placed" />
                                <asp:Button ID="btnCancel"
                                    CssClass="button bg-red-500"
                                    OnClientClick="return confirm('Are you sure you want to cancel this order?');"
                                    OnClick="btnCancel_Click"
                                    runat="server" Text="Cancel" />
                                <asp:Button ID="btnShipped"
                                    CssClass="button bg-green-500 text-white"
                                    OnClientClick="return confirm('Are you sure you want to mark this order as shipped?');"
                                    OnClick="btnShipped_Click"
                                    runat="server" Text="Shipped" />
                            </div>
                            <div>
                                <asp:Label ID="lblUpdateStatusMsg" CssClass="opacity-50" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <div class="mb-2">
                        <div>
                            <span class="font-[600]">Order ID :</span>
                            <asp:Label ID="lblOrderID" runat="server"></asp:Label>
                        </div>
                        <div>
                            <span class="font-[600]">Order Date :</span>
                            <asp:Label ID="lblOrderDate" runat="server"></asp:Label>
                        </div>
                        <div>
                            <span class="font-[600]">Order Status :</span>
                            <asp:Label ID="lblOrderStatus" runat="server"></asp:Label>
                        </div>
                    </div>

                    <asp:ListView
                        OnItemCommand="lvProductOrder_ItemCommand"
                        OnItemDataBound="lvProductOrder_ItemDataBound"
                        OnPagePropertiesChanged="lvProductOrder_PagePropertiesChanged"
                        ID="lvProductOrder" runat="server">
                        <LayoutTemplate>
                            <table id="data-table" style="width: 100%; text-align: center;">
                                <thead>
                                    <tr class="data-table-head">
                                        <th>Name
                                        </th>
                                        <th>Size
                                        </th>
                                        <th>Colors
                                        </th>
                                        <th>Price (RM)
                                        </th>
                                        <th>Quantity
                                        </th>
                                        <th>SubTotal (RM)
                                        </th>
                                    </tr>
                                </thead>
                                <tr runat="server" id="itemPlaceholder" class="data-table-item"></tr>
                                <tfoot>
                                    <tr class="border-none text-right">
                                        <td colspan="5" class="font-[600]" style="text-align: right;">Subtotal (RM): </td>
                                        <td class="pr-4">
                                            <asp:Label ID="lblSubtotal" runat="server">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="border-none text-right">
                                        <td colspan="5" class="font-[600]" style="text-align: right;">Delivery Fee (RM): </td>
                                        <td class="pr-4">+<asp:Label ID="lblDeliveryFee" runat="server">
                                        </asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="border-none text-right">
                                        <td colspan="3" class="font-[600] text-right">Promo Code Applied : 
                                        </td>
                                        <td class="text-left">
                                            <asp:Label ID="lblPromoCode" runat="server"></asp:Label>
                                            (<asp:Label ID="lblDiscount" runat="server"></asp:Label>%)
                                        </td>
                                        <td class="font-[600]" style="text-align: right;">Discount (RM):
                                        </td>
                                        <td class="pr-4">-<asp:Label ID="lblDiscountPrice" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="border-none text-right font-bold text-xl">
                                        <td colspan="5" class=" pt-4 font-bold text-xl" style="text-align: right;">Total : </td>
                                        <td class="pr-4 pt-4 font-bold text-xl">RM
                                            <asp:Label ID="lblTotal" CssClass="font-bold text-xl" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </tfoot>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Image ID="imgPath" CssClass="mx-auto" runat="server" Width="4em" ImageUrl='<%# Eval("Path") %>' />
                                    <%# Eval("ProductName") %>
                                </td>
                                <td><%# Eval("SizeName") %></td>
                                <td><%# Eval("ColorName") %></td>
                                <td><%# Eval("UnitPrice", "{0:0.00}") %></td>
                                <td><%# Eval("Quantity") %></td>
                                <td class="text-right" style="padding-right: 1rem;"><%# Eval("Subtotal", "{0:0.00}") %></td>
                            </tr>
                        </ItemTemplate>

                    </asp:ListView>

                </div>

                <div class="flex-shrink-0 sticky top-16 self-start">
                    <!--Customer Detail-->
                    <div class="border border-gray-500 drop-shadow rounded bg-white p-2">
                        <div class="text-xl font-bold mb-3">Customer Info</div>
                        <div>
                            <i class="fa-regular fa-user"></i>
                            <asp:HyperLink ID="hlCustomerDetail"
                                CssClass="hover:underline"
                                runat="server">
                                <asp:Label ID="lblCusName" runat="server"></asp:Label>
                            </asp:HyperLink>
                        </div>
                        <div class="mt-2">
                            <i class="fa-regular fa-envelope"></i>
                            <asp:Label ID="lblCusEmail" runat="server"></asp:Label>
                        </div>
                        <div class="mt-2">
                            <i class="fa-regular fa-phone"></i>
                            <asp:Label ID="lblCusPhoneNo" runat="server"></asp:Label>
                        </div>
                        <div class="mt-2 flex gap-2">
                            <i class="fa-regular fa-location-dot"></i>
                            <div>
                                <div class="opacity-65">
                                    Shipping Address
                                </div>
                                <div>
                                    <asp:Label ID="lblAddressLine" runat="server"></asp:Label>
                                </div>
                                <div>
                                    <asp:Label ID="lblPostalCode" runat="server"></asp:Label>, 
                                        <asp:Label ID="lblState" runat="server"></asp:Label>
                                </div>
                                <div>
                                    <asp:Label ID="lblCountry" runat="server"></asp:Label>
                                </div>
                            </div>

                        </div>
                    </div>
                    <!--Payment Info-->
                    <div class="border border-gray-500 drop-shadow rounded bg-white p-2 mt-5">
                        <div class="text-xl font-bold mb-3">Payment Info</div>
                        <div>
                            <div class="opacity-65">Payment Date</div>
                            <div class="ml-4">
                                <asp:Label ID="lblPaymentDate" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="mt-2">
                            <div class="opacity-65">Payment Method</div>
                            <div class="ml-4">
                                <asp:Label ID="lblPaymentMethod" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

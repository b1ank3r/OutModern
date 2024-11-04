<%@ Page Language="C#" MasterPageFile="~/src/Client/ClientMaster/Client.Master" AutoEventWireup="true" CodeBehind="Cancelled.aspx.cs" Inherits="OutModern.src.Client.UserProfile.Cancelled" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css/MyOrder.css" rel="stylesheet" />

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
    <div class="containColumn">
        <div class="columns-containColumn">
            <!-- left column-->
            <div class="column1">
                <div class="sectionLeft">
                    <%--<div class="leftBox">
                        <asp:Image ID="img_profile" runat="server" class="imgProfile" ImageUrl="~/images/login-img/login-background13.jpg" />
                    </div>--%>

                    <div class="leftBox">
                        <asp:Image ID="img_profile" runat="server" Width="10em" Height="10em" CssClass="product-img object-cover" />
                    </div>

                    <div class="rightBox">
                        <asp:Label ID="lbl_username" runat="server" class="username"></asp:Label>
                    </div>

                    <div class="borderLine"></div>

                    <div class="boxBottom">

                        <asp:Button ID="btn_togo_profile" runat="server" class="button" Text="My Profile" OnClick="btn_togo_profile_Click" />
                        <asp:Button ID="btn_togo_my_order" runat="server" class="button" Text="My Order" OnClick="btn_togo_my_order_Click" />
                        <asp:Button ID="btn_rw_his" runat="server" class="button" Text="Review History" OnClick="btn_rw_his_Click1"/>
                    </div>

                </div>
            </div>


            <!-- right column-->
            <div class="column2">

                <div class="boxRightTop">
                    <asp:Button ID="btn_to_ship" runat="server" Text="To Ship" class="btn_myOrder" OnClick="btn_to_ship_Click" />
                    <asp:Button ID="btn_to_receive" runat="server" Text="To Receive" class="btn_myOrder" OnClick="btn_to_receive_Click" />
                    <asp:Button ID="btn_completed" runat="server" Text="Completed" class="btn_myOrder" OnClick="btn_completed_Click" />
                    <asp:Button ID="btn_cancelled" runat="server" Text="Cancelled" class="btn_myOrder1" Style="background-color: black;" OnClick="btn_cancelled_Click" />
                </div>

                <asp:Label ID="lblStatusUpdataMsg" runat="server"
                    CssClass="block my-5 text-green-600"></asp:Label>

                <asp:ListView
                    DataKeyNames="OrderId"
                    ID="lvOrders" runat="server">
                    <LayoutTemplate>
                        <table id="data-table" style="width: 100%; text-align: center;">
                            <thead>
                                <tr class="data-table-head">
                                    <th>Order Date</th>
                                    <th>Product Ordered</th>
                                    <th>Total</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr runat="server" id="itemPlaceholder" class="data-table-item"></tr>
                            </tbody>
                        </table>

                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr onclick="window.location='<%# Page.ResolveClientUrl(urls[CompletedDetails2] + "?OrderId=" +  Eval("OrderId") )%>'">
                            <td><%# Eval("OrderDateTime", "{0:dd/MM/yyyy </br> h:mm tt}") %></td>
                            <td>
                                <asp:Repeater ID="rptProducts" DataSource='<%# Eval("ProductDetails") %>' runat="server">
                                    <ItemTemplate>
                                        <div><%# Eval("ProductName")  %> -> <%# Eval("Quantity")  %></div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </td>
                            <td><%# Eval("Total") %></td>

                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        No data..
                    </EmptyDataTemplate>
                </asp:ListView>

            </div>
        </div>

    </div>

</asp:Content>

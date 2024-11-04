<%@ Page Language="C#" MasterPageFile="~/src/Client/ClientMaster/Client.Master" AutoEventWireup="true" CodeBehind="CompletedDetails.aspx.cs" Inherits="OutModern.src.Client.UserProfile.CompletedDetails" %>

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



            <asp:ListView
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
                                <th>Review
                                </th>
                            </tr>
                        </thead>
                        <tr runat="server" id="itemPlaceholder" class="data-table-item"></tr>
                        <tfoot>
                            <tr class="border-none text-right">
                                <div style="margin-bottom: 90px;">
                                    <td colspan="5" class="font-[700]" style="text-align: right; font-size: 2rem;">Subtotal : </td>
                                    <td class="pr-4">RM
                                    <asp:Label ID="lblSubtotal" runat="server" style="font-size: 2rem;"></asp:Label>
                                    </td>
                                </div>

                            </tr>

                        </tfoot>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr onclick="window.location='<%# Page.ResolveClientUrl(urls[Comment] + "?ProductDetailId=" +  Eval("ProductDetailId") )%>'">
                        <td>
                            <asp:Image ID="imgPath" CssClass="mx-auto" runat="server" Width="4em" ImageUrl='<%# Eval("Path") %>' />
                            <%# Eval("ProductName") %>
                        </td>
                        <td><%# Eval("SizeName") %></td>
                        <td><%# Eval("ColorName") %></td>
                        <td><%# Eval("UnitPrice", "{0:0.00}") %></td>
                        <td><%# Eval("Quantity") %></td>
                        <td class="text-right" style="padding-right: 1rem;"><%# Eval("Subtotal", "{0:0.00}") %></td>
                        <td><span style="background-color: black; color: white; border-radius: 20%; cursor: pointer; padding: 10px;">Review</span></td>
                    </tr>
                </ItemTemplate>

            </asp:ListView>


        </div>
    </div>
</asp:Content>

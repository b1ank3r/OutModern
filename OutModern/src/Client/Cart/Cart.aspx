<%@ Page Title="Cart" Language="C#" MasterPageFile="~/src/Client/ClientMaster/Client.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="OutModern.src.Client.Cart.Cart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="snackbar" class="-translate-x-1/2 fixed left-1/2 top-20 z-50 hidden transform rounded bg-red-500 px-6 py-3 text-center text-lg text-white shadow-lg" style="z-index: 50;">
        <div class="justify-center">
            <asp:Label ID="lblSnackbar" runat="server" CssClass="text-center text-white" Text=""></asp:Label>
        </div>
    </div>

    <div class="w-[72%] min-h-[60vh] mx-auto pb-20 pt-10">
        <asp:SiteMapPath ID="SiteMapPath1" runat="server" CssClass="mb-4 text-3xl font-bold"></asp:SiteMapPath>
        <h1 class="mb-4 text-3xl font-bold">Checkout</h1>
        <div class="flex justify-between">

            <!--Cart Item-->
            <div class="w-[70%]">

                <div class="rounded-xl bg-white p-6 shadow-md" style="width: 100%">
                    <div class="container mx-auto">
                        <table class="container table-auto">
                            <thead class="border-b">
                                <tr>
                                    <th class="w-3/6 px-4 py-4 text-left text-base font-semibold capitalize text-gray-800">Products</th>
                                    <th class="w-1/6 px-4 py-4 text-base font-semibold capitalize text-gray-800">Price</th>
                                    <th class="w-1/6 px-4 py-4 text-base font-semibold capitalize text-gray-800">Quantity</th>
                                    <th class="w-1/6 px-4 py-4 text-base font-semibold capitalize text-gray-800">Subtotal</th>
                                    <th class="w-1/6 px-4 py-4 text-base font-semibold capitalize text-gray-800"></th>
                                </tr>
                            </thead>
                            <tbody>

                                <asp:ListView ID="ProductListView" runat="server">
                                    <LayoutTemplate>
                                        <tr runat="server" id="itemPlaceholder"></tr>
                                    </LayoutTemplate>
                                    <ItemTemplate>

                                        <asp:HiddenField ID="hidProductDetailId" runat="server" Value='<%# Eval("ProductDetailId") %>' />

                                        <!--Product-->
                                        <tr class="border-b border-gray-200">
                                            <td class="px-4 py-6">
                                                <div class="flex items-center">
                                                    <asp:Image ID="imgProduct" runat="server" AlternateText="Image" ImageUrl='<%# Eval("ProductImageUrl") %>' CssClass="w-16 h-16 mr-4 cursor-pointer" onclick='<%# Eval("ProductId", "window.location.href = \"/src/Client/ProductDetails/ProductDetails.aspx?productID={0}\";") %>' />
                                                    <div>
                                                        <div class="cursor-pointer text-lg font-bold capitalize text-black" onclick='<%# Eval("ProductId", "window.location.href = \"/src/Client/ProductDetails/ProductDetails.aspx?productID={0}\";") %>'><%# Eval("ProductName") %></div>
                                                        <div class="text-sm text-gray-500">Color: <%# Eval("ColorName") %></div>
                                                        <div class="text-sm text-gray-500">Size: <%# Eval("SizeName") %></div>
                                                    </div>
                                                </div>
                                            </td>

                                            <!--Price-->
                                            <td class="px-4 py-2 text-center font-bold">RM<%# Eval("UnitPrice", "{0:N2}") %></td>

                                            <!--Quantity Button-->
                                            <td class="px-6 py-2">
                                                <div class="flex items-center justify-center rounded-md border border-gray-300">
                                                    <asp:LinkButton runat="server" ID="btnDecrement" CssClass="cursor-pointer px-2 py-1 text-2xl text-gray-600 hover:text-gray-800" Text="-" OnClick="btnDecrement_Click" />
                                                    <asp:TextBox runat="server" ID="txtQuantity" CssClass="w-12 border-none text-center focus:outline-none" Text='<%# Eval("Quantity") %>' ReadOnly="true" />
                                                    <asp:LinkButton runat="server" ID="btnIncrement" CssClass="cursor-pointer px-2 py-1 text-2xl text-gray-600 hover:text-gray-800" Text="+" OnClick="btnIncrement_Click" />
                                                </div>

                                            </td>
                                            <td class="px-4 py-2 text-center font-bold">RM<%# Eval("Subtotal", "{0:N2}") %></td>
                                            <td class="px-4 py-2">
                                                <asp:LinkButton runat="server" ID="btnDelete" CssClass="cursor-pointer text-red-600 hover:text-red-800" Text="Delete" OnClick="btnDelete_Click" aria-label="Delete">
                                                <i class="fa-light fa-trash-can text-xl" style="color: #ff0000;"></i>
                                                </asp:LinkButton>
                                            </td>


                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </tbody>

                        </table>

                        <div class="mt-7 flex flex-col items-center justify-center">
                            <div>
                                <asp:Image ID="ImgEmptyCart" runat="server" ImageUrl="~/images/cartEmpty.png" CssClass="h-40 w-44" />
                            </div>
                            <div>
                                <asp:Label ID="lblEmptyCart" runat="server" CssClass="text-center text-xl" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!--OrderSummary-->
            <div class="w-1/4 overflow-y-auto rounded-md bg-white p-6 shadow-md">
                <div class="mb-4">
                    <div class="flex flex-wrap justify-between border-b border-gray-300 pb-4">
                        <h2 class="text-xl font-bold">Subtotal</h2>
                        <asp:Label ID="lblSubtotal" runat="server" CssClass="font-bold" Text="RM200.00"></asp:Label>
                    </div>

                    <div class="mt-4">
                        <span class="text-sm">Enter Discount Code</span>
                    </div>

                    <asp:Panel ID="panelApplyPromo" runat="server" DefaultButton="btnApply">
                        <div class="border-2 mb-2 flex items-center rounded-lg border-black">
                            <asp:TextBox runat="server" ID="txtDiscountCode" CssClass="w-full flex-grow rounded-lg px-4 py-4 text-sm outline-none" />
                            <asp:Button runat="server" ID="btnApply" CssClass="right-0 cursor-pointer rounded-md bg-black px-6 py-4 text-sm text-white hover:bg-gray-800" Text="Apply" OnClick="btnApply_Click" />
                        </div>
                    </asp:Panel>

                    <div class="flex flex-wrap justify-between">
                        <asp:Label ID="lblCodeError" runat="server" CssClass="text-red-700" Text=""></asp:Label>
                    </div>

                    <div class="flex flex-wrap justify-between">
                        <span>Discount</span>
                        <div>
                            <asp:Label ID="lblDiscountRate" runat="server" CssClass="text-gray-700" Text="(0%)"></asp:Label>
                            <asp:Label ID="lblDiscount" runat="server" CssClass="text-gray-700" Text="RM0.00"></asp:Label>
                        </div>
                    </div>

                    <div class="flex flex-wrap justify-between border-b border-gray-300 pb-4">
                        <span>Delivery Charge</span>
                        <asp:Label ID="lblDeliveryCost" runat="server" CssClass="text-gray-700" Text="RM5.00"></asp:Label>
                    </div>
                    <div class="mt-4 flex flex-wrap justify-between text-lg font-bold">
                        <h2 class="mb-4 text-xl font-bold">Grand Total</h2>
                        <asp:Label ID="lblGrandTotal" runat="server" CssClass="font-bold" Text="RM200.00"></asp:Label>
                    </div>
                </div>

                <asp:Button runat="server" ID="btnCheckout" CssClass="w-full cursor-pointer rounded-md bg-black py-3 text-white hover:bg-gray-800" Text="Proceed to Checkout" OnClick="btnCheckout_Click" />

            </div>
        </div>
    </div>

    <script>
        window.showSnackbar = function () {
            var snackbar = document.getElementById("snackbar");

            snackbar.classList.remove('hidden');
            snackbar.classList.add('flex');

            setTimeout(function () {
                snackbar.classList.remove('flex');
                snackbar.classList.add('hidden');
            }, 3000);
        }
    </script>
</asp:Content>

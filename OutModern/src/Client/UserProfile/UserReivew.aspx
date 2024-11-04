<%@ Page Language="C#" MasterPageFile="~/src/Client/ClientMaster/Client.Master" AutoEventWireup="true" CodeBehind="UserReivew.aspx.cs" Inherits="OutModern.src.Client.UserProfile.UserReivew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .datapagerStyle {
            padding: 8px 12px;
            margin: 0 3px;
            border: 1px solid black;
            border-radius: 4px;
            color: #333;
            background-color: #fff;
            cursor: pointer;
        }

            .datapagerStyle:hover, .active {
                padding: 8px 12px;
                margin: 0 3px;
                border: 1px solid black;
                border-radius: 4px;
                background-color: black;
                color: #fff;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mx-auto py-8">
        <h1 class="mb-6 text-3xl font-bold">User Review History</h1>
        <div class="mt-8 rounded-lg bg-white p-6 shadow-md">
            <h1 class="mb-6 text-3xl font-semibold">My Reviews</h1>
            <div class="flex flex-wrap gap-4">
                <asp:Button ID="btnShowAllReviews" runat="server" Text="Show All Reviews" CssClass="btnShowReviews cursor-pointer rounded-md bg-gray-800 px-6 py-3 text-white hover:bg-gray-700" OnClick="btnShowAllReviews_Click" />
                <asp:Button ID="btnShowSellerReplies" runat="server" Text="Show Seller Replies" CssClass="btnShowReviews cursor-pointer rounded-md bg-gray-800 px-6 py-3 text-white hover:bg-gray-700" OnClick="btnShowSellerReplies_Click" />
            </div>
            <div class="grid-cols-1 grid gap-5">
                <!-- Review Item -->
                <asp:ListView ID="lvReviews" runat="server" OnPagePropertiesChanged="lvReviews_PagePropertiesChanged">
                    <LayoutTemplate>
                        <div runat="server" id="itemPlaceholder"></div>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <div class="mb-4 flex items-start rounded-lg bg-white p-6 shadow-md">
                            <!-- Product Image -->
                            <asp:Image ImageUrl='<%# Eval("ProductImageUrl") %>' alt="Product Image" class="w-48 h-48 object-cover rounded-md mr-9 cursor-pointer" ID="Image1" runat="server" onclick='<%# Eval("ProductID", "window.location.href = \"/src/Client/ProductDetails/ProductDetails.aspx?ProductId={0}\";") %>' />

                            <!-- Review Details -->
                            <div class="w-full">
                                <h2 class="mb-2 cursor-pointer text-xl font-semibold hover:underline" onclick='<%# Eval("ProductID", "window.location.href = \"/src/Client/ProductDetails/ProductDetails.aspx?ProductId={0}\";") %>'><%# Eval("ProductName") %></h2>
                                <p class="mb-2 text-gray-600">Rating: <%# GenerateStars(Convert.ToDouble(Eval("Rating"))) %></p>
                                <p class="mb-2 text-gray-600">Variation: <%# Eval("ColorName") %> Color, <%# Eval("SizeName") %> Size</p>
                                <div class="mb-2 rounded-md bg-gray-100 p-4 pb-2">
                                    <p class="text-gray-800"><%# Eval("ReviewDescription") %></p>
                                    <p class="mb-2 mt-2 text-gray-600"><%# Eval("ReviewDateTime") %></p>
                                </div>
                                <!-- Review Replies Repeater -->
                                <asp:Repeater ID="rptReplies" runat="server" DataSource='<%# Eval("Replies") %>'>
                                    <ItemTemplate>
                                        <div class="mt-4 w-full bg-gray-100 py-0.5">
                                            <div class="overflow-hidden rounded-lg bg-white shadow-lg">
                                                <div class="px-6 py-4">
                                                    <p class="font-bold text-black">Seller's Response:</p>
                                                    <p class="mt-2 text-gray-700">
                                                        <%# Eval("ReplyText") %>
                                                    </p>
                                                    <p class="mt-2"><%# Eval("ReplyTime") %></p>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
                <div class="flex justify-center">
                    <asp:DataPager ID="ddpReviews" CssClass="data-pager" runat="server" PageSize="5" PagedControlID="lvReviews">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="datapagerStyle" ShowFirstPageButton="False" ShowNextPageButton="False" ShowPreviousPageButton="True" PreviousPageText="<" />
                            <asp:NumericPagerField NumericButtonCssClass="datapagerStyle" CurrentPageLabelCssClass="active" ButtonCount="5" />
                            <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="datapagerStyle" ShowLastPageButton="False" ShowNextPageButton="True" ShowPreviousPageButton="False" NextPageText=">" />
                        </Fields>
                    </asp:DataPager>
                </div>
            </div>
        </div>
    </div>
</asp:Content>


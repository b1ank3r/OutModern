<%@ Page Language="C#" MasterPageFile="~/src/Client/ClientMaster/Client.Master" AutoEventWireup="true" CodeBehind="ProductDetails.aspx.cs" Inherits="OutModern.src.Client.ProductDetails.ProductDetails" MaintainScrollPositionOnPostback="true" EnableViewState="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="productDetails.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="p-30 mx-auto mb-10 mt-10 flex w-full flex-col md:p-15" style="min-width: 1200px;">
        <div class="mb-5 ml-10 flex w-2/5 flex-wrap items-center justify-center">
            <div class="flex w-4/5 space-x-2">
                <span class="text-sm text-black">
                    <asp:HyperLink ID="HyperLink1" NavigateUrl="~/src/Client/Home/Home.aspx" runat="server" CssClass="text-sm text-black hover:underline">Home</asp:HyperLink>
                    >
                </span>
                <span class="text-sm text-black">
                    <asp:HyperLink ID="HyperLink2" NavigateUrl="~/src/Client/Products/Products.aspx" runat="server" CssClass="text-sm text-black hover:underline">Products</asp:HyperLink>
                    >
                </span>
                <asp:Label ID="productNameUrl" runat="server" Text="Label" CssClass="text-sm text-black"></asp:Label>
            </div>
        </div>
        <div class="flex w-full">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="flex w-full">
                        <div class="ml-10 flex w-2/5 flex-wrap items-center justify-center" style="min-width: 570px;">
                            <div class="w-4/5">
                                <div id="myModal" class="image-modal">
                                    <span class="close-modal" onclick="closeModal()">&times;</span>
                                    <img src="" class="image-modal-content" id="modal-img">
                                </div>
                                <div class="relative max-w-7xl bg-gray-300 bg-opacity-50" id="image-container">
                                    <asp:Repeater ID="MainImageRepeater" runat="server">
                                        <ItemTemplate>
                                            <asp:Image ID="MainImage" CssClass="slides" runat="server"
                                                Style='<%# Container.ItemIndex == 0 ? "width: 100%; cursor: pointer;": "width: 100%; display: none; cursor: pointer;" %>'
                                                ImageUrl='<%# Eval("ImageUrl") %>'
                                                onclick='<%# "modal(this.src)" %>' />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <img class="pointer-events-none absolute right-2 top-2" src="../../../images/product-img/zoom-in.png" alt="Zoom In">

                                    <a class="mt-[-22px] absolute left-0 top-1/2 cursor-pointer select-none rounded-r-lg bg-black px-4 py-2 text-lg font-bold text-white transition duration-300" onclick="plusSlides(-1)">&#10094;</a>
                                    <a class="mt-[-22px] absolute right-0 top-1/2 cursor-pointer select-none rounded-r-lg bg-black px-4 py-2 text-lg font-bold text-white transition duration-300" onclick="plusSlides(1)">&#10095;</a>

                                    <div class="mb-4 mt-4 px-2">
                                        <asp:Repeater ID="ImageRepeater" runat="server">
                                            <ItemTemplate>
                                                <div class="float-left w-1/3 px-2 py-1">
                                                    <asp:Image ID="Image1" ImageUrl='<%# Eval("ImageUrl") %>' runat="server" CssClass="demo opacity hover-opacity-off"
                                                        Style="width: 100%; cursor: pointer" onclick='<%# "currentDiv(" + (Container.ItemIndex + 1) + ")" %>' />
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="mx-auto mr-10 flex w-3/5 flex-col py-2" style="min-width: 60%;">
                            <div class="w-4/5">
                                <h1 class="mb-2">
                                    <asp:Label ID="lblProdName" runat="server" Text="Label" class="text-4xl font-bold text-black">
                                    </asp:Label>
                                </h1>

                                <div class="mb-2 flex items-center">
                                    <div runat="server" id="ratingStars" class="flex items-center">
                                    </div>
                                    <a href="#review" class="ml-2 mt-0.5 hover:underline">
                                        <asp:Label ID="lblReviews" runat="server" Text="Label">
                        
                                        </asp:Label>
                                    </a>
                                </div>
                                <asp:Label ID="lblDescription" runat="server" class="mb-2 mt-1.5 text-gray-700"></asp:Label>
                                <div class="mb-2 mt-2 flex items-center text-3xl">
                                    <asp:Label ID="lblPrice" runat="server" Text="Label" class="text-3xl font-bold text-black"></asp:Label>
                                </div>
                                <div class="mb-5">
                                    <h3 class="mb-3 text-lg font-semibold">Color:
                             <asp:Label ID="lblColor" runat="server" Text="" Visible="false"></asp:Label></h3>
                                    <div class="colorGrp flex gap-3 space-x-2">
                                        <asp:SqlDataSource ID="ColorSqlDataSource" runat="server" ConnectionString='<%$ ConnectionStrings:ConnectionString %>' SelectCommand="SELECT DISTINCT pd.ColorId, c.ColorName, c.HexColor FROM Product p, Color c, ProductDetail pd WHERE p.ProductId = @ProductId AND pd.ColorId = c.ColorId AND p.ProductId = pd.ProductId AND isDeleted = 0">
                                            <SelectParameters>
                                                <asp:QueryStringParameter QueryStringField="ProductId" Name="ProductId"></asp:QueryStringParameter>
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:Repeater ID="ColorRepeater" runat="server" DataSourceID="ColorSqlDataSource" OnItemCommand="ColorRepeater_ItemCommand" OnItemDataBound="ColorRepeater_ItemDataBound">
                                            <ItemTemplate>
                                                <label class="inline-flex cursor-pointer items-center">
                                                    <asp:LinkButton ID="lbtnColor" CommandName="SelectColor" value='<%# Eval("ColorName") %>' CommandArgument='<%# Eval("ColorId") %>' data-colorId='<%# Eval("ColorId") %>' runat="server"
                                                        CssClass='<%# "border-opacity-10 h-9 w-9 rounded-full border border-gray-600" + (ViewState["ColorId"] != null && ViewState["ColorId"].ToString() == Eval("ColorId").ToString() ? " selectedColor" : "") %>' Style='<%# "background-color: #"+Eval("HexColor")+";" %>'></asp:LinkButton>
                                                </label>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                                <div class="mb-7">
                                    <h3 class="mb-3 text-lg font-semibold">Size:
                                     <asp:Label ID="lblSize" runat="server" Text="" Visible="false"></asp:Label></h3>
                                    <div class="flex gap-3 space-x-2">
                                        <asp:SqlDataSource ID="SizeSqlDataSource" runat="server" ConnectionString='<%$ ConnectionStrings:ConnectionString %>' SelectCommand="SELECT pd.SizeId, SizeName FROM [ProductDetail] AS pd INNER JOIN [Size] AS s ON pd.SizeId = s.SizeId WHERE ProductId = @ProductId GROUP BY pd.SizeId, SizeName">
                                            <SelectParameters>
                                                <asp:QueryStringParameter QueryStringField="ProductId" Name="ProductId"></asp:QueryStringParameter>
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:Repeater ID="SizeRepeater" DataSourceID="SizeSqlDataSource" runat="server" OnItemCommand="SizeRepeater_ItemCommand" OnItemDataBound="SizeRepeater_ItemDataBound">
                                            <ItemTemplate>
                                                <label class="inline-flex cursor-pointer items-center">
                                                    <asp:LinkButton ID="lbtnSize" CommandName="SelectSize" value='<%# Eval("SizeName") %>' CommandArgument='<%# Eval("SizeId") %>' data-sizeId='<%# Eval("SizeId") %>' runat="server"
                                                        CssClass='<%# "flex items-center justify-center rounded-md px-4 py-3 hover:bg-gray-300" + (ViewState["SizeId"] != null && ViewState["SizeId"].ToString() == Eval("SizeId").ToString() ? " selectedSize" : "") %>' Style="min-height: 2.5rem; min-width: 5rem; border: 1px solid rgba(0, 0, 0, .20);"><%# Eval("SizeName") %></asp:LinkButton>
                                                </label>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                                <div class="mb-8">
                                    <div class="flex items-center">
                                        <div class="mr-5">
                                            <h3 class="text-lg font-semibold">Quantity: </h3>
                                        </div>
                                        <div class="flex items-center rounded border">
                                            <asp:Button ID="btnDecrease" runat="server" CssClass="cursor-pointer rounded-l bg-gray-200 px-3 py-2 hover:bg-gray-300" Text="-" OnClick="btnDecrease_Click" />
                                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="w-12 py-2 text-center" Text="1" onchange=" validateQuantity()" Max=""></asp:TextBox>
                                            <asp:Button ID="btnIncrease" runat="server" CssClass="cursor-pointer rounded-r bg-gray-200 px-3 py-2 hover:bg-gray-300" Text="+" OnClick="btnIncrease_Click" />
                                        </div>
                                        <span class="ml-4 text-gray-600">
                                            <asp:Label ID="lblQuantity" class="ml-4 text-gray-600" runat="server" Text="Label"></asp:Label>
                                            pieces available
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="mb-5" style="border-top: 1.5px solid lightgray">
                                <div class="mt-7 flex items-center">
                                    <div class="mr-5">
                                        <asp:LinkButton ID="AddToCart" runat="server" CssClass="flex-1 flex min-w-20 cursor-pointer items-center justify-center rounded-md border border-transparent bg-black px-8 py-3 text-base font-medium text-white hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-offset-gray-50 focus:ring-indigo-500 sm:w-full" OnClick="AddToCart_Click">
     <i class="fas fa-shopping-cart mr-2 text-white"></i> Add To Cart
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ColorRepeater" EventName="ItemCommand" />
                    <asp:AsyncPostBackTrigger ControlID="SizeRepeater" EventName="ItemCommand" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="mt-12">
        <div class="border-t-2 border-black">
            <span class="">
                <p class="mx-auto mt-4 text-center text-xl font-bold underline" style="line-height: 3.75;">Description</p>
            </span>
        </div>
        <div class="w-30 min-w-[300px] mx-auto my-4 grid place-items-center">
            <ul class="product-desc-ul">
                <li class="list-disc">100% cotton fabric is both soft and gentle on the skin.</li>
                <li class="list-disc">Simple, sleek design to match any occasion.</li>
                <li class="list-disc">Four sizes available for a perfect fit.</li>
                <li class="list-disc">Two pockets: one on either side of the shirt; as well as one chest pocket to keep your
                            valuables secure and close at hand.</li>
            </ul>
        </div>
        <asp:Image ID="ImgSizeChart" runat="server" Style="width: 60%; margin: 1em auto; display: block;"/>
      
        <p style="text-align: center;" class="pt-2">*Difference of 1-2cm may occur</p>
        <p style="text-align: center; font-size: 1.1rem;" class="p-1 underline"><b>Size Chart</b></p>
    </div>
    <div class="max-w-1200 mx-auto mt-5 flex w-4/5 flex-wrap justify-center gap-3">
        <div class="flex flex-col items-center justify-center gap-5 rounded-lg p-6 text-lg shadow-lg" id="review">
            <p class="text-2xl text-black">Average User Rating</p>
            <p class="text-gray-500">
                <asp:Label ID="lblAvgRatings" runat="server" Text="4.0" class="text-2xl font-bold text-black"></asp:Label>
                / 5.0
            </p>
            <div runat="server" id="ratingStar2" class="ratingStar flex gap-4">
            </div>

            <div class="flex items-center gap-4 text-xs">
                <i class="fas fa-user text-black" style="font-weight: bold !important;"></i>
                <span class="text-lg text-black">
                    <asp:Label ID="lblTotalReview" runat="server" Text="238" class="text-lg text-black"></asp:Label>
                    Total Reviews</span>
            </div>
        </div>
        <div class="flex-1 min-w-[300px] flex flex-col justify-center rounded-lg p-6 text-lg shadow-lg">
            <div class="mb-3 flex items-center gap-4 text-base">
                <p style="width: 10px">5</p>
                <i class="fas fa-star text-black" style="font-weight: bold !important;"></i>
                <div class="relative w-full rounded-full bg-gray-400 bg-opacity-50" style="height: 12.5px;">
                    <div id="starBar5" runat="server" style="" class="absolute h-full rounded-full bg-green-500"></div>
                </div>
                <p class="text-left">
                    <asp:Label ID="lbl5star" runat="server" Text="80"></asp:Label>%
                </p>
            </div>
            <div class="mb-3 flex items-center gap-4 text-base">
                <p style="width: 10px" class="text-center">4</p>
                <i class="fas fa-star text-black" style="font-weight: bold !important;"></i>
                <div class="relative w-full rounded-full bg-gray-400 bg-opacity-50" style="height: 12.5px;">
                    <div id="starBar4" runat="server" style="" class="absolute h-full rounded-full bg-blue-500"></div>
                </div>
                <p class="text-left">
                    <asp:Label ID="lbl4star" runat="server" Text="60"></asp:Label>%
                </p>
            </div>
            <div class="mb-3 flex items-center gap-4 text-base">
                <p style="width: 10px" class="text-center">3</p>
                <i class="fas fa-star text-black" style="font-weight: bold !important;"></i>
                <div class="relative w-full rounded-full bg-gray-400 bg-opacity-50" style="height: 12.5px;">
                    <div id="starBar3" runat="server" style="" class="absolute h-full rounded-full bg-blue-300"></div>
                </div>
                <p class="text-left">
                    <asp:Label ID="lbl3star" runat="server" Text="40"></asp:Label>%
                </p>
            </div>
            <div class="mb-3 flex items-center gap-4 text-base">
                <p style="width: 10px" class="text-center">2</p>
                <i class="fas fa-star text-black" style="font-weight: bold !important;"></i>
                <div class="relative w-full rounded-full bg-gray-400 bg-opacity-50" style="height: 12.5px;">
                    <div id="starBar2" runat="server" style="" class="absolute h-full rounded-full bg-yellow-500"></div>
                </div>
                <p class="text-center">
                    <asp:Label ID="lbl2star" runat="server" Text="20"></asp:Label>%
                </p>
            </div>
            <div class="flex items-center gap-4 text-base">
                <p style="width: 10px" class="text-center">1</p>
                <i class="fas fa-star text-black" style="font-weight: bold !important;"></i>
                <div class="relative w-full rounded-full bg-gray-400 bg-opacity-50" style="height: 12.5px;">
                    <div id="starBar1" runat="server" style="" class="absolute h-full rounded-full bg-red-500"></div>
                </div>
                <p class="text-center">
                    <asp:Label ID="lbl1star" runat="server" Text="15"></asp:Label>%
                </p>
            </div>
        </div>
    </div>
    <div class="mx-auto mb-10 mt-5 w-4/5 rounded-lg p-4 shadow-lg">
        <div class="w-full">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="flex flex-wrap items-center p-4 text-lg font-bold">
                        <p class="underline">All Ratings and Reviews</p>
                        <div style="margin-left: auto">
                            <asp:Button OnClick="btnLatest_Click" ID="btnLatest" class="btnSorting clicked latestBtn" runat="server" Text="Latest" />
                            <asp:Button OnClick="btnTopRated_Click" ID="btnTopRated" class="btnSorting TopRatedBtn" runat="server" Text="TopRated" />
                        </div>
                    </div>
                    <asp:ListView ID="lvReviews" runat="server" OnPagePropertiesChanged="lvReviews_PagePropertiesChanged">
                        <ItemTemplate>
                            <div class="relative flex w-full p-2">
                                <div class="border-b-2 flex w-full border-b-gray-300">
                                    <div class="py-4">
                                        <asp:Image ID="Image2" ImageUrl='<%# Eval("CustomerPicture") %>' runat="server" alt="user-pic" Width="55px" class="rounded-full" />
                                    </div>
                                    <div class="flex w-full flex-col gap-2 p-4">
                                        <div class="flex flex-wrap items-center">
                                            <div class="flex flex-col">
                                                <asp:Label ID="lblCustName" data-ratings='<%# Eval("ReviewRating") %>' runat="server" Text="" class="font-bold text-black"><%# Eval("CustomerName") %></asp:Label>
                                                <div class="mb-2 mt-1 flex items-center">
                                                    <%# GenerateStars(Convert.ToInt32(Eval("ReviewRating"))) %>
                                                </div>
                                                <p class="text-sm text-gray-500">
                                                    <span class="text-sm italic text-gray-500">
                                                        <%# Eval("ReviewTime") %>
                                                    </span>
                                                    | Variation: <span class="text-sm italic text-gray-500"><%# Eval("ColorName") %> Color</span>, 
         <span class="text-sm italic text-gray-500"><%# Eval("SizeName") %> Size</span>
                                                </p>
                                            </div>

                                        </div>
                                        <div class="flex flex-col gap-4">
                                            <p class="pr-5">
                                                <%# Eval("ReviewText") %>
                                            </p>
                                        </div>
                                        <asp:Repeater ID="rptReplies" runat="server" DataSource='<%# Eval("Replies") %>'>
                                            <ItemTemplate>
                                                <div class="mt-2 w-full bg-gray-100 py-0.5">
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
                            </div>
                        </ItemTemplate>
                        <LayoutTemplate>
                            <!-- Layout for your ListView; ensure it has a placeholder for items -->
                            <div runat="server" id="itemPlaceholder"></div>
                        </LayoutTemplate>
                    </asp:ListView>
                    <div class="mt-4 flex justify-center">
                        <asp:DataPager ID="ddpReviews" CssClass="data-pager" runat="server" PageSize="5" PagedControlID="lvReviews">
                            <Fields>
                                <asp:NumericPagerField NumericButtonCssClass="datapagerStyle" CurrentPageLabelCssClass="active" ButtonCount="5" />
                            </Fields>
                        </asp:DataPager>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lvReviews" EventName="PagePropertiesChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <script type="text/javascript" src="productDetails.js"></script>
    <script type="text/javascript">
        function validateQuantity() {
            var txtQuantity = document.getElementById('<%= txtQuantity.ClientID %>');
            var maxQuantity = parseInt(txtQuantity.getAttribute('Max'));
            var quantity = parseInt(txtQuantity.value);
            if (isNaN(quantity)) {
                txtQuantity.value = '1';
                return;
            }

            if (quantity <= 0 || quantity > maxQuantity) {
                txtQuantity.value = maxQuantity.toString();
            }
        }

    </script>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/src/Client/ClientMaster/Client.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="OutModern.src.Client.Products.Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        i.fas {
            color: #fbbf24;
        }

        .radio-no-bullet input[type="radio"]:checked + label {
            background-color: #ebebeb;
            border-radius: 5px;
            font-weight: 500;
        }

        .radio-no-bullet input[type="radio"] + label,
        .button-list input[type="radio"] + label,
        .checkbox-list input[type="checkbox"] + label {
            cursor: pointer;
        }

        .checkbox-list label {
            display: inline-block;
            margin-bottom: 8px; /* Adjust this value to control the spacing */
        }

        .checkbox-list input[type="checkbox"] {
            margin-right: 6px; /* Adjust the spacing between checkbox and text */
        }

        .button-list label {
            margin-left: 6px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Wrapper of whole webpage -->
    <div class="left-0 mb-8 mt-8 w-full">
        <div class="m-[auto] p-5 text-center">
            <div class="flex space-x-2">
                <span class="text-black">
                    <asp:HyperLink ID="HyperLink1" NavigateUrl="~/src/Client/Home/Home.aspx" runat="server" CssClass="text-sm text-black hover:underline">Home</asp:HyperLink>
                    >
                </span>
                <span class="text-black">
                    <asp:HyperLink ID="HyperLink2" NavigateUrl="~/src/Client/Products/Products.aspx" runat="server" CssClass="text-sm text-black hover:underline">Products</asp:HyperLink>
                </span>
            </div>
        </div>
        <div class="flex items-center">
            <div class="mb-2 w-1/5">
                <h3 class="text-center font-light">Search Filter <i class="fa fa-filter"></i></h3>
            </div>
            <div class="border-b-2 mr-3 flex w-4/5 items-center justify-between border-gray-200 px-2 py-0.5">
                <asp:Label ID="lblTotalProducts" runat="server" class="flex items-center">
                    
                </asp:Label>
                <div class="mb-1 flex items-center space-x-4">
                    <div class="relative" style="">
                        <asp:DropDownList ID="ddlSort" AutoPostBack="True" OnSelectedIndexChanged="ddlSort_SelectedIndexChanged" runat="server" CssClass="w-full appearance-none rounded-md border border-gray-300 bg-white py-2 pl-3 pr-10 text-sm font-medium text-gray-700 shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500">
                            <asp:ListItem Value="Featured" Text="Sort By:&nbsp;Featured" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="LowestPrice" Text="Price: Low to High"></asp:ListItem>
                            <asp:ListItem Value="HighestPrice" Text="Price: High to Low"></asp:ListItem>
                            <asp:ListItem Value="Customer Ratings" Text="Avg. Customer Ratings"></asp:ListItem>
                            <asp:ListItem Value="TotalSold" Text="Best Sellers"></asp:ListItem>
                        </asp:DropDownList>
                        <div class="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2">
                            <svg class="h-5 w-5 text-gray-400" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" aria-hidden="true">
                                <path fill-rule="evenodd" d="M10 3a1 1 0 01.707.293l3 3a1 1 0 01-1.414 1.414L10 5.414 7.707 7.707a1 1 0 01-1.414-1.414l3-3A1 1 0 0110 3zm-3.707 9.293a1 1 0 011.414 0L10 14.586l2.293-2.293a1 1 0 011.414 1.414l-3 3a1 1 0 01-1.414 0l-3-3a1 1 0 010-1.414z" clip-rule="evenodd" />
                            </svg>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="flex">
            <!--PAGE TITLE-->
            <div class="relative block w-1/5 overflow-hidden">
                <ul>
                    <li class="list-none border-b border-gray-300">
                        <div class="p-5 hover:bg-[#f8efd6] hover:cursor-pointer hover:[transition:0.3s]" onclick="openPriceList();invertIcon(this)">
                            Price Range<i class="fa fa-caret-down !font-black float-right"></i>
                        </div>
                        <ul class="pb-5 pl-5 pt-2">
                            <li class="list-none">
                                <asp:TextBox Min="1" class="text-center" Type="Number" ID="txtMinPrice" runat="server" Style="width: 45%; border: 1px solid rgba(0, 0, 0, .26); border-radius: .125rem;" placeholder="Min Price"></asp:TextBox>
                                -
                                <asp:TextBox class="text-center" Type="Number" placeholder="Max Price" ID="txtMaxPrice" runat="server" Style="width: 45%; border: 1px solid rgba(0, 0, 0, .26); border-radius: .125rem;"></asp:TextBox>
                            </li>
                            <li class="mt-5 flex list-none items-center">
                                <asp:Button ID="btnPriceFilter" runat="server" Text="Apply" CssClass="w-11/12 cursor-pointer bg-black p-2.5 text-center text-white" OnClick="btnPriceFilter_Click" />
                            </li>
                        </ul>
                    </li>
                    <li class="list-none border-b border-gray-300">
                        <div class="p-5 hover:bg-[#f8efd6] hover:cursor-pointer hover:[transition:0.3s]" onclick="openSortList();invertIcon(this)">
                            By Category<i class="fa fa-caret-down !font-black float-right"></i>
                        </div>
                        <ul class="pl-5" id="">
                            <li class="list-none pb-2.5 pl-5">
                                <asp:CheckBoxList ID="CategoryCheckBoxList" runat="server" CssClass="checkbox-list" AutoPostBack="true" OnSelectedIndexChanged="CategoryCheckBoxList_SelectedIndexChanged">
                                    <asp:ListItem Text="Hoodies" Value="Hoodies"></asp:ListItem>
                                    <asp:ListItem Text="Shorts and Pants" Value="Shorts and Pants"></asp:ListItem>
                                    <asp:ListItem Text="Sweaters" Value="Sweaters"></asp:ListItem>
                                    <asp:ListItem Text="Tee Shirts" Value="Tee Shirts"></asp:ListItem>
                                    <asp:ListItem Text="Trousers" Value="Trousers"></asp:ListItem>
                                </asp:CheckBoxList>
                            </li>
                        </ul>
                    </li>
                    <li class="list-none border-b border-gray-300">
                        <div class="p-5 hover:bg-[#f8efd6] hover:cursor-pointer hover:[transition:0.3s]" onclick="openSortList();invertIcon(this)">
                            Customer Ratings<i class="fa fa-caret-down !font-black float-right"></i>
                        </div>
                        <ul class="pl-5">
                            <li class="radio-no-bullet list-none pb-5 pl-5">
                                <asp:RadioButtonList runat="server" CssClass="button-list" ID="rbRatings" AutoPostBack="True" OnSelectedIndexChanged="rbRatings_SelectedIndexChanged">
                                    <asp:ListItem Value="5s">
                                        <i class="fas fa-star"></i>
                                        <i class="fas fa-star"></i>
                                        <i class="fas fa-star"></i>
                                        <i class="fas fa-star"></i>
                                        <i class="fas fa-star"></i>
                                    </asp:ListItem>
                                    <asp:ListItem Value="4s">
                                        <i class="fas fa-star"></i>
                                        <i class="fas fa-star"></i>
                                        <i class="fas fa-star"></i>
                                        <i class="fas fa-star"></i>
                                        <i class="far fa-star"></i> & above
                                    </asp:ListItem>
                                    <asp:ListItem Value="3s">
                                        <i class="fas fa-star"></i>
                                        <i class="fas fa-star"></i>
                                        <i class="fas fa-star"></i>
                                        <i class="far fa-star"></i>
                                        <i class="far fa-star"></i> & above
                                    </asp:ListItem>
                                    <asp:ListItem Value="2s">
                                        <i class="fas fa-star"></i>
                                        <i class="fas fa-star"></i>
                                        <i class="far fa-star"></i>
                                        <i class="far fa-star"></i>
                                        <i class="far fa-star"></i> & above
                                    </asp:ListItem>
                                    <asp:ListItem Value="1s">
                                        <i class="fas fa-star"></i>
                                        <i class="far fa-star"></i>
                                        <i class="far fa-star"></i>
                                        <i class="far fa-star"></i>
                                        <i class="far fa-star"></i> & above
                                    </asp:ListItem>
                                </asp:RadioButtonList>
                            </li>
                        </ul>
                    </li>
                    <li class="list-none border-b border-gray-300">
                        <div class="p-5 hover:bg-[#f8efd6] hover:cursor-pointer hover:[transition:0.3s]" onclick="openSortList();invertIcon(this)">
                            Colors Filtering<i class="fa fa-caret-down !font-black float-right"></i>
                        </div>
                        <ul class="pl-5">
                            <li class="radio-no-bullet pb-5 pl-5">
                                <asp:CheckBoxList ID="chkColorSelection" runat="server" CssClass="checkbox-list" AutoPostBack="true" OnSelectedIndexChanged="chkColorSelection_SelectedIndexChanged">
                                    <asp:ListItem Text="Black" Value="000000"></asp:ListItem>
                                    <asp:ListItem Text="White" Value="FFFFFF"></asp:ListItem>
                                    <asp:ListItem Text="Blue" Value="0000FF"></asp:ListItem>
                                    <asp:ListItem Text="Beige" Value="F5F5DC"></asp:ListItem>
                                    <asp:ListItem Text="Navy" Value="000080"></asp:ListItem>
                                    <asp:ListItem Text="Light Gray" Value="D3D3D3"></asp:ListItem>
                                    <asp:ListItem Text="Red" Value="FF0000"></asp:ListItem>
                                    <asp:ListItem Text="Dark Olive Green" Value="556b2f "></asp:ListItem>
                                </asp:CheckBoxList>
                            </li>
                        </ul>
                    </li>
                    <li class="mt-4 flex list-none items-center justify-center">
                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="w-11/12 cursor-pointer bg-black p-2.5 text-center text-white" OnClick="btnReset_Click" />
                    </li>
                </ul>
            </div>
            <div class="ml-5 mr-8 w-4/5 py-4">
                <div class="grid-cols-1 grid gap-x-6 gap-y-6 sm:grid-cols-2 md:grid-cols-3 xl:grid-cols-4 3xl:grid-cols-5">
                    <!-- Product 1 -->
                    <asp:Repeater ID="ProductRepeater" runat="server" OnItemDataBound="ProductRepeater_ItemDataBound">
                        <ItemTemplate>
                            <div onclick='<%# Eval("productID", "window.location.href = \"/src/Client/ProductDetails/ProductDetails.aspx?ProductId={0}\";") %>' class="cursor-pointer bg-white rounded-lg shadow-md overflow-hidden hover:bg-yellow-100 hover:shadow-md hover:-translate-x-2 hover:-translate-y-2 transition duration-100 z-0">
                                <asp:Image ImageUrl='<%# Eval("productImageUrl1", "{0}") %>' ID="Image1" runat="server" class="w-full h-100 object-cover bg-gray-200 hover:cursor-pointer" Style="transition: transform 0.5s ease-in-out;"
                                    onmouseover='<%# "this.src=\"" + ResolveClientUrl(Eval("productImageUrl2").ToString()) + "\"" %>'
                                    onmouseout='<%# "this.src=\"" + ResolveClientUrl(Eval("productImageUrl1").ToString()) + "\"" %>' />
                                <div class="p-4 pr-0">
                                    <h2 class="mb-2">
                                        <asp:Label class="text-xl font-bold text-gray-800 hover:cursor-pointer hover:underline" ID="lblName" runat="server">
                                            <%# Eval("ProductName") %>
                                        </asp:Label></h2>
                                    <div class="colorGrp mb-2 inline-flex gap-2">
                                        <asp:Repeater ID="ColorRepeater" runat="server">
                                            <ItemTemplate>
                                                <span class="border-opacity-10 h-6 w-6 rounded-full border border-gray-600" style='<%# "background-color:#" + Container.DataItem %>'></span>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <p class="mb-2">
                                        <asp:Label ID="lblPrice" class="text-xl font-bold text-gray-700" runat="server">
                                            RM<%# Eval("UnitPrice") %>
                                        </asp:Label>
                                    </p>
                                    <p class="mb-2">
                                        <span class="font-bold text-gray-600"><%# Eval("TotalSold").ToString() == "0" ? "No Sold" : Eval("TotalSold") + " Sold" %></span>
                                    </p>
                                    <div class="mb-2 flex items-center">
                                        <%# GenerateStars(Convert.ToDouble(Eval("OverallRatings"))) %>
                                        <span class="ml-2 text-gray-600">(<%# Eval("TotalReview") %> Reviews)</span>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div class="flex h-screen flex-col items-center justify-center" runat="server" id="noProductsFound" visible="false">
                    <div class="max-w-md text-center">
                        <svg class="mx-auto mb-6 text-gray-400" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" style="width: 64px; height: 64px;">
                            <path stroke-linecap="round" stroke-linejoin="round" d="M15.182 16.318A4.486 4.486 0 0012.016 15a4.486 4.486 0 00-3.198 1.318M21 12a9 9 0 11-18 0 9 9 0 0118 0zM9.75 9.75c0 .414-.168.75-.375.75S9 10.164 9 9.75 9.168 9 9.375 9s.375.336.375.75zm-.375 0h.008v.015h-.008V9.75zm5.625 0c0 .414-.168.75-.375.75s-.375-.336-.375-.75.168-.75.375-.75.375.336.375.75zm-.375 0h.008v.015h-.008V9.75z" />
                        </svg>
                        <h2 class="mb-4 text-2xl font-bold text-gray-800">No Products Found</h2>
                        <p class="mb-8 text-gray-600">We couldn't find any products matching your search criteria.</p>
                        <asp:LinkButton ID="lbtnReset" OnClick="lbtnReset_Click" runat="server" CssClass="inline-block cursor-pointer rounded bg-black px-4 py-2 font-bold text-white hover:bg-indigo-600">Shop All Products / Reset Filters</asp:LinkButton>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

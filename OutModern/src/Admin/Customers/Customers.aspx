<%@ Page Title="" Language="C#" MasterPageFile="~/src/Admin/AdminMaster/Admin.Master" AutoEventWireup="true" CodeBehind="Customers.aspx.cs" Inherits="OutModern.src.Admin.Customers.Customers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/tailwindcss">
        @layer components {

            .user-status {
                @apply rounded p-1;
            }

                .user-status.activated {
                    @apply bg-green-300;
                }

                .user-status.locked {
                    @apply bg-amber-300;
                }

                .user-status.deleted {
                    @apply bg-red-300;
                }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mt-2">

        <!-- Filter and add item -->
        <div class="flex justify-between items-center mt-8">
            <div>
                <h2>Customer List</h2>
            </div>

            <!-- Filter -->
            <div class="filter-item flex">
                <div class="item">
                    <asp:DropDownList ID="ddlFilterStatus" OnSelectedIndexChanged="ddlFilterStatus_SelectedIndexChanged" AutoPostBack="true" runat="server" OnDataBound="ddlFilterStatus_DataBound">
                    </asp:DropDownList>
                    <i class="fa-regular fa-user-magnifying-glass"></i>
                </div>
            </div>

        </div>

        <!-- Display Product -->
        <div class="mt-5">
            <!--Pagination-->
            <asp:DataPager ID="dpTopCustomers" class="pagination" runat="server" PagedControlID="lvCustomers">
                <Fields>
                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="False" ShowNextPageButton="False" ShowPreviousPageButton="True" PreviousPageText="<" />
                    <asp:NumericPagerField CurrentPageLabelCssClass="active" ButtonCount="10" />
                    <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="False" ShowNextPageButton="True" ShowPreviousPageButton="False" NextPageText=">" />
                </Fields>
            </asp:DataPager>

            <asp:ListView
                OnItemDataBound="lvCustomers_ItemDataBound"
                OnPagePropertiesChanged="lvCustomers_PagePropertiesChanged"
                OnSorting="lvCustomers_Sorting"
                ID="lvCustomers"
                runat="server">
                <LayoutTemplate>
                    <table id="data-table" style="width: 100%; text-align: center;">
                        <thead>
                            <tr class="data-table-head">
                                <th class="active">
                                    <asp:LinkButton ID="lbCustomerId" runat="server" CommandName="Sort" CommandArgument="CustomerId">
                             ID
                                    </asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lbCustomerName" runat="server" CommandName="Sort" CommandArgument="CustomerFullname">
                             Customer Name
                                    </asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lbUsername" runat="server" CommandName="Sort" CommandArgument="CustomerUsername">
                              Username
                                    </asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lbProductOrdered" runat="server" CommandName="Sort" CommandArgument="CustomerEmail">
                              Email
                                    </asp:LinkButton>
                                </th>

                                <th>
                                    <asp:LinkButton ID="lbSubtotal" runat="server" CommandName="Sort" CommandArgument="CustomerPhoneNumber">
                             Phone No
                                    </asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lbOrderStatus" runat="server" CommandName="Sort" CommandArgument="UserStatusName">
                             Status
                                    </asp:LinkButton>
                                </th>
                                <th>Action</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr runat="server" id="itemPlaceholder" class="data-table-item"></tr>
                        </tbody>
                    </table>

                </LayoutTemplate>
                <ItemTemplate>
                    <tr onclick="window.location='<%# Page.ResolveClientUrl(urls[CustomerDetails] + "?CustomerId=" +  Eval("CustomerId") )%>'">
                        <td><%# Eval("CustomerId") %></td>
                        <td><%# Eval("CustomerFullname") %></td>
                        <td><%# Eval("CustomerUsername") %></td>
                        <td><%# Eval("CustomerEmail") %></td>
                        <td><%# Eval("CustomerPhoneNumber") %></td>
                        <td><span runat="server" id="userStatus" class="user-status"><%# Eval("UserStatusName") %></span></td>
                        <td>
                            <asp:HyperLink ID="hlEdit" runat="server" CssClass="button" NavigateUrl='<%#urls[CustomerEdit] +"?CustomerId=" +Eval("CustomerId") %>'>
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
            <asp:DataPager ID="dpBottomCustomers" class="pagination" runat="server" PageSize="10" PagedControlID="lvCustomers">
                <Fields>
                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="False" ShowNextPageButton="False" ShowPreviousPageButton="True" PreviousPageText="<" />
                    <asp:NumericPagerField CurrentPageLabelCssClass="active" ButtonCount="10" />
                    <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="False" ShowNextPageButton="True" ShowPreviousPageButton="False" NextPageText=">" />
                </Fields>
            </asp:DataPager>
        </div>
    </div>
</asp:Content>

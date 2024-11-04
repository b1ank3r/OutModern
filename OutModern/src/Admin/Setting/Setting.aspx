<%@ Page Title="" Language="C#" MasterPageFile="~/src/Admin/AdminMaster/Admin.Master" AutoEventWireup="true" CodeBehind="Setting.aspx.cs" Inherits="OutModern.src.Admin.Setting.Setting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/tailwindcss">
        /*Edit form*/
        #edit-form {
        }

            #edit-form .edit-section {
                @apply mt-2 ml-2;
            }

            #edit-form .edit-label {
                @apply font-[600];
            }

            #edit-form .edit-item {
                @apply block px-4 py-1 border border-gray-600 rounded w-56;
            }

                #edit-form .edit-item.disabled {
                    @apply text-gray-400 border-gray-300;
                }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mt-2 flex gap-5">

        <!-- my Info -->
        <div id="edit-form" class="mt-4 w-fit border border-gray-700 rounded  p-4 relative">

            <div class="absolute top-2 right-2">
                <asp:LinkButton
                    CssClass="button bg-green-500"
                    ID="lbUpdateInfo"
                    OnClientClick="return confirm('Are you sure you want to update your info?');"
                    OnClick="lbUpdateInfo_Click"
                    runat="server">Update</asp:LinkButton>
            </div>

            <div class="text-2xl mt-5 font-[600]">
                General Info
            </div>

            <div class="">
                <!--u ID-->
                <div class="edit-section">
                    <div class="edit-label">Your ID</div>
                    <div>
                        <asp:Label ID="lblId" CssClass="edit-item disabled" runat="server"></asp:Label>
                    </div>
                </div>

                <!--u NAme-->
                <div class="edit-section">
                    <div class="edit-label">Your FullName</div>
                    <div>
                        <asp:TextBox ID="txtFName" CssClass="edit-item" runat="server"></asp:TextBox>
                    </div>
                </div>

                <!--u Username-->
                <div class="edit-section">
                    <div class="edit-label">Your Username</div>
                    <div>
                        <asp:TextBox ID="txtUsername" CssClass="edit-item" runat="server"></asp:TextBox>
                    </div>
                </div>

                <!-- u Email -->
                <div class="edit-section">
                    <div class="edit-label">Email</div>
                    <div>
                        <asp:TextBox ID="txtEmail" CssClass="edit-item" runat="server"></asp:TextBox>
                    </div>
                </div>

                <!-- u Phone Num -->
                <div class="edit-section">
                    <div class="edit-label">Phone No</div>
                    <div>
                        <asp:TextBox ID="txtPhoneNo" CssClass="edit-item" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>

        <!-- change password-->
        <div class="mt-4 w-fit border border-gray-700 rounded p-4 relative">

            <div class="absolute top-2 right-2">
                <asp:LinkButton ID="lbUpdatePassword"
                    CssClass="button bg-green-500"
                    OnClientClick="return confirm('Are you sure you want to change your password?');"
                    OnClick="lbUpdatePassword_Click"
                    runat="server">Change</asp:LinkButton>
            </div>
            <div class="font-[600] text-2xl mt-5">
                Change Password
            </div>

            <div class="px-2">
                <!--current Pass-->
                <div class="mt-2">
                    <div class="edit-label">Current Password</div>
                    <div>
                        <asp:TextBox ID="txtCurrPass" CssClass="px-2" TextMode="Password" runat="server"></asp:TextBox>
                    </div>
                </div>

                <!--New Pass-->
                <div class="edit-section mt-2">
                    <div class="edit-label">New Password</div>
                    <div>
                        <asp:TextBox ID="txtNewPass" CssClass="px-2" TextMode="Password" runat="server"></asp:TextBox>
                    </div>
                </div>

                <!--Confirm Pass-->
                <div class="edit-section mt-2">
                    <div class="edit-label">Confirm Password</div>
                    <div>
                        <asp:TextBox ID="txtConfirmPass" CssClass="px-2" TextMode="Password" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

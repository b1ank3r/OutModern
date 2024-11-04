<%@ Page Title="Success" Language="C#" MasterPageFile="~/src/Client/ClientMaster/Client.Master" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="OutModern.src.Client.Payment.Success" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="min-h-[60vh]">

        <div id="orderModal" class="modal z-10 fixed inset-0 flex items-center justify-center overflow-y-auto">
            <div class="modal-overlay absolute h-full w-full bg-gray-900 opacity-50"></div>
            <div class="modal-container -translate-x-1/2 -translate-y-1/2 w-[30%] fixed left-1/2 top-1/2 w-1/3 transform rounded-lg bg-white p-6 shadow-lg">
                <div class="mb-4 flex justify-center text-4xl">
                    <asp:Image ID="Image12" ImageUrl="~/images/tick-mark.png" CssClass="h-[30%] w-[30%] ml-4" runat="server" />
                </div>

                <div class="flex flex-col items-center justify-center px-8">
                    <h2 class="mb-2 text-center text-2xl font-bold">Your order is confirmed</h2>
                    <p class="mb-6 text-center text-gray-600">Thanks for shopping! Your order hasn't shipped yet, but we will send you an email when it's done.</p>
                </div>


                <div class="modal-buttons flex flex-col justify-center">
                    <asp:Button ID="BtnViewOrder" runat="server" Text="View Order" CssClass="bg-[#131118] mb-2 w-full cursor-pointer rounded-xl px-4 py-2 font-semibold text-white hover:bg-white hover:text-black hover:border hover:border-black" OnClick="BtnViewOrder_Click" />
                    <asp:Button ID="ButtonHome" runat="server" Text="Back to Home" CssClass="w-full cursor-pointer rounded-xl border border-black bg-white px-4 py-2 font-semibold text-black hover:bg-black hover:text-white" OnClick="ButtonHome_Click" />
                </div>
            </div>
        </div>

    </div>

</asp:Content>

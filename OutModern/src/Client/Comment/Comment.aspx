<%@ Page Language="C#" MasterPageFile="~/src/Client/ClientMaster/Client.Master"  AutoEventWireup="true" CodeBehind="Comment.aspx.cs" Inherits="OutModern.src.Client.Comment.Comment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="comment-container mb-10 mt-10 flex items-center justify-center" style="min-height:600px;">
        <div class="flex w-4/5">
            <div class="product-image mr-8">
                <asp:Image ID="imgProduct" runat="server" ImageUrl="~/images/product-img/hoodies/black-Hoodie/unisex-heavy-blend-hoodie-black-front-60e815eedacd3.png" CssClass="h-96 w-96 rounded-md object-cover" />
            </div>
            <div class="product-details flex-1">
                <div class="product-info mb-3">
                    <asp:Label ID="lblProductName" runat="server" Text="Unisex Heavy Hoodie" CssClass="text-3xl font-bold"></asp:Label>
                </div>
                <div class="mb-3">
                    <asp:Label ID="lblProductPrice" runat="server" Text="RM49.99" CssClass="text-2xl font-bold text-gray-600"></asp:Label>
                </div>
                <div class="mb-4">
                    <asp:Label ID="lblProductColour" runat="server" Text="Colour: Black" CssClass="text-gray-600"></asp:Label> |
                    <asp:Label ID="lblProductSize" runat="server" Text="Size: L" CssClass="text-gray-600"></asp:Label>           
                </div>

                 <div class="rating-stars mb-4 flex items-center">
                    <span class="mr-2 text-gray-600">Rating:</span>
                     <div class="flex">
                         <i class="far fa-star star cursor-pointer text-2xl text-gray-400" aria-hidden="true"></i>
                         <i class="far fa-star star cursor-pointer text-2xl text-gray-400" aria-hidden="true"></i>
                         <i class="far fa-star star cursor-pointer text-2xl text-gray-400" aria-hidden="true"></i>
                         <i class="far fa-star star cursor-pointer text-2xl text-gray-400" aria-hidden="true"></i>
                         <i class="far fa-star star cursor-pointer text-2xl text-gray-400" aria-hidden="true"></i>
                     </div>
                     <asp:Label ID="lblStarErrorMessage" runat="server" Text="Rating is required!!!" Visible="false" CssClass="ml-3.5 text-red-500" ></asp:Label>
                     <asp:HiddenField ID="hdnSelectedRating" runat="server" />
                </div>

                <div class="comment-form">
                    <h2 class="mb-2 text-xl font-bold">Add a Comment</h2>
                    <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Rows="5" Placeholder="Enter your comment here" CssClass="w-full rounded-md border-gray-400 p-2 focus:border-indigo-500 focus:ring-indigo-500"></asp:TextBox>
                    <asp:Label ID="lblMessage" runat="server" Text="Please fill in your comment" Visible="false" CssClass="text-sm text-red-500"></asp:Label>                   
                </div>
                <asp:Button ID="btnSubmitComment" OnClick="btnSubmitComment_Click" runat="server" Text="Submit Comment" CssClass="mt-2 cursor-pointer rounded-md bg-black px-4 py-2 font-bold text-white hover:bg-indigo-600" />
            </div>
        </div>
        <div id="commentMessage" class="modal z-10 fixed inset-0 flex items-center justify-center overflow-y-auto" runat="server" visible="false">
            <div class="modal-overlay absolute h-full w-full bg-gray-900 opacity-50"></div>
            <div class="modal-container -translate-x-1/2 -translate-y-1/2 w-[30%] fixed left-1/2 top-1/2 w-1/3 transform rounded-lg bg-white p-6 shadow-lg">
                <div class="mb-4 flex justify-center text-4xl">
                    <asp:Image ID="Image12" ImageUrl="~/images/tick-mark.png" CssClass="h-[30%] w-[30%] ml-4" runat="server" />
                </div>
                <div class="flex flex-col items-center justify-center px-8">
                    <h2 class="mb-2 text-center text-2xl font-bold">Your comment has been submitted successfully</h2>
                    <p class="mb-6 text-center text-gray-600">Thank you for your feedback!</p>
                </div>
                <div class="modal-buttons flex flex-col justify-center">
                    <asp:Button ID="BtnOk" OnClick="BtnOk_Click" runat="server" Text="OK" CssClass="bg-[#131118] mb-2 w-full cursor-pointer rounded-xl px-4 py-2 font-semibold text-white hover:bg-white hover:text-black hover:border hover:border-black" />
                </div>
            </div>
        </div>
    </div>
    <script>
        const stars = document.querySelectorAll('.star');
        let selectedRating = 0;

        stars.forEach((star, index) => {
            star.addEventListener('click', () => {
                selectedRating = index + 1;
                updateStarRating();
                // Update the hidden field value
                document.getElementById('<%= hdnSelectedRating.ClientID %>').value = selectedRating;
            });

            star.addEventListener('mouseover', () => {
                updateStarRating(index + 1);
            });

            star.addEventListener('mouseout', () => {
                updateStarRating();
            });
        });

        function updateStarRating(rating = selectedRating) {
            stars.forEach((star, index) => {
                if (index < rating) {
                    star.classList.remove('far');
                    star.classList.add('fas');
                    star.classList.add('text-yellow-500');
                } else {
                    star.classList.remove('fas');
                    star.classList.add('far');
                    star.classList.remove('text-yellow-500');
                }
            });
        }
    </script>
</asp:Content>
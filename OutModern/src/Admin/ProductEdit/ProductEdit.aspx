<%@ Page Title="" Language="C#" MasterPageFile="~/src/Admin/AdminMaster/Admin.Master" AutoEventWireup="true" CodeBehind="ProductEdit.aspx.cs" Inherits="OutModern.src.Admin.ProductEdit.ProductEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/tailwindcss">
        .button {
            @apply text-white bg-green-500 text-center px-1 py-0 cursor-pointer hover:opacity-50 border border-gray-400;
        }

        /*Edit form*/
        #edit-form {
            @apply w-full;
        }

            #edit-form .edit-section {
                @apply mt-2 ml-2;
            }

            #edit-form .edit-label {
                @apply font-[600];
            }

            #edit-form .edit-item {
                @apply block px-4 py-1 border border-gray-600 rounded;
            }

                #edit-form .edit-item.disabled {
                    @apply text-gray-400 border-gray-300;
                }

        /*Variation*/
        #variation-form {
            @apply ml-2;
        }

            #variation-form .size-added {
                @apply rounded-full bg-black text-white size-5 inline-flex justify-center items-center;
            }

            #variation-form .product-img {
                @apply border border-black;
            }

            #variation-form .add-image {
                @apply block size-40 border border-black hover:opacity-50 cursor-pointer rounded-3xl flex justify-center items-center ml-5;
            }

                #variation-form .add-image i {
                    @apply text-[4em];
                }



        .color-varient {
            @apply cursor-pointer size-5 rounded-full border drop-shadow border-gray-300 box-border hover:opacity-50;
        }

            .color-varient.active {
                @apply outline outline-black outline-1 hover:opacity-100;
            }


            .color-varient.add-color {
                @apply outline outline-black outline-1 cursor-default hover:opacity-100;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mt-2">
        <div class="border p-5 border-black rounded">
            <div class="">
                <!-- Prod Desc -->
                <div class="flex items-center justify-between">
                    <div class="text-2xl font-bold ">Edit Product</div>
                    <asp:LinkButton
                        OnClick="lbUpdate_Click"
                        OnClientClick="return confirm('Are you sure you want Update Information?');"
                        CssClass="text-white p-2 rounded bg-green-500 hover:opacity-50"
                        ID="lbUpdate" runat="server" NavigateUrl='#'>
                        Save and Update Info
                    </asp:LinkButton>
                </div>
            </div>

            <!-- update status -->
            <asp:Label ID="lblUpdateProductStatus" CssClass="float-right opacity-50" runat="server" Text=""></asp:Label>

            <div id="edit-form" class="mt-4 gap-10 flex">
                <div>
                    <!--Prod ID-->
                    <div class="edit-section">
                        <div class="edit-label">Product ID</div>
                        <div>
                            <asp:Label ID="lblProdId" CssClass="edit-item disabled" runat="server"></asp:Label>
                        </div>
                    </div>

                    <!--Prod NAme-->
                    <div class="edit-section">
                        <div class="edit-label">Product Name</div>
                        <div>
                            <asp:TextBox ID="txtProdName" CssClass="edit-item" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <!--Prod Category-->
                    <div class="edit-section">
                        <div class="edit-label">Category</div>
                        <div>
                            <asp:DropDownList EnableViewState="true" ID="ddlCategory" CssClass="edit-item" runat="server"></asp:DropDownList>
                        </div>
                    </div>

                    <!--Prod Price-->
                    <div class="edit-section">
                        <div class="edit-label">Price (RM)</div>
                        <div>
                            <asp:TextBox ID="txtPrice" CssClass="edit-item" runat="server"></asp:TextBox>

                        </div>
                    </div>

                    <!--Prod Status-->
                    <div class="edit-section">
                        <div class="edit-label">Status</div>
                        <div>
                            <asp:DropDownList ID="ddlStatus" CssClass="edit-item" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="flex-grow">
                    <!--Prod Desc-->
                    <div class="edit-section">
                        <div class="edit-label">Product Description</div>
                        <div>
                            <asp:TextBox
                                ID="txtProdDescription"
                                TextMode="MultiLine"
                                CssClass="edit-item w-full resize-none h-36"
                                runat="server">
                            </asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <!-- Variation -->
        <div class="border p-5 border-black mt-10 rounded">
            <div id="variation-form">
                <div class="text-2xl font-[600]">Variation</div>

                <!--Size and quantity-->
                <div class="flex mt-4 items-center">
                    <!--Size selection-->
                    <div>
                        <div class="font-[600]">Size</div>
                        <div>
                            <asp:DropDownList AutoPostBack="true" ID="ddlSize" runat="server" OnSelectedIndexChanged="ddlSize_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="ml-7 mr-2">
                        <div class="font-[600]">Quantity</div>
                        <div>
                            <asp:TextBox CssClass="w-16" TextMode="Number" ID="txtProdQuantity" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="self-end mr-2">
                        <asp:Button CssClass="button"
                            OnClientClick="return confirm('Are you sure you want to update the quantity?');"
                            ID="btnUpdateQuantity" OnClick="btnUpdateQuantity_Click" runat="server" Text="Set" />
                    </div>
                    <div class="opacity-50 mt-4 self-end">
                        <asp:Label ID="lblSetQuantityStatus" runat="server"></asp:Label>
                    </div>
                </div>

                <!--Colors-->
                <div class=" mt-4">
                    <div class="font-[600]">Colors </div>
                    <!--Add color-->
                    <div class="flex gap-2 items-center">
                        <asp:DropDownList ID="ddlColorAdd" runat="server"></asp:DropDownList>
                        <div class="color-varient add-color"></div>
                        <asp:Button ID="btnAddColor"
                            OnClick="btnAddColor_Click"
                            OnClientClick="return confirm('Are you sure you want to add this color?');"
                            CssClass="button"
                            runat="server" Text="Add" />
                        <asp:Label ID="lblAddColorStatus" runat="server" CssClass="opacity-50"></asp:Label>
                    </div>


                    <!--Color selection-->
                    <div class="flex gap-3 items-center mt-5">
                        <asp:Repeater ID="repeaterColors" OnItemCommand="repeaterColors_ItemCommand" runat="server">
                            <ItemTemplate>
                                <div class="flex items-center gap-2 bg-gray-100 p-1 rounded">
                                    <asp:LinkButton
                                        data-colorId='<%# Eval("ColorId") %>'
                                        CommandName="ChangeColor"
                                        CommandArgument='<%#Eval("ColorId") %>'
                                        ID="lbColor" runat="server"
                                        Style='<%# "background-color: #" + Eval("HexColor") +";" %>'
                                        CssClass='<%# "color-varient" + (ViewState["ColorId"].ToString() == Eval("ColorId").ToString() ? " active" : "") %>'>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lbRemoveProdColor"
                                        OnClientClick="return confirm('Are you sure you want remove this color?');"
                                        CommandName="DeleteColor"
                                        CommandArgument='<%#Eval("ColorId") %>'
                                        CssClass="text-red-500 hover:opacity-50" runat="server">
                                        <i class="fa-solid fa-xmark"></i>
                                    </asp:LinkButton>
                                </div>

                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblColorEmpty" CssClass="text-red-500" runat="server"
                                    Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>'
                                    Text="**No Color added** </br> **Add color in order to add an Image**" />
                            </FooterTemplate>
                        </asp:Repeater>
                        <asp:Label ID="lblDeleteColorStatus" runat="server" CssClass="opacity-50"></asp:Label>
                    </div>
                </div>



                <!-- Images -->
                <div class="mt-5 ">
                    <div class="flex items-center gap-7">
                        <div class="font-[600]">Image : </div>
                    </div>
                    <div>
                        <asp:Label ID="lblAddImgStatus" CssClass="ml-5 opacity-50" runat="server"></asp:Label>
                    </div>
                    <div class="flex items-center">

                        <!-- Repeater images-->
                        <div class="flex flex-wrap gap-2 items-center">
                            <asp:Repeater ID="repeaterImages" runat="server" OnItemCommand="repeaterImages_ItemCommand">
                                <ItemTemplate>
                                    <div class="relative">
                                        <asp:LinkButton
                                            CssClass="block absolute font top-1 right-1 text-red-500 leading-5 hover:opacity-50"
                                            ID="lbDeleteImg" runat="server"
                                            OnClientClick="return confirm('Are you sure you want remove this image?');"
                                            CommandArgument='<%# Eval("path") %>'
                                            CommandName="DeleteImage">
                                            <i class="fa-solid fa-xmark fa-lg"></i>
                                        </asp:LinkButton>

                                        <div>
                                            <asp:Image ID="imgProd" runat="server" Width="10em" Height="10em"
                                                CssClass="product-img object-cover"
                                                ImageUrl='<%# Eval("path") %>' />
                                        </div>
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblNoImg" CssClass="text-red-500" runat="server"
                                        Visible='<%# ((Repeater)Container.NamingContainer).Items.Count == 0 %>'
                                        Text="**No Image added**" />
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>

                        <% if (repeaterColors.Items.Count > 0)
                            { %>
                        <div class="add-image">
                            <asp:FileUpload ID="fileImgUpload"
                                AllowMultiple="true"
                                CssClass="hidden"
                                runat="server" accept=".png,.jpg,.jpeg,.webp" />
                            <i class="fa-regular fa-plus"></i>
                            <asp:Button ID="btnAddImage" CssClass="hidden"
                                runat="server" OnClick="btnAddImage_Click" />
                        </div>
                        <% }%>
                    </div>


                </div>
            </div>

        </div>
    </div>

    </div>
    <script>

        function init() {
            //color choser
            (() => {
                const ddlColor = document.querySelector("#<%= ddlColorAdd.ClientID %>");
                const addColorBtn = document.querySelector(".color-varient.add-color");
                const hexColor = ddlColor.querySelector("option:checked").dataset.hex
                addColorBtn.style.backgroundColor = `#${hexColor}`

                ddlColor.onchange = () => {
                    const hexColor = ddlColor.querySelector("option:checked").dataset.hex
                    console.log(hexColor)
                    addColorBtn.style.backgroundColor = `#${hexColor}`
                }
            })();

            //img upload
            (() => {
                const imgUpload = document.querySelector("#<%= fileImgUpload.ClientID %>");
                const imgUploadContainer = document.querySelector("#variation-form .add-image");

                imgUploadContainer.onclick = () => {
                    imgUpload.click();
                }

                // validate the files type uploaded
                imgUpload.onchange = () => {
                    validateFileUpload();
                }

                function validateFileUpload() {
                    const FileUploadPath = imgUpload.value;

                    //To check if user upload any file 
                    if (FileUploadPath == '') {
                        alert("Please upload an image");
                    }
                    else {
                        const Extension = FileUploadPath.substring(
                            FileUploadPath.lastIndexOf('.') + 1).toLowerCase();

                        if (!confirm("Are you sure you want to upload this image?")) {
                            return;
                        }

                        //The file uploaded is an image
                        if (Extension == "webp" || Extension == "png"
                            || Extension == "jpeg" || Extension == "jpg") {

                            __doPostBack('<%= btnAddImage.UniqueID %>', '');
                        }
                        //The file upload is NOT an image
                        else {
                            alert("Photo only allows file types of PNG, JPG, JPEG and WEBP. ");
                        }
                    }
                }
            })();

        }
        init();

        // Create the event handler for PageRequestManager.endRequest
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(init);

    </script>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/src/Client/ClientMaster/Client.Master" AutoEventWireup="true" CodeBehind="EditUserProfile.aspx.cs" Inherits="OutModern.src.Client.UserProfile.EditUserProfile" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
    <link href="css/UserProfile.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="containColumn">
        <div class="columns-containColumn">
            <!-- left column-->
            <div class="column1">
                <div class="sectionLeft">
                    <%--<div class="leftBox">
                        <asp:Image ID="img_profile" runat="server" class="imgProfile" ImageUrl="~/images/login-img/login-background13.jpg" />
                    </div>--%>

                    <div class="leftBox">
                        <asp:Image ID="img_profile" runat="server" Width="10em" Height="10em" CssClass="product-img object-cover" />
                    </div>

                    <div class="leftBox1">
                        <asp:FileUpload ID="imgUpload" runat="server" />
                    </div>

                    <div class="leftBox2">
                        <asp:Button ID="btnUploadImage" runat="server" Text="Upload Image" Style="background-color: black; color: white; padding: 0.5em 1em; margin: 1rem; border: none; cursor: pointer; border-radius: 5px;" OnClick="btnUploadImage_Click" />
                    </div>

                    <div class="rightBox">
                        <asp:Label ID="lbl_username" runat="server" class="username" Text="Prochorus"></asp:Label>
                    </div>

                    <div class="borderLine"></div>

                    <div class="boxBottom">

                        <asp:Button ID="btn_togo_profile" runat="server" class="button" Text="My Profile" OnClick="btn_togo_profile_Click" />
                        <asp:Button ID="btn_togo_my_order" runat="server" class="button" Text="My Order" OnClick="btn_togo_my_order_Click" />
                        <asp:Button ID="btn_rw_his" runat="server" class="button" Text="Review History" OnClick="btn_rw_his_Click1" />
                    </div>

                </div>
            </div>

            <div class="flex justify-center items-center flex-col w-full">

                <div class="self-start m-4">
                    <span id="ContentPlaceHolder1_SiteMapPath1" class="">
                        <a href="#ContentPlaceHolder1_SiteMapPath1_SkipLink" style="position: absolute; left: -10000px; top: auto; width: 1px; height: 1px; overflow: hidden;">Skip Navigation Links
                        </a>
                        <span>
                            <a></a>
                        </span><span>&gt; 
                        </span><span>
                            <a title="User Profile" href="/src/Client/UserProfile/UserProfile.aspx">Profile
                            </a>
                        </span><span>&gt; 
                        </span><span>Edit Profile
                        </span>
                        <a id="ContentPlaceHolder1_SiteMapPath1_SkipLink"></a></span>
                </div>

                <!--Edit Profile-->
                <div class="section_edit">
                    <div class="topBox">
                        <div class="titleBox">
                            <span class="title">My Profile</span>
                        </div>

                        <div class="titleBox2">
                            <asp:HyperLink ID="hl_back_to_user_profile" runat="server" NavigateUrl="~/src/Client/UserProfile/UserProfile.aspx">Back ></asp:HyperLink>
                        </div>

                    </div>

                    <div class="borderLine"></div>

                    <div class="bigBox">
                        <div class="boxItem">
                            <span class="labelData">Username</span>
                            <asp:TextBox ID="txt_edit_username" runat="server" class="edit_data"></asp:TextBox>
                        </div>

                        <!-- Error Message-->
                        <asp:Label ID="usernameErrMsg" runat="server" Style="color: red"></asp:Label>



                        <div class="boxItem">
                            <span class="labelData">Fullname</span>
                            <asp:TextBox ID="txt_edit_fullname" runat="server" class="edit_data"></asp:TextBox>
                        </div>

                        <!-- Error Message-->
                        <asp:Label ID="fullnameErrMsg" runat="server" Style="color: red"></asp:Label>

                        <div class="boxItem">
                            <span class="labelData">Phone Number</span>
                            <asp:TextBox ID="txt_edit_phone_number" runat="server" class="edit_data" TextMode="Phone"></asp:TextBox>
                        </div>

                        <!-- Error Message-->
                        <asp:Label ID="phoneNumErrMsg" runat="server" Style="color: red"></asp:Label>

                        <div class="boxItem1">
                            <span id="addresslbl1" class="labelData1">Address Name</span>
                            <asp:TextBox ID="txt_edit_address_name" runat="server" class="edit_data"></asp:TextBox>
                        </div>

                        <!-- Error Message-->
                        <asp:Label ID="addressNameErrMsg" runat="server" Style="color: red"></asp:Label>

                        <div class="boxItem1">
                            <span id="addresslbl2" class="labelData1">Address</span>
                            <asp:TextBox ID="txt_edit_address" runat="server" class="edit_data"></asp:TextBox>
                        </div>

                        <!-- Error Message-->
                        <asp:Label ID="addressLineErrMsg" runat="server" Style="color: red"></asp:Label>

                        <div class="boxItem1">
                            <span id="addresslbl3" class="labelData1">Country</span>
                            <asp:TextBox ID="txt_edit_country" runat="server" class="edit_data"></asp:TextBox>
                        </div>

                        <!-- Error Message-->
                        <asp:Label ID="countryErrMsg" runat="server" Style="color: red"></asp:Label>

                        <div class="boxItem1">
                            <span id="addresslbl4" class="labelData1">State</span>
                            <asp:TextBox ID="txt_edit_state" runat="server" class="edit_data"></asp:TextBox>
                        </div>

                        <!-- Error Message-->
                        <asp:Label ID="stateErrMsg" runat="server" Style="color: red"></asp:Label>

                        <div class="boxItem1">
                            <span id="addresslbl5" class="labelData1">Postal Code</span>
                            <asp:TextBox ID="txt_edit_postal_code" runat="server" class="edit_data"></asp:TextBox>
                        </div>

                        <!-- Error Message-->
                        <asp:Label ID="postalCodeErrMsg" runat="server" Style="color: red"></asp:Label>

                    </div>


                    <div class="EPBoxButton">
                        <asp:Button ID="btn_save" runat="server" class="EPButton" Text="Save" OnClick="btn_save_Click" CssClass="bg-black hover:bg-gray-700" Style="width: 50%; margin-top: 50px; padding: 0.50rem 1rem; font-weight: bold; border: 1px solid #f5f5f5; cursor: pointer; color: whitesmoke; border-radius: 30px;" />
                    </div>

                    <asp:Label ID="lblMessage" runat="server"></asp:Label>


                </div>
            </div>
        </div>
    </div>
</asp:Content>


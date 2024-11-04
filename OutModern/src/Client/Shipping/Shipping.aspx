<%@ Page Title="Shipping" Language="C#" MasterPageFile="~/src/Client/ClientMaster/Client.Master" AutoEventWireup="true" CodeBehind="Shipping.aspx.cs" Inherits="OutModern.src.Client.Shipping.Shipping" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="snackbar" class="-translate-x-1/2 fixed left-1/2 top-20 z-50 hidden transform rounded bg-green-500 px-6 py-3 text-center text-lg text-white shadow-lg" style="z-index: 50;">
        <div class="justify-center">
            <asp:Label ID="lblSnackbar" runat="server" CssClass="text-center text-white" Text=""></asp:Label>
        </div>
    </div>

    <div class="mx-10">

        <div class="mx-56 mt-10">
            <asp:SiteMapPath ID="SiteMapPath1" runat="server" CssClass="text-3xl font-bold"></asp:SiteMapPath>
            <h2 class="flex text-3xl font-bold">Shipping Address</h2>
        </div>

        <div class="mx-56 mb-10 mt-4 flex justify-between rounded-md">
            <!--Left Container-->
            <div class="w-[65%] min-h-[60vh] flex-col rounded-xl bg-white p-8 drop-shadow-lg">
                <div class="flex flex-col">
                    <h3 class="mb-2 font-bold">Select a delivery address</h3>
                    <p class="mb-4">Is the address you'd like to use displayed below? If so, click the corresponding address. Or you can enter a new delivery address.</p>
                </div>

                <div class="border-b">
                    <asp:UpdatePanel ID="AddressUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <asp:ListView ID="AddressListView" runat="server" OnItemCommand="AddressListView_ItemCommand" DataKeyNames="AddressId" OnSelectedIndexChanged="AddressListView_SelectedIndexChanged" OnSelectedIndexChanging="AddressListView_SelectedIndexChanging" OnItemDataBound="AddressListView_ItemDataBound">
                                <LayoutTemplate>
                                    <div class="grid-cols-2 grid gap-x-16">
                                        <div runat="server" id="itemPlaceholder"></div>
                                    </div>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Select" CommandArgument="<%# Container.DisplayIndex %>"
                                        CssClass="w-[100%] address-item mb-6 flex max-w-sm cursor-pointer rounded-2xl border bg-white shadow">
            <div class="w-full p-4">
                <div class="flex justify-between">
                    <div class="text-xl font-bold capitalize text-black"><%# Eval("AddressName") %></div>
                </div>
                <div class="flex flex-col">
                    <div class="text-sm"><%# Eval("AddressLine") %>,</div>
                    <div class="text-sm"><%# Eval("PostalCode") %>,  <%# Eval("State") %></div>
                    <div class="text-sm"><%# Eval("Country") %></div>
                </div>
            </div>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:ListView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="AddressListView" EventName="ItemCommand" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

                <!--Add address-->
                <asp:Panel ID="panelAddShipping" runat="server" DefaultButton="btnAddAddress">
                    <div class="flex flex-col">
                        <h3 class="my-6 font-bold">Add a new address</h3>

                        <div class="mb-2">
                            <p class="text-sm text-black">Nickname</p>
                            <asp:TextBox ID="txtNickname" runat="server" CssClass="h-12 w-full rounded-lg border border-black p-6" placeholder="Enter NickName" onKeyUp="validateNickname();" MaxLength="20"></asp:TextBox>
                            <asp:Label ID="lblNameError" runat="server" CssClass="text-red-700" Text=""></asp:Label>
                        </div>
                        <div class="my-2">
                            <p class="text-sm text-black">Address Line</p>
                            <asp:TextBox ID="txtAddr" runat="server" CssClass="h-12 w-full rounded-lg border border-black p-6" placeholder="Enter Address" MaxLength="50" onKeyUp="validateAddr();"></asp:TextBox>
                            <asp:Label ID="lblAddrError" runat="server" CssClass="text-red-700" Text=""></asp:Label>
                        </div>
                        <div class="my-2">
                            <p class="text-sm text-black">Postal Code</p>
                            <asp:TextBox ID="txtPostal" runat="server" CssClass="h-12 w-full rounded-lg border border-black p-6" placeholder="Enter Postal Code" MaxLength="12" onkeyup="validateNumericInput();"></asp:TextBox>
                            <asp:Label ID="lblPostalError" runat="server" CssClass="text-red-700" Text=""></asp:Label>
                        </div>
                        <div class="my-2">
                            <p class="text-sm text-black">State</p>
                            <asp:TextBox ID="txtState" runat="server" CssClass="h-12 w-full rounded-lg border border-black p-6" placeholder="Enter State" MaxLength="50" onKeyUp="validateState();"></asp:TextBox>
                            <asp:Label ID="lblStateError" runat="server" CssClass="text-red-700" Text=""></asp:Label>
                        </div>
                        <div class="my-2">
                            <p class="text-sm text-black">Country</p>
                            <asp:DropDownList ID="ddlCountryOrigin" runat="server" CssClass="h-12 w-full rounded-lg border border-black px-6 text-black" onchange="validateDropDownSelection();">
                                <asp:ListItem Value="default" Selected="true">Select Country</asp:ListItem>
                                <asp:ListItem>United States</asp:ListItem>
                                <asp:ListItem>Afghanistan</asp:ListItem>
                                <asp:ListItem>Albania</asp:ListItem>
                                <asp:ListItem>Algeria</asp:ListItem>
                                <asp:ListItem>American Samoa</asp:ListItem>
                                <asp:ListItem>Andorra</asp:ListItem>
                                <asp:ListItem>Angola</asp:ListItem>
                                <asp:ListItem>Anguilla</asp:ListItem>
                                <asp:ListItem>Antarctica</asp:ListItem>
                                <asp:ListItem>Antigua And Barbuda</asp:ListItem>
                                <asp:ListItem>Argentina</asp:ListItem>
                                <asp:ListItem>Armenia</asp:ListItem>
                                <asp:ListItem>Aruba</asp:ListItem>
                                <asp:ListItem>Australia</asp:ListItem>
                                <asp:ListItem>Austria</asp:ListItem>
                                <asp:ListItem>Azerbaijan</asp:ListItem>
                                <asp:ListItem>Bahamas</asp:ListItem>
                                <asp:ListItem>Bahrain</asp:ListItem>
                                <asp:ListItem>Bangladesh</asp:ListItem>
                                <asp:ListItem>Barbados</asp:ListItem>
                                <asp:ListItem>Belarus</asp:ListItem>
                                <asp:ListItem>Belgium</asp:ListItem>
                                <asp:ListItem>Belize</asp:ListItem>
                                <asp:ListItem>Benin</asp:ListItem>
                                <asp:ListItem>Bermuda</asp:ListItem>
                                <asp:ListItem>Bhutan</asp:ListItem>
                                <asp:ListItem>Bolivia</asp:ListItem>
                                <asp:ListItem>Bosnia And Herzegowina</asp:ListItem>
                                <asp:ListItem>Botswana</asp:ListItem>
                                <asp:ListItem>Bouvet Island</asp:ListItem>
                                <asp:ListItem>Brazil</asp:ListItem>
                                <asp:ListItem>British Indian Ocean Territory</asp:ListItem>
                                <asp:ListItem>Brunei Darussalam</asp:ListItem>
                                <asp:ListItem>Bulgaria</asp:ListItem>
                                <asp:ListItem>Burkina Faso</asp:ListItem>
                                <asp:ListItem>Burundi</asp:ListItem>
                                <asp:ListItem>Cambodia</asp:ListItem>
                                <asp:ListItem>Cameroon</asp:ListItem>
                                <asp:ListItem>Canada</asp:ListItem>
                                <asp:ListItem>Cape Verde</asp:ListItem>
                                <asp:ListItem>Cayman Islands</asp:ListItem>
                                <asp:ListItem>Central African Republic</asp:ListItem>
                                <asp:ListItem>Chad</asp:ListItem>
                                <asp:ListItem>Chile</asp:ListItem>
                                <asp:ListItem>China</asp:ListItem>
                                <asp:ListItem>Christmas Island</asp:ListItem>
                                <asp:ListItem>Cocos (Keeling) Islands</asp:ListItem>
                                <asp:ListItem>Colombia</asp:ListItem>
                                <asp:ListItem>Comoros</asp:ListItem>
                                <asp:ListItem>Congo</asp:ListItem>
                                <asp:ListItem>Cook Islands</asp:ListItem>
                                <asp:ListItem>Costa Rica</asp:ListItem>
                                <asp:ListItem>Cote D'Ivoire</asp:ListItem>
                                <asp:ListItem>Croatia (Local Name: Hrvatska)</asp:ListItem>
                                <asp:ListItem>Cuba</asp:ListItem>
                                <asp:ListItem>Cyprus</asp:ListItem>
                                <asp:ListItem>Czech Republic</asp:ListItem>
                                <asp:ListItem>Denmark</asp:ListItem>
                                <asp:ListItem>Djibouti</asp:ListItem>
                                <asp:ListItem>Dominica</asp:ListItem>
                                <asp:ListItem>Dominican Republic</asp:ListItem>
                                <asp:ListItem>East Timor</asp:ListItem>
                                <asp:ListItem>Ecuador</asp:ListItem>
                                <asp:ListItem>Egypt</asp:ListItem>
                                <asp:ListItem>El Salvador</asp:ListItem>
                                <asp:ListItem>Equatorial Guinea</asp:ListItem>
                                <asp:ListItem>Eritrea</asp:ListItem>
                                <asp:ListItem>Estonia</asp:ListItem>
                                <asp:ListItem>Ethiopia</asp:ListItem>
                                <asp:ListItem>Falkland Islands (Malvinas)</asp:ListItem>
                                <asp:ListItem>Faroe Islands</asp:ListItem>
                                <asp:ListItem>Fiji</asp:ListItem>
                                <asp:ListItem>Finland</asp:ListItem>
                                <asp:ListItem>France</asp:ListItem>
                                <asp:ListItem>French Guiana</asp:ListItem>
                                <asp:ListItem>French Polynesia</asp:ListItem>
                                <asp:ListItem>French Southern Territories</asp:ListItem>
                                <asp:ListItem>Gabon</asp:ListItem>
                                <asp:ListItem>Gambia</asp:ListItem>
                                <asp:ListItem>Georgia</asp:ListItem>
                                <asp:ListItem>Germany</asp:ListItem>
                                <asp:ListItem>Ghana</asp:ListItem>
                                <asp:ListItem>Gibraltar</asp:ListItem>
                                <asp:ListItem>Greece</asp:ListItem>
                                <asp:ListItem>Greenland</asp:ListItem>
                                <asp:ListItem>Grenada</asp:ListItem>
                                <asp:ListItem>Guadeloupe</asp:ListItem>
                                <asp:ListItem>Guam</asp:ListItem>
                                <asp:ListItem>Guatemala</asp:ListItem>
                                <asp:ListItem>Guinea</asp:ListItem>
                                <asp:ListItem>Guinea-Bissau</asp:ListItem>
                                <asp:ListItem>Guyana</asp:ListItem>
                                <asp:ListItem>Haiti</asp:ListItem>
                                <asp:ListItem>Honduras</asp:ListItem>
                                <asp:ListItem>Hong Kong</asp:ListItem>
                                <asp:ListItem>Hungary</asp:ListItem>
                                <asp:ListItem>Icel And</asp:ListItem>
                                <asp:ListItem>India</asp:ListItem>
                                <asp:ListItem>Indonesia</asp:ListItem>
                                <asp:ListItem>Iran (Islamic Republic Of)</asp:ListItem>
                                <asp:ListItem>Iraq</asp:ListItem>
                                <asp:ListItem>Ireland</asp:ListItem>
                                <asp:ListItem>Israel</asp:ListItem>
                                <asp:ListItem>Italy</asp:ListItem>
                                <asp:ListItem>Jamaica</asp:ListItem>
                                <asp:ListItem>Japan</asp:ListItem>
                                <asp:ListItem>Jordan</asp:ListItem>
                                <asp:ListItem>Kazakhstan</asp:ListItem>
                                <asp:ListItem>Kenya</asp:ListItem>
                                <asp:ListItem>Kiribati</asp:ListItem>
                                <asp:ListItem>Korea</asp:ListItem>
                                <asp:ListItem>Kuwait</asp:ListItem>
                                <asp:ListItem>Kyrgyzstan</asp:ListItem>
                                <asp:ListItem>Lao People'S Dem Republic</asp:ListItem>
                                <asp:ListItem>Latvia</asp:ListItem>
                                <asp:ListItem>Lebanon</asp:ListItem>
                                <asp:ListItem>Lesotho</asp:ListItem>
                                <asp:ListItem>Liberia</asp:ListItem>
                                <asp:ListItem>Libyan Arab Jamahiriya</asp:ListItem>
                                <asp:ListItem>Liechtenstein</asp:ListItem>
                                <asp:ListItem>Lithuania</asp:ListItem>
                                <asp:ListItem>Luxembourg</asp:ListItem>
                                <asp:ListItem>Macau</asp:ListItem>
                                <asp:ListItem>Macedonia</asp:ListItem>
                                <asp:ListItem>Madagascar</asp:ListItem>
                                <asp:ListItem>Malawi</asp:ListItem>
                                <asp:ListItem>Malaysia</asp:ListItem>
                                <asp:ListItem>Maldives</asp:ListItem>
                                <asp:ListItem>Mali</asp:ListItem>
                                <asp:ListItem>Malta</asp:ListItem>
                                <asp:ListItem>Marshall Islands</asp:ListItem>
                                <asp:ListItem>Martinique</asp:ListItem>
                                <asp:ListItem>Mauritania</asp:ListItem>
                                <asp:ListItem>Mauritius</asp:ListItem>
                                <asp:ListItem>Mayotte</asp:ListItem>
                                <asp:ListItem>Mexico</asp:ListItem>
                                <asp:ListItem>Micronesia, Federated States</asp:ListItem>
                                <asp:ListItem>Moldova, Republic Of</asp:ListItem>
                                <asp:ListItem>Monaco</asp:ListItem>
                                <asp:ListItem>Mongolia</asp:ListItem>
                                <asp:ListItem>Montserrat</asp:ListItem>
                                <asp:ListItem>Morocco</asp:ListItem>
                                <asp:ListItem>Mozambique</asp:ListItem>
                                <asp:ListItem>Myanmar</asp:ListItem>
                                <asp:ListItem>Namibia</asp:ListItem>
                                <asp:ListItem>Nauru</asp:ListItem>
                                <asp:ListItem>Nepal</asp:ListItem>
                                <asp:ListItem>Netherlands</asp:ListItem>
                                <asp:ListItem>Netherlands Ant Illes</asp:ListItem>
                                <asp:ListItem>New Caledonia</asp:ListItem>
                                <asp:ListItem>New Zealand</asp:ListItem>
                                <asp:ListItem>Nicaragua</asp:ListItem>
                                <asp:ListItem>Niger</asp:ListItem>
                                <asp:ListItem>Nigeria</asp:ListItem>
                                <asp:ListItem>Niue</asp:ListItem>
                                <asp:ListItem>Norfolk Island</asp:ListItem>
                                <asp:ListItem>Northern Mariana Islands</asp:ListItem>
                                <asp:ListItem>Norway</asp:ListItem>
                                <asp:ListItem>Oman</asp:ListItem>
                                <asp:ListItem>Pakistan</asp:ListItem>
                                <asp:ListItem>Palau</asp:ListItem>
                                <asp:ListItem>Panama</asp:ListItem>
                                <asp:ListItem>Papua New Guinea</asp:ListItem>
                                <asp:ListItem>Paraguay</asp:ListItem>
                                <asp:ListItem>Peru</asp:ListItem>
                                <asp:ListItem>Philippines</asp:ListItem>
                                <asp:ListItem>Pitcairn</asp:ListItem>
                                <asp:ListItem>Poland</asp:ListItem>
                                <asp:ListItem>Portugal</asp:ListItem>
                                <asp:ListItem>Puerto Rico</asp:ListItem>
                                <asp:ListItem>Qatar</asp:ListItem>
                                <asp:ListItem>Reunion</asp:ListItem>
                                <asp:ListItem>Romania</asp:ListItem>
                                <asp:ListItem>Russian Federation</asp:ListItem>
                                <asp:ListItem>Rwanda</asp:ListItem>
                                <asp:ListItem>Saint K Itts And Nevis</asp:ListItem>
                                <asp:ListItem>Saint Lucia</asp:ListItem>
                                <asp:ListItem>Saint Vincent, The Grenadines</asp:ListItem>
                                <asp:ListItem>Samoa</asp:ListItem>
                                <asp:ListItem>San Marino</asp:ListItem>
                                <asp:ListItem>Sao Tome And Principe</asp:ListItem>
                                <asp:ListItem>Saudi Arabia</asp:ListItem>
                                <asp:ListItem>Senegal</asp:ListItem>
                                <asp:ListItem>Seychelles</asp:ListItem>
                                <asp:ListItem>Sierra Leone</asp:ListItem>
                                <asp:ListItem>Singapore</asp:ListItem>
                                <asp:ListItem>Slovakia (Slovak Republic)</asp:ListItem>
                                <asp:ListItem>Slovenia</asp:ListItem>
                                <asp:ListItem>Solomon Islands</asp:ListItem>
                                <asp:ListItem>Somalia</asp:ListItem>
                                <asp:ListItem>South Africa</asp:ListItem>
                                <asp:ListItem>South Georgia , S Sandwich Is.</asp:ListItem>
                                <asp:ListItem>Spain</asp:ListItem>
                                <asp:ListItem>Sri Lanka</asp:ListItem>
                                <asp:ListItem>St. Helena</asp:ListItem>
                                <asp:ListItem>St. Pierre And Miquelon</asp:ListItem>
                                <asp:ListItem>Sudan</asp:ListItem>
                                <asp:ListItem>Suriname</asp:ListItem>
                                <asp:ListItem>Svalbard, Jan Mayen Islands</asp:ListItem>
                                <asp:ListItem>Sw Aziland</asp:ListItem>
                                <asp:ListItem>Sweden</asp:ListItem>
                                <asp:ListItem>Switzerland</asp:ListItem>
                                <asp:ListItem>Syrian Arab Republic</asp:ListItem>
                                <asp:ListItem>Taiwan</asp:ListItem>
                                <asp:ListItem>Tajikistan</asp:ListItem>
                                <asp:ListItem>Tanzania, United Republic Of</asp:ListItem>
                                <asp:ListItem>Thailand</asp:ListItem>
                                <asp:ListItem>Togo</asp:ListItem>
                                <asp:ListItem>Tokelau</asp:ListItem>
                                <asp:ListItem>Tonga</asp:ListItem>
                                <asp:ListItem>Trinidad And Tobago</asp:ListItem>
                                <asp:ListItem>Tunisia</asp:ListItem>
                                <asp:ListItem>Turkey</asp:ListItem>
                                <asp:ListItem>Turkmenistan</asp:ListItem>
                                <asp:ListItem>Turks And Caicos Islands</asp:ListItem>
                                <asp:ListItem>Tuvalu</asp:ListItem>
                                <asp:ListItem>Uganda</asp:ListItem>
                                <asp:ListItem>Ukraine</asp:ListItem>
                                <asp:ListItem>United Arab Emirates</asp:ListItem>
                                <asp:ListItem>United Kingdom</asp:ListItem>
                                <asp:ListItem>United States</asp:ListItem>
                                <asp:ListItem>United States Minor Is.</asp:ListItem>
                                <asp:ListItem>Uruguay</asp:ListItem>
                                <asp:ListItem>Uzbekistan</asp:ListItem>
                                <asp:ListItem>Vanuatu</asp:ListItem>
                                <asp:ListItem>Venezuela</asp:ListItem>
                                <asp:ListItem>Viet Nam</asp:ListItem>
                                <asp:ListItem>Virgin Islands (British)</asp:ListItem>
                                <asp:ListItem>Virgin Islands (U.S.)</asp:ListItem>
                                <asp:ListItem>Wallis And Futuna Islands</asp:ListItem>
                                <asp:ListItem>Western Sahara</asp:ListItem>
                                <asp:ListItem>Yemen</asp:ListItem>
                                <asp:ListItem>Zaire</asp:ListItem>
                                <asp:ListItem>Zambia</asp:ListItem>
                                <asp:ListItem>Zimbabwe</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblCountryError" runat="server" CssClass="text-red-700" Text=""></asp:Label>
                        </div>

                        <!--Add address button-->
                        <div class="mt-8">
                            <asp:Button ID="btnAddAddress" OnClick="btnAddAddress_Click" runat="server" Text="Add New Address" CssClass="bg-[#131118] h-12 w-1/3 cursor-pointer rounded-xl px-4 py-2 font-semibold text-white hover:bg-gray-700" />
                        </div>

                    </div>
                </asp:Panel>
            </div>

            <!--Right Container-->
            <div class="w-[30%]">

                <!-- Order Summary -->
                <div id="orderSummary" runat="server" class="flex-wrap rounded-xl bg-white p-4 drop-shadow-lg">
                    <h2 class="mb-2 ml-6 mr-6 mt-2 text-2xl font-bold">Order Summary</h2>

                    <div>
                        <asp:ListView ID="ProductListView" runat="server">
                            <LayoutTemplate>
                                <div>
                                    <table>
                                        <tr runat="server" id="itemPlaceholder"></tr>
                                    </table>
                                </div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <!-- Product item template -->
                                <div class="border-b border-gray-200">
                                    <div class="px-4 py-6">
                                        <div class="flex flex-wrap items-center">
                                            <asp:Image ID="imgProduct" runat="server" AlternateText="Image" ImageUrl='<%# Eval("ProductImageUrl") %>' CssClass="w-16 h-16 mr-4" />
                                            <div>
                                                <div class="text-lg font-semibold capitalize text-black"><%# Eval("ProductName") %></div>
                                                <div class="text-sm">Color: <%# Eval("ColorName") %></div>
                                                <div class="text-sm">Size: <%# Eval("SizeName") %></div>
                                                <div class="text-sm">Quantity: <%# Eval("Quantity") %></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>

                    </div>

                    <div class="mb-2 ml-6 mr-6 flex flex-wrap justify-between pt-4">
                        <span class="text-gray-500">Items</span>
                        <asp:Label ID="lblItemPrice" runat="server" CssClass="text-gray-500" Text="itemprice"></asp:Label>
                    </div>

                    <div class="mb-2 ml-6 mr-6 flex flex-wrap justify-between">
                        <span class="text-gray-500">Discount</span>
                        <div>
                            <asp:Label ID="lblDiscountRate" runat="server" CssClass="text-gray-500" Text="(0%)"></asp:Label>
                            <asp:Label ID="lblDiscount" runat="server" CssClass="text-gray-500" Text="RM0.00"></asp:Label>
                        </div>
                    </div>

                    <div class="mb-2 ml-6 mr-6 flex flex-wrap justify-between">
                        <span class="text-gray-500">Delivery Cost</span>
                        <asp:Label ID="lblDeliveryCost" runat="server" CssClass="text-gray-500" Text="RM5.00"></asp:Label>
                    </div>
                    <div class="ml-6 mr-6 flex flex-wrap justify-between font-semibold">
                        <span class="text-gray-500">Total</span>
                        <asp:Label ID="lblTotal" runat="server" CssClass="text-gray-500" Text="total"></asp:Label>
                    </div>
                </div>

                <!--Submit button-->
                <div class="mt-8">
                    <asp:Button ID="btnProceed" runat="server" Text="Proceed to Payment" CssClass="bg-[#131118] w-full cursor-pointer rounded-xl px-4 py-2 font-semibold text-white hover:bg-gray-700" OnClick="btnProceed_Click" />

                    <asp:Label ID="lblMessage" runat="server" CssClass="text-xl text-red-600" Visible="false" Text="ERROR! Please select an address."></asp:Label>
                </div>


            </div>
            <!-- Add more input fields as needed -->

        </div>

    </div>

    <script>

        function validateNickname() {
            var txtNickname = document.getElementById('<%= txtNickname.ClientID %>');

            if (txtNickname.value.trim() !== '') {
                // Remove the error-border class to revert the border color to black
                txtNickname.classList.remove('error-border');
            } else {
                txtNickname.classList.add('error-border');
            }
        }

        function validateAddr() {
            var addr = document.getElementById('<%= txtAddr.ClientID %>');

            if (addr.value.trim() !== '') {
                // Remove the error-border class to revert the border color to black
                addr.classList.remove('error-border');
            } else {
                addr.classList.add('error-border');
            }
        }

        function validateState() {
            var state = document.getElementById('<%= txtState.ClientID %>');

            if (state.value.trim() !== '') {
                // Remove the error-border class to revert the border color to black
                state.classList.remove('error-border');
            } else {
                state.classList.add('error-border');
            }
        }

        function validateNumericInput() {

            var postal = document.getElementById('<%= txtPostal.ClientID %>');

            postal.value = postal.value.replace(/\D/g, '');

            if (postal.value.trim() !== '') {
                // Remove the error-border class to revert the border color to black
                postal.classList.remove('error-border');
            } else {
                postal.classList.add('error-border');
            }
        }

        function validateDropDownSelection() {
            var ddlCountryOrigin = document.getElementById('<%= ddlCountryOrigin.ClientID %>');
            var selectedValue = ddlCountryOrigin.value;

            if (selectedValue !== 'default') {
                ddlCountryOrigin.classList.remove('error-border');
            }
            else {
                ddlCountryOrigin.classList.add('error-border');
            }
        }

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

    <style>
        .error-border {
            border-color: red;
        }
    </style>


</asp:Content>


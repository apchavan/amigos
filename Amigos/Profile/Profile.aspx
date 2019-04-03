<%@ Page Language="C#" MasterPageFile="~/AccountMaster/UserAccountMaster.master" AutoEventWireup="true" CodeFile="Profile.aspx.cs" Inherits="Profile_Profile" %>

<asp:Content ID="profile_Head" ContentPlaceHolderID="head" runat="Server">
    <link rel="icon" type="image/x-icon" href="../Images/amigos_tab_icon.ico" />
    <link rel="stylesheet" type="text/css" href="Profile_StyleSheet.css" />

    <title>Profile</title>
</asp:Content>

<asp:Content ID="profile_Content" ContentPlaceHolderID="accountMaster_Content" runat="Server">

    <!-- Reference of code below :=> https://bootsnipp.com/snippets/X275r -->

    <div class="container">
        <asp:Button ID="updateProfileBtn" CssClass="updateProfileBtn" OnClick="updateProfileBtn_Click" runat="server" Text="Update profile" />
        <div class="row">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Your Profile</h4>
                </div>
                <hr />

                <div class="panel-body">
                    <div class="col-md-4 col-xs-12 col-sm-6 col-lg-4">
                        <img alt="User Pic" oncontextmenu="return false;" src="~/Images/no_image.jpg" id="profile_Image" class="rounded-circle card-img" runat="server" />
                    </div>
                    <div class="col-md-8 col-xs-12 col-sm-6 col-lg-8">
                        <div class="container">
                            <h2>
                                <asp:Label ID="uname_Label" Text="--" runat="server"></asp:Label>
                            </h2>
                        </div>
                        <hr />
                        <ul class="container details">
                            <li>
                                <p>
                                    <span class="fa fa-envelope" aria-hidden="true" style="width: 36px;"></span><strong>Email : </strong>
                                    <asp:Label ID="email_Label" Text="--" runat="server"></asp:Label>
                                </p>
                            </li>
                            <li>
                                <p>
                                    <span class="fa fa-user" aria-hidden="true" style="width: 36px;"></span><strong>Profession : </strong>
                                    <asp:Label ID="profession_Label" Text="Not provided." runat="server"></asp:Label>
                                </p>
                            </li>
                            <li>
                                <p>
                                    <span class="fa fa-briefcase" aria-hidden="true" style="width: 36px;"></span><strong>Profession At : </strong>
                                    <asp:Label ID="at_Label" Text="Not provided." runat="server"></asp:Label>
                                </p>
                            </li>
                            <li>
                                <p>
                                    <span class="fa fa-birthday-cake" aria-hidden="true" style="width: 36px;"></span><strong>Date Of Birth : </strong>
                                    <asp:Label ID="dob_Label" Text="--" runat="server"></asp:Label>
                                </p>
                            </li>
                        </ul>
                        <hr />
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>


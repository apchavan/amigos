<%@ Page Title="" Language="C#" MasterPageFile="~/AccountMaster/UserAccountMaster.master" AutoEventWireup="true" CodeFile="FriendRequestsProfile.aspx.cs" Inherits="FriendRequests_FriendRequestsProfile" %>

<asp:Content ID="friendRequestsProfile_Head" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="FriendRequestsProfile_StyleSheet.css" />
    <title id="otherUserProfile_Title" runat="server"></title>
</asp:Content>
<asp:Content ID="friendRequestsProfile_Content" ContentPlaceHolderID="accountMaster_Content" runat="Server">

    <!-- Reference of code below :=> https://bootsnipp.com/snippets/X275r -->

    <div class="container">

        <asp:Button ID="rejectRequest_Btn" OnClick="rejectRequest_Btn_Click" CssClass="btn btn-outline-danger rejectRequestBtn" runat="server" Text="Reject" />
        
        <asp:Button ID="acceptRequest_Btn" OnClick="acceptRequest_Btn_Click" CssClass="btn btn-outline-dark acceptRequestBtn" runat="server" Text="Accept" />

        <div class="row">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>
                        <asp:Label ID="heading_Label" Text="User Profile : " runat="server" />
                    </h4>
                </div>
                <hr />

                <div class="panel-body">
                    <div class="col-md-4 col-xs-12 col-sm-6 col-lg-4">
                        <img id="profile_Image" alt="User Pic" oncontextmenu="return false;" src="~/Images/no_image.jpg" class="rounded-circle card-img" runat="server" />
                    </div>
                    <div class="col-md-8 col-xs-12 col-sm-6 col-lg-8">
                        <div class="container">
                            <h2>
                                <asp:Label ID="uname_Label" Text="--" runat="server"></asp:Label>
                                <!--<asp:Label ID="profileStatus_Label" Text="" runat="server"></asp:Label>-->
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


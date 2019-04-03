<%@ Page Title="" Language="C#" MasterPageFile="~/AccountMaster/UserAccountMaster.master" AutoEventWireup="true" CodeFile="FriendRequests.aspx.cs" Inherits="FriendRequests_FriendRequests" %>

<asp:Content ID="friendRequests_Head" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="FriendRequests_StyleSheet.css" />

    <title>Friend requests 🎉😍😎🎉</title>

</asp:Content>
<asp:Content ID="friendRequests_Content" ContentPlaceHolderID="accountMaster_Content" runat="Server">
    <!--
    <div class="box">
        <div class="container">
            <div class="row">

                <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">

                    <div class="box-part text-center">

                        <asp:Image ID="otherUserProfile_Image" CssClass="rounded-circle" Width="45" ImageUrl="~/Images/Steve_Jobs_Apple.png" oncontextmenu="return false;" runat="server" />

                        <div class="title">
                            <h4>
                                <asp:LinkButton ID="otherUserName_LinkBtn" ToolTip="Click here to show profile..." CommandName="otherUserName_CommandName" CssClass="otherUserNameLinkBtn"
                                    oncontextmenu="return false;"
                                    Text="NAME" runat="server" />
                            </h4>
                        </div>

                        <div class="text">
                            <span>
                                <asp:Label ID="profession_and_at_Label" Text="PROF_AT" runat="server" />
                            </span>
                        </div>

                        <asp:Button ID="accept_Btn" Text="Accept" CssClass="btn btn-outline-dark" ToolTip="Accept friend request" runat="server" />
                        <asp:Button ID="reject_Btn" Text="Reject" CssClass="btn btn-outline-danger" ToolTip="Reject friend request" runat="server" />

                    </div>
                </div>
            </div>
        </div>
    </div>
    -->
    <div style="text-align: center; top: 0;">
        <asp:Label ID="heading_Label" Width="100%" runat="server" Text=""></asp:Label>
        <hr />
    </div>
    <asp:DataList ID="friendRequest_DataList" CellPadding="0" CellSpacing="0" RepeatColumns="2" RepeatDirection="Horizontal" Width="100%" runat="server" OnItemCommand="friendRequest_DataList_ItemCommand" OnItemDataBound="friendRequest_DataList_ItemDataBound">
        <ItemStyle Width="50%" />
        <ItemTemplate>
            <div class="box">
                <div class="container">
                    <div class="row">

                        <!--<div class="col-lg-4 col-md-4 col-sm-4 col-xs-12">-->
                        <div class="col-md-12">
                            <div class="box-part text-center">

                                <asp:Image ID="otherUserProfile_Image" CssClass="rounded-circle" Width="50" Height="50" ImageUrl='<%#Eval("photo") %>' oncontextmenu="return false;" runat="server" />

                                <div class="title">
                                    <h4>
                                        <asp:LinkButton ID="otherUserName_LinkBtn" CommandName="otherUserName_LinkBtn_CommandName" CssClass="otherUserNameLinkBtn" ToolTip="Click here to show profile..."
                                            oncontextmenu="return false;"
                                            Text='<%# String.Format("{0} {1}", Eval("firstname"), Eval("lastname")) %>' runat="server" />
                                    </h4>
                                    <asp:HiddenField ID="otherUserID_HiddenField" runat="server"
                                        Value='<%#Eval("UserID") %>' />
                                </div>

                                <div class="text">
                                    <span>
                                        <asp:Label ID="profession_and_at_Label" Text='<%# String.Format("{0} -- {1}", Eval("profession"), Eval("at")) %>' runat="server" />
                                    </span>
                                </div>

                                <asp:Button ID="accept_Btn" CommandName="accept_Btn_CommandName" Text="Accept" CssClass="btn btn-outline-dark" ToolTip="Accept friend request" runat="server" />
                                <asp:Button ID="reject_Btn" CommandName="reject_Btn_CommandName" Text="Reject" CssClass="btn btn-outline-danger" ToolTip="Reject friend request" runat="server" />

                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <hr />
        </ItemTemplate>
    </asp:DataList>

</asp:Content>


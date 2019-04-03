<%@ Page Title="" Language="C#" MasterPageFile="~/AccountMaster/UserAccountMaster.master" AutoEventWireup="true" CodeFile="FriendsList.aspx.cs" Inherits="FriendsList_FriendsList" %>

<asp:Content ID="friendsList_Head" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="FriendsList_StyleSheet.css" />

    <title>Friends list 🎉😍😎🎉</title>
</asp:Content>


<asp:Content ID="friendsList_Content" ContentPlaceHolderID="accountMaster_Content" runat="Server">

    <div style="text-align: center; top: 0;">
        <asp:Label ID="heading_Label" Width="100%" runat="server" Text=""></asp:Label>
        <hr />
    </div>
    <asp:DataList ID="friendsList_DataList" CellPadding="0" CellSpacing="0" RepeatColumns="2" RepeatDirection="Horizontal" Width="100%" runat="server" OnItemCommand="friendsList_DataList_ItemCommand" OnItemDataBound="friendsList_DataList_ItemDataBound">
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
                                        <asp:LinkButton ID="otherUserName_LinkBtn" CommandName="otherUserName_LinkBtn_CommandName"
                                            oncontextmenu="return false;" CssClass="otherUserNameLinkBtn"
                                            ToolTip="Click to show profile..."
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

                                <asp:Button ID="unfriend_Btn" CommandName="unfriend_Btn_CommandName" Text="Unfriend" CssClass="btn btn-outline-danger" runat="server" />

                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <hr />
        </ItemTemplate>
    </asp:DataList>

</asp:Content>


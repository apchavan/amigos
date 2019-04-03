<%@ Page Title="" Language="C#" MasterPageFile="~/AccountMaster/AdminAccountMaster.master" AutoEventWireup="true" CodeFile="BlockedUsers.aspx.cs" Inherits="BlockedUsers_BlockedUsers" %>

<asp:Content ID="blockedUsers_Head" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="BlockedUsers_StyleSheet.css" />
    <title>🚫❌ Blocked users ❌🚫</title>
</asp:Content>

<asp:Content ID="blockedUsers_Content" ContentPlaceHolderID="accountMaster_Content" runat="Server">

    <asp:ScriptManager ID="blockedUsers_ScriptManager" runat="server" />
    <asp:UpdatePanel ID="blockedUsers_UpdatePanel" UpdateMode="Conditional" runat="server">
        <ContentTemplate>

            <div style="text-align: center; top: 0;">
                <asp:Label ID="heading_Label" Width="100%" runat="server" Text=""></asp:Label>
                <hr />
            </div>
            <br />

            <asp:DataList ID="blockedUsers_DataList" CellPadding="0" CellSpacing="0" runat="server" Width="100%"
                OnItemCommand="blockedUsers_DataList_ItemCommand" OnItemDataBound="blockedUsers_DataList_ItemDataBound">
                <ItemTemplate>

                    <div class="shadow-sm card gedf-card">
                        <div class="card-header">
                            <div class="d-flex justify-content-between align-items-center" style="width: 100%;">
                                <div class="d-flex justify-content-between align-items-center">
                                    <div class="mr-2">
                                        <asp:Image ID="otherUserProfile_Image" CssClass="rounded-circle" Width="45" ImageUrl='<%#Eval("photo") %>' oncontextmenu="return false;" runat="server" />
                                    </div>
                                    <div class="ml-2">
                                        <div class="h5 m-0">
                                            <asp:Label ID="otherUserName_Label" runat="server" oncontextmenu="return false;"
                                                CssClass="otherUserNameLabel"
                                                Text='<%# String.Format("{0} {1}", Eval("firstname"), Eval("lastname")) %>'></asp:Label>
                                            <asp:HiddenField ID="otherUserID_HiddenField" runat="server"
                                                Value='<%#Eval("UserID") %>' />
                                        </div>

                                        <div class="h7 text-muted">
                                            <asp:Label ID="profession_and_at_Label" Text='<%# String.Format("{0} -- {1}", Eval("profession"), Eval("at")) %>' runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <asp:Button ID="unBlock_Btn" CommandName="unBlock_Btn_CommandName" CssClass="btn btn-outline-success" Text="Unblock" runat="server" />
                            </div>
                        </div>


                        <asp:Panel ID="confirmUnblock_Panel" Visible="false" Width="100%" runat="server">
                            <div style="margin-top: 40px;">
                                <div style="text-align: center;">
                                    <div style="font-size: xx-large;">{&nbsp;<i class="fa fa-unlock" aria-hidden="true"></i>&nbsp;}</div>
                                    <asp:Label ID="unblockUser_Label" runat="server"
                                        Text=""></asp:Label>
                                </div>
                            </div>

                            <asp:Button ID="cancelUnblockUser_Button" CommandName="cancelUnblockUser_Button_CommandName"
                                Style="margin-top: 10px; float: right; margin-right: 10px; margin-bottom: 10px;"
                                runat="server" CssClass="btn btn-warning" Text="Cancel" />

                            <asp:Button ID="yesUnblockUser_Button" CommandName="yesUnblockUser_Button_CommandName"
                                Style="margin-top: 10px; float: right; margin-right: 10px; margin-bottom: 10px;"
                                runat="server" CssClass="btn btn-outline-success" Text="Yes, unblock ..." />
                        </asp:Panel>

                    </div>

                </ItemTemplate>
            </asp:DataList>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


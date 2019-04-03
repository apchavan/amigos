<%@ Page Title="" Language="C#" MasterPageFile="~/AccountMaster/UserAccountMaster.master" AutoEventWireup="true" CodeFile="SearchResult.aspx.cs" Inherits="SearchResult_SearchResult" %>

<asp:Content ID="searchResult_Head" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="SearchResult_StyleSheet.css" />
    <link rel="stylesheet" href="../Styles_and_scripts/bootstrap.min.css" />

    <script src="../Styles_and_scripts/jquery-3.3.1.min.js"></script>
    <script src="../Styles_and_scripts/popper.min.js"></script>
    <script src="../Styles_and_scripts/bootstrap.min.js"></script>

    <title>Search friends 🤝</title>
</asp:Content>

<asp:Content ID="searchResult_Content" ContentPlaceHolderID="accountMaster_Content" runat="Server">

    <div style="text-align: center; top: 0;">
        <asp:Label ID="heading_Label" Width="100%" runat="server" Text=""></asp:Label>
        <hr />
    </div>
    <br />

    <asp:ScriptManager ID="search_ScriptManager" runat="server" />
    <asp:UpdatePanel ID="search_UpdatePanel" UpdateMode="Conditional" runat="server">
        <ContentTemplate>

            <asp:DataList ID="search_DataList" CellPadding="0" CellSpacing="0" runat="server" Width="100%" OnItemCommand="search_DataList_ItemCommand" OnItemDataBound="search_DataList_ItemDataBound">
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
                                            <asp:LinkButton ID="otherUserName_LinkBtn" CommandName="otherUserName_LinkBtn_CommandName"
                                                oncontextmenu="return false;" CssClass="otherUserNameLinkBtn"
                                                data-toggle="tooltip" data-placement="top" ToolTip="Click to show profile..."
                                                Text='<%# String.Format("{0} {1}", Eval("firstname"), Eval("lastname")) %>' runat="server" />

                                            <asp:Label ID="requestStatus_Label" CssClass="h7" Text="" runat="server" />

                                            <asp:HiddenField ID="otherUserID_HiddenField" runat="server"
                                                Value='<%#Eval("UserID") %>' />
                                        </div>

                                        <div class="h7 text-muted">
                                            <asp:Label ID="profession_and_at_Label" Text='<%# String.Format("{0} -- {1}", Eval("profession"), Eval("at")) %>' runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <asp:Button ID="sendFriendRequest_Btn" CommandName="sendFriendRequest_Btn_CommandName" CssClass="btn btn-outline-dark" Text="Send friend request" runat="server" />
                            </div>
                        </div>
                    </div>

                </ItemTemplate>
            </asp:DataList>

        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- https://www.aspforums.net/Threads/189355/Bootstrap-Popover-and-ToolTip-not-working-inside-Update-Panel-after-postback/ -->
    <script>
        function pageLoad() {
            $('[data-toggle="tooltip"]').tooltip()
        };
    </script>

</asp:Content>

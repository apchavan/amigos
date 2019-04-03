<%@ Page Title="" Language="C#" MasterPageFile="~/AccountMaster/AdminAccountMaster.master" AutoEventWireup="true" CodeFile="TrendingPosts.aspx.cs" Inherits="TrendingPosts_TrendingPosts" %>

<asp:Content ID="trendingPosts_Head" ContentPlaceHolderID="head" runat="Server">

    <link rel="stylesheet" href="TrendingPosts_StyleSheet.css" />
    <link rel="stylesheet" href="../Styles_and_scripts/bootstrap.min.css" />

    <script src="../Styles_and_scripts/jquery-3.3.1.min.js"></script>
    <script src="../Styles_and_scripts/popper.min.js"></script>
    <script src="../Styles_and_scripts/bootstrap.min.js"></script>
    
    <title>😍🔥📈⚡ Trending posts ⚡📈🔥😍</title>
</asp:Content>

<asp:Content ID="trendingPosts_Content" ContentPlaceHolderID="accountMaster_Content" runat="Server">

    <asp:ScriptManager ID="trendingPosts_ScriptManager" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="trendingPosts_UpdatePanel" runat="server">
        <ContentTemplate>

            <asp:DataList ID="trendingPosts_DataList" Width="100%" runat="server"
                OnItemDataBound="trendingPosts_DataList_ItemDataBound">
                <ItemTemplate>

                    <div class="card gedf-card" style="top: 0;">
                        <div class="card-header">
                            <div class="d-flex justify-content-between align-items-center">
                                <div class="d-flex justify-content-between align-items-center">
                                    <div class="mr-2">
                                        <!-- <img class="rounded-circle" width="45" src="https://picsum.photos/50/50" alt="" /> -->
                                        <asp:Image ID="otherUserProfile_Image" Width="50" Height="40" CssClass="rounded-circle shadow-lg"
                                            Style="border-style: groove;" oncontextmenu="return false;"
                                            ImageUrl='<%#Eval("photo") %>' runat="server" />
                                    </div>
                                    <div class="ml-2">
                                        <div class="h5 m-0">

                                            <asp:Label ID="otherUserName_Label" runat="server" oncontextmenu="return false;"
                                                CssClass="otherUserNameLabel"
                                                Text='<%# String.Format("{0} {1}", Eval("firstname"), Eval("lastname")) %>'></asp:Label>


                                            <!-- Label below shows '(😎 It's you !)' message if post is of current user -->
                                            <asp:Label ID="postOwnerStatus_Label" CssClass="h7" Text="" runat="server" />
                                        </div>
                                        <!-- <div class="h7 text-muted">Miracles Lee Cross</div> -->
                                        <div class="text-muted h7 mb-2">
                                            <i class="fa fa-clock-o"></i>
                                            <asp:Label ID="postTime_Label" Text='<%# String.Format("Posted at: {0}", Eval("dated")) %>' runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="card-body">

                                <span class="card-link">
                                    <h5 class="card-title" style="text-align: center;">
                                        <asp:Label ID="postHeading_Label" CssClass="postHeadingLabel" runat="server"
                                            Text='<%#Eval("post_heading") %>'></asp:Label>
                                        <!-- Lorem ipsum dolor sit amet, consectetur adip. -->
                                    </h5>
                                </span>

                                <div class="cardbox-item">
                                    <!-- https://images.pexels.com/photos/1295036/pexels-photo-1295036.jpeg?auto=compress&cs=tinysrgb&dpr=2&h=750&w=1260 -->
                                    <!-- <img class="figure-img rounded" src="http://www.themashabrand.com/templates/bootsnipp/post/assets/img/1.jpg" style="display: block; margin-left: auto; margin-right: auto; width: 25%; height: 10%;" alt="Image" /> -->
                                    <asp:Image ID="postImage_Image" CssClass="figure-img rounded shadow-lg" oncontextmenu="return false;"
                                        Style="display: block; margin-left: auto; margin-right: auto; width: 35%; height: 20%;"
                                        ImageUrl='<%#Eval("post_image") %>' runat="server" />
                                    <!-- "http://www.themashabrand.com/templates/bootsnipp/post/assets/img/1.jpg" -->
                                </div>
                                <!-- cardbox-item -->


                                <div style="text-align: center; clear: both;">
                                    <!-- class="card-text" -->
                                    <asp:Label ID="postText_Label" runat="server"
                                        Text='<%#Eval("post_text") %>'></asp:Label>
                                </div>
                            </div>

                            <div class="card-footer card-link shadow-lg" style="text-align: center; border-radius: 25px;">
                                <asp:Label ID="likesTotal_Label" data-toggle="tooltip" data-placement="left" ToolTip="Likes"
                                    CssClass="card-link likesTotalLabel" Text="" runat="server" />
                                <asp:Label ID="commentsTotal_Label" data-toggle="tooltip" data-placement="bottom" ToolTip="Comments"
                                    CssClass="card-link commentsTotalLabel" Text="" runat="server" />
                                <asp:Label ID="reportedTotal_Label" data-toggle="tooltip" data-placement="right" ToolTip="Reports"
                                    CssClass="card-link reportedTotalLabel" Text="" runat="server" />
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


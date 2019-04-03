<%@ Page Title="" Language="C#" MasterPageFile="~/AccountMaster/AdminAccountMaster.master" AutoEventWireup="true" CodeFile="ReportedPosts.aspx.cs" Inherits="ReportedPosts_ReportedPosts" %>

<asp:Content ID="reportedPosts_Head" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="ReportedPosts_StyleSheet.css" />
    <link rel="stylesheet" href="../Styles_and_scripts/bootstrap.min.css" />

    <script src="../Styles_and_scripts/jquery-3.3.1.min.js"></script>
    <script src="../Styles_and_scripts/popper.min.js"></script>
    <script src="../Styles_and_scripts/bootstrap.min.js"></script>

    <title>🔽❓📉❌ Reported posts ❌📉❓🔽</title>
</asp:Content>
<asp:Content ID="reportedPosts_Content" ContentPlaceHolderID="accountMaster_Content" runat="Server">

    <!-- 'reportedPosts_ScriptManager' to handle all AJAX stuff or AJAX extensions in page. -->
    <asp:ScriptManager ID="reportedPosts_ScriptManager" runat="server"></asp:ScriptManager>


    <asp:UpdatePanel ID="reportedPosts_UpdatePanel" UpdateMode="Conditional" runat="server">
        <ContentTemplate>

            <div class="text-capitalize" style="text-align: center;">
                <!-- 'text-capitalize' class makes first letters of words in sentence in capital case -->
                <asp:Label ID="topHeading_Label" runat="server" Text=""></asp:Label>
            </div>

            <asp:DataList ID="reportedPosts_DataList" Width="100%" runat="server"
                OnItemCommand="reportedPosts_DataList_ItemCommand"
                OnItemDataBound="reportedPosts_DataList_ItemDataBound">
                <ItemTemplate>

                    <div class="card gedf-card shadow" style="top: 0;">
                        <div class="card-header">

                            <div class="card-body">

                                <div class="card-link">

                                    <div class="text-secondary text-center" style="float: right;">
                                        Reported at:&nbsp;
                                        <asp:Label ID="reportedTime_Label" runat="server"
                                            Text='<%# String.Format("{0}", Eval("report_dated")) %>'></asp:Label>
                                    </div>
                                    <div class="card-title" style="text-align: left;">
                                        <strong>Post author:</strong>
                                        <asp:Label ID="postAuthorName_Label" runat="server" oncontextmenu="return false;"
                                            CssClass="otherUserNameLabel"
                                            Text='<%# String.Format("{0} {1}", Eval("poster_fname"), Eval("poster_lname")) %>'></asp:Label>

                                        &nbsp;&nbsp;&nbsp;&nbsp;

                                        <strong>Reported by:</strong>
                                        <asp:Label ID="reportedByName_Label" runat="server" oncontextmenu="return false;"
                                            CssClass="otherUserNameLabel"
                                            Text='<%# String.Format("{0} {1}", Eval("reporter_fname"), Eval("reporter_lname")) %>'></asp:Label>

                                        <br />
                                        <br />
                                        <strong>Report Description: </strong>
                                        <br />
                                        <asp:Label ID="reportText_Label" runat="server" oncontextmenu="return false;"
                                            Text='<%#Eval("report_text") %>'></asp:Label>
                                    </div>

                                </div>

                            </div>

                            <div class="card-footer card-link shadow-lg" style="text-align: center; border-radius: 25px;">

                                <asp:LinkButton ID="showPost_LinkBtn" CommandName="showPost_LinkBtn_CommandName"
                                    CssClass="card-link likeCommentReportLinkBtn" oncontextmenu="return false;" runat="server">
                            <!-- <i class="fa fa-eye" aria-hidden="true" style="margin-right: 5px;"></i> -->
                                </asp:LinkButton>


                                <asp:LinkButton ID="blockPostAuthor_LinkBtn" CommandName="blockPostAuthor_LinkBtn_CommandName"
                                    CssClass="card-link likeCommentReportLinkBtn" oncontextmenu="return false;" runat="server">
                            <!-- <i class="fa fa-ban" aria-hidden="true" style="margin-right: 5px;"></i> -->
                                </asp:LinkButton>


                                <asp:LinkButton ID="blockReporter_LinkBtn" CommandName="blockReporter_LinkBtn_CommandName"
                                    CssClass="card-link likeCommentReportLinkBtn" oncontextmenu="return false;" runat="server">
                             <!-- <i class="fa fa-ban" aria-hidden="true" style="margin-right: 5px;"></i>  -->
                                </asp:LinkButton>


                                <asp:LinkButton ID="ignore_LinkBtn" CommandName="ignore_LinkBtn_CommandName"
                                    CssClass="card-link likeCommentReportLinkBtn" oncontextmenu="return false;" runat="server">
                             <!-- <i class="fa fa-hand-peace" aria-hidden="true" style="margin-right: 5px;"></i>  -->
                                </asp:LinkButton>

                            </div>

                            <!-- Show post panel -->
                            <asp:Panel ID="showPost_Panel" Visible="false" Width="100%" runat="server">

                                <asp:DataList ID="showPost_DataList" runat="server" Width="100%"
                                    OnItemDataBound="showPost_DataList_ItemDataBound">
                                    <ItemTemplate>

                                        <div class="card gedf-card" style="margin-top: 40px;">
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

                                <div style="float: right; margin-top: 10px;">
                                    <asp:Button ID="cancelShowPost_Button" CommandName="cancelShowPost_Button_CommandName"
                                        CssClass="btn btn-warning" runat="server" Text="Cancel" />
                                </div>

                            </asp:Panel>
                            <!-- Show post panel closed. -->

                            <!-- Confirm block post author panel -->
                            <asp:Panel ID="confirmBlockPostAuthor_Panel" Visible="false" Width="100%" runat="server">

                                <div style="margin-top: 40px;">
                                    <div style="text-align: center;">
                                        <div style="font-size: xx-large;">{&nbsp;<i class="fa fa-ban" aria-hidden="true"></i>&nbsp;}</div>
                                        <asp:Label ID="blockPostAuthor_Label" runat="server"
                                            Text=""></asp:Label>
                                    </div>
                                </div>

                                <asp:Button ID="cancelBlockPostAuthor_Button" CommandName="cancelBlockPostAuthor_Button_CommandName" Style="margin-top: 10px; float: right;"
                                    runat="server" CssClass="btn btn-warning" Text="Cancel" />

                                <asp:Button ID="yesBlockPostAuthor_Button" CommandName="yesBlockPostAuthor_Button_CommandName" Style="margin-top: 10px; float: right; margin-right: 10px;"
                                    runat="server" CssClass="btn btn-outline-danger" Text="Block" />

                            </asp:Panel>
                            <!-- Confirm block post author panel closed. -->

                            <!-- Confirm block reporter panel -->
                            <asp:Panel ID="confirmBlockReporter_Panel" Visible="false" Width="100%" runat="server">

                                <div style="margin-top: 40px;">
                                    <div style="text-align: center;">
                                        <div style="font-size: xx-large;">{&nbsp;<i class="fa fa-ban" aria-hidden="true"></i>&nbsp;}</div>
                                        <asp:Label ID="blockReporter_Label" runat="server"
                                            Text=""></asp:Label>
                                    </div>
                                </div>

                                <asp:Button ID="cancelBlockReporter_Button" CommandName="cancelBlockReporter_Button_CommandName" Style="margin-top: 10px; float: right;"
                                    runat="server" CssClass="btn btn-warning" Text="Cancel" />

                                <asp:Button ID="yesBlockReporter_Button" CommandName="yesBlockReporter_Button_CommandName" Style="margin-top: 10px; float: right; margin-right: 10px;"
                                    runat="server" CssClass="btn btn-outline-danger" Text="Block" />

                            </asp:Panel>
                            <!-- Confirm block reporter panel closed. -->

                            <!-- Confirm ignore report panel -->
                            <asp:Panel ID="confirmIgnoreReport_Panel" Visible="false" Width="100%" runat="server">
                                <div style="margin-top: 40px;">
                                    <div style="text-align: center;">
                                        <div style="font-size: xx-large;">{&nbsp;<i class="fa fa-hand-peace" aria-hidden="true"></i>&nbsp;}</div>
                                        <asp:Label ID="ignoreReport_Label" runat="server"
                                            Text=""></asp:Label>
                                    </div>
                                </div>

                                <asp:Button ID="cancelIgnoreReport_Button" CommandName="cancelIgnoreReport_Button_CommandName" Style="margin-top: 10px; float: right;"
                                    runat="server" CssClass="btn btn-warning" Text="Cancel" />

                                <asp:Button ID="yesIgnoreReport_Button" CommandName="yesIgnoreReport_Button_CommandName" Style="margin-top: 10px; float: right; margin-right: 10px;"
                                    runat="server" CssClass="btn btn-info" Text="Ignore report" />

                            </asp:Panel>
                            <!-- Confirm ignore report panel -->

                        </div>
                    </div>

                    <hr />
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


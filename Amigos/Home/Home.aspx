<%@ Page Language="C#" MasterPageFile="~/AccountMaster/UserAccountMaster.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home_Home" %>

<asp:Content ID="home_Head" ContentPlaceHolderID="head" runat="Server">

    <link rel="stylesheet" href="Home_StyleSheet.css" />
    <link rel="stylesheet" href="../Styles_and_scripts/bootstrap.min.css" />

    <script src="../Styles_and_scripts/jquery-3.3.1.min.js"></script>
    <script src="../Styles_and_scripts/popper.min.js"></script>
    <script src="../Styles_and_scripts/bootstrap.min.js"></script>

    <title>Home | Amigos</title>
</asp:Content>

<asp:Content ID="home_Content" ContentPlaceHolderID="accountMaster_Content" runat="Server">

    <!-- 'posts_ScriptManager' to handle all AJAX stuff or AJAX extensions in page. -->
    <asp:ScriptManager ID="posts_ScriptManager" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="posts_UpdatePanel" UpdateMode="Conditional" runat="server">
        <ContentTemplate>

            <div style="text-align: center; top: 0;">
                <asp:Label ID="topHeading_Label" runat="server" Text=""></asp:Label>
                <hr />
            </div>

            <asp:DataList ID="posts_DataList" Width="100%" runat="server"
                OnItemCommand="posts_DataList_ItemCommand" OnItemDataBound="posts_DataList_ItemDataBound">
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
                                            <asp:LinkButton ID="otherUserName_LinkBtn" CommandName="otherUserName_LinkBtn_CommandName"
                                                CssClass="otherUserNameLinkBtn" oncontextmenu="return false;"
                                                data-toggle="tooltip" data-placement="top" ToolTip="Click to show profile..."
                                                Text='<%# String.Format("{0} {1}", Eval("firstname"), Eval("lastname")) %>' runat="server" />


                                            <!-- Label below shows '(😎 It's you !)' message if post is of current user -->
                                            <asp:Label ID="postOwnerStatus_Label" CssClass="h7" Text="" runat="server" />
                                        </div>
                                        <!-- <div class="h7 text-muted">Miracles Lee Cross</div> -->
                                        <div class="text-muted h7 mb-2">
                                            <i class="fa fa-clock-o"></i>
                                            <asp:Label ID="postTime_Label" Text='<%# String.Format("Posted at: {0}", Eval("dated")) %>' runat="server" />
                                            <!-- Text="10 min ago" -->
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="card-body">

                                <a class="card-link" href="#">
                                    <h5 class="card-title" style="text-align: center;">
                                        <asp:Label ID="postHeading_Label" CssClass="postHeadingLabel" runat="server"
                                            Text='<%#Eval("post_heading") %>'></asp:Label>
                                        <!-- Lorem ipsum dolor sit amet, consectetur adip. -->
                                    </h5>
                                </a>

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

                                <asp:LinkButton ID="like_LinkBtn" CommandName="like_LinkBtn_CommandName"
                                    CssClass="card-link likeCommentReportLinkBtn" oncontextmenu="return false;" runat="server">
                            <!-- <i class="fa fa-heart" aria-hidden="true" style="margin-right: 5px;"></i> -->
                                </asp:LinkButton>


                                <asp:LinkButton ID="comment_LinkBtn" CommandName="comment_LinkBtn_CommandName"
                                    CssClass="card-link likeCommentReportLinkBtn" oncontextmenu="return false;" runat="server">
                            <!-- <i class="fa fa-comments" aria-hidden="true" style="margin-right: 5px;"></i> -->
                                </asp:LinkButton>


                                <asp:LinkButton ID="report_LinkBtn" CommandName="report_LinkBtn_CommandName"
                                    CssClass="card-link likeCommentReportLinkBtn" oncontextmenu="return false;" runat="server">
                            <!-- <i class="fa fa-flag" aria-hidden="true" style="margin-right: 5px;"></i> -->
                                </asp:LinkButton>


                                <asp:LinkButton ID="remove_LinkBtn" CommandName="remove_LinkBtn_CommandName"
                                    CssClass="card-link likeCommentReportLinkBtn" oncontextmenu="return false;" runat="server">
                            <!-- <i class="fa fa-trash" aria-hidden="true" style="margin-right: 5px;"></i> -->
                                </asp:LinkButton>

                            </div>

                            <!-- Add comments panel -->
                            <asp:Panel ID="addComments_Panel" Visible="false" Width="100%" runat="server">
                                <div class="input-group" style="margin-top: 40px;">
                                    <div class="input-group-prepend">
                                        <div class="input-group-text"><i class="fa fa-comment" aria-hidden="true"></i></div>
                                    </div>
                                    <asp:TextBox ID="comment_TextBox" Style="resize: none;" TextMode="MultiLine" CssClass="form-control"
                                        placeholder="Type comment for this post..." runat="server"></asp:TextBox>
                                </div>

                                <asp:Button ID="cancelComment_Button" CommandName="cancelComment_Button_CommandName" Style="margin-top: 10px; float: right;"
                                    runat="server" CssClass="btn btn-warning" Text="Cancel" />

                                <asp:Button ID="addComment_Button" CommandName="addComment_Button_CommandName" Style="margin-top: 10px; float: right; margin-right: 10px;"
                                    runat="server" CssClass="btn btn-outline-success" Text="Add comment" />

                                <asp:Button ID="showComments_Button" CommandName="showComments_Button_CommandName" Style="margin-top: 10px; float: right; margin-right: 10px;"
                                    runat="server" CssClass="btn btn-outline-dark" Text="Show comments" />
                            </asp:Panel>
                            <!-- Add comments panel closed. -->

                            <!-- Report post panel -->
                            <asp:Panel ID="reportPost_Panel" Visible="false" Width="100%" runat="server">
                                <div class="input-group" style="margin-top: 40px;">
                                    <div class="input-group-prepend">
                                        <div class="input-group-text"><i class="fa fa-flag" aria-hidden="true"></i></div>
                                    </div>
                                    <asp:TextBox ID="report_TextBox" Style="resize: none;" TextMode="MultiLine" CssClass="form-control"
                                        placeholder="What's wrong about post ? (Describe about post that you found not good)" runat="server"></asp:TextBox>
                                </div>

                                <asp:Button ID="cancelReport_Button" CommandName="cancelReport_Button_CommandName" Style="margin-top: 10px; float: right;"
                                    runat="server" CssClass="btn btn-warning" Text="Cancel" />
                                <asp:Button ID="reportToAdmin_Button" CommandName="reportToAdmin_Button_CommandName" Style="margin-top: 10px; float: right; margin-right: 10px;"
                                    runat="server" CssClass="btn btn-outline-danger" Text="Report to admin" />
                            </asp:Panel>
                            <!-- Report post panel closed. -->

                            <!-- Confirm remove post panel -->
                            <asp:Panel ID="confirmRemovePost_Panel" Visible="false" Width="100%" runat="server">

                                <div style="margin-top: 40px;">
                                    <div style="text-align: center;">
                                        <div style="font-size: xx-large;">{&nbsp;<i class="fa fa-trash" aria-hidden="true"></i>&nbsp;}</div>
                                        <asp:Label ID="removePostHeading_Label" runat="server"
                                            Text="<strong>Confirm to remove your post ? 🤔<br /> It can not be recovered ...</strong>"></asp:Label>
                                    </div>
                                </div>

                                <asp:Button ID="cancelRemovePost_Button" CommandName="cancelRemovePost_Button_CommandName" Style="margin-top: 10px; float: right;"
                                    runat="server" CssClass="btn btn-warning" Text="Cancel" />

                                <asp:Button ID="yesRemovePost_Button" CommandName="yesRemovePost_Button_CommandName" Style="margin-top: 10px; float: right; margin-right: 10px;"
                                    runat="server" CssClass="btn btn-outline-danger" Text="Yes, remove ..." />

                            </asp:Panel>
                            <!-- Confirm remove post panel closed. -->

                            <!-- Reference for 'show comments' design :=> https://bootsnipp.com/snippets/M5obX -->
                            <!-- Show comments panel -->
                            <asp:Panel ID="showComments_Panel" Style="margin-top: 80px;" Visible="false" runat="server">

                                <asp:DataList ID="showComments_DataList" Width="100%"
                                    OnItemCommand="showComments_DataList_ItemCommand"
                                    OnItemDataBound="showComments_DataList_ItemDataBound"
                                    runat="server">
                                    <ItemTemplate>

                                        <div class="card shadow">
                                            <div class="card-body">
                                                <div class="row">
                                                    <div class="col-md-2">
                                                        <asp:Image ID="otherUserProfileComments_Image" Width="80" Height="80" CssClass="rounded-circle shadow-lg"
                                                            Style="border-style: groove; margin-left: 30%; margin-top: auto;" oncontextmenu="return false;"
                                                            ImageUrl='<%#Eval("commenter_photo") %>' runat="server" />
                                                        <br />
                                                        <asp:Label ID="postCommenterStatus_Label" CssClass="text-secondary text-center" Style="margin-left: 20%;"
                                                            Text="(Post author 👑)" runat="server" />
                                                    </div>
                                                    <div class="col-md-10">

                                                        <p>
                                                            <asp:Label ID="commenterName_Label" CssClass="float-left" Style="color: blueviolet; font-weight: 700; font-size: larger;"
                                                                Text='<%# String.Format("{0} {1}", Eval("commenter_fname"), Eval("commenter_lname")) %>' runat="server" />

                                                            <asp:Label ID="commentedDateTime_Label" CssClass="text-secondary text-center" Style="float: right; font-size: small;"
                                                                Text='<%# String.Format("Commented at: {0}", Eval("comment_datetime")) %>' runat="server" />
                                                        </p>
                                                        <div class="clearfix"></div>
                                                        <p>
                                                            <asp:Label ID="commentText_Label" runat="server"
                                                                Text='<%#Eval("comment_text") %>' />
                                                        </p>
                                                        <p>
                                                            <asp:LinkButton ID="removeComment_LinkButton" CommandName="removeComment_LinkButton_CommandName"
                                                                CssClass="card-link likeCommentReportLinkBtn" oncontextmenu="return false;" Style="float: right;"
                                                                runat="server">
                                                                {
                                                                <i class="fa fa-trash" aria-hidden="true"></i>
                                                                }
                                                                        Remove comment
                                                            </asp:LinkButton>
                                                        </p>

                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <hr />

                                    </ItemTemplate>
                                </asp:DataList>

                            </asp:Panel>
                            <!-- Show comments panel closed. -->


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

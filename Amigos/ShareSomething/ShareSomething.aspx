<%@ Page Title="" Language="C#" MasterPageFile="~/AccountMaster/UserAccountMaster.master" AutoEventWireup="true" CodeFile="ShareSomething.aspx.cs" Inherits="ShareSomething_ShareSomething" %>

<asp:Content ID="shareSomething_Head" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="ShareSomething_StyleSheet.css" />
    <title>Share something 💡</title>
</asp:Content>


<asp:Content ID="shareSomething_Content" ContentPlaceHolderID="accountMaster_Content" runat="Server">

    <div class="card gedf-card">
        <div class="card-header">
            <ul class="nav nav-tabs card-header-tabs" id="myTab" role="tablist">
                <li class="nav-item">
                    <a class="nav-link active" id="posts-tab" data-toggle="tab" href="#posts" role="tab" aria-controls="posts" aria-selected="true">Make
                                    a publication</a>
                </li>
                <!--
                <li class="nav-item">
                    <a class="nav-link" id="images-tab" data-toggle="tab" role="tab" aria-controls="images" aria-selected="false" href="#images">Images</a>
                </li>
                -->
            </ul>
        </div>
        <div class="card-body">
            <div class="tab-content" id="myTabContent">
                <div class="tab-pane fade show active" id="posts" role="tabpanel" aria-labelledby="posts-tab">
                    <div class="form-group">
                        <!--<label class="sr-only" for="message">post</label>-->
                        <!--<textarea class="form-control" id="message" rows="3" placeholder="What are you thinking?"></textarea>-->
                        <asp:TextBox ID="postHeading_TextBox" CssClass="form-control" MaxLength="100" TextMode="SingleLine" placeholder="What's the post title ? 🤔" runat="server" />
                        <span style="margin-right: 10px;" />
                        <asp:TextBox ID="postText_TextBox" CssClass="form-control" TextMode="MultiLine" placeholder="Have some description ... 🗣" Rows="3" runat="server" />
                    </div>
                    <!--
                    <div class="custom-file" style="margin-bottom: 20px;">
                        <input type="file" class="custom-control-input" style="width: 20%;" id="fileUpload"/>
                        <label class="custom-file-label" for="fileUpload">Upload image</label>
                    </div>
                    -->

                    <div style="margin-bottom: 20px;">
                        <label>Upload image : </label>
                        <asp:FileUpload ID="postImage_FileUpload" runat="server" />
                    </div>
                </div>
            </div>

            <!--
            <div class="tab-pane fade" id="images" role="tabpanel" aria-labelledby="images-tab">
                    <div class="form-group">
                        <div class="custom-file">
                            <input type="file" class="custom-file-input" id="customFile" />
                            <label class="custom-file-label" for="customFile">Upload image</label>
                        </div>
                    </div>
                <div class="py-4"></div>
                </div>
            </div>
                    -->
            <div class="btn-toolbar justify-content-between">
                <div class="btn-group">
                    <!--<button type="submit" class="btn btn-outline-dark">Share</button>-->
                    <asp:Button ID="shareBtn" OnClick="shareBtn_Click" CssClass="btn btn-outline-dark" Text="Share !" runat="server" />
                </div>
            </div>

        </div>
    </div>

</asp:Content>


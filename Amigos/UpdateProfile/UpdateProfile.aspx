<%@ Page Title="" Language="C#" MasterPageFile="~/AccountMaster/UserAccountMaster.master" AutoEventWireup="true" CodeFile="UpdateProfile.aspx.cs" Inherits="UpdateProfile_UpdateProfile" %>

<asp:Content ID="updateProfile_Head" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="UpdateProfile_StyleSheet.css" />
    <link rel="icon" type="image/x-icon" href="../Images/amigos_tab_icon.ico" />

    <title>Update/ edit Profile</title>
</asp:Content>
<asp:Content ID="updateProfile_Content" ContentPlaceHolderID="accountMaster_Content" runat="Server">

    <!-- Reference :=> https://www.bootply.com/X8w3pTBYSV -->

    <div class="container">
        <h1>Update/ edit Profile</h1>
        <hr />
        <div class="row">
            <div class="col-md-9 personal-info">
                <div class="alert alert-info alert-dismissable">
                    <!-- <a class="panel-close close" data-dismiss="alert">x</a> -->
                    <i class="fa fa-coffee"></i>
                    Welcome to <strong>update/ edit</strong> profile. The data below is your current one, change it to modify...
                </div>
                <h3>Personal info</h3>

                <div class="form-group">
                    <label class="col-lg-3 control-label">Upload a photo...</label>
                    <div class="col-lg-8">
                        <asp:FileUpload ID="profilePicture_FileUpload" runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-lg-3 control-label">First name:</label>
                    <div class="col-lg-8">
                        <asp:TextBox ID="firstName_TextBox" CssClass="form-control" TextMode="SingleLine" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-lg-3 control-label">Last name:</label>
                    <div class="col-lg-8">
                        <asp:TextBox ID="lastName_TextBox" CssClass="form-control" TextMode="SingleLine" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-lg-3 control-label">Profession:</label>
                    <div class="col-lg-8">
                        <asp:TextBox ID="profession_TextBox" CssClass="form-control" TextMode="SingleLine" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-lg-3 control-label">Your profession At:</label>
                    <div class="col-lg-8">
                        <asp:TextBox ID="professionAt_TextBox" CssClass="form-control" TextMode="SingleLine" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-lg-3 control-label">Email:</label>
                    <div class="col-lg-8">
                        <asp:TextBox ID="email_TextBox" CssClass="form-control" TextMode="Email" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-md-3 control-label">Password:</label>
                    <div class="col-md-8">
                        <asp:TextBox ID="password_TextBox" CssClass="form-control" TextMode="Password" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-md-3 control-label">Confirm password:</label>
                    <div class="col-md-8">
                        <asp:TextBox ID="confirmPassword_TextBox" CssClass="form-control" TextMode="Password" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-md-3 control-label"></label>
                    <div class="col-md-8">
                        <!-- <input type="button" class="btn btn-primary" value="Save Changes" /> -->
                        <asp:Button ID="saveProfileBtn" OnClick="saveProfileBtn_Click" Text="✔ Save profile" CssClass="updateProfileBtn" runat="server" />
                        <span/>
                        <!-- <input type="reset" class="btn btn-default" value="Cancel" /> -->
                        <asp:Button ID="discardChanges_Btn" OnClick="discardChanges_Btn_Click" Text="❌ Discard changes" CssClass="updateProfileBtn" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- </div> -->
    <hr />

</asp:Content>


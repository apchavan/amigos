<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Signup.aspx.cs" Inherits="Signup_Signup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <%-- Before all other stylesheets to load Bootstrap CSS & other libraries. --%>
    <%--  NOTE: USE THE LATEST VERSION MENTIONED ON THE OFFICIAL SITES OF RESPECTIVE LIBRARIES BELOW. --%>
    <link rel="stylesheet" href="../Styles_and_scripts/bootstrap.min.css" />
    <script src="../Styles_and_scripts/bootstrap.bundle.min.js"></script>
    <script src="../Styles_and_scripts/jquery-3.3.1.min.js"></script>

    <%-- Load stylesheet for Signup page below. --%>
    <link rel="stylesheet" href="Signup_StyleSheet.css" />
    <link rel="stylesheet" href="../Styles_and_scripts/LandingPage_Navigation_Bar_StyleSheet.css" />

    <%-- Info about adding icon to tab of website : https://en.wikipedia.org/wiki/Favicon --%>
    <link rel="icon" type="image/x-icon" href="../Images/amigos_tab_icon.ico" />
    <title>Signup | Amigos</title>
</head>
<body>
    <form id="signup_form" runat="server">
        <asp:ScriptManager ID="signup_ScriptManager" runat="server"></asp:ScriptManager>
        <%-- Top navigation bar --%>
        <div>
            <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
                <!-- Navbar brand/main item -->
                <a id="amigos-landingpage-name" href="../LandingPage/LandingPage.aspx" class="navbar-brand"><i>Amigos</i></a>
                <%-- Navbar sub-items --%>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav">
                        <li class="nav-item">
                            <a class="nav-link transition-nav-item" href="../About/About.aspx">About</a>
                        </li>
                    </ul>
                </div>

            </nav>
        </div>

        <%-- User signup code --%>
        <div class="signup-form">
            <div class="main-div">
                <div class="panel">
                    <h2 style="font-family: 'Harlow Solid'; color: gray; font-size: 250%; font-weight: normal;">😎 Signup new account 😎</h2>
                    <p style="font-size: medium; font-weight: 600;">Please provide the following info: </p>
                </div>

                <asp:MultiView ID="signup_MultiView" runat="server" ActiveViewIndex="0">

                    <asp:View ID="basicInfo_View" runat="server">
                        <div class="form-group">
                            <asp:TextBox ID="firstNameTextBox" CssClass="form-control" MaxLength="100" TextMode="SingleLine" placeholder="📛 First name" runat="server" />
                        </div>

                        <div class="form-group">
                            <asp:TextBox ID="lastNameTextBox" CssClass="form-control" MaxLength="100" TextMode="SingleLine" placeholder="📛 Last name" runat="server" />
                        </div>

                        <div class="form-group">
                            <asp:TextBox ID="mobileTextBox" CssClass="form-control" TextMode="Phone" MaxLength="10" placeholder="📱 Mobile number" runat="server" />
                        </div>

                        <div class="form-group">
                            <asp:TextBox ID="dobTextBox" CssClass="form-control" TextMode="SingleLine" ToolTip="Select DOB by clicking right corner down arrow ⬇️" placeholder="🎂🥳 Date of birth " runat="server" />
                            <ajaxToolkit:CalendarExtender ID="dobTextBox_CalendarExtender" runat="server" BehaviorID="dobTextBox_CalendarExtender" Format="dd-MM-yyyy" PopupPosition="Right" TargetControlID="dobTextBox" TodaysDateFormat="dd-MMMM-yyyy" />
                        </div>
                        <asp:Button ID="nextBtn" CssClass="signupBtn" Text="Next 👉🏻" OnClick="nextBtn_Click" runat="server" />
                    </asp:View>

                    <asp:View ID="accountInfo_View" runat="server">
                        <div class="form-group">
                            <asp:TextBox ID="emailTextBox" CssClass="form-control" TextMode="Email" MaxLength="100" placeholder="📧 Email Address" runat="server" />
                        </div>

                        <div class="form-group">
                            <asp:TextBox ID="passwordTextBox" CssClass="form-control" TextMode="Password" MaxLength="100" placeholder="🔑 Password" runat="server" />
                        </div>

                        <div class="form-group">
                            <asp:TextBox ID="confirmPasswordTextBox" CssClass="form-control" TextMode="Password" MaxLength="100" placeholder="🔑 Confirm password" runat="server" />
                        </div>

                        <div class="form-group">
                            <asp:TextBox ID="questionTextBox" CssClass="form-control" TextMode="SingleLine" MaxLength="200" placeholder="❓ Type your security question" runat="server" />
                        </div>

                        <div class="form-group">
                            <asp:TextBox ID="answerTextBox" CssClass="form-control" TextMode="SingleLine" MaxLength="200" placeholder="✌🏻 Type answer for your security question" runat="server" />
                        </div>

                        <div class="form-group">
                            <asp:RadioButton ID="femaleRadioButton" CssClass="gender-control" GroupName="GenderRadioButtonGroup" Text="Female 👩" runat="server" />
                            <asp:RadioButton ID="maleRadioButton" CssClass="gender-control" GroupName="GenderRadioButtonGroup" Text="Male 👨" runat="server" />
                        </div>

                        <asp:Button ID="backBtn" CssClass="signupBtn" Text="👈🏻 Back" OnClick="backBtn_Click" runat="server" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="signupBtn" CssClass="signupBtn" Text="Signup 🗸" OnClick="signupBtn_Click" runat="server" />
                    </asp:View>

                </asp:MultiView>

            </div>
            <!-- <div class="main-div"> CLOSED. -->
        </div>
        <!-- <div class="signup-form"> CLOSED. -->

        <div>
            <script src="Typewriter_JavaScript.js"></script>
            <h3 class="wrapper-right typewriterAmigos-align" style="user-select: none;" oncontextmenu="return false;">
                <a href="#" id="typewriterAmigos" class="typewrite anchor-typewriterAmigos" data-period="2000"
                    data-type='[ "Welcome to Amigos! 🥳", "Be Creative! ✨", "Share new ideas! 💡", "You are unique in all! 🧐", "Be connected 💛 with your friends! 🤗" ]'>
                    <span class="wrap"></span>
                </a>
            </h3>
        </div>
    </form>

</body>
</html>

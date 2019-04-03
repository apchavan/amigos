<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Forgot_password.aspx.cs" Inherits="Forgot_password_Forgot_password" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="stylesheet" href="../Styles_and_scripts/bootstrap.min.css" />
    <link rel="stylesheet" href="Forgot_password_StyleSheet.css" />
    <link rel="stylesheet" href="../Styles_and_scripts/LandingPage_Navigation_Bar_StyleSheet.css" />

    <script src="../Styles_and_scripts/bootstrap.bundle.min.js"></script>
    <script src="../Styles_and_scripts/jquery-3.3.1.min.js"></script>
    <script src="../Styles_and_scripts/popper.min.js"></script>
    <script src="../Styles_and_scripts/bootstrap.min.js"></script>

    <link rel="icon" type="image/x-icon" href="../Images/amigos_tab_icon.ico" />
    <title>Forgot password - Amigos</title>

</head>
<body>
    <form id="forgot_password_form" runat="server">

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

                <label id="not_a_member_label" class="navbar-text pull-md-right" style="margin: 0.1% 0.5% 0% 0%;">Not a member? Just</label>
                <asp:Button ID="signupBtn" CssClass="signupBtn" Text="Signup" runat="server" OnClick="signupBtn_Click" />
            </nav>
        </div>

        <%-- Forgot password form code --%>
        <div class="forgot-form">
            <div class="main-div">
                 
                <asp:MultiView ID="forgotPassword_MultiView" runat="server" ActiveViewIndex="0">

                    <asp:View ID="inputEmail_View" runat="server">
                        <div class="panel">
                            <h2 style="font-family: 'Harlow Solid'; color: gray; font-size: 300%; font-weight: normal;"><strong>Forgot password 🤔?</strong></h2>
                            <p style="font-size: medium; font-weight: 600;">Please provide the following info: </p>
                        </div>

                        <div class="form-group">
                            <asp:TextBox ID="inputEmail_TextBox" CssClass="form-control" TextMode="Email" placeholder="Registered Email Address 📧" runat="server" />
                        </div>

                        <asp:Button ID="nextEmail_Button" CssClass="submitBtn" Style="vertical-align: middle;"
                            Text="Next 👉🏻" OnClick="nextEmail_Button_Click" runat="server" />
                    </asp:View>

                    <asp:View ID="security_View" runat="server">
                        <div class="panel">
                            <h2 style="font-family: 'Harlow Solid'; color: gray; font-size: 300%; font-weight: normal;"><strong>Forgot password 🤔?</strong></h2>
                            <p style="font-size: medium; font-weight: 600;">Please provide the answer for following security question: </p>
                        </div>

                        <asp:Label ID="secQue_Label" runat="server" Text=""></asp:Label>
                        
                        <div style="margin-top: 20px;" class="form-group">
                            <asp:TextBox ID="securityAnswer_TextBox" CssClass="form-control" TextMode="SingleLine" placeholder="Your answer here ... 🗣️" runat="server" />
                        </div>

                        <asp:Button ID="backSecurity_Button" CssClass="submitBtn" Text="👈🏻 Back" OnClick="backSecurity_Button_Click" runat="server" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="nextSecurity_Button" CssClass="submitBtn" Text="Next 👉🏻" OnClick="nextSecurity_Button_Click" runat="server" />
                    </asp:View>

                    <asp:View ID="password_View" runat="server">
                        <div class="panel">
                            <h2 style="font-family: 'Harlow Solid'; color: gray; font-size: 300%; font-weight: normal;"><strong>Correct info ! 👏😉</strong></h2>
                            <!-- <p style="font-size: medium; font-weight: 600;">Password of your account is: </p> -->
                        </div>

                        <br />
                        <asp:Label ID="password_Label" runat="server" Text=""></asp:Label>
                        <br />
                        <asp:Button ID="login_Button" CssClass="submitBtn" Text="Login" OnClick="login_Button_Click" runat="server" />
                    </asp:View>

                </asp:MultiView>

            </div>

        </div>

    </form>

</body>
</html>

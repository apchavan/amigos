<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SuccessSignup.aspx.cs" Inherits="SuccessSignup_SuccessSignup" %>

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
    <link rel="stylesheet" href="SuccessSignup_StyleSheet.css" />
    <link rel="stylesheet" href="../Styles_and_scripts/LandingPage_Navigation_Bar_StyleSheet.css" />

    <link rel="icon" type="image/x-icon" href="../Images/amigos_tab_icon.ico" />

    <title>Signup successful 😎!</title>
</head>
<body>
    <form id="success_signup_form" runat="server">
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

        <%-- Successful signup form code --%>
        <div class="login-form">
            <div class="main-div">
                <div class="panel">
                    <h2 style="font-family: 'Harlow Solid'; color: gray; font-size: 300%; font-weight: normal;"><strong>Signup successful 😎!</strong></h2>
                    <p style="font-size: medium; font-weight: 600;">Please login to your account by email & password ...</p>
                </div>


                <%--<div class="form-group">
                    <asp:TextBox ID="inputEmail" CssClass="form-control" TextMode="Email" placeholder="Registered Email Address 📧" runat="server" />
                </div>--%>

                <asp:Button ID="loginBtn" CssClass="loginBtn" Style="vertical-align: middle;" OnClick="loginBtn_Click" Text="Login" runat="server" />
            </div>
        </div>

    </form>
</body>
</html>

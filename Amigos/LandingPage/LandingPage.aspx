<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LandingPage.aspx.cs" Inherits="LandingPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <%-- Before all other stylesheets to load Bootstrap CSS & other libraries. --%>
    <%--  NOTE: USE THE LATEST VERSION MENTIONED ON THE OFFICIAL SITES OF RESPECTIVE LIBRARIES BELOW. --%>
    <link rel="stylesheet" href="../Styles_and_scripts/bootstrap.min.css" />
    <script src="../Styles_and_scripts/bootstrap.bundle.min.js"></script>
    <script src="../Styles_and_scripts/jquery-3.3.1.min.js"></script>

    <%-- Load stylesheet for LandingPage below. --%>
    <link rel="stylesheet" href="LandingPage_StyleSheet.css" />
    <link rel="stylesheet" href="../Styles_and_scripts/LandingPage_Navigation_Bar_StyleSheet.css" />

    <%-- Info about adding icon to tab of website : https://en.wikipedia.org/wiki/Favicon --%>
    <link rel="icon" type="image/x-icon" href="../Images/amigos_tab_icon.ico" />
    <title>Amigos | Social Media Platform</title>
</head>

<body>

    <form id="landing_page_form" runat="server">

        <%-- Top navigation bar --%>
        <div>
            <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
                <!-- Navbar brand/main item -->
                <a id="amigos-landingpage-name" href="#" class="navbar-brand active"><i>Amigos</i></a>
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

        <%-- Motion graphic used below from link: https://bootsnipp.com/snippets/xrKXG --%>
        <div class="wrapper-left">
            <svg xmlns="http://www.w3.org/2000/svg" x="0px" y="0px" viewBox="0 0 288 288">
                <linearGradient id="PSgrad_0" x1="70.711%" x2="0%" y1="70.711%" y2="0%">
                    <stop offset="0%" stop-color="rgb(95,54,152)" stop-opacity="1" />
                    <stop offset="100%" stop-color="rgb(247,109,138)" stop-opacity="1" />
                </linearGradient>
                <path d="" fill="url(#PSgrad_0)">
                    <animate repeatCount="indefinite" attributeName="d" dur="5s"
                        values="M37.5,186c-12.1-10.5-11.8-32.3-7.2-46.7c4.8-15,13.1-17.8,30.1-36.7C91,68.8,83.5,56.7,103.4,45
	c22.2-13.1,51.1-9.5,69.6-1.6c18.1,7.8,15.7,15.3,43.3,33.2c28.8,18.8,37.2,14.3,46.7,27.9c15.6,22.3,6.4,53.3,4.4,60.2
	c-3.3,11.2-7.1,23.9-18.5,32c-16.3,11.5-29.5,0.7-48.6,11c-16.2,8.7-12.6,19.7-28.2,33.2c-22.7,19.7-63.8,25.7-79.9,9.7
	c-15.2-15.1,0.3-41.7-16.6-54.9C63,186,49.7,196.7,37.5,186z;
	
	
	M51,171.3c-6.1-17.7-15.3-17.2-20.7-32c-8-21.9,0.7-54.6,20.7-67.1c19.5-12.3,32.8,5.5,67.7-3.4C145.2,62,145,49.9,173,43.4
	c12-2.8,41.4-9.6,60.2,6.6c19,16.4,16.7,47.5,16,57.7c-1.7,22.8-10.3,25.5-9.4,46.4c1,22.5,11.2,25.8,9.1,42.6
	c-2.2,17.6-16.3,37.5-33.5,40.8c-22,4.1-29.4-22.4-54.9-22.6c-31-0.2-40.8,39-68.3,35.7c-17.3-2-32.2-19.8-37.3-34.8
	C48.9,198.6,57.8,191,51,171.3z;
	
	M37.5,186c-12.1-10.5-11.8-32.3-7.2-46.7c4.8-15,13.1-17.8,30.1-36.7C91,68.8,83.5,56.7,103.4,45
	c22.2-13.1,51.1-9.5,69.6-1.6c18.1,7.8,15.7,15.3,43.3,33.2c28.8,18.8,37.2,14.3,46.7,27.9c15.6,22.3,6.4,53.3,4.4,60.2
	c-3.3,11.2-7.1,23.9-18.5,32c-16.3,11.5-29.5,0.7-48.6,11c-16.2,8.7-12.6,19.7-28.2,33.2c-22.7,19.7-63.8,25.7-79.9,9.7
	c-15.2-15.1,0.3-41.7-16.6-54.9C63,186,49.7,196.7,37.5,186z	" />
                </path>
            </svg>
        </div>

        <%-- User login code --%>
        <div class="login-form">
            <div class="main-div">
                <div class="panel">
                    <h2 style="font-family: 'Harlow Solid'; color: gray; font-size: 300%; font-weight: normal;"><strong>Login</strong></h2>
                    <p style="font-size: medium; font-weight: 600;">Please enter credentials to begin 🎉!</p>
                </div>


                <div class="form-group">
                    <asp:TextBox ID="inputEmail" CssClass="form-control" TextMode="Email" placeholder="📧 Email Address" runat="server" />

                    <!-- Default 'input' control for email -->
                    <%--<input type="email" class="form-control" id="inputEmail" placeholder="Email Address" />--%>
                </div>

                <div class="form-group">
                    <asp:TextBox ID="inputPassword" CssClass="form-control" TextMode="Password" placeholder="🔑 Password" runat="server" />

                    <!-- Default 'input' control for password -->
                    <%--<input type="password" class="form-control" id="inputPassword" placeholder="Password" />--%>
                </div>

                <div class="forgot">
                    <%--<a href="#">Forgot password ?</a>--%>
                    <asp:HyperLink ID="forgotPassword_HyperLink" NavigateUrl="~/Forgot_password/Forgot_password.aspx" Text="Forgot password 🤔?" runat="server" />
                </div>
                <%--<button type="submit" class="btn btn-primary">Login</button>--%>
                <%--<button class="loginBtn" onclick="goClick" style="vertical-align:middle"><span>Go </span></button>--%>
                <asp:Button ID="goBtn" CssClass="loginBtn" Style="vertical-align: middle;" Text="GO !" OnClick="goBtn_Click" runat="server" />
            </div>
        </div>

    </form>

    <footer style="float: right;">
        <p class="footer-text">
            <strong class="text-capitalize text-center" style="font-family: 'Harlow Solid'; font-style: oblique; font-size: x-large;">Social media platform</strong>
            <br />
            Designed with ❤ by Amigos team!
        </p>
    </footer>
</body>
</html>

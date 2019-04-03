<%@ Page Language="C#" AutoEventWireup="true" CodeFile="About.aspx.cs" Inherits="About_About" %>

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
    <link rel="stylesheet" href="About_StyleSheet.css" />
    <link rel="stylesheet" href="../Styles_and_scripts/LandingPage_Navigation_Bar_StyleSheet.css" />

    <%-- Info about adding icon to tab of website : https://en.wikipedia.org/wiki/Favicon --%>
    <link rel="icon" type="image/x-icon" href="../Images/amigos_tab_icon.ico" />
    <title>About - Amigos</title>
</head>
<body>
    <form id="about_form" runat="server">

        <%-- Top navigation bar --%>
        <div>
            <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
                <!-- Navbar brand/main item -->
                <a id="amigos-landingpage-name" href="../LandingPage/LandingPage.aspx" class="navbar-brand"><i>Amigos</i></a>
                <%-- Navbar sub-items --%>
                <div class="collapse navbar-collapse" id="navbarNav">
                    <ul class="navbar-nav">
                        <li class="nav-item">
                            <a class="nav-link active transition-nav-item" href="#">About</a>
                        </li>
                    </ul>
                </div>

                <label id="not_a_member_label" class="navbar-text pull-md-right" style="margin: 0.1% 0.5% 0% 0%;">Not a member? Just</label>
                <asp:Button ID="signupBtn" CssClass="signupBtn" Text="Signup" runat="server" OnClick="signupBtn_Click" />
            </nav>
        </div>

        <%-- About content --%>
        <div class="team-section">
            <h1>Our team</h1>
            <span class="border"></span>

            <div class="ps">
                <a href="#p1">
                    <img id="firstImage" src="../Images/Shrihari.jpg" alt="Steve Jobs" oncontextmenu="return false;" /></a>
                <a href="#p2">
                    <img id="secondImage" src="../Images/Amey.png" alt="Amey Chavan" oncontextmenu="return false;" /></a>
                <a href="#p3">
                    <img id="thirdImage" src="../Images/Elon_Musk.png" alt="Elon Musk" oncontextmenu="return false;" /></a>
            </div>

            <div id="p1" class="section">
                <span class="name">Shrihari Shinde</span>
                <span class="border"></span>
                <p>
                    <strong>Shrihari Manohar Shinde</strong> (September 22, 1995) is a web-designer that likes to do new innovation in web design area.
                    He also likes to live in pure nature, farms etc. His goal to become a tester in one of the leading industry around the globe such as
                    'EA sports', 'Ubisoft', 'Supermassive games' and so on. He is very friendly in nature and continuously learn new things over the
                    internet.
                </p>
            </div>

            <div id="p2" class="section">
                <span class="name">Amey Chavan</span>
                <span class="border"></span>
                <p>
                    <strong>Amey Prasannakumar Chavan</strong> (born August 25, 1996) is a technology entrepreneur, developer, and research enthusiastic.
                     He holds citizenship of India and interested to develop innovative technology. Artificial intelligence, machine learning or
                     deep learning are the fields liked by him mostly.
                     He also self-taught by most of the skills he have from rich source of resources called 'internet'!
                </p>
            </div>

            <div id="p3" class="section">
                <span class="name">Elon Musk</span>
                <span class="border"></span>
                <p>
                    <strong>Elon Reeve Musk</strong> FRS (born June 28, 1971) is a technology entrepreneur, investor, and engineer.
                     He holds South African, Canadian, and U.S. citizenship and is the founder, CEO, and lead designer of SpaceX;
                     co-founder, CEO, and product architect of Tesla, Inc.; co-founder and CEO of Neuralink and co-founder of PayPal.
                     In December 2016, he was ranked 21st on the Forbes list of The World's Most Powerful People.
                     As of October 2018, he has a net worth of $22.8 billion and is listed by Forbes as the 54th-richest person in the world.
                </p>
            </div>
        </div>

    </form>
</body>
</html>

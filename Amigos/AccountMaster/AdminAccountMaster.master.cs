using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AccountMaster_AdminAccountMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadProfilePictureName();
    }

    private void LoadProfilePictureName()
    {
        string cmdText = "SELECT photo FROM user_profile WHERE (UserID = " + Session["UserID"].ToString() + ")";
        DataTable dt = new DataTable();

        dt = SQLHelper.FillDataTable(cmdText);

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["photo"].ToString().Trim() == "")
                profilePicture_Image.ImageUrl = "~/Images/no_image.jpg";
            else
                profilePicture_Image.ImageUrl = dt.Rows[0]["photo"].ToString();
        }
        else
            profilePicture_Image.ImageUrl = "~/Images/no_image.jpg";

        profile_LinkBtn.Text = Session["firstname"].ToString();
    }

    // Search for other users
    protected void searchBtn_ServerClick(object sender, EventArgs e)
    {
        if (searchText.Value.Trim() == "")
            return;
        Response.Cookies["searchName"].Value = searchText.Value;
        Response.Redirect("~/SearchResult/SearchResult.aspx");
    }

    protected void logout_LinkBtn_Click(object sender, EventArgs e)
    {
        // Set user current 'islogin' status to 0 (False) 
        string cmdText = "UPDATE user_creds SET islogin = 0 WHERE (UserID = " + Session["UserID"].ToString() + ")";
        SQLHelper.ExecuteNonQuery(cmdText);

        Commons.ClearCookies();
        Commons.ClearSession();

        Response.Redirect("~/LandingPage/LandingPage.aspx");
    }

    protected void profile_LinkBtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Profile/Profile.aspx");
    }

    protected void home_LinkBtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Home/Home.aspx");
    }

    protected void shareSomething_LinkBtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/ShareSomething/ShareSomething.aspx");
    }

    protected void letsChat_LinkBtn_Click(object sender, EventArgs e)
    {
        // Go for chatting
        Response.Redirect("~/LetsChat/LetsChat.aspx");
    }

    protected void friends_LinkBtn_Click(object sender, EventArgs e)
    {
        // Show friends
        Response.Redirect("~/FriendsList/FriendsList.aspx");
    }

    protected void friendRequests_LinkBtn_Click(object sender, EventArgs e)
    {
        // Show new friend requests
        Response.Redirect("~/FriendRequests/FriendRequests.aspx");
    }

    protected void trendingPosts_LinkBtn_Click(object sender, EventArgs e)
    {
        // Redirect to show trending posts
        Response.Redirect("~/TrendingPosts/TrendingPosts.aspx");
    }

    protected void reportedPosts_LinkBtn_Click(object sender, EventArgs e)
    {
        // Redirect to show reported posts
        Response.Redirect("~/ReportedPosts/ReportedPosts.aspx");
    }

    protected void blockedUsers_LinkBtn_Click(object sender, EventArgs e)
    {
        // Redirect to show blocked users
        Response.Redirect("~/BlockedUsers/BlockedUsers.aspx");
    }
}

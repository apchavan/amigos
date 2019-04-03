using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FriendRequests_FriendRequests : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Session["RoleID"].ToString() == "1")
        {
            Session["accountMasterPage"] = "~/AccountMaster/AdminAccountMaster.master";
            this.MasterPageFile = Session["accountMasterPage"].ToString();
        }
        else if (Session["RoleID"].ToString() == "2")
        {
            Session["accountMasterPage"] = "~/AccountMaster/UserAccountMaster.master";
            this.MasterPageFile = Session["accountMasterPage"].ToString();
        }
    }   // Method 'Page_PreInit(object sender, EventArgs e)' closed.

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"].ToString() == "")
        {
            Response.Redirect("~/LandingPage/LandingPage.aspx");
        }   // 'if (Session["UserID"].ToString() == "")' closed.

        if (!Page.IsPostBack)
            FetchFriendRequests();
    }

    private void FetchFriendRequests()
    {
        string cmdText = "SELECT friends.from_UserID AS UserID, friends.to_UserID, friends.confirmed, user_creds.firstname, user_creds.lastname, " +
                         "user_profile.photo, user_profile.profession, user_profile.at FROM friends LEFT JOIN " +
                         "user_creds ON friends.from_UserID = user_creds.UserID LEFT JOIN " +
                         "user_profile ON friends.from_UserID = user_profile.UserID " +
                         "WHERE (friends.to_UserID = " + Session["UserID"].ToString() + " AND friends.confirmed = 0)";

        DataTable dt_friendRequests = SQLHelper.FillDataTable(cmdText);

        if (dt_friendRequests.Rows.Count <= 0)
        {
            heading_Label.Text = "<strong> 😊 Currently, you do not have any new friend requests ... 😊</strong>";
            return;
        }

        heading_Label.Text = "<strong> 😎 You have <i>'" + dt_friendRequests.Rows.Count + "'</i> new friend request(s) ... 😎</strong>";

        friendRequest_DataList.DataSource = dt_friendRequests;
        friendRequest_DataList.DataBind();
    }

    protected void friendRequest_DataList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "accept_Btn_CommandName")
        {
            LinkButton otherUserName_LinkBtn = (LinkButton)e.Item.FindControl("otherUserName_LinkBtn");

            // Get UserID for other user
            HiddenField otherUserID_HiddenField = (HiddenField)e.Item.FindControl("otherUserID_HiddenField");

            string cmdText = "SELECT UserID FROM user_creds WHERE (firstname = '" + otherUserName_LinkBtn.Text.ToString().Split(' ')[0] + "')";
            DataTable dt_OtherUserIDResult = SQLHelper.FillDataTable(cmdText);

            cmdText = "UPDATE friends SET confirmed = 1 WHERE (from_UserID = " + otherUserID_HiddenField.Value.ToString() +
                      " AND to_UserID = " + Session["UserID"].ToString() + ")";
            SQLHelper.ExecuteNonQuery(cmdText);

            Commons.ShowAlertMsg(" ✔ You and " + otherUserName_LinkBtn.Text.ToString() + " are now friends ! ✔");
            Response.Redirect("FriendRequests.aspx");
        }
        else if (e.CommandName == "reject_Btn_CommandName")
        {
            LinkButton otherUserName_LinkBtn = (LinkButton)e.Item.FindControl("otherUserName_LinkBtn");

            // Get UserID for other user
            HiddenField otherUserID_HiddenField = (HiddenField)e.Item.FindControl("otherUserID_HiddenField");

            string cmdText = "SELECT UserID FROM user_creds WHERE (firstname = '" + otherUserName_LinkBtn.Text.ToString().Split(' ')[0] + "')";
            DataTable dt_OtherUserIDResult = SQLHelper.FillDataTable(cmdText);

            cmdText = "DELETE FROM friends WHERE (from_UserID = " + otherUserID_HiddenField.Value.ToString() +
                      " AND to_UserID = " + Session["UserID"].ToString() + ")";
            SQLHelper.ExecuteNonQuery(cmdText);
            /*
             * Commons.ShowAlertMsg(" 🚫 You rejected friend request from " + otherUserName_LinkBtn.Text.ToString() + " ... 🚫");
            */
            Response.Redirect("FriendRequests.aspx");
        }
        else if (e.CommandName == "otherUserName_LinkBtn_CommandName")
        {
            LinkButton otherUserName_LinkBtn = (LinkButton)e.Item.FindControl("otherUserName_LinkBtn");

            // Get UserID for other user
            HiddenField otherUserID_HiddenField = (HiddenField)e.Item.FindControl("otherUserID_HiddenField");

            string cmdText = "SELECT UserID FROM user_creds WHERE (firstname = '" + otherUserName_LinkBtn.Text.ToString().Split(' ')[0] + "')";
            DataTable dt_UserIDResult = SQLHelper.FillDataTable(cmdText);

            Response.Cookies["otherUserID"].Value = otherUserID_HiddenField.Value.ToString();
            Response.Redirect("FriendRequestsProfile.aspx");
        }
    }



    protected void friendRequest_DataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ShowDefaultProfileImageForNull(e);
        }
    }

    // Method for show default profile if user profile is not set
    private void ShowDefaultProfileImageForNull(DataListItemEventArgs e)
    {
        Image otherUserProfile_Image = (Image)e.Item.FindControl("otherUserProfile_Image");

        if (otherUserProfile_Image.ImageUrl.ToString().Trim() == "")
            otherUserProfile_Image.ImageUrl = "~/Images/no_image.jpg";
    }
}
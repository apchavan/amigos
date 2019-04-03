using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SearchResult_SearchResult : System.Web.UI.Page
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
            Response.Redirect("~/LandingPage/LandingPage.aspx");

        if (!Page.IsPostBack)
            FetchSearchData();
    }

    // Method to fetch all searched users
    protected void FetchSearchData()
    {
        if (Request.Cookies["searchName"].Value.Trim() == "" || Request.Cookies["searchName"] == null)
        {
            Response.Redirect("~/Home/Home.aspx");
            return;
        }

        string cmdText = "SELECT user_creds.UserID, user_creds.firstname, user_creds.lastname, user_creds.gender, " +
                         "user_profile.photo, user_profile.profession, user_profile.at FROM user_creds LEFT JOIN user_profile " +
                         "ON user_creds.UserID = user_profile.UserID WHERE " +
                         "(user_creds.firstname LIKE '" + Request.Cookies["searchName"].Value + "%' OR " +
                         "user_creds.lastname LIKE '" + Request.Cookies["searchName"].Value + "%')";

        DataTable dt_userSearchResult = new DataTable();
        dt_userSearchResult = SQLHelper.FillDataTable(cmdText);

        if (dt_userSearchResult.Rows.Count <= 0)
        {
            heading_Label.Text = "<strong>🤔 No people found with name having <i>'" + Request.Cookies["searchName"].Value + "'</i> !" +
                                 "<br />Make sure you spelled the name correctly ...</strong>";
            return;
        }   // 'if (dt_userSearchResult.Rows.Count <= 0)' closed.

        heading_Label.Text = "<strong>😎 <i>" + dt_userSearchResult.Rows.Count + "</i> user(s) found with name having <i>'" +
                              Request.Cookies["searchName"].Value + "'</i> ...</strong>";

        search_DataList.DataSource = dt_userSearchResult;
        search_DataList.DataBind();

        //Response.Cookies["searchName"].Value = "";
    }   // Method 'FetchSearchData()' closed.


    protected void search_DataList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "otherUserName_LinkBtn_CommandName")
        {
            LinkButton otherUserName_LinkBtn = (LinkButton)e.Item.FindControl("otherUserName_LinkBtn");
            
            // Get UserID for other user
            HiddenField otherUserID_HiddenField = (HiddenField)e.Item.FindControl("otherUserID_HiddenField");

            string cmdText = "SELECT UserID FROM user_creds WHERE (firstname = '" + otherUserName_LinkBtn.Text.ToString().Split(' ')[0] +
                             "' AND lastname = '" + otherUserName_LinkBtn.Text.ToString().Split(' ')[1] + "')";
            DataTable dt_UserIDResult = SQLHelper.FillDataTable(cmdText);

            Response.Cookies["otherUserID"].Value = otherUserID_HiddenField.Value.ToString();
            Response.Redirect("SearchUserProfile.aspx");
        }   // 'if (e.CommandName == "otherUserName_LinkBtn_CommandName")' closed.

        else if (e.CommandName == "sendFriendRequest_Btn_CommandName")
        {
            LinkButton otherUserName_LinkBtn = (LinkButton)e.Item.FindControl("otherUserName_LinkBtn");

            // Get UserID for other user to send friend request
            HiddenField otherUserID_HiddenField = (HiddenField)e.Item.FindControl("otherUserID_HiddenField");

            string cmdText = "SELECT UserID FROM user_creds WHERE (firstname = '" + otherUserName_LinkBtn.Text.ToString().Split(' ')[0] +
                             "' AND lastname = '" + otherUserName_LinkBtn.Text.ToString().Split(' ')[1] + "' AND " + 
                             "UserID = " + otherUserID_HiddenField.Value.ToString() + ")";

            DataTable dt_UserIDResult = SQLHelper.FillDataTable(cmdText);

            // Check if already friend request accepted by other user
            cmdText = "SELECT from_UserID, to_UserID, confirmed FROM friends WHERE " + 
                      "(from_UserID = " + Session["UserID"].ToString() + " AND to_UserID = " + dt_UserIDResult.Rows[0]["UserID"].ToString() + 
                      " AND confirmed = 0) " + 
                      " OR (from_UserID = " + dt_UserIDResult.Rows[0]["UserID"].ToString() + 
                      " AND to_UserID = " + Session["UserID"].ToString() + " AND confirmed = 0)";
            DataTable dt_confirmAlreadyFriends = SQLHelper.FillDataTable(cmdText);

            if(dt_confirmAlreadyFriends.Rows.Count > 0)
            {
                FetchSearchData();
                search_UpdatePanel.Update();
                return;
            }

            

            // Accept as friend
            cmdText = "INSERT INTO friends(from_UserID, to_UserID, confirmed) VALUES(" + Session["UserID"].ToString() +
                             ", " + dt_UserIDResult.Rows[0]["UserID"].ToString() + ", 0)";
            SQLHelper.ExecuteNonQuery(cmdText);
            
            cmdText = "SELECT user_creds.UserID, user_creds.firstname, user_creds.lastname, user_creds.gender, " +
                             "user_profile.photo, user_profile.profession, user_profile.at FROM user_creds LEFT JOIN user_profile " +
                             "ON user_creds.UserID = user_profile.UserID WHERE " +
                             "(user_creds.firstname LIKE '" + Request.Cookies["searchName"].Value + "%' OR " +
                             "user_creds.lastname LIKE '" + Request.Cookies["searchName"].Value + "%')";

            DataTable dt_userSearchResult = SQLHelper.FillDataTable(cmdText);
            search_DataList.DataSource = dt_userSearchResult;
            search_DataList.DataBind();
        }   // 'else if (e.CommandName == "sendFriendRequest_Btn_CommandName")' closed.
    }   // Method 'search_DataList_ItemCommand(object source, DataListCommandEventArgs e)' closed.


    protected void search_DataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ShowDefaultProfileImageForNull(e);
            IsEnabled_sendFriendRequest_Btn(e);
        }   // 'if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)' closed.
    }   // Method 'search_DataList_ItemDataBound(object sender, DataListItemEventArgs e)' closed.


    // Method for show default profile if user profile is not set
    private void ShowDefaultProfileImageForNull(DataListItemEventArgs e)
    {
        Image otherUserProfile_Image = (Image)e.Item.FindControl("otherUserProfile_Image");

        if (otherUserProfile_Image.ImageUrl.ToString().Trim() == "")
            otherUserProfile_Image.ImageUrl = "~/Images/no_image.jpg";
    }   // Method 'ShowDefaultProfileImageForNull(DataListItemEventArgs e)' closed.

    // Method to decide whether to enable 'sendFriendRequest_Btn' button or not and change 'requestStatus_Label'.
    private void IsEnabled_sendFriendRequest_Btn(DataListItemEventArgs e)
    {
        LinkButton otherUserName_LinkBtn = (LinkButton)e.Item.FindControl("otherUserName_LinkBtn");
        Label requestStatus_Label = (Label)e.Item.FindControl("requestStatus_Label");

        // Get UserID for other user to send friend request
        HiddenField otherUserID_HiddenField = (HiddenField)e.Item.FindControl("otherUserID_HiddenField");

        // Check whether search name ID & current ID is same
        if (Session["UserID"].ToString() == otherUserID_HiddenField.Value.ToString())
        {
            ((Button)e.Item.FindControl("sendFriendRequest_Btn")).Enabled = false;
            requestStatus_Label.Text = "<font size='1'> (😎 It's you !)</font>";
            return;
        }

        // Check if friend request already sent by current user
        string cmdText = "SELECT from_UserID, to_UserID, confirmed FROM friends WHERE (from_UserID = " + Session["UserID"].ToString() +
                  " AND to_UserID = " + otherUserID_HiddenField.Value.ToString() + " AND confirmed = 0)";
        DataTable dt_FriendsResult = SQLHelper.FillDataTable(cmdText);

        if (dt_FriendsResult.Rows.Count > 0)
        {
            ((Button)e.Item.FindControl("sendFriendRequest_Btn")).Enabled = false;
            requestStatus_Label.Text = "<font size='1'> (✔ Friend request sent ...) </font>";
            return;
        }

        dt_FriendsResult.Reset();

        // Check if friend request already received to current user
        cmdText = "SELECT from_UserID, to_UserID, confirmed FROM friends WHERE (from_UserID = " + otherUserID_HiddenField.Value.ToString() +
                  " AND to_UserID = " + Session["UserID"].ToString() + " AND confirmed = 0)";
        dt_FriendsResult = SQLHelper.FillDataTable(cmdText);
        if (dt_FriendsResult.Rows.Count > 0)
        {
            ((Button)e.Item.FindControl("sendFriendRequest_Btn")).Enabled = false;
            requestStatus_Label.Text = "<font size='1'> (✔ Friend request already received to you ...) </font>";
            return;
        }

        dt_FriendsResult.Reset();

        // Check if current user and other user are already friends
        cmdText = "SELECT from_UserID, to_UserID, confirmed FROM friends WHERE (from_UserID = " + otherUserID_HiddenField.Value.ToString() +
                  " AND to_UserID = " + Session["UserID"].ToString() + " AND confirmed = 1) OR " +
                  "(from_UserID = " + Session["UserID"].ToString() +
                  " AND to_UserID = " + otherUserID_HiddenField.Value.ToString() + " AND confirmed = 1)";
        dt_FriendsResult = SQLHelper.FillDataTable(cmdText);
        if (dt_FriendsResult.Rows.Count > 0)
        {
            ((Button)e.Item.FindControl("sendFriendRequest_Btn")).Enabled = false;
            requestStatus_Label.Text = "<font size='1'> (✔ Already friends ...) </font>";
            return;
        }
        
        dt_FriendsResult.Reset();

        // Check if other user account is blocked by administrator
        cmdText = "SELECT active FROM user_creds WHERE (UserID = " + otherUserID_HiddenField.Value.ToString() + ")";
        dt_FriendsResult = SQLHelper.FillDataTable(cmdText);

        if(otherUserID_HiddenField.Value.ToString() == "False")
        {
            ((Button)e.Item.FindControl("sendFriendRequest_Btn")).Enabled = false;
            requestStatus_Label.Text = "<font size='1'> (🚫 Blocked by administrator ...) </font>";
            return;
        }

        ((Button)e.Item.FindControl("sendFriendRequest_Btn")).Enabled = true;
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BlockedUsers_BlockedUsers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"].ToString() == "")
            Response.Redirect("~/LandingPage/LandingPage.aspx");

        if (!Page.IsPostBack)
            FetchBlockedUsers();
    }   // Method 'Page_Load(object sender, EventArgs e)' closed.

    #region Methods for fetch records for blocked users, show default image for user with no image and unblock user
    // Method to fetch all blocked users
    private void FetchBlockedUsers()
    {
        string cmdText = "SELECT user_creds.UserID, user_creds.firstname, user_creds.lastname, user_creds.gender, " +
                         "user_profile.photo, user_profile.profession, user_profile.at FROM user_creds LEFT JOIN user_profile " +
                         "ON user_creds.UserID = user_profile.UserID WHERE (active = 0)";

        DataTable dt_userSearchResult = new DataTable();
        dt_userSearchResult = SQLHelper.FillDataTable(cmdText);

        if (dt_userSearchResult.Rows.Count <= 0)
        {
            heading_Label.Text = "<strong>😉 Well, no blocked people found ...</strong>";
            blockedUsers_DataList.Visible = false;
            blockedUsers_UpdatePanel.Update();      // Update the 'blockedUsers_UpdatePanel' to reflect new changes
            return;
        }   // 'if (dt_userSearchResult.Rows.Count <= 0)' closed.

        heading_Label.Text = "<strong>😐 <i>" + dt_userSearchResult.Rows.Count + "</i> user(s) found to be blocked ...</strong>";
        blockedUsers_DataList.Visible = true;

        blockedUsers_DataList.DataSource = dt_userSearchResult;
        blockedUsers_DataList.DataBind();

    }   // Method 'FetchBlockedUsers()' closed.

    // Method for show default profile if user profile is not set
    private void ShowDefaultProfileImageForNull(DataListItemEventArgs e)
    {
        Image otherUserProfile_Image = (Image)e.Item.FindControl("otherUserProfile_Image");

        if (otherUserProfile_Image.ImageUrl.ToString().Trim() == "")
            otherUserProfile_Image.ImageUrl = "~/Images/no_image.jpg";
    }   // Method 'ShowDefaultProfileImageForNull(DataListItemEventArgs e)' closed.

    // Method to unblock specific user
    private void UnblockUser(DataListCommandEventArgs e)
    {
        HiddenField otherUserID_HiddenField = (HiddenField)e.Item.FindControl("otherUserID_HiddenField");

        // Unblock user
        string cmdText = "UPDATE user_creds SET active = 1 WHERE (UserID = " + otherUserID_HiddenField.Value.ToString() + ")";

        SQLHelper.ExecuteNonQuery(cmdText);
    }   // Method 'UnblockUser(DataListCommandEventArgs e)' closed.
    #endregion

    #region 'blockedUsers_DataList' Events
    protected void blockedUsers_DataList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "unBlock_Btn_CommandName":
                Panel confirmUnblock_Panel = (Panel)e.Item.FindControl("confirmUnblock_Panel");
                if (confirmUnblock_Panel.Visible)
                    confirmUnblock_Panel.Visible = false;
                else
                {
                    confirmUnblock_Panel.Visible = true;
                    ((Label)e.Item.FindControl("unblockUser_Label")).Text = "<strong>Confirm unblock user ? 🤔</strong>"
                                                                            + "<br /> (Unblock "
                                                                            + ((Label)e.Item.FindControl("otherUserName_Label")).Text
                                                                            + ")";
                }   // 'else' closed.
                break;

            case "cancelUnblockUser_Button_CommandName":
                ((Panel)e.Item.FindControl("confirmUnblock_Panel")).Visible = false;
                break;

            case "yesUnblockUser_Button_CommandName":
                UnblockUser(e);                     // Unblock selected user
                FetchBlockedUsers();                // Fetch new data for blocked users
                break;
        }   // 'switch (e.CommandName)' closed.

    }   // Method 'blockedUsers_DataList_ItemCommand(object source, DataListCommandEventArgs e)' closed.

    protected void blockedUsers_DataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ShowDefaultProfileImageForNull(e);      // Show default profile if user profile is not set
        }   // 'if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)' closed.

    }   // Method 'blockedUsers_DataList_ItemDataBound(object sender, DataListItemEventArgs e)' closed.

    #endregion
}   // class 'BlockedUsers_BlockedUsers' closed.
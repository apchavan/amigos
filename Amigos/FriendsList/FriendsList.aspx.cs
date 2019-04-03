using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FriendsList_FriendsList : System.Web.UI.Page
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
            FetchFriendsList();
    }

    private void FetchFriendsList()
    {
        string cmdText = "SELECT friends.from_UserID, friends.to_UserID, friends.confirmed, user_creds.firstname, user_creds.lastname, " +
                         "user_profile.photo, user_profile.profession, user_profile.at FROM friends LEFT JOIN " +
                         "user_creds ON friends.from_UserID = user_creds.UserID LEFT JOIN " +
                         "user_profile ON friends.from_UserID = user_profile.UserID " +
                         "WHERE (friends.to_UserID = " + Session["UserID"].ToString() +
                         " OR friends.from_UserID = " + Session["UserID"].ToString() +
                         ") AND friends.confirmed = 1";

        DataTable dt_totalNumberFriendsList = SQLHelper.FillDataTable(cmdText);

        if (dt_totalNumberFriendsList.Rows.Count <= 0)
        {
            heading_Label.Text = "<strong> 😊 Currently, you do not have any friends ... 😊<br /> Try to search people you know to get connected !</strong>";
            return;
        }

        heading_Label.Text = "<strong> 😎 You have <i>'" + dt_totalNumberFriendsList.Rows.Count + "'</i> friend(s) ... 😎</strong>";

        DataTable dt_friendsList = new DataTable();

        //dt_friendsList.Columns.Add("from_UserID");
        dt_friendsList.Columns.Add("UserID");
        dt_friendsList.Columns.Add("firstname");
        dt_friendsList.Columns.Add("lastname");
        dt_friendsList.Columns.Add("photo");
        dt_friendsList.Columns.Add("profession");
        dt_friendsList.Columns.Add("at");

        for (int i = 0; i < dt_totalNumberFriendsList.Rows.Count; i++)
        {
            if (dt_totalNumberFriendsList.Rows[i]["from_UserID"].ToString() == Session["UserID"].ToString())
            {
                cmdText = "SELECT user_creds.UserID, user_creds.firstname, user_creds.lastname, user_profile.photo, " +
                          "user_profile.profession, user_profile.at FROM user_creds LEFT JOIN user_profile " + 
                          "ON (user_creds.UserID = user_profile.UserID) WHERE (user_creds.UserID = " + 
                          dt_totalNumberFriendsList.Rows[i]["to_UserID"].ToString() + ")";
            }
            else if (dt_totalNumberFriendsList.Rows[i]["to_UserID"].ToString() == Session["UserID"].ToString())
            {
                cmdText = "SELECT user_creds.UserID, user_creds.firstname, user_creds.lastname, user_profile.photo, " +
                          "user_profile.profession, user_profile.at FROM user_creds LEFT JOIN user_profile " +
                          "ON (user_creds.UserID = user_profile.UserID) WHERE (user_creds.UserID = " +
                          dt_totalNumberFriendsList.Rows[i]["from_UserID"].ToString() + ")";
            }

            DataTable dt_row = SQLHelper.FillDataTable(cmdText);
            dt_friendsList.Rows.Add(
                                    dt_row.Rows[0]["UserID"].ToString(), dt_row.Rows[0]["firstname"].ToString(),
                                    dt_row.Rows[0]["lastname"].ToString(), dt_row.Rows[0]["photo"].ToString(), 
                                    dt_row.Rows[0]["profession"].ToString(), dt_row.Rows[0]["at"].ToString()
                                   );
        }
        
        friendsList_DataList.DataSource = dt_friendsList;
        friendsList_DataList.DataBind();
    }

    protected void friendsList_DataList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "unfriend_Btn_CommandName")
        {
            LinkButton otherUserName_LinkBtn = (LinkButton)e.Item.FindControl("otherUserName_LinkBtn");
            Label profession_and_at_Label = (Label)e.Item.FindControl("profession_and_at_Label");

            // Get UserID for other user
            HiddenField otherUserID_HiddenField = (HiddenField)e.Item.FindControl("otherUserID_HiddenField");
            
            // Unfriend
            string cmdText = "DELETE FROM friends WHERE (from_UserID = " + otherUserID_HiddenField.Value.ToString() +
                             " AND to_UserID = " + Session["UserID"].ToString() + " AND " +
                             "confirmed = 1)";
            SQLHelper.ExecuteNonQuery(cmdText);

            cmdText = "DELETE FROM friends WHERE (from_UserID = " + Session["UserID"].ToString() +
                             " AND to_UserID = " + otherUserID_HiddenField.Value.ToString() + " AND " +
                             "confirmed = 1)";
            SQLHelper.ExecuteNonQuery(cmdText);

            /*
             * Show message as 'No longer friends !'.
            */
            Response.Redirect("FriendsList.aspx");
        }
        else if (e.CommandName == "otherUserName_LinkBtn_CommandName")
        {
            LinkButton otherUserName_LinkBtn = (LinkButton)e.Item.FindControl("otherUserName_LinkBtn");

            // Get UserID for other user
            HiddenField otherUserID_HiddenField = (HiddenField)e.Item.FindControl("otherUserID_HiddenField");

            // Check if still friends or not
            string cmdText = "SELECT confirmed FROM friends WHERE " + 
                             "(from_UserID = " + Session["UserID"].ToString() + " AND to_UserID = " + 
                             otherUserID_HiddenField.Value.ToString() + ")";
            DataTable dt_confirmedStatusFrom = SQLHelper.FillDataTable(cmdText);
            
            cmdText = "SELECT confirmed FROM friends WHERE " +
                      "(from_UserID = " + otherUserID_HiddenField.Value.ToString() + " AND to_UserID = " +
                      Session["UserID"].ToString() + ")";
            DataTable dt_confirmedStatusTo = SQLHelper.FillDataTable(cmdText);
            
            if (dt_confirmedStatusFrom.Rows.Count == 0 && dt_confirmedStatusTo.Rows.Count == 0)
            {
                Response.Redirect("FriendsList.aspx");
                return;
            }

            Response.Cookies["otherUserID"].Value = otherUserID_HiddenField.Value.ToString();
            Response.Redirect("FriendsListProfile.aspx");
        }
    }   // Method 'friendsList_DataList_ItemCommand(object source, DataListCommandEventArgs e)' closed.

    protected void friendsList_DataList_ItemDataBound(object sender, DataListItemEventArgs e)
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

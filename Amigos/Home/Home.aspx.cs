using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Home_Home : System.Web.UI.Page
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
            LoadPosts();
    }   // 'Page_Load(object sender, EventArgs e)' closed.

    // Method to load all posts related to current user
    private void LoadPosts()
    {
        string cmdText = "SELECT friends.from_UserID, friends.to_UserID, friends.confirmed, user_creds.firstname, user_creds.lastname, " +
                         "user_profile.photo, user_profile.profession, user_profile.at FROM friends LEFT JOIN " +
                         "user_creds ON friends.from_UserID = user_creds.UserID LEFT JOIN " +
                         "user_profile ON friends.from_UserID = user_profile.UserID " +
                         "WHERE (friends.to_UserID = " + Session["UserID"].ToString() +
                         " OR friends.from_UserID = " + Session["UserID"].ToString() +
                         ") AND friends.confirmed = 1";

        DataTable dt_friendsList = SQLHelper.FillDataTable(cmdText);

        // Get data of friends & also current user's ID in DataTable object below
        DataTable dt_allFriendsList = new DataTable();

        dt_allFriendsList.Columns.Add("UserID");
        dt_allFriendsList.Columns.Add("firstname");
        dt_allFriendsList.Columns.Add("lastname");
        dt_allFriendsList.Columns.Add("photo");

        for (int i = 0; i < dt_friendsList.Rows.Count; ++i)
        {
            if (dt_friendsList.Rows[i]["from_UserID"].ToString() == Session["UserID"].ToString())
            {
                cmdText = "SELECT user_creds.UserID, user_creds.firstname, user_creds.lastname, user_profile.photo " +
                          "FROM user_creds LEFT JOIN user_profile ON user_creds.UserID = user_profile.UserID " +
                          "WHERE (user_creds.UserID = " + dt_friendsList.Rows[i]["to_UserID"].ToString() + ")";
            }
            else if (dt_friendsList.Rows[i]["to_UserID"].ToString() == Session["UserID"].ToString())
            {
                cmdText = "SELECT user_creds.UserID, user_creds.firstname, user_creds.lastname, user_profile.photo " +
                          "FROM user_creds LEFT JOIN user_profile ON user_creds.UserID = user_profile.UserID " +
                          "WHERE (user_creds.UserID = " + dt_friendsList.Rows[i]["from_UserID"].ToString() + ")";
            }
            DataTable dt_row = SQLHelper.FillDataTable(cmdText);

            dt_allFriendsList.Rows.Add(
                                       dt_row.Rows[0]["UserID"].ToString(), dt_row.Rows[0]["firstname"].ToString(),
                                       dt_row.Rows[0]["lastname"].ToString(), dt_row.Rows[0]["photo"].ToString()
                                       );
        }

        // Get info of current user
        cmdText = "SELECT user_creds.firstname, user_creds.lastname, user_profile.photo FROM " +
                  "user_creds LEFT JOIN user_profile ON (user_creds.UserID = user_profile.UserID) " +
                  "WHERE (user_creds.UserID = " + Session["UserID"].ToString() + ")";
        DataTable dt_currentUser = SQLHelper.FillDataTable(cmdText);
        if (dt_currentUser.Rows[0]["photo"].ToString().Trim() == "")
            dt_currentUser.Rows[0]["photo"] = "~/Images/no_image.jpg";

        // Then add current user details to 'dt_allFriendsList' DataTable for ease of use & 
        // store all data related to current user in single object of DataTable
        dt_allFriendsList.Rows.Add(
                                   Session["UserID"].ToString(), dt_currentUser.Rows[0]["firstname"].ToString(),
                                   dt_currentUser.Rows[0]["lastname"].ToString(), dt_currentUser.Rows[0]["photo"]
                                  );


        DataTable dt_userRelatedPosts = new DataTable();
        dt_userRelatedPosts.Columns.Add("UserID");
        dt_userRelatedPosts.Columns.Add("firstname");
        dt_userRelatedPosts.Columns.Add("lastname");
        dt_userRelatedPosts.Columns.Add("photo");
        dt_userRelatedPosts.Columns.Add("post_heading");
        dt_userRelatedPosts.Columns.Add("post_text");
        dt_userRelatedPosts.Columns.Add("post_image");
        dt_userRelatedPosts.Columns.Add("dated");

        cmdText = "SELECT UserID, post_heading, post_text, post_image, CAST(dated as DATE) AS post_date, dated FROM posts_mst " +
                  "ORDER BY post_date DESC, dated DESC";
        DataTable dt_allPosts = SQLHelper.FillDataTable(cmdText);
        int postCount = dt_allPosts.Rows.Count;

        for (int i = 0; i < postCount; i++)
        {
            for (int j = 0; j < dt_allFriendsList.Rows.Count; j++)
            {
                if (dt_allPosts.Rows[i]["UserID"].ToString() == dt_allFriendsList.Rows[j]["UserID"].ToString())
                {
                    dt_userRelatedPosts.Rows.Add(
                                                 dt_allFriendsList.Rows[j]["UserID"].ToString(),
                                                 dt_allFriendsList.Rows[j]["firstname"].ToString(),
                                                 dt_allFriendsList.Rows[j]["lastname"].ToString(),
                                                 dt_allFriendsList.Rows[j]["photo"].ToString(),
                                                 dt_allPosts.Rows[i]["post_heading"].ToString(),
                                                 dt_allPosts.Rows[i]["post_text"].ToString(),
                                                 dt_allPosts.Rows[i]["post_image"].ToString(),
                                                 dt_allPosts.Rows[i]["dated"].ToString()
                                                );
                }   // 'if(...)' closed.
            }       // 'for (int j = 0; j < dt_allFriendsList.Rows.Count; j++)' closed.
        }           // 'for (int i = 0; i < postCount; i++)' closed.

        if(dt_userRelatedPosts.Rows.Count <= 0)
        {
            posts_DataList.Visible = false;
            topHeading_Label.Text = "<strong>🤔 No posts found ! <br /> Posts shared by you and your friends will be shown here ... ";
            return;
        }   // 'if(dt_userRelatedPosts.Rows.Count <= 0)' closed.

        posts_DataList.Visible = true;
        topHeading_Label.Text = "";
        topHeading_Label.Visible = false;

        posts_DataList.DataSource = dt_userRelatedPosts;
        posts_DataList.DataBind();
    }   // Method 'LoadPosts()' closed.


    #region 'posts_DataList' Events
    protected void posts_DataList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "like_LinkBtn_CommandName":
                SaveOrRemoveLike(e);
                break;

            case "comment_LinkBtn_CommandName":

                if (((Panel)e.Item.FindControl("reportPost_Panel")).Visible)
                    ((Panel)e.Item.FindControl("reportPost_Panel")).Visible = false;

                Panel addComments_Panel_Show = (Panel)e.Item.FindControl("addComments_Panel");

                if (addComments_Panel_Show.Visible == true)
                {
                    ((Button)e.Item.FindControl("showComments_Button")).Text = "Show comments";
                    ((Panel)e.Item.FindControl("showComments_Panel")).Visible = false;
                    addComments_Panel_Show.Visible = false;
                }
                else
                    addComments_Panel_Show.Visible = true;
                break;

            case "report_LinkBtn_CommandName":

                if (((Panel)e.Item.FindControl("addComments_Panel")).Visible)
                    ((Panel)e.Item.FindControl("addComments_Panel")).Visible = false;

                Panel reportPost_Panel = (Panel)e.Item.FindControl("reportPost_Panel");
                if (reportPost_Panel.Visible)
                {
                    ((TextBox)e.Item.FindControl("report_TextBox")).Text = "";
                    reportPost_Panel.Visible = false;
                    return;
                }
                else
                {
                    reportPost_Panel.Visible = true;
                    if (IsPostReportedOrOwner(e))
                        return;
                }

                break;

            case "cancelReport_Button_CommandName":
                // Cancel to hide 'reportPost_Panel'
                ((TextBox)e.Item.FindControl("report_TextBox")).Text = "";
                ((Panel)e.Item.FindControl("reportPost_Panel")).Visible = false;
                break;

            case "reportToAdmin_Button_CommandName":
                ReportPost(e);  // Report post to admin
                LoadPosts();    // Load all posts again to reflect changes
                break;

            case "remove_LinkBtn_CommandName":
                Panel confirmRemovePost_Panel = (Panel)e.Item.FindControl("confirmRemovePost_Panel");

                if (confirmRemovePost_Panel.Visible)
                    confirmRemovePost_Panel.Visible = false;    // Hide 'confirmRemovePost_Panel' if already visible
                else
                    confirmRemovePost_Panel.Visible = true;     // Show 'confirmRemovePost_Panel' if already NOT visible

                break;

            case "otherUserName_LinkBtn_CommandName":
                ShowProfile(e);     // Redirect to clicked name of poster's profile page
                break;

            case "cancelComment_Button_CommandName":
                // Cancel to hide 'comments_Panel'
                Panel addComments_Panel_Cancel = (Panel)e.Item.FindControl("addComments_Panel");
                Panel showComments_Panel_Cancel = (Panel)e.Item.FindControl("showComments_Panel");

                if (showComments_Panel_Cancel.Visible == true)
                {
                    ((Button)e.Item.FindControl("showComments_Button")).Text = "Show comments";
                    showComments_Panel_Cancel.Visible = false;
                }

                addComments_Panel_Cancel.Visible = false;
                break;

            case "showComments_Button_CommandName":
                Panel showComments_Panel = (Panel)e.Item.FindControl("showComments_Panel");

                if (showComments_Panel.Visible == true)
                {
                    ((Button)e.Item.FindControl("showComments_Button")).Text = "Show comments";
                    showComments_Panel.Visible = false;     // Hide 'showComments_Panel'
                }
                else
                {
                    ((Button)e.Item.FindControl("showComments_Button")).Text = "Hide comments";
                    showComments_Panel.Visible = true;      // Show 'showComments_Panel'
                    ShowComments(e);                        // Show all comments
                }
                break;

            case "addComment_Button_CommandName":
                if (((TextBox)e.Item.FindControl("comment_TextBox")).Text.Trim() == "")
                    ((TextBox)e.Item.FindControl("comment_TextBox")).Focus();
                else
                {
                    SaveComment(e);     // Save new comment
                    LoadPosts();        // Load all posts again to reflect comment count around 'comment_LinkBtn'
                }
                break;

            case "yesRemovePost_Button_CommandName":
                ((Panel)e.Item.FindControl("confirmRemovePost_Panel")).Visible = false;     // Hide 'confirmRemovePost_Panel'
                RemovePost(e);  // Remove post
                LoadPosts();    // Load all posts again to reflect changes
                break;

            case "cancelRemovePost_Button_CommandName":
                ((Panel)e.Item.FindControl("confirmRemovePost_Panel")).Visible = false;     // Hide 'confirmRemovePost_Panel'
                break;

        }   // 'switch (e.CommandName)'closed.
    }       // Method 'posts_DataList_ItemCommand(object source, DataListCommandEventArgs e)' closed.

    protected void posts_DataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ((LinkButton)e.Item.FindControl("like_LinkBtn")).Text = "<i class='fa fa-heart' aria-hidden='true' style='margin-right: 5px;'></i> " + "Like";
            ((LinkButton)e.Item.FindControl("comment_LinkBtn")).Text = "<i class='fa fa-comments' aria-hidden='true' style='margin-right: 5px;'></i> " + "Comment";
            ((LinkButton)e.Item.FindControl("report_LinkBtn")).Text = "<i class='fa fa-flag' aria-hidden='true' style='margin-right: 5px;'></i> " + "Report";
            ((LinkButton)e.Item.FindControl("remove_LinkBtn")).Text = "<i class='fa fa-trash' aria-hidden='true' style='margin-right: 5px;'></i> " + "Remove";

            // Load total existing likes count
            LoadLikesCount(e);

            // Load total existing comments count
            LoadCommentsCount(e);

            // Load total existing reported count
            LoadReportedCount(e);

            DataTable dt_userRelatedPosts = (DataTable)posts_DataList.DataSource;

            // Check whether to show 'Remove' LinkButton or not. If post is of current user, then show otherwise hide it
            if (dt_userRelatedPosts.Rows[e.Item.ItemIndex]["UserID"].ToString() == Session["UserID"].ToString())
            {
                ((LinkButton)e.Item.FindControl("remove_LinkBtn")).Enabled =
                    ((LinkButton)e.Item.FindControl("remove_LinkBtn")).Visible = true;
                ((Label)e.Item.FindControl("postOwnerStatus_Label")).Text = "<font size='1'> (😎 It's you !)</font>";
            }

            else
            {
                ((LinkButton)e.Item.FindControl("remove_LinkBtn")).Enabled =
                        ((LinkButton)e.Item.FindControl("remove_LinkBtn")).Visible = false;
                ((Label)e.Item.FindControl("postOwnerStatus_Label")).Text = "";
                ((Label)e.Item.FindControl("postOwnerStatus_Label")).Visible = false;
            }


            // Check whether user shared post has an image or not, if available then show post image otherwise hide image section
            if (dt_userRelatedPosts.Rows[e.Item.ItemIndex]["post_image"].ToString() == "NoImage")
                ((Image)e.Item.FindControl("postImage_Image")).Enabled =
                    ((Image)e.Item.FindControl("postImage_Image")).Visible = false;
            else
                ((Image)e.Item.FindControl("postImage_Image")).Enabled =
                    ((Image)e.Item.FindControl("postImage_Image")).Visible = true;

            // Check whether user profile image is available or not, if not available then show default image
            if (dt_userRelatedPosts.Rows[e.Item.ItemIndex]["photo"].ToString().Trim() == "")
                ((Image)e.Item.FindControl("otherUserProfile_Image")).ImageUrl = "~/Images/no_image.jpg";

        }   // 'if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)' closed.
    }   // Method 'posts_DataList_ItemDataBound(object sender, DataListItemEventArgs e)' closed.
    #endregion


    #region Methods for save, remove, load & show: likes & comments. Also methods for report, remove post & show profile.

    // Method to save like or remove if already liked a post
    private void SaveOrRemoveLike(DataListCommandEventArgs e)
    {
        LinkButton otherUserName_LinkBtn = (LinkButton)e.Item.FindControl("otherUserName_LinkBtn");
        LinkButton like_LinkBtn = (LinkButton)e.Item.FindControl("like_LinkBtn");

        Label postTime_Label = (Label)e.Item.FindControl("postTime_Label");
        Label postHeading_Label = (Label)e.Item.FindControl("postHeading_Label");
        Label postText_Label = (Label)e.Item.FindControl("postText_Label");

        Image postImage_Image = (Image)e.Item.FindControl("postImage_Image");

        // Check if already liked 
        string cmdText = "SELECT PostID, UserID FROM posts_mst WHERE " +
                         "(post_heading = @post_heading AND " +
                         "post_text = @post_text AND " +
                         "post_image = @post_image AND " +
                         "dated = @dated)";

        SqlCommand command = new SqlCommand(cmdText, new SqlConnection(Commons.GetConnectionStringFor_user_creds));

        command.Parameters.Add("@post_heading", SqlDbType.NVarChar).Value = postHeading_Label.Text.Trim();
        command.Parameters.Add("@post_text", SqlDbType.NVarChar).Value = postText_Label.Text.Trim();
        command.Parameters.Add("@post_image", SqlDbType.VarChar).Value = postImage_Image.ImageUrl.Trim();
        command.Parameters.Add("@dated", SqlDbType.DateTime).Value = postTime_Label.Text.Substring(11);

        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = command;

        DataTable dt_postID_PostUserID = new DataTable();
        sda.Fill(dt_postID_PostUserID);

        cmdText = "SELECT PostID FROM posts_likes WHERE " +
                         "(PostID = " + dt_postID_PostUserID.Rows[0]["PostID"].ToString() + " AND " +
                         "liked_by_UserID = " + Session["UserID"].ToString() + ")";
        DataTable dt_CheckAlreadyLiked = SQLHelper.FillDataTable(cmdText);

        if (dt_CheckAlreadyLiked.Rows.Count > 0)
        {
            cmdText = "DELETE FROM posts_likes WHERE " +
                      "(PostID = " + dt_postID_PostUserID.Rows[0]["PostID"].ToString() + " AND " +
                      "liked_by_UserID = " + Session["UserID"].ToString() + ")";
            SQLHelper.ExecuteNonQuery(cmdText);

            // Count total number of available likes
            cmdText = "SELECT count(liked_by_UserID) AS TotalLikes FROM posts_likes WHERE " +
                        "(PostID = " + dt_postID_PostUserID.Rows[0]["PostID"].ToString() + ")";

            DataTable dt_countAfterRemoveLike = SQLHelper.FillDataTable(cmdText);

            if (long.Parse(dt_countAfterRemoveLike.Rows[0]["TotalLikes"].ToString()) == 0L)
                like_LinkBtn.Text = "<i class='fa fa-heart' aria-hidden='true' style='margin-right: 5px;'></i> " + "Like";
            else
                like_LinkBtn.Text = "<i class='fa fa-heart' aria-hidden='true' style='margin-right: 5px;'></i> " + "Like (" +
                                    long.Parse(dt_countAfterRemoveLike.Rows[0]["TotalLikes"].ToString()) + ")";
            return;
        }

        cmdText = "INSERT INTO posts_likes(PostID, liked_by_UserID) VALUES(" + dt_postID_PostUserID.Rows[0]["PostID"].ToString() +
                  ", " + Session["UserID"].ToString() + ")";
        SQLHelper.ExecuteNonQuery(cmdText);

        // Count total number of available likes
        cmdText = "SELECT count(liked_by_UserID) AS TotalLikes FROM posts_likes WHERE " + "(PostID = " +
                  dt_postID_PostUserID.Rows[0]["PostID"].ToString() + ")";
        DataTable dt_countAfterSaveLike = SQLHelper.FillDataTable(cmdText);
        like_LinkBtn.Text = "<i class='fa fa-heart' aria-hidden='true' style='margin-right: 5px;'></i> " + "Unlike (" +
                            long.Parse(dt_countAfterSaveLike.Rows[0]["TotalLikes"].ToString()) + ")";
    } // Method 'SaveOrRemoveLike(DataListCommandEventArgs e)' closed.

    // Method to save comment on a post
    private void SaveComment(DataListCommandEventArgs e)
    {
        // Get post details
        LinkButton otherUserName_LinkBtn = (LinkButton)e.Item.FindControl("otherUserName_LinkBtn");
        LinkButton comment_LinkBtn = (LinkButton)e.Item.FindControl("comment_LinkBtn");

        Label postTime_Label = (Label)e.Item.FindControl("postTime_Label");
        Label postHeading_Label = (Label)e.Item.FindControl("postHeading_Label");
        Label postText_Label = (Label)e.Item.FindControl("postText_Label");

        Image postImage_Image = (Image)e.Item.FindControl("postImage_Image");

        string cmdText = "SELECT PostID, UserID FROM posts_mst WHERE " +
                         "(post_heading = @post_heading AND " +
                         "post_text = @post_text AND " +
                         "post_image = @post_image AND " +
                         "dated = @dated)";

        SqlCommand commandSelect = new SqlCommand(cmdText, new SqlConnection(Commons.GetConnectionStringFor_user_creds));

        commandSelect.Parameters.Add("@post_heading", SqlDbType.NVarChar).Value = postHeading_Label.Text.Trim();
        commandSelect.Parameters.Add("@post_text", SqlDbType.NVarChar).Value = postText_Label.Text.Trim();
        commandSelect.Parameters.Add("@post_image", SqlDbType.VarChar).Value = postImage_Image.ImageUrl.Trim();
        commandSelect.Parameters.Add("@dated", SqlDbType.DateTime).Value = postTime_Label.Text.Substring(11);

        SqlDataAdapter sqlDataAdapter_Select = new SqlDataAdapter();
        sqlDataAdapter_Select.SelectCommand = commandSelect;

        DataTable dt_postID_PostUserID = new DataTable();
        sqlDataAdapter_Select.Fill(dt_postID_PostUserID);

        // Save comment
        cmdText = "INSERT INTO posts_comments(PostID, commenter_UserID, comment_text, dated) " +
                  "VALUES(@PostID, @commenter_UserID, @comment_text, @dated)";
        SqlConnection connection = new SqlConnection(Commons.GetConnectionStringFor_user_creds);
        SqlCommand commandInsert = new SqlCommand(cmdText, connection);

        // Find 'comment_TextBox' control to align new line in comments properly
        TextBox comment_TextBox = (TextBox)e.Item.FindControl("comment_TextBox");

        // Separate "\n" using 'Split()' method
        string[] comment_text_data = comment_TextBox.Text.Trim().Split(new[] { "\n" }, StringSplitOptions.None);
        string comment_text = "";

        // Assign "<br />" at position where "\n" was found using loop below
        for (int i = 0; i < comment_text_data.Length; ++i)
            comment_text += comment_text_data[i] + "<br />";

        commandInsert.Parameters.Add("@PostID", SqlDbType.Decimal).Value = dt_postID_PostUserID.Rows[0]["PostID"].ToString();
        commandInsert.Parameters.Add("@commenter_UserID", SqlDbType.Decimal).Value = Session["UserID"].ToString();
        commandInsert.Parameters.Add("@comment_text", SqlDbType.NVarChar).Value = comment_text;
        commandInsert.Parameters.Add("@dated", SqlDbType.DateTime).Value = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt");

        connection.Open();
        commandInsert.ExecuteNonQuery();
        connection.Close();

        // Clear 'comment_TextBox'
        comment_TextBox.Text = "";

        // Reload or refresh total number of comments
        cmdText = "SELECT count(commenter_UserID) AS TotalComments FROM posts_comments WHERE " +
                         "(PostID = " + dt_postID_PostUserID.Rows[0]["PostID"].ToString() + ")";
        DataTable dt_CheckAlreadyCommentedCount = SQLHelper.FillDataTable(cmdText);

        if (long.Parse(dt_CheckAlreadyCommentedCount.Rows[0]["TotalComments"].ToString()) > 0)
        {
            comment_LinkBtn.Text = "<i class='fa fa-comments' aria-hidden='true' style='margin-right: 5px;'></i> " + "Comment (" +
                                long.Parse(dt_CheckAlreadyCommentedCount.Rows[0]["TotalComments"].ToString()) + ")";
            return;
        }   // 'if (long.Parse(dt_CheckAlreadyCommentCount.Rows[0]["TotalComments"].ToString()) > 0)' closed.
        comment_LinkBtn.Text = "<i class='fa fa-comments' aria-hidden='true' style='margin-right: 5px;'></i> " + "Comment";

    }   // Method 'SaveComment(DataListCommandEventArgs e)' closed.

    // Method to check already reported or reporting own post
    private bool IsPostReportedOrOwner(DataListCommandEventArgs e)
    {
        // Get post details
        LinkButton otherUserName_LinkBtn = (LinkButton)e.Item.FindControl("otherUserName_LinkBtn");
        Button reportToAdmin_Button = (Button)e.Item.FindControl("reportToAdmin_Button");

        Label postTime_Label = (Label)e.Item.FindControl("postTime_Label");
        Label postHeading_Label = (Label)e.Item.FindControl("postHeading_Label");
        Label postText_Label = (Label)e.Item.FindControl("postText_Label");

        Image postImage_Image = (Image)e.Item.FindControl("postImage_Image");

        string cmdText = "SELECT PostID, UserID FROM posts_mst WHERE " +
                         "(post_heading = @post_heading AND " +
                         "post_text = @post_text AND " +
                         "post_image = @post_image AND " +
                         "dated = @dated)";

        SqlCommand commandSelect = new SqlCommand(cmdText, new SqlConnection(Commons.GetConnectionStringFor_user_creds));

        commandSelect.Parameters.Add("@post_heading", SqlDbType.NVarChar).Value = postHeading_Label.Text.Trim();
        commandSelect.Parameters.Add("@post_text", SqlDbType.NVarChar).Value = postText_Label.Text.Trim();
        commandSelect.Parameters.Add("@post_image", SqlDbType.VarChar).Value = postImage_Image.ImageUrl.Trim();
        commandSelect.Parameters.Add("@dated", SqlDbType.DateTime).Value = postTime_Label.Text.Substring(11);

        SqlDataAdapter sqlDataAdapter_Select = new SqlDataAdapter();
        sqlDataAdapter_Select.SelectCommand = commandSelect;

        DataTable dt_postID_PostUserID = new DataTable();
        sqlDataAdapter_Select.Fill(dt_postID_PostUserID);

        TextBox report_TextBox = (TextBox)e.Item.FindControl("report_TextBox");

        // Check if post owner wants to report own post, if yes then avoid this
        if (dt_postID_PostUserID.Rows.Count > 0)
        {
            if (dt_postID_PostUserID.Rows[0]["UserID"].ToString() == Session["UserID"].ToString())
            {
                report_TextBox.Text = "YOU CAN NOT REPORT YOUR OWN POST !\n " +
                                      "IF YOU FEEL SOMETHING NOT GOOD IN POST, CONSIDER REMOVING IT...\n " +
                                      "🙏 THANKS FOR YOUR SUPPORT ! 🙏";
                reportToAdmin_Button.Text = "Can not be reported !";

                reportToAdmin_Button.Enabled = report_TextBox.Enabled = false;
                return true;
            }
        }

        // Check whether current user already reported the post, if yes then avoid to report again
        cmdText = "SELECT PostID, poster_UserID, reporter_UserID, report_text, dated FROM reported_posts " +
                  "WHERE (PostID = " + dt_postID_PostUserID.Rows[0]["PostID"].ToString() + " AND " +
                  "reporter_UserID = " + Session["UserID"].ToString() + ")";
        DataTable dt_CheckAlreadyReported = SQLHelper.FillDataTable(cmdText);
        if (dt_CheckAlreadyReported.Rows.Count > 0)
        {
            if (dt_CheckAlreadyReported.Rows[0]["reporter_UserID"].ToString() == Session["UserID"].ToString())
            {
                report_TextBox.Text = "YOU REPORTED ABOUT THIS POST TO ADMIN !\n " +
                                      "PLEASE WAIT WHILE ADMIN WILL REVIEW THE POST & TAKE FURTHER DECISION...\n " +
                                      "🙏 THANKS FOR YOUR PATIENCE ! 🙏";
                reportToAdmin_Button.Text = "Already reported !";

                reportToAdmin_Button.Enabled = report_TextBox.Enabled = false;
                return true;
            }
        }
        return false;
    }   // Method 'IsPostReportedOrOwner(DataListCommandEventArgs e)' closed.

    // Method to report a post by current user
    private void ReportPost(DataListCommandEventArgs e)
    {
        // Get post details
        LinkButton otherUserName_LinkBtn = (LinkButton)e.Item.FindControl("otherUserName_LinkBtn");
        LinkButton report_LinkBtn = (LinkButton)e.Item.FindControl("report_LinkBtn");

        Label postTime_Label = (Label)e.Item.FindControl("postTime_Label");
        Label postHeading_Label = (Label)e.Item.FindControl("postHeading_Label");
        Label postText_Label = (Label)e.Item.FindControl("postText_Label");

        Image postImage_Image = (Image)e.Item.FindControl("postImage_Image");

        string cmdText = "SELECT PostID, UserID FROM posts_mst WHERE " +
                         "(post_heading = @post_heading AND " +
                         "post_text = @post_text AND " +
                         "post_image = @post_image AND " +
                         "dated = @dated)";

        SqlCommand commandSelect = new SqlCommand(cmdText, new SqlConnection(Commons.GetConnectionStringFor_user_creds));

        commandSelect.Parameters.Add("@post_heading", SqlDbType.NVarChar).Value = postHeading_Label.Text.Trim();
        commandSelect.Parameters.Add("@post_text", SqlDbType.NVarChar).Value = postText_Label.Text.Trim();
        commandSelect.Parameters.Add("@post_image", SqlDbType.VarChar).Value = postImage_Image.ImageUrl.Trim();
        commandSelect.Parameters.Add("@dated", SqlDbType.DateTime).Value = postTime_Label.Text.Substring(11);

        SqlDataAdapter sqlDataAdapter_Select = new SqlDataAdapter();
        sqlDataAdapter_Select.SelectCommand = commandSelect;

        DataTable dt_postID_PostUserID = new DataTable();
        sqlDataAdapter_Select.Fill(dt_postID_PostUserID);

        TextBox report_TextBox = (TextBox)e.Item.FindControl("report_TextBox");
        /*
        // Check if post owner wants to report own post, if yes then avoid this
        if (dt_postID_PostUserID.Rows[0]["UserID"].ToString() == Session["UserID"].ToString())
        {
            report_TextBox.Text = "<strong> <i> YOU CAN NOT REPORT YOUR OWN POST ! <br /> " +
                                  "IF YOU FEEL SOMETHING NOT GOOD IN POST, CONSIDER REMOVING IT... <br /> " +
                                  "🙏 THANKS FOR YOUR SUPPORT ! 🙏";
            report_LinkBtn.Text = "<i> Can not be reported ! </i>";
            return;
        }

        // Check whether current user already reported the post, if yes then avoid to report again
        cmdText = "SELECT PostID, poster_UserID, reporter_UserID, report_text, dated FROM reported_posts " +
                  "WHERE (PostID = " + dt_postID_PostUserID.Rows[0]["PostID"].ToString() + " AND " + 
                  "reported_UserID = " + Session["UserID"].ToString() + ")";
        DataTable dt_CheckAlreadyReported = SQLHelper.FillDataTable(cmdText);
        if (dt_CheckAlreadyReported.Rows[0]["reporter_UserID"].ToString() == Session["UserID"].ToString())
        {
            report_TextBox.Text = "<strong> <i> YOU REPORTED ABOUT THIS POST TO ADMIN ! <br /> " + 
                                  "PLEASE WAIT WHILE ADMIN WILL REVIEW THE POST & TAKE FURTHER DECISION... <br /> " +
                                  "🙏 THANKS FOR YOUR PATIENCE ! 🙏";
            report_LinkBtn.Text = "<i> Already reported ! </i>";
            return;
        }
        */
        if (report_TextBox.Text.Trim() == "")
        {
            cmdText = "INSERT INTO reported_posts(PostID, poster_UserID, reporter_UserID, report_text, dated) " +
                      "VALUES(@PostID, @poster_UserID, @reporter_UserID, @report_text, @dated)";
            SqlConnection connection = new SqlConnection(Commons.GetConnectionStringFor_user_creds);
            SqlCommand commandInsert = new SqlCommand(cmdText, connection);

            commandInsert.Parameters.Add("PostID", SqlDbType.Decimal).Value = dt_postID_PostUserID.Rows[0]["PostID"].ToString();
            commandInsert.Parameters.Add("poster_UserID", SqlDbType.Decimal).Value = dt_postID_PostUserID.Rows[0]["UserID"].ToString();
            commandInsert.Parameters.Add("reporter_UserID", SqlDbType.Decimal).Value = Session["UserID"].ToString();
            commandInsert.Parameters.Add("report_text", SqlDbType.NVarChar).Value = "";
            commandInsert.Parameters.Add("dated", SqlDbType.DateTime).Value = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt");

            connection.Open();
            commandInsert.ExecuteNonQuery();
            connection.Close();
        }   // 'if (report_TextBox.Text.Trim() == "")' closed.
        else
        {
            // Separate "\n" using 'Split()' method
            string[] report_text_data = report_TextBox.Text.Trim().Split(new[] { "\n" }, StringSplitOptions.None);
            string report_text = "";

            // Assign "<br />" at position where "\n" was found using loop below
            for (int i = 0; i < report_text_data.Length; ++i)
                report_text += report_text_data[i] + "<br />";

            cmdText = "INSERT INTO reported_posts(PostID, poster_UserID, reporter_UserID, report_text, dated) " +
                      "VALUES(@PostID, @poster_UserID, @reporter_UserID, @report_text, @dated)";
            SqlConnection connection = new SqlConnection(Commons.GetConnectionStringFor_user_creds);
            SqlCommand commandInsert = new SqlCommand(cmdText, connection);

            commandInsert.Parameters.Add("PostID", SqlDbType.Decimal).Value = dt_postID_PostUserID.Rows[0]["PostID"].ToString();
            commandInsert.Parameters.Add("poster_UserID", SqlDbType.Decimal).Value = dt_postID_PostUserID.Rows[0]["UserID"].ToString();
            commandInsert.Parameters.Add("reporter_UserID", SqlDbType.Decimal).Value = Session["UserID"].ToString();
            commandInsert.Parameters.Add("report_text", SqlDbType.NVarChar).Value = report_text;
            commandInsert.Parameters.Add("dated", SqlDbType.DateTime).Value = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt");

            connection.Open();
            commandInsert.ExecuteNonQuery();
            connection.Close();
        }   // 'else' closed.

        // Clear 'report_TextBox' at the end
        report_TextBox.Text = "";

        // Reload or refresh total number of reports
        cmdText = "SELECT count(reporter_UserID) AS TotalReports FROM reported_posts WHERE " +
                         "(PostID = " + dt_postID_PostUserID.Rows[0]["PostID"].ToString() + ")";
        DataTable dt_CheckAlreadyReportedCount = SQLHelper.FillDataTable(cmdText);

        if (long.Parse(dt_CheckAlreadyReportedCount.Rows[0]["TotalReports"].ToString()) > 0)
        {
            report_LinkBtn.Text = "<i class='fa fa-flag' aria-hidden='true' style='margin-right: 5px;'></i> " + "Report (" +
                                long.Parse(dt_CheckAlreadyReportedCount.Rows[0]["TotalReports"].ToString()) + ")";
            return;
        }   // 'if (long.Parse(dt_CheckAlreadyReportedCount.Rows[0]["TotalReports"].ToString()) > 0)' closed.
        report_LinkBtn.Text = "<i class='fa fa-flag' aria-hidden='true' style='margin-right: 5px;'></i> " + "Report";

    }   // Method 'ReportPost(DataListCommandEventArgs e)' closed.

    // Method to load total number of report for current post
    private void LoadReportedCount(DataListItemEventArgs e)
    {
        // Get post details
        LinkButton otherUserName_LinkBtn = (LinkButton)e.Item.FindControl("otherUserName_LinkBtn");
        LinkButton report_LinkBtn = (LinkButton)e.Item.FindControl("report_LinkBtn");

        Label postTime_Label = (Label)e.Item.FindControl("postTime_Label");
        Label postHeading_Label = (Label)e.Item.FindControl("postHeading_Label");
        Label postText_Label = (Label)e.Item.FindControl("postText_Label");

        Image postImage_Image = (Image)e.Item.FindControl("postImage_Image");

        string cmdText = "SELECT PostID, UserID FROM posts_mst WHERE " +
                         "(post_heading = @post_heading AND " +
                         "post_text = @post_text AND " +
                         "post_image = @post_image AND " +
                         "dated = @dated)";

        SqlCommand commandSelect = new SqlCommand(cmdText, new SqlConnection(Commons.GetConnectionStringFor_user_creds));

        commandSelect.Parameters.Add("@post_heading", SqlDbType.NVarChar).Value = postHeading_Label.Text.Trim();
        commandSelect.Parameters.Add("@post_text", SqlDbType.NVarChar).Value = postText_Label.Text.Trim();
        commandSelect.Parameters.Add("@post_image", SqlDbType.VarChar).Value = postImage_Image.ImageUrl.Trim();
        commandSelect.Parameters.Add("@dated", SqlDbType.DateTime).Value = postTime_Label.Text.Substring(11);

        SqlDataAdapter sqlDataAdapter_Select = new SqlDataAdapter();
        sqlDataAdapter_Select.SelectCommand = commandSelect;

        DataTable dt_postID_PostUserID = new DataTable();
        sqlDataAdapter_Select.Fill(dt_postID_PostUserID);

        // Get total number of reports
        cmdText = "SELECT count(reporter_UserID) AS TotalReports FROM reported_posts WHERE " +
                         "(PostID = " + dt_postID_PostUserID.Rows[0]["PostID"].ToString() + ")";
        DataTable dt_CheckAlreadyReportedCount = SQLHelper.FillDataTable(cmdText);

        if (long.Parse(dt_CheckAlreadyReportedCount.Rows[0]["TotalReports"].ToString()) > 0)
        {
            report_LinkBtn.Text = "<i class='fa fa-flag' aria-hidden='true' style='margin-right: 5px;'></i> " + "Report (" +
                                long.Parse(dt_CheckAlreadyReportedCount.Rows[0]["TotalReports"].ToString()) + ")";
            return;
        }   // 'if (long.Parse(dt_CheckAlreadyReportedCount.Rows[0]["TotalReports"].ToString()) > 0)' closed.

        report_LinkBtn.Text = "<i class='fa fa-flag' aria-hidden='true' style='margin-right: 5px;'></i> " + "Report";
    }   // Method 'LoadReportedCount(DataListItemEventArgs e)' closed.

    // Method to remove own post by current user
    private void RemovePost(DataListCommandEventArgs e)
    {
        // Get post details
        LinkButton otherUserName_LinkBtn = (LinkButton)e.Item.FindControl("otherUserName_LinkBtn");

        Label postTime_Label = (Label)e.Item.FindControl("postTime_Label");
        Label postHeading_Label = (Label)e.Item.FindControl("postHeading_Label");
        Label postText_Label = (Label)e.Item.FindControl("postText_Label");

        Image postImage_Image = (Image)e.Item.FindControl("postImage_Image");

        string cmdText = "SELECT PostID, UserID FROM posts_mst WHERE " +
                         "(post_heading = @post_heading AND " +
                         "post_text = @post_text AND " +
                         "post_image = @post_image AND " +
                         "dated = @dated)";

        SqlCommand commandSelect = new SqlCommand(cmdText, new SqlConnection(Commons.GetConnectionStringFor_user_creds));

        commandSelect.Parameters.Add("@post_heading", SqlDbType.NVarChar).Value = postHeading_Label.Text.Trim();
        commandSelect.Parameters.Add("@post_text", SqlDbType.NVarChar).Value = postText_Label.Text.Trim();
        commandSelect.Parameters.Add("@post_image", SqlDbType.VarChar).Value = postImage_Image.ImageUrl.Trim();
        commandSelect.Parameters.Add("@dated", SqlDbType.DateTime).Value = postTime_Label.Text.Substring(11);

        SqlDataAdapter sqlDataAdapter_Select = new SqlDataAdapter();
        sqlDataAdapter_Select.SelectCommand = commandSelect;

        DataTable dt_postID_PostUserID = new DataTable();
        sqlDataAdapter_Select.Fill(dt_postID_PostUserID);

        // First, remove all likes 
        cmdText = "DELETE FROM posts_likes WHERE (PostID = " + dt_postID_PostUserID.Rows[0]["PostID"].ToString() + ")";
        SQLHelper.ExecuteNonQuery(cmdText);

        // Second, remove all comments
        cmdText = "DELETE FROM posts_comments WHERE (PostID = " + dt_postID_PostUserID.Rows[0]["PostID"].ToString() + ")";
        SQLHelper.ExecuteNonQuery(cmdText);

        // Third, remove post image if exists
        if (postImage_Image.ImageUrl != "NoImage")
        {
            // Get image name using 'Split()'
            string[] separatedImagePath = postImage_Image.ImageUrl.Split(new[] { "/" }, StringSplitOptions.None);

            try
            {
                // Delete image
                System.IO.File.Delete(Server.MapPath("~/User_uploads/") + Session["UserID"].ToString() + "/" + separatedImagePath[3]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }   // 'if (postImage_Image.ImageUrl != "NoImage")' closed.

        // Finally, remove the post
        cmdText = "DELETE FROM posts_mst WHERE (PostID = " + dt_postID_PostUserID.Rows[0]["PostID"].ToString() + ")";
        SQLHelper.ExecuteNonQuery(cmdText);
    }   // Method 'RemovePost(DataListCommandEventArgs e)' closed.

    // Method to remove own comment by current user
    private void RemoveComment(DataListCommandEventArgs e)
    {
        // Get comment details
        Label commentedDateTime_Label = e.Item.FindControl("commentedDateTime_Label") as Label;
        Label commentText_Label = e.Item.FindControl("commentText_Label") as Label;

        // Delete comment 
        string cmdText = "DELETE FROM posts_comments WHERE " +
                         //"(PostID = @PostID AND " +
                         "(commenter_UserID = @commenter_UserID AND " +
                         "comment_text = @comment_text AND " +
                         "dated = @dated)";
        SqlConnection connection = new SqlConnection(Commons.GetConnectionStringFor_user_creds);
        SqlCommand commandDelete = new SqlCommand(cmdText, connection);

        //commandDelete.Parameters.Add("@PostID", SqlDbType.Decimal).Value = dt_postID_PostUserID.Rows[0]["PostID"].ToString();
        commandDelete.Parameters.Add("@commenter_UserID", SqlDbType.Decimal).Value = Session["UserID"].ToString();
        commandDelete.Parameters.Add("@comment_text", SqlDbType.NVarChar).Value = commentText_Label.Text.ToString();
        commandDelete.Parameters.Add("@dated", SqlDbType.DateTime).Value = commentedDateTime_Label.Text.Substring(14).Trim();

        connection.Open();
        commandDelete.ExecuteNonQuery();
        connection.Close();
    }   // Method 'RemoveComment(DataListCommandEventArgs e)' closed.

    // Method to load total number of like(s)
    private void LoadLikesCount(DataListItemEventArgs e)
    {
        LinkButton otherUserName_LinkBtn = (LinkButton)e.Item.FindControl("otherUserName_LinkBtn");
        LinkButton like_LinkBtn = (LinkButton)e.Item.FindControl("like_LinkBtn");

        Label postTime_Label = (Label)e.Item.FindControl("postTime_Label");
        Label postHeading_Label = (Label)e.Item.FindControl("postHeading_Label");
        Label postText_Label = (Label)e.Item.FindControl("postText_Label");

        Image postImage_Image = (Image)e.Item.FindControl("postImage_Image");

        // Check already liked count
        string cmdText = "SELECT PostID, UserID FROM posts_mst WHERE " +
                         "(post_heading = @post_heading AND " +
                         "post_text = @post_text AND " +
                         "post_image = @post_image AND " +
                         "dated = @dated)";

        SqlCommand command = new SqlCommand(cmdText, new SqlConnection(Commons.GetConnectionStringFor_user_creds));

        command.Parameters.Add("@post_heading", SqlDbType.NVarChar).Value = postHeading_Label.Text.Trim();
        command.Parameters.Add("@post_text", SqlDbType.NVarChar).Value = postText_Label.Text.Trim();
        command.Parameters.Add("@post_image", SqlDbType.VarChar).Value = postImage_Image.ImageUrl.Trim();
        command.Parameters.Add("@dated", SqlDbType.DateTime).Value = postTime_Label.Text.Substring(11);

        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = command;

        DataTable dt_postID_PostUserID = new DataTable();
        sda.Fill(dt_postID_PostUserID);

        cmdText = "SELECT count(liked_by_UserID) AS TotalLikes FROM posts_likes WHERE " +
                  "(PostID = " + dt_postID_PostUserID.Rows[0]["PostID"].ToString() + ")";
        DataTable dt_CheckAlreadyLikedCount = SQLHelper.FillDataTable(cmdText);

        if (long.Parse(dt_CheckAlreadyLikedCount.Rows[0]["TotalLikes"].ToString()) > 0)
        {
            cmdText = "SELECT PostID, liked_by_UserID FROM posts_likes WHERE " +
                      "(PostID = " + dt_postID_PostUserID.Rows[0]["PostID"].ToString() + " AND " +
                      "liked_by_UserID = " + Session["UserID"].ToString() + ")";
            DataTable dt_checkCurrentLiked = SQLHelper.FillDataTable(cmdText);

            if (dt_checkCurrentLiked.Rows.Count > 0)
                like_LinkBtn.Text = "<i class='fa fa-heart' aria-hidden='true' style='margin-right: 5px;'></i> " + "Unlike (" +
                                long.Parse(dt_CheckAlreadyLikedCount.Rows[0]["TotalLikes"].ToString()) + ")";
            else
                like_LinkBtn.Text = "<i class='fa fa-heart' aria-hidden='true' style='margin-right: 5px;'></i> " + "Like (" +
                                    long.Parse(dt_CheckAlreadyLikedCount.Rows[0]["TotalLikes"].ToString()) + ")";
        }   // 'if (long.Parse(dt_CheckAlreadyLikedCount.Rows[0]["TotalLikes"].ToString()) > 0)' closed.
        else
            like_LinkBtn.Text = "<i class='fa fa-heart' aria-hidden='true' style='margin-right: 5px;'></i> " + "Like";
    }   // Method 'LoadLikesCount(DataListItemEventArgs e)' closed.

    // Method to load total number of comment(s)
    private void LoadCommentsCount(DataListItemEventArgs e)
    {
        // Get post details
        LinkButton otherUserName_LinkBtn = (LinkButton)e.Item.FindControl("otherUserName_LinkBtn");
        LinkButton comment_LinkBtn = (LinkButton)e.Item.FindControl("comment_LinkBtn");

        Label postTime_Label = (Label)e.Item.FindControl("postTime_Label");
        Label postHeading_Label = (Label)e.Item.FindControl("postHeading_Label");
        Label postText_Label = (Label)e.Item.FindControl("postText_Label");

        Image postImage_Image = (Image)e.Item.FindControl("postImage_Image");

        // Check if already liked 
        string cmdText = "SELECT PostID, UserID FROM posts_mst WHERE " +
                         "(post_heading = @post_heading AND " +
                         "post_text = @post_text AND " +
                         "post_image = @post_image AND " +
                         "dated = @dated)";

        SqlCommand command = new SqlCommand(cmdText, new SqlConnection(Commons.GetConnectionStringFor_user_creds));

        command.Parameters.Add("@post_heading", SqlDbType.NVarChar).Value = postHeading_Label.Text.Trim();
        command.Parameters.Add("@post_text", SqlDbType.NVarChar).Value = postText_Label.Text.Trim();
        command.Parameters.Add("@post_image", SqlDbType.VarChar).Value = postImage_Image.ImageUrl.Trim();
        command.Parameters.Add("@dated", SqlDbType.DateTime).Value = postTime_Label.Text.Substring(11);

        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = command;

        DataTable dt_postID_PostUserID = new DataTable();
        sda.Fill(dt_postID_PostUserID);

        // Load total number of comments
        cmdText = "SELECT count(commenter_UserID) AS TotalComments FROM posts_comments WHERE " +
                         "(PostID = " + dt_postID_PostUserID.Rows[0]["PostID"].ToString() + ")";
        DataTable dt_CheckAlreadyCommentCount = SQLHelper.FillDataTable(cmdText);

        if (long.Parse(dt_CheckAlreadyCommentCount.Rows[0]["TotalComments"].ToString()) > 0)
        {
            comment_LinkBtn.Text = "<i class='fa fa-comments' aria-hidden='true' style='margin-right: 5px;'></i> " + "Comment (" +
                                long.Parse(dt_CheckAlreadyCommentCount.Rows[0]["TotalComments"].ToString()) + ")";
            return;
        }   // 'if (... > 0)' closed.
        comment_LinkBtn.Text = "<i class='fa fa-comments' aria-hidden='true' style='margin-right: 5px;'></i> " + "Comment";
    }   // Method 'LoadCommentsCount(DataListItemEventArgs e)' closed.

    // Method to show existing comments on post
    private void ShowComments(DataListCommandEventArgs e)
    {
        // Get post details
        Label postTime_Label = (Label)e.Item.FindControl("postTime_Label");
        Label postHeading_Label = (Label)e.Item.FindControl("postHeading_Label");
        Label postText_Label = (Label)e.Item.FindControl("postText_Label");

        Image postImage_Image = (Image)e.Item.FindControl("postImage_Image");

        // Check if already liked 
        string cmdText = "SELECT PostID, UserID FROM posts_mst WHERE " +
                         "(post_heading = @post_heading AND " +
                         "post_text = @post_text AND " +
                         "post_image = @post_image AND " +
                         "dated = @dated)";

        SqlCommand command = new SqlCommand(cmdText, new SqlConnection(Commons.GetConnectionStringFor_user_creds));

        command.Parameters.Add("@post_heading", SqlDbType.NVarChar).Value = postHeading_Label.Text.Trim();
        command.Parameters.Add("@post_text", SqlDbType.NVarChar).Value = postText_Label.Text.Trim();
        command.Parameters.Add("@post_image", SqlDbType.VarChar).Value = postImage_Image.ImageUrl.Trim();
        command.Parameters.Add("@dated", SqlDbType.DateTime).Value = postTime_Label.Text.Substring(11);

        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = command;

        DataTable dt_postID_PostUserID = new DataTable();
        sda.Fill(dt_postID_PostUserID);

        cmdText = "SELECT posts_comments.PostID, posts_comments.commenter_UserID, posts_comments.comment_text, " +
                  "CAST(posts_comments.dated AS DATE) AS comment_date, posts_comments.dated AS comment_datetime, " +
                  "user_creds.firstname AS commenter_fname, user_creds.lastname AS commenter_lname, " +
                  "user_profile.photo AS commenter_photo " +
                  "FROM posts_comments LEFT JOIN user_creds ON (posts_comments.commenter_UserID = user_creds.UserID) LEFT JOIN " +
                  "user_profile ON (posts_comments.commenter_UserID = user_profile.UserID) WHERE " +
                  "(posts_comments.PostID = " + dt_postID_PostUserID.Rows[0]["PostID"].ToString() + ") " +
                  "ORDER BY comment_date ASC, dated ASC";

        DataTable dt_Comments = SQLHelper.FillDataTable(cmdText);

        DataList showComments_DataList = e.Item.FindControl("showComments_DataList") as DataList;
        showComments_DataList.DataSource = dt_Comments;
        showComments_DataList.DataBind();
    }   // Method 'LoadCommentsCount(DataListItemEventArgs e)' closed.

    // Method to show profile of clicked name of poster
    private void ShowProfile(DataListCommandEventArgs e)
    {
        // Get post details
        LinkButton otherUserName_LinkBtn = (LinkButton)e.Item.FindControl("otherUserName_LinkBtn");
        LinkButton comment_LinkBtn = (LinkButton)e.Item.FindControl("comment_LinkBtn");

        Label postTime_Label = (Label)e.Item.FindControl("postTime_Label");
        Label postHeading_Label = (Label)e.Item.FindControl("postHeading_Label");
        Label postText_Label = (Label)e.Item.FindControl("postText_Label");

        Image postImage_Image = (Image)e.Item.FindControl("postImage_Image");

        string cmdText = "SELECT PostID, UserID FROM posts_mst WHERE " +
                         "(post_heading = @post_heading AND " +
                         "post_text = @post_text AND " +
                         "post_image = @post_image AND " +
                         "dated = @dated)";

        SqlCommand commandSelect = new SqlCommand(cmdText, new SqlConnection(Commons.GetConnectionStringFor_user_creds));

        commandSelect.Parameters.Add("@post_heading", SqlDbType.NVarChar).Value = postHeading_Label.Text.Trim();
        commandSelect.Parameters.Add("@post_text", SqlDbType.NVarChar).Value = postText_Label.Text.Trim();
        commandSelect.Parameters.Add("@post_image", SqlDbType.VarChar).Value = postImage_Image.ImageUrl.Trim();
        commandSelect.Parameters.Add("@dated", SqlDbType.DateTime).Value = postTime_Label.Text.Substring(11);

        SqlDataAdapter sqlDataAdapter_Select = new SqlDataAdapter();
        sqlDataAdapter_Select.SelectCommand = commandSelect;

        DataTable dt_postID_PostUserID = new DataTable();
        sqlDataAdapter_Select.Fill(dt_postID_PostUserID);

        // Check whether name poster and current user are same, if yes then redirect to current user's profile page
        if (dt_postID_PostUserID.Rows[0]["UserID"].ToString() == Session["UserID"].ToString())
            Response.Redirect("~/Profile/Profile.aspx");
        else
        {
            // Otherwise redirect to profile page shown in friends list
            Response.Cookies["otherUserID"].Value = dt_postID_PostUserID.Rows[0]["UserID"].ToString();
            Response.Redirect("~/FriendsList/FriendsListProfile.aspx");
        }
    }   // Method 'ShowProfile(DataListCommandEventArgs e)' closed.

    #endregion


    #region 'showComments_DataList' Events
    protected void showComments_DataList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "removeComment_LinkButton_CommandName":
                RemoveComment(e);       // Remove the comment
                LoadPosts();            // Load all posts again to reflect comment count around 'comment_LinkBtn'
                break;
        }
    }   // Method 'showComments_DataList_ItemCommand(object source, DataListCommandEventArgs e)' closed.

    protected void showComments_DataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            // Get incrementally single row at a time from 'showComments_DataList'
            DataRowView drv_comment = e.Item.DataItem as DataRowView;

            // Get post owner details
            string cmdText = "SELECT PostID, UserID, post_heading, post_text, post_image, dated FROM posts_mst WHERE " +
                             "(PostID = " + drv_comment.Row["PostID"].ToString() + ")";

            DataTable dt_postUserID = SQLHelper.FillDataTable(cmdText);

            // Check if commenter does not have any profile set then show default image
            if (drv_comment.Row["commenter_photo"].ToString().Trim() == "")
                ((Image)e.Item.FindControl("otherUserProfileComments_Image")).ImageUrl = "~/Images/no_image.jpg";

            // Check if commenter is post owner or not
            if (drv_comment.Row["commenter_UserID"].ToString().Trim() == dt_postUserID.Rows[0]["UserID"].ToString())
            {
                ((Label)e.Item.FindControl("postCommenterStatus_Label")).Text = "(Post author 👑)";
                ((Label)e.Item.FindControl("postCommenterStatus_Label")).Visible = true;
            }
            else if (drv_comment.Row["commenter_UserID"].ToString().Trim() != dt_postUserID.Rows[0]["UserID"].ToString())
            {
                ((Label)e.Item.FindControl("postCommenterStatus_Label")).Text = "";
                ((Label)e.Item.FindControl("postCommenterStatus_Label")).Visible = false;
            }

            // Check if commenter is post author or not to enable & show 'removeComment_LinkButton'
            if (drv_comment.Row["commenter_UserID"].ToString().Trim() != Session["UserID"].ToString())
                ((LinkButton)e.Item.FindControl("removeComment_LinkButton")).Enabled =
                    ((LinkButton)e.Item.FindControl("removeComment_LinkButton")).Visible = false;

            else if (drv_comment.Row["commenter_UserID"].ToString().Trim() == Session["UserID"].ToString())
                ((LinkButton)e.Item.FindControl("removeComment_LinkButton")).Enabled =
                    ((LinkButton)e.Item.FindControl("removeComment_LinkButton")).Visible = true;
        }   // 'if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)' closed.

    }   // Method 'showComments_DataList_ItemDataBound(object sender, DataListItemEventArgs e)' closed.
    #endregion

}   // class 'class Home_Home' closed.

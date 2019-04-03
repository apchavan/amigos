using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ReportedPosts_ReportedPosts : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"].ToString() == "")
            Response.Redirect("~/LandingPage/LandingPage.aspx");

        if (!Page.IsPostBack)
            LoadReportedPosts();
    }   // Method 'Page_Load(object sender, EventArgs e)' closed.

    #region Methods to load, show, ignore reported posts, block either post author or reporter and remove all posts, likes, comments, reports of blocked user
    // Method to load all reported posts
    private void LoadReportedPosts()
    {
        string cmdText = "SELECT PostID, poster_UserID, reporter_UserID, report_text, dated FROM reported_posts ORDER BY dated DESC";
        DataTable dt_reported_posts = SQLHelper.FillDataTable(cmdText);

        if (dt_reported_posts.Rows.Count <= 0)
        {
            topHeading_Label.Text = "<strong> 😊 No reported posts found ... 😊<br />🥳 All is going well ! 🥳</strong>";
            reportedPosts_DataList.Visible = false;     // Hide 'reportedPosts_DataList' since no longer required
            reportedPosts_UpdatePanel.Update();         // Update the 'reportedPosts_UpdatePanel' to reflect new changes
            return;
        }

        topHeading_Label.Text = "<font size='4'> Total reported posts : <b><i>'" +
                                dt_reported_posts.Rows.Count + "'</i></b></font><hr />";

        reportedPosts_DataList.Visible = true;

        DataTable dt_posterReporterDetails = new DataTable();
        dt_posterReporterDetails.Columns.Add("report_dated");
        dt_posterReporterDetails.Columns.Add("poster_fname");
        dt_posterReporterDetails.Columns.Add("poster_lname");
        dt_posterReporterDetails.Columns.Add("reporter_fname");
        dt_posterReporterDetails.Columns.Add("reporter_lname");
        dt_posterReporterDetails.Columns.Add("report_text");

        for (int i = 0; i < dt_reported_posts.Rows.Count; i++)
        {
            cmdText = "SELECT UserID, firstname, lastname FROM user_creds " +
                      "WHERE (UserID = " + dt_reported_posts.Rows[i]["poster_UserID"].ToString() + ")";
            DataTable dt_posterName = SQLHelper.FillDataTable(cmdText);

            cmdText = "SELECT UserID, firstname, lastname FROM user_creds " +
                      "WHERE (UserID = " + dt_reported_posts.Rows[i]["reporter_UserID"].ToString() + ")";
            DataTable dt_reporterName = SQLHelper.FillDataTable(cmdText);

            string report_text = dt_reported_posts.Rows[i]["report_text"].ToString().Trim();
            if (report_text.Trim() == "")
                report_text = "None";

            dt_posterReporterDetails.Rows.Add(
                                              dt_reported_posts.Rows[i]["dated"].ToString(),
                                              dt_posterName.Rows[0]["firstname"].ToString(),
                                              dt_posterName.Rows[0]["lastname"].ToString(),
                                              dt_reporterName.Rows[0]["firstname"].ToString(),
                                              dt_reporterName.Rows[0]["lastname"].ToString(),
                                              report_text
                                              );
            dt_posterName.Reset();
            dt_reporterName.Reset();
        }   // 'for (int i = 0; i < dt_reported_posts.Rows.Count; i++)' closed.

        reportedPosts_DataList.DataSource = dt_posterReporterDetails;
        reportedPosts_DataList.DataBind();
    }   // Method 'LoadReportedPosts()' closed.

    // Method to show reported post
    private void ShowPost(DataListCommandEventArgs e)
    {
        Label reportedTime_Label = (Label)e.Item.FindControl("reportedTime_Label");
        Label reportText_Label = (Label)e.Item.FindControl("reportText_Label");

        // Get post details
        string cmdText = "SELECT PostID, poster_UserID, reporter_UserID, report_text, dated FROM reported_posts " +
                         "WHERE (dated = @dated AND " +
                         "report_text = @report_text)";

        string report_text = reportText_Label.Text.Trim();
        if (report_text == "None")
            report_text = "";

        SqlConnection connection = new SqlConnection(Commons.GetConnectionStringFor_user_creds);
        SqlCommand commandGetPostDetails = new SqlCommand(cmdText, connection);

        commandGetPostDetails.Parameters.Add("dated", SqlDbType.DateTime).Value = reportedTime_Label.Text.Trim();
        commandGetPostDetails.Parameters.Add("report_text", SqlDbType.NVarChar).Value = report_text;

        SqlDataAdapter sda_getPostDetails = new SqlDataAdapter();
        sda_getPostDetails.SelectCommand = commandGetPostDetails;

        DataTable dt_getPostDetails = new DataTable();
        sda_getPostDetails.Fill(dt_getPostDetails);

        cmdText = "SELECT posts_mst.PostID, posts_mst.UserID, posts_mst.post_heading, posts_mst.post_text, posts_mst.post_image, " +
                  "posts_mst.dated, user_creds.firstname, user_creds.lastname, user_profile.photo FROM posts_mst LEFT JOIN " +
                  "user_creds ON (posts_mst.UserID = user_creds.UserID) LEFT JOIN " +
                  "user_profile ON (posts_mst.UserID = user_profile.UserID) " +
                  "WHERE (posts_mst.PostID = " + dt_getPostDetails.Rows[0]["PostID"].ToString() + " AND " +
                  "posts_mst.UserID = " + dt_getPostDetails.Rows[0]["poster_UserID"].ToString() + ")";

        DataTable dt_showPostDetails = SQLHelper.FillDataTable(cmdText);


        DataList showPost_DataList = e.Item.FindControl("showPost_DataList") as DataList;
        showPost_DataList.DataSource = dt_showPostDetails;
        showPost_DataList.DataBind();

    }   // Method 'ShowPost(DataListCommandEventArgs e)' closed.

    // Method to ignore reported post
    private void IgnoreReportedPost(DataListCommandEventArgs e)
    {
        Label reportedTime_Label = (Label)e.Item.FindControl("reportedTime_Label");
        //Label postAuthorName_Label = (Label)e.Item.FindControl("postAuthorName_Label");
        //Label reportedByName_Label = (Label)e.Item.FindControl("reportedByName_Label");
        Label reportText_Label = (Label)e.Item.FindControl("reportText_Label");

        // Get post details
        string cmdText = "SELECT ReportID, PostID, poster_UserID, reporter_UserID, report_text, dated FROM reported_posts " +
                         "WHERE (dated = @dated AND " +
                         "report_text = @report_text)";

        string report_text = reportText_Label.Text.Trim();
        if (report_text == "None")
            report_text = "";

        SqlConnection connection = new SqlConnection(Commons.GetConnectionStringFor_user_creds);
        SqlCommand commandGetReportedPostDetails = new SqlCommand(cmdText, connection);

        commandGetReportedPostDetails.Parameters.Add("dated", SqlDbType.DateTime).Value = reportedTime_Label.Text.Trim();
        commandGetReportedPostDetails.Parameters.Add("report_text", SqlDbType.NVarChar).Value = report_text;

        SqlDataAdapter sda_getReportedPostDetails = new SqlDataAdapter();
        sda_getReportedPostDetails.SelectCommand = commandGetReportedPostDetails;

        DataTable dt_getReportedPostDetails = new DataTable();
        sda_getReportedPostDetails.Fill(dt_getReportedPostDetails);

        // Delete report from database to ignore as reported post
        cmdText = "DELETE FROM reported_posts WHERE (ReportID = " + dt_getReportedPostDetails.Rows[0]["ReportID"].ToString() + ")";
        SQLHelper.ExecuteNonQuery(cmdText);
    }   // Method 'IgnoreReportedPost(DataListCommandEventArgs e)' closed.

    // Method to block post author
    private void BlockPostAuthor(DataListCommandEventArgs e)
    {
        Label reportedTime_Label = (Label)e.Item.FindControl("reportedTime_Label");
        Label reportText_Label = (Label)e.Item.FindControl("reportText_Label");

        // Get post details
        string cmdText = "SELECT ReportID, PostID, poster_UserID, reporter_UserID, report_text, dated FROM reported_posts " +
                         "WHERE (dated = @dated AND " +
                         "report_text = @report_text)";

        string report_text = reportText_Label.Text.Trim();
        if (report_text == "None")
            report_text = "";

        SqlConnection connection = new SqlConnection(Commons.GetConnectionStringFor_user_creds);
        SqlCommand commandGetPostDetails = new SqlCommand(cmdText, connection);

        commandGetPostDetails.Parameters.Add("dated", SqlDbType.DateTime).Value = reportedTime_Label.Text.Trim();
        commandGetPostDetails.Parameters.Add("report_text", SqlDbType.NVarChar).Value = report_text;

        SqlDataAdapter sda_getPostDetails = new SqlDataAdapter();
        sda_getPostDetails.SelectCommand = commandGetPostDetails;

        DataTable dt_getPostDetails = new DataTable();
        sda_getPostDetails.Fill(dt_getPostDetails);

        // Remove post author related all stuff
        RemovePostsLikesCommentsReportsOfBlockedUser(dt_getPostDetails.Rows[0]["poster_UserID"].ToString());

        // Block post author
        cmdText = "UPDATE user_creds SET active = 0 WHERE (UserID = " + dt_getPostDetails.Rows[0]["poster_UserID"].ToString() + ")";
        SQLHelper.ExecuteNonQuery(cmdText);

        // Remove Report
        cmdText = "DELETE FROM reported_posts WHERE (ReportID = " + dt_getPostDetails.Rows[0]["ReportID"].ToString() + ")";
        SQLHelper.ExecuteNonQuery(cmdText);
    }   // Method 'BlockPostAuthor(DataListCommandEventArgs e)' closed.

    // Method to block reporter
    private void BlockReporter(DataListCommandEventArgs e)
    {
        Label reportedTime_Label = (Label)e.Item.FindControl("reportedTime_Label");
        Label reportText_Label = (Label)e.Item.FindControl("reportText_Label");

        // Get post details
        string cmdText = "SELECT ReportID, PostID, poster_UserID, reporter_UserID, report_text, dated FROM reported_posts " +
                         "WHERE (dated = @dated AND " +
                         "report_text = @report_text)";

        string report_text = reportText_Label.Text.Trim();
        if (report_text == "None")
            report_text = "";

        SqlConnection connection = new SqlConnection(Commons.GetConnectionStringFor_user_creds);
        SqlCommand commandGetPostDetails = new SqlCommand(cmdText, connection);

        commandGetPostDetails.Parameters.Add("dated", SqlDbType.DateTime).Value = reportedTime_Label.Text.Trim();
        commandGetPostDetails.Parameters.Add("report_text", SqlDbType.NVarChar).Value = report_text;

        SqlDataAdapter sda_getPostDetails = new SqlDataAdapter();
        sda_getPostDetails.SelectCommand = commandGetPostDetails;

        DataTable dt_getPostDetails = new DataTable();
        sda_getPostDetails.Fill(dt_getPostDetails);

        // Remove post reporter related all stuff
        RemovePostsLikesCommentsReportsOfBlockedUser(dt_getPostDetails.Rows[0]["reporter_UserID"].ToString());

        // Block post reporter
        cmdText = "UPDATE user_creds SET active = 0 WHERE (UserID = " + dt_getPostDetails.Rows[0]["reporter_UserID"].ToString() + ")";
        SQLHelper.ExecuteNonQuery(cmdText);

        // Remove Report
        cmdText = "DELETE FROM reported_posts WHERE (ReportID = " + dt_getPostDetails.Rows[0]["ReportID"].ToString() + ")";
        SQLHelper.ExecuteNonQuery(cmdText);
    }   // Method 'BlockReporter(DataListCommandEventArgs e)' closed.

    // Method to remove posts, likes, comments & reports of blocked user
    private void RemovePostsLikesCommentsReportsOfBlockedUser(string UserID)
    {
        // Remove all chat messages from 'user_chat' table
        string cmdText = "DELETE FROM user_chat WHERE " +
                         "(sender_UserID = " + UserID + " OR receiver_UserID = " + UserID + ")";
        SQLHelper.ExecuteNonQuery(cmdText);

        // Remove user as friend from 'friends' table
        cmdText = "DELETE FROM friends WHERE " +
                         "(from_UserID = " + UserID + " OR to_UserID = " + UserID + ")";
        SQLHelper.ExecuteNonQuery(cmdText);

        // Remove user's all comments for any post from 'posts_comments' table
        cmdText = "DELETE FROM posts_comments WHERE " +
                  "(commenter_UserID = " + UserID + ")";
        SQLHelper.ExecuteNonQuery(cmdText);

        // Remove user's all likes for any post from 'posts_likes' table
        cmdText = "DELETE FROM posts_likes WHERE " +
                  "(liked_by_UserID = " + UserID + ")";
        SQLHelper.ExecuteNonQuery(cmdText);

        // Get user's PostID(s) from 'posts_mst' table for deleting all comments & likes associated with user's own post
        cmdText = "SELECT PostID, UserID, post_image from posts_mst WHERE " +
                  "(UserID = " + UserID + ")";
        DataTable dt_userPostDetails = SQLHelper.FillDataTable(cmdText);

        // Remove all comments, likes, saved post_image for all post(s) using 'PostID' for this user
        for (int i = 0; i < dt_userPostDetails.Rows.Count; i++)
        {
            // Remove all comments for current user's post from 'posts_comments' table
            cmdText = "DELETE FROM posts_comments WHERE (PostID = " + dt_userPostDetails.Rows[i]["PostID"].ToString() + ")";
            SQLHelper.ExecuteNonQuery(cmdText);

            // Remove all likes for current user's post from 'posts_comments' table
            cmdText = "DELETE FROM posts_likes WHERE (PostID = " + dt_userPostDetails.Rows[i]["PostID"].ToString() + ")";
            SQLHelper.ExecuteNonQuery(cmdText);

            // Remove saved image of post if available
            string post_image = dt_userPostDetails.Rows[i]["post_image"].ToString().Trim();

            if (post_image != "NoImage")
            {
                // Get image name using 'Split()'
                string[] separatedImagePath = post_image.Split(new[] { "/" }, StringSplitOptions.None);
                try
                {
                    // Delete image
                    System.IO.File.Delete(Server.MapPath("~/User_uploads/") + dt_userPostDetails.Rows[i]["UserID"].ToString() +
                                          "/" + separatedImagePath[3]);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }   // 'if (post_image != "")' closed.
        }   // 'for (int i = 0; i < dt_userPostDetails.Rows.Count; i++)' closed.

        // Remove user's all posts from 'posts_mst' table
        cmdText = "DELETE FROM posts_mst WHERE " +
                  "(UserID = " + UserID + ")";
        SQLHelper.ExecuteNonQuery(cmdText);
    }   // Method 'RemovePostsLikesCommentsReportsOfBlockedUser(string UserID)' closed.
    #endregion

    #region Methods for load counts for like(s), comment(s) & report(s)
    // Method to load total number of like(s)
    private void LoadLikesCount(DataListItemEventArgs e, string PostID)
    {
        Label likesTotal_Label = (Label)e.Item.FindControl("likesTotal_Label");

        string cmdText = "SELECT count(liked_by_UserID) AS TotalLikes FROM posts_likes WHERE " +
                         "(PostID = " + PostID + ")";
        DataTable dt_CheckAlreadyLikedCount = SQLHelper.FillDataTable(cmdText);

        if (long.Parse(dt_CheckAlreadyLikedCount.Rows[0]["TotalLikes"].ToString()) > 0)
            likesTotal_Label.Text = "<i class='fa fa-heart' aria-hidden='true' style='margin-right: 5px;'></i> " + " (" +
                                long.Parse(dt_CheckAlreadyLikedCount.Rows[0]["TotalLikes"].ToString()) + ")";
        else
            likesTotal_Label.Text = "<i class='fa fa-heart' aria-hidden='true' style='margin-right: 5px;'></i> " + "";
    }   // Method 'LoadLikesCount(DataListItemEventArgs e, object PostID)' closed.

    // Method to load total number of comment(s)
    private void LoadCommentsCount(DataListItemEventArgs e, string PostID)
    {
        Label commentsTotal_Label = (Label)e.Item.FindControl("commentsTotal_Label");

        string cmdText = "SELECT count(commenter_UserID) AS TotalComments FROM posts_comments WHERE " +
                         "(PostID = " + PostID + ")";
        DataTable dt_CheckAlreadyCommentCount = SQLHelper.FillDataTable(cmdText);

        if (long.Parse(dt_CheckAlreadyCommentCount.Rows[0]["TotalComments"].ToString()) > 0)
        {
            commentsTotal_Label.Text = "<i class='fa fa-comments' aria-hidden='true' style='margin-right: 5px;'></i> " + " (" +
                                long.Parse(dt_CheckAlreadyCommentCount.Rows[0]["TotalComments"].ToString()) + ")";
            return;
        }   // 'if (... > 0)' closed.
        commentsTotal_Label.Text = "<i class='fa fa-comments' aria-hidden='true' style='margin-right: 5px;'></i> " + "";
    }   // Method 'LoadCommentsCount(DataListItemEventArgs e, string PostID)' closed.

    // Method to load total number of report for current post
    private void LoadReportedCount(DataListItemEventArgs e, string PostID)
    {
        Label reportedTotal_Label = (Label)e.Item.FindControl("reportedTotal_Label");

        string cmdText = "SELECT count(reporter_UserID) AS TotalReports FROM reported_posts WHERE " +
                         "(PostID = " + PostID + ")";
        DataTable dt_CheckAlreadyReportedCount = SQLHelper.FillDataTable(cmdText);

        if (long.Parse(dt_CheckAlreadyReportedCount.Rows[0]["TotalReports"].ToString()) > 0)
        {
            reportedTotal_Label.Text = "<i class='fa fa-flag' aria-hidden='true' style='margin-right: 5px;'></i> " + " (" +
                                long.Parse(dt_CheckAlreadyReportedCount.Rows[0]["TotalReports"].ToString()) + ")";
            return;
        }   // 'if (long.Parse(dt_CheckAlreadyReportedCount.Rows[0]["TotalReports"].ToString()) > 0)' closed.

        reportedTotal_Label.Text = "<i class='fa fa-flag' aria-hidden='true' style='margin-right: 5px;'></i> " + "";
    }   // Method 'LoadReportedCount(DataListItemEventArgs e, string PostID)' closed.

    #endregion

    #region 'reportedPosts_DataList' Events
    protected void reportedPosts_DataList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "showPost_LinkBtn_CommandName":

                ((Panel)e.Item.FindControl("confirmBlockPostAuthor_Panel")).Visible =
                    ((Panel)e.Item.FindControl("confirmBlockReporter_Panel")).Visible =
                    ((Panel)e.Item.FindControl("confirmIgnoreReport_Panel")).Visible = false;

                Panel showPost_Panel = (Panel)e.Item.FindControl("showPost_Panel");

                if (showPost_Panel.Visible)
                {
                    showPost_Panel.Visible = false;     //Hide 'showPost_Panel'
                    ((LinkButton)e.Item.FindControl("showPost_LinkBtn")).Text =
                        "<i class='fa fa-eye' aria-hidden='true' style='margin-right: 5px;'></i> " + "Show post";
                }

                else
                {
                    ((LinkButton)e.Item.FindControl("showPost_LinkBtn")).Text =
                        "<i class='fa fa-eye-slash' aria-hidden='true' style='margin-right: 5px;'></i> " + "Hide post";
                    showPost_Panel.Visible = true;      // Show the 'showPost_Panel'
                    ShowPost(e);                        // Show the post that is reported
                }
                break;

            case "blockPostAuthor_LinkBtn_CommandName":

                ((Panel)e.Item.FindControl("showPost_Panel")).Visible =
                    ((Panel)e.Item.FindControl("confirmBlockReporter_Panel")).Visible =
                    ((Panel)e.Item.FindControl("confirmIgnoreReport_Panel")).Visible = false;

                ((LinkButton)e.Item.FindControl("showPost_LinkBtn")).Text =
                    "<i class='fa fa-eye' aria-hidden='true' style='margin-right: 5px;'></i> " + "Show post";

                Panel confirmBlockPostAuthor_Panel = (Panel)e.Item.FindControl("confirmBlockPostAuthor_Panel");
                if (confirmBlockPostAuthor_Panel.Visible)
                    confirmBlockPostAuthor_Panel.Visible = false;
                else
                {
                    confirmBlockPostAuthor_Panel.Visible = true;
                    ((Label)e.Item.FindControl("blockPostAuthor_Label")).Text = "<strong>Confirm block post author ? 🤔</strong>"
                                                                              + "<br /> (Block "
                                                                              + ((Label)e.Item.FindControl("postAuthorName_Label")).Text
                                                                              + ")";
                }   // 'else' closed.

                break;

            case "blockReporter_LinkBtn_CommandName":

                ((Panel)e.Item.FindControl("showPost_Panel")).Visible =
                    ((Panel)e.Item.FindControl("confirmBlockPostAuthor_Panel")).Visible =
                    ((Panel)e.Item.FindControl("confirmIgnoreReport_Panel")).Visible = false;

                ((LinkButton)e.Item.FindControl("showPost_LinkBtn")).Text =
                    "<i class='fa fa-eye' aria-hidden='true' style='margin-right: 5px;'></i> " + "Show post";

                Panel confirmBlockReporter_Panel = (Panel)e.Item.FindControl("confirmBlockReporter_Panel");
                if (confirmBlockReporter_Panel.Visible)
                    confirmBlockReporter_Panel.Visible = false;
                else
                {
                    confirmBlockReporter_Panel.Visible = true;
                    ((Label)e.Item.FindControl("blockReporter_Label")).Text = "<strong>Confirm block reporter ? 🤔</strong>"
                                                                              + "<br /> (Block "
                                                                              + ((Label)e.Item.FindControl("reportedByName_Label")).Text
                                                                              + ")";
                }   // 'else' closed.

                break;

            case "ignore_LinkBtn_CommandName":

                ((Panel)e.Item.FindControl("showPost_Panel")).Visible =
                    ((Panel)e.Item.FindControl("confirmBlockPostAuthor_Panel")).Visible =
                    ((Panel)e.Item.FindControl("confirmBlockReporter_Panel")).Visible = false;

                ((LinkButton)e.Item.FindControl("showPost_LinkBtn")).Text =
                    "<i class='fa fa-eye' aria-hidden='true' style='margin-right: 5px;'></i> " + "Show post";

                Panel confirmIgnoreReport_Panel = (Panel)e.Item.FindControl("confirmIgnoreReport_Panel");
                if (confirmIgnoreReport_Panel.Visible)
                    confirmIgnoreReport_Panel.Visible = false;
                else
                {
                    confirmIgnoreReport_Panel.Visible = true;
                    ((Label)e.Item.FindControl("ignoreReport_Label")).Text = "<strong>Confirm ignore reported post ? 🤔</strong>";
                }   // 'else' closed.

                break;

            case "cancelShowPost_Button_CommandName":
                ((Panel)e.Item.FindControl("showPost_Panel")).Visible = false;
                ((LinkButton)e.Item.FindControl("showPost_LinkBtn")).Text =
                    "<i class='fa fa-eye' aria-hidden='true' style='margin-right: 5px;'></i> " + "Show post";
                break;

            case "cancelBlockPostAuthor_Button_CommandName":
                ((Panel)e.Item.FindControl("confirmBlockPostAuthor_Panel")).Visible = false;
                break;

            case "yesBlockPostAuthor_Button_CommandName":
                BlockPostAuthor(e);         // Block post author
                LoadReportedPosts();        // Load reported posts to refresh the list
                break;

            case "cancelBlockReporter_Button_CommandName":
                ((Panel)e.Item.FindControl("confirmBlockReporter_Panel")).Visible = false;
                break;

            case "yesBlockReporter_Button_CommandName":
                BlockReporter(e);           // Block post reporter
                LoadReportedPosts();        // Load reported posts to refresh the list
                break;

            case "cancelIgnoreReport_Button_CommandName":
                ((Panel)e.Item.FindControl("confirmIgnoreReport_Panel")).Visible = false;
                break;

            case "yesIgnoreReport_Button_CommandName":
                IgnoreReportedPost(e);      // Ignore reported post
                LoadReportedPosts();        // Load reported posts to refresh the list
                break;
        }
    }   // Method 'reportedPosts_DataList_ItemCommand(object source, DataListCommandEventArgs e)' closed.

    protected void reportedPosts_DataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ((LinkButton)e.Item.FindControl("showPost_LinkBtn")).Text = "<i class='fa fa-eye' aria-hidden='true' style='margin-right: 5px;'></i> " + "Show post";
            ((LinkButton)e.Item.FindControl("blockPostAuthor_LinkBtn")).Text = "<i class='fa fa-ban' aria-hidden='true' style='margin-right: 5px;'></i> " + "Block post author";
            ((LinkButton)e.Item.FindControl("blockReporter_LinkBtn")).Text = "<i class='fa fa-ban' aria-hidden='true' style='margin-right: 5px;'></i> " + "Block reporter";
            ((LinkButton)e.Item.FindControl("ignore_LinkBtn")).Text = "<i class='fa fa-hand-peace' aria-hidden='true' style='margin-right: 5px;'></i> " + "Ignore";
        }
    }   // Method 'reportedPosts_DataList_ItemDataBound(object sender, DataListItemEventArgs e)' closed.

    #endregion

    #region 'showPost_DataList' Events
    protected void showPost_DataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView drv_showPostDetails = e.Item.DataItem as DataRowView;

            // Set default profile picture if no profile picture is set
            if (drv_showPostDetails.Row["photo"].ToString().Trim() == "" ||
                drv_showPostDetails.Row["photo"].ToString().Trim() == "~/Images/no_image.jpg")
                ((Image)e.Item.FindControl("otherUserProfile_Image")).ImageUrl = "~/Images/no_image.jpg";

            if (drv_showPostDetails.Row["post_image"].ToString().Trim() == "" ||
               drv_showPostDetails.Row["post_image"].ToString().Trim() == "NoImage")
                ((Image)e.Item.FindControl("postImage_Image")).Visible = false;

            // Load total existing likes count
            LoadLikesCount(e, drv_showPostDetails.Row["PostID"].ToString());

            // Load total existing comments count
            LoadCommentsCount(e, drv_showPostDetails.Row["PostID"].ToString());

            // Load total existing reported count
            LoadReportedCount(e, drv_showPostDetails.Row["PostID"].ToString());
        }
    }   // Method 'showPost_DataList_ItemDataBound(object sender, DataListItemEventArgs e)' closed.

    #endregion
}   // class 'class ReportedPosts_ReportedPosts' closed.
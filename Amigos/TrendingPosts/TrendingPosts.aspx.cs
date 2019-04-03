using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TrendingPosts_TrendingPosts : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"].ToString() == "")
            Response.Redirect("~/LandingPage/LandingPage.aspx");

        if (!Page.IsPostBack)
            LoadTrendingPosts();
    }

    // Method to load all trending posts
    private void LoadTrendingPosts()
    {
        // Get most liked posts in descending order (most liked on top & so on)

        /*
        string cmdText = "SELECT pm.PostID, count(pl.PostID) AS TotalLikes " + 
                         "FROM posts_mst pm LEFT JOIN posts_likes pl " + 
                         "ON pm.PostID = pl.PostID GROUP BY pm.PostID ORDER BY TotalLikes DESC, posts_mst.PostID DESC";
         */
        // OR use query below

        string cmdText = "SELECT posts_mst.PostID, count(posts_likes.PostID) AS TotalLikes " +
                         "FROM posts_mst " +
                         "LEFT JOIN posts_likes " +
                         "ON posts_mst.PostID = posts_likes.PostID " +
                         "GROUP BY posts_mst.PostID " +
                         "ORDER BY TotalLikes DESC, posts_mst.PostID DESC";

        DataTable dt_mostPostsLikes = SQLHelper.FillDataTable(cmdText);

        // Get post details for trending posts in 'dt_trendingPosts' DataTable object below
        DataTable dt_trendingPosts = new DataTable();

        dt_trendingPosts.Columns.Add("PostID");
        dt_trendingPosts.Columns.Add("UserID");
        dt_trendingPosts.Columns.Add("firstname");
        dt_trendingPosts.Columns.Add("lastname");
        dt_trendingPosts.Columns.Add("photo");
        dt_trendingPosts.Columns.Add("post_heading");
        dt_trendingPosts.Columns.Add("post_text");
        dt_trendingPosts.Columns.Add("post_image");
        dt_trendingPosts.Columns.Add("dated");

        for (int i = 0; i < dt_mostPostsLikes.Rows.Count; i++)
        {
            //string post_image = "";
            cmdText = "SELECT posts_mst.UserID, posts_mst.post_heading, posts_mst.post_text, posts_mst.post_image, posts_mst.dated, " +
                      "user_creds.firstname, user_creds.lastname, user_profile.photo FROM posts_mst " +
                      "LEFT JOIN user_creds ON (posts_mst.UserID = user_creds.UserID) " +
                      "LEFT JOIN user_profile ON (posts_mst.UserID = user_profile.UserID) " +
                      "WHERE (posts_mst.PostID = " + dt_mostPostsLikes.Rows[i]["PostID"].ToString() + ")";

            DataTable dt_postDetails = SQLHelper.FillDataTable(cmdText);

            //if (dt_postDetails.Rows[0]["post_image"].ToString().Trim() == "")
            dt_trendingPosts.Rows.Add(
                                      dt_mostPostsLikes.Rows[i]["PostID"].ToString(),
                                      dt_postDetails.Rows[0]["UserID"].ToString(),
                                      dt_postDetails.Rows[0]["firstname"].ToString(),
                                      dt_postDetails.Rows[0]["lastname"].ToString(),
                                      dt_postDetails.Rows[0]["photo"].ToString(),
                                      dt_postDetails.Rows[0]["post_heading"].ToString(),
                                      dt_postDetails.Rows[0]["post_text"].ToString(),
                                      dt_postDetails.Rows[0]["post_image"].ToString(),
                                      dt_postDetails.Rows[0]["dated"].ToString()
                                     );
        }   // 'for (int i = 0; i < dt_mostPostsLikes.Rows.Count; i++)' closed.

        trendingPosts_DataList.DataSource = dt_trendingPosts;
        trendingPosts_DataList.DataBind();
    }   // Method 'LoadTrendingPosts()' closed.

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


    #region 'trendingPosts_DataList' Events

    protected void trendingPosts_DataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataTable dt_trendingPosts = (DataTable)trendingPosts_DataList.DataSource;

            // Check whether user shared post has an image or not, if available then show post image otherwise hide image section 
            if (dt_trendingPosts.Rows[e.Item.ItemIndex]["post_image"].ToString() == "NoImage")
                ((Image)e.Item.FindControl("postImage_Image")).Enabled =
                    ((Image)e.Item.FindControl("postImage_Image")).Visible = false;
            else
                ((Image)e.Item.FindControl("postImage_Image")).Enabled =
                    ((Image)e.Item.FindControl("postImage_Image")).Visible = true;

            Label likesTotal_Label = (Label)e.Item.FindControl("likesTotal_Label");
            Label commentsTotal_Label = (Label)e.Item.FindControl("commentsTotal_Label");
            Label reportedTotal_Label = (Label)e.Item.FindControl("reportedTotal_Label");

            likesTotal_Label.Text = "<i class='fa fa-heart' aria-hidden='true' style='margin-right: 5px;'></i> " + "Like";
            commentsTotal_Label.Text = "<i class='fa fa-comments' aria-hidden='true' style='margin-right: 5px;'></i> " + "Comment";
            reportedTotal_Label.Text = "<i class='fa fa-flag' aria-hidden='true' style='margin-right: 5px;'></i> " + "Report";

            // Load total existing likes count
            LoadLikesCount(e, dt_trendingPosts.Rows[e.Item.ItemIndex]["PostID"].ToString());

            // Load total existing comments count
            LoadCommentsCount(e, dt_trendingPosts.Rows[e.Item.ItemIndex]["PostID"].ToString());

            // Load total existing reported count
            LoadReportedCount(e, dt_trendingPosts.Rows[e.Item.ItemIndex]["PostID"].ToString());

        }   // 'if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)' closed.
    }   // Method 'trendingPosts_DataList_ItemDataBound(object sender, DataListItemEventArgs e)' closed.

    #endregion
}
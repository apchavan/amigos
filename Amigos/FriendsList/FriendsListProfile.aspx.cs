using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FriendsList_FriendsListProfile : System.Web.UI.Page
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

        if (Request.Cookies["otherUserID"].Value.Trim() == "" || Request.Cookies["otherUserID"] == null)
        {
            Response.Redirect("~/Home/Home.aspx");
            return;
        }

        if (!Page.IsPostBack)
        {
            Load_NameEmailDOB();
            Load_PhotoProfessionAt();
        }   // 'if(!Page.IsPostBack)' closed.
    }

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        Commons.ClearCookies();
    }

    protected string Get_DOB_Month_Name(string monthNoText)
    {
        switch (int.Parse(monthNoText))
        {
            case 1: return "January";
            case 2: return "February";
            case 3: return "March";
            case 4: return "April";
            case 5: return "May";
            case 6: return "June";
            case 7: return "July";
            case 8: return "August";
            case 9: return "September";
            case 10: return "October";
            case 11: return "November";
            case 12: return "December";
            default: return "";
        }
    }

    private void Load_NameEmailDOB()
    {
        try
        {
            string cmdText = "SELECT firstname, lastname, email, dob FROM user_creds WHERE (UserID = " + Request.Cookies["otherUserID"].Value + ")";
            DataTable dt_user_creds = new DataTable();
            dt_user_creds = SQLHelper.FillDataTable(cmdText);

            uname_Label.Text = dt_user_creds.Rows[0]["firstname"].ToString() + " " + dt_user_creds.Rows[0]["lastname"];
            email_Label.Text = dt_user_creds.Rows[0]["email"].ToString();

            string dob = dt_user_creds.Rows[0]["dob"].ToString();
            dob = dob.Replace("-", "");
            dob_Label.Text = dob.Substring(0, 2) + "-" + Get_DOB_Month_Name(dob.Substring(2, 2)) + "-" + dob.Substring(4, 4);

            // Set title of page
            friendsListProfile_Title.InnerHtml = "Profile : " + dt_user_creds.Rows[0]["firstname"].ToString() + " " + dt_user_creds.Rows[0]["lastname"];
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private DataTable Get_PhotoProfessionAt()
    {
        try
        {
            string cmdText = "SELECT photo, profession, at FROM user_profile WHERE (UserID = " + Request.Cookies["otherUserID"].Value + ")";
            return (SQLHelper.FillDataTable(cmdText));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void Load_PhotoProfessionAt()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = Get_PhotoProfessionAt();

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["photo"].ToString().Trim() == "")
                    profile_Image.Src = "~/Images/no_image.jpg";
                else
                    profile_Image.Src = dt.Rows[0]["photo"].ToString();

                if (dt.Rows[0]["profession"].ToString().Trim() == "")
                    profession_Label.Text = "Not provided.";
                else
                    profession_Label.Text = dt.Rows[0]["profession"].ToString();

                if (dt.Rows[0]["at"].ToString().Trim() == "")
                    at_Label.Text = "Not provided.";
                else
                    at_Label.Text = dt.Rows[0]["at"].ToString();
            }
            else
            {
                profile_Image.Src = "~/Images/no_image.jpg";
                profession_Label.Text = "Not provided.";
                at_Label.Text = "Not provided.";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void unfriend_Btn_Click(object sender, EventArgs e)
    {
        // Check if still friends or not
        string cmdText = "SELECT confirmed FROM friends WHERE " +
                         "(from_UserID = " + Session["UserID"].ToString() + " AND to_UserID = " +
                         Request.Cookies["otherUserID"].Value + ")";
        DataTable dt_confirmedStatusFrom = SQLHelper.FillDataTable(cmdText);

        cmdText = "SELECT confirmed FROM friends WHERE " +
                  "(from_UserID = " + Request.Cookies["otherUserID"].Value + " AND to_UserID = " +
                  Session["UserID"].ToString() + ")";
        DataTable dt_confirmedStatusTo = SQLHelper.FillDataTable(cmdText);

        if (dt_confirmedStatusFrom.Rows.Count == 0 && dt_confirmedStatusTo.Rows.Count == 0)
        {
            Response.Redirect("FriendsList.aspx");
            return;
        }

        // If still friends then unfriend as usual
        cmdText = "SELECT user_creds.UserID FROM user_creds LEFT JOIN user_profile ON " +
                  "user_creds.UserID = user_profile.UserID WHERE " +
                  "(user_creds.firstname = '" + uname_Label.Text.ToString().Split(' ')[0].Trim() +
                  "' AND user_creds.lastname = '" + uname_Label.Text.ToString().Split(' ')[1].Trim() + "') AND " +
                  "(user_profile.profession = '" + profession_Label.Text.ToString() + "' AND user_profile.at = '" +
                  at_Label.Text.ToString() + "')";

        DataTable dt_OtherUserIDResult = SQLHelper.FillDataTable(cmdText);

        cmdText = "DELETE FROM friends WHERE (from_UserID = " + Request.Cookies["otherUserID"].Value +
                  " AND to_UserID = " + Session["UserID"].ToString() + " AND " +
                  "confirmed = 1) OR (from_UserID = " + Session["UserID"].ToString() + 
                  " AND to_UserID = " + Request.Cookies["otherUserID"].Value + " AND confirmed = 1)";
        SQLHelper.ExecuteNonQuery(cmdText);

        Response.Redirect("FriendsList.aspx");
    }
}
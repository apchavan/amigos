using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Profile_Profile : System.Web.UI.Page
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
        {
            Load_NameEmailDOB();
            Load_PhotoProfessionAt();
        }   // 'if(!Page.IsPostBack)' closed.
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
            string cmdText = "SELECT firstname, lastname, email, dob FROM user_creds WHERE (UserID = " + Session["UserID"].ToString() + ")";
            DataTable dt = new DataTable();
            dt = SQLHelper.FillDataTable(cmdText);

            uname_Label.Text = dt.Rows[0]["firstname"].ToString() + " " + dt.Rows[0]["lastname"];
            email_Label.Text = dt.Rows[0]["email"].ToString();

            string dob = dt.Rows[0]["dob"].ToString();
            dob = dob.Replace("-", "");
            dob_Label.Text = dob.Substring(0, 2) + "-" + Get_DOB_Month_Name(dob.Substring(2, 2)) + "-" + dob.Substring(4, 4);
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
            string cmdText = "SELECT photo, profession, at FROM user_profile WHERE (UserID = " + Session["UserID"].ToString() + ")";
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


    protected void updateProfileBtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/UpdateProfile/UpdateProfile.aspx");
    }
}
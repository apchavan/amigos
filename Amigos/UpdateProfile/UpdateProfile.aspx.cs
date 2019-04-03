using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;

public partial class UpdateProfile_UpdateProfile : System.Web.UI.Page
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
            saveProfileBtn.Enabled = discardChanges_Btn.Enabled = true;
            LoadCurrentDetails();
        }
    }

    private void LoadCurrentDetails()
    {
        string cmdText = "SELECT firstname, lastname, email FROM user_creds WHERE (UserID = " + Session["UserID"].ToString() + ")";
        DataTable dt = new DataTable();

        dt = SQLHelper.FillDataTable(cmdText);

        firstName_TextBox.Text = dt.Rows[0]["firstname"].ToString();
        lastName_TextBox.Text = dt.Rows[0]["lastname"].ToString();
        email_TextBox.Text = dt.Rows[0]["email"].ToString();

        cmdText = "SELECT profession, at FROM user_profile WHERE (UserID = " + Session["UserID"].ToString() + ")";

        dt = SQLHelper.FillDataTable(cmdText);
        if (dt.Rows.Count > 0)
        {
            profession_TextBox.Text = dt.Rows[0]["profession"].ToString();
            professionAt_TextBox.Text = dt.Rows[0]["at"].ToString();
        }
    }

    private bool UpdateUserProfileDetails()
    {
        if (!ValidateUserDetails())
            return false;

        string cmdText = "SELECT firstname, lastname, email, upassword FROM user_creds WHERE (UserID = " + Session["UserID"].ToString() + ")";
        DataTable dt_user_creds = new DataTable();
        dt_user_creds = SQLHelper.FillDataTable(cmdText);

        cmdText = "SELECT photo, profession, at FROM user_profile WHERE (UserID = " + Session["UserID"].ToString() + ")";
        DataTable dt_user_profile = new DataTable();
        dt_user_profile = SQLHelper.FillDataTable(cmdText);

        string fname = "", lname = "", profession = "", professionAt = "", email = "", password = "";

        if (firstName_TextBox.Text.ToString().Trim() == "")
            fname = dt_user_creds.Rows[0]["firstname"].ToString();
        else
            fname = firstName_TextBox.Text;

        if (lastName_TextBox.Text.ToString().Trim() == "")
            lname = dt_user_creds.Rows[0]["lastname"].ToString();
        else
            lname = lastName_TextBox.Text;

        if (email_TextBox.Text.ToString().Trim() == "")
            email = dt_user_creds.Rows[0]["email"].ToString();
        else
            email = email_TextBox.Text;

        if (password_TextBox.Text.ToString().Trim() == "")
            password = dt_user_creds.Rows[0]["upassword"].ToString();
        else
            password = password_TextBox.Text;

        cmdText = "UPDATE user_creds SET firstname = '" + fname + "', lastname = '" +
            lname + "', email = '" + email + "', upassword = '" + password +
            "' WHERE (UserID = " + Session["UserID"].ToString() + ")";

        SQLHelper.ExecuteNonQuery(cmdText);
        Session["firstname"] = fname;
        Session["email"] = email;

        string userProfileImagePath = "";
        if (dt_user_profile.Rows.Count > 0)
        {
            if (profession_TextBox.Text.ToString().Trim() == "")
                profession = "";    // If 'profession' not provided, set to empty.
            else
                profession = profession_TextBox.Text;

            if (professionAt_TextBox.Text.ToString().Trim() == "")
                professionAt = "";  // If 'professionAt' not provided, set to empty.
            else
                professionAt = professionAt_TextBox.Text;

            if (profilePicture_FileUpload.HasFile)
            {
                string imageExtensionType = Path.GetExtension(profilePicture_FileUpload.FileName);

                if (imageExtensionType.ToLower() == ".jpg" ||
                    imageExtensionType.ToLower() == ".jpeg" ||
                    imageExtensionType.ToLower() == ".bmp" ||
                    imageExtensionType.ToLower() == ".png" ||
                    imageExtensionType.ToLower() == ".gif")
                {
                    string userDirectoryPath = Server.MapPath("~/User_uploads/");
                    
                    if (!Directory.Exists(userDirectoryPath + Session["UserID"].ToString()))
                        Directory.CreateDirectory(userDirectoryPath + Session["UserID"].ToString());

                    userDirectoryPath = userDirectoryPath + Session["UserID"].ToString() + "/";

                    if (File.Exists(userDirectoryPath + "profilePicture.jpg"))
                        File.Delete(userDirectoryPath + "profilePicture.jpg");
                    else if (File.Exists(userDirectoryPath + "profilePicture.jpeg"))
                        File.Delete(userDirectoryPath + "profilePicture.jpeg");
                    else if (File.Exists(userDirectoryPath + "profilePicture.bmp"))
                        File.Delete(userDirectoryPath + "profilePicture.bmp");
                    else if (File.Exists(userDirectoryPath + "profilePicture.png"))
                        File.Delete(userDirectoryPath + "profilePicture.png");
                    else if (File.Exists(userDirectoryPath + "profilePicture.gif"))
                        File.Delete(userDirectoryPath + "profilePicture.gif");

                    userProfileImagePath = "~/User_uploads/" + Session["UserID"].ToString() + "/" + "profilePicture" + imageExtensionType;
                    profilePicture_FileUpload.SaveAs(userDirectoryPath + "profilePicture" + imageExtensionType);
                }   // 'if(imageExtensionType.ToLower() ... ".gif")' closed.
                else
                {
                    Commons.ShowAlertMsg("❌ Profile picture image has INVALID format... ❌");
                    profilePicture_FileUpload.Focus();
                }   // 'else' closed.
            }   // 'if (profilePicture_FileUpload.HasFile)' closed.
            else
            {
                userProfileImagePath = dt_user_profile.Rows[0]["photo"].ToString();
            }   // 'else' closed.

            cmdText = "UPDATE user_profile SET photo = '" + userProfileImagePath + "', profession = '" +
                profession + "', at = '" + professionAt + "' WHERE (UserID = " + Session["UserID"].ToString() + ")";
        }   // 'if (dt_user_profile.Rows.Count > 0)' closed.
        else
        {
            if (profession_TextBox.Text.ToString().Trim() == "")
                profession = "Not provided.";
            else
                profession = profession_TextBox.Text;

            if (professionAt_TextBox.Text.ToString().Trim() == "")
                professionAt = "Not provided.";
            else
                professionAt = professionAt_TextBox.Text;

            if (profilePicture_FileUpload.HasFile)
            {
                string imageExtensionType = Path.GetExtension(profilePicture_FileUpload.FileName);

                if (imageExtensionType.ToLower() == ".jpg" ||
                    imageExtensionType.ToLower() == ".jpeg" ||
                    imageExtensionType.ToLower() == ".bmp" ||
                    imageExtensionType.ToLower() == ".png" ||
                    imageExtensionType.ToLower() == ".gif")
                {
                    string userDirectoryPath = Server.MapPath("~/User_uploads/");

                    if (!Directory.Exists(userDirectoryPath + Session["UserID"].ToString()))
                        Directory.CreateDirectory(userDirectoryPath + Session["UserID"].ToString());

                    userDirectoryPath = userDirectoryPath + Session["UserID"].ToString() + "/";

                    userProfileImagePath = "~/User_uploads/" + Session["UserID"].ToString() + "/" + "profilePicture" + imageExtensionType;
                    profilePicture_FileUpload.SaveAs(userDirectoryPath + "profilePicture" + imageExtensionType);
                }   // 'if(imageExtensionType.ToLower() ... ".gif")' closed.
                else
                {
                    Commons.ShowAlertMsg("❌ Profile picture image has INVALID format... ❌");
                    profilePicture_FileUpload.Focus();
                }   // 'else' closed.
            }   // 'if (profilePicture_FileUpload.HasFile)' closed.
            else
            {
                userProfileImagePath = "~/Images/no_image.jpg";
            }   // 'else' closed.

            cmdText = "INSERT INTO user_profile(UserID, photo, profession, at) " +
                    "VALUES(" + Session["UserID"].ToString() + ", '" + userProfileImagePath + "', '" + profession + "', '" + professionAt + "') ";
        }   // outer 'else' closed.
        SQLHelper.ExecuteNonQuery(cmdText);
        return true;
    }   // Method 'UpdateUserProfileDetails()' closed.

    protected void saveProfileBtn_Click(object sender, EventArgs e)
    {
        saveProfileBtn.Enabled = false;
        if (UpdateUserProfileDetails())
            Response.Redirect("~/Profile/Profile.aspx");
        else
            saveProfileBtn.Enabled = true;
    }   // Method 'saveProfileBtn_Click(object sender, EventArgs e)' closed.

    protected void discardChanges_Btn_Click(object sender, EventArgs e)
    {
        saveProfileBtn.Enabled = false;

        // Just go back to profile page 
        Response.Redirect("~/Profile/Profile.aspx");
    }   // Method 'discardChanges_Btn_Click(object sender, EventArgs e)' closed.

    private bool ValidateUserDetails()
    {
        if (Commons.CheckQuotes(firstName_TextBox.Text))
        {
            Commons.ShowAlertMsg(" ❌ First name should not contain any kind of quotes ! ❌ ");
            firstName_TextBox.Focus();
            return false;
        }
        else if (Commons.CheckQuotes(lastName_TextBox.Text))
        {
            Commons.ShowAlertMsg(" ❌ Last name should not contain any kind of quotes ! ❌ ");
            lastName_TextBox.Focus();
            return false;
        }
        else if (Commons.CheckQuotes(email_TextBox.Text))
        {
            Commons.ShowAlertMsg(" ❌ Email should not contain any kind of quotes ! ❌ ");
            email_TextBox.Focus();
            return false;
        }
        else if (!email_TextBox.Text.Contains("@") || !email_TextBox.Text.Contains(".")) // Check for email in proper format. 
        {
            Commons.ShowAlertMsg(" ❌ Email address 📧 is of INVALID format ! ❌ ");
            email_TextBox.Focus();
            return false;
        }
        
        if (!password_TextBox.Text.ToString().Equals(confirmPassword_TextBox.Text.ToString()))
        {
            Commons.ShowAlertMsg(" ❌ Password and confirm password NOT matched, please type both exactly the same !!! ❌ ");
            password_TextBox.Focus();
            return false;
        }

        return true;
    }   // Method 'ValidateUserDetails()' closed.

}
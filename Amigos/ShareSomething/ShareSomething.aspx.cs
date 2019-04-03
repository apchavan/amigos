using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Data;

public partial class ShareSomething_ShareSomething : System.Web.UI.Page
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
    }


    protected void shareBtn_Click(object sender, EventArgs e)
    {
        if (!ValidateShareInputs())
            return;

        // Save post to database
        string cmdText = "", userPostImagePath = "", formattedShareDateTime = "";

        if (postImage_FileUpload.HasFile)
        {
            string imageExtensionType = Path.GetExtension(postImage_FileUpload.FileName);

            if (imageExtensionType.ToLower() == ".jpg" ||
                imageExtensionType.ToLower() == ".jpeg" ||
                imageExtensionType.ToLower() == ".bmp" ||
                imageExtensionType.ToLower() == ".png" ||
                imageExtensionType.ToLower() == ".gif")
            {
                string userDirectoryPath = Server.MapPath("~/User_uploads/");

                formattedShareDateTime = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt");
                string[] splitDateTime = formattedShareDateTime.Split(' ');

                string saveFormatDateTime = splitDateTime[0] + "_" + splitDateTime[1].Replace(':', '-') + "_" + splitDateTime[2];

                string dated = splitDateTime[0] + " at " + splitDateTime[1] + splitDateTime[2];

                if (!Directory.Exists(userDirectoryPath + Session["UserID"].ToString()))
                    Directory.CreateDirectory(userDirectoryPath + Session["UserID"].ToString());

                userDirectoryPath = userDirectoryPath + Session["UserID"].ToString() + "/";


                userPostImagePath = "~/User_uploads/" + Session["UserID"].ToString() + "/" + saveFormatDateTime + imageExtensionType;
                postImage_FileUpload.SaveAs(userDirectoryPath + saveFormatDateTime + imageExtensionType);

            }   // 'if(imageExtensionType.ToLower() ... ".gif")' closed.
            else
            {
                Commons.ShowAlertMsg("❌ Profile picture image has INVALID format... ❌");
                postImage_FileUpload.Focus();
                return;
            }   // 'else' closed.

            cmdText = "INSERT INTO posts_mst(UserID, post_heading, post_text, post_image, dated) VALUES(@UserID, @post_heading, @post_text, @post_image, @dated)";

            SqlConnection connection = new SqlConnection(Commons.GetConnectionStringFor_user_creds);
            SqlCommand command = new SqlCommand(cmdText, connection);

            command.Parameters.Add("UserID", SqlDbType.Decimal).Value = Session["UserID"].ToString();
            command.Parameters.Add("post_heading", SqlDbType.NVarChar).Value = postHeading_TextBox.Text.Trim().Replace(Environment.NewLine, "<br />");
            command.Parameters.Add("post_text", SqlDbType.NVarChar).Value = postText_TextBox.Text.Trim().Replace(Environment.NewLine, "<br />");
            command.Parameters.Add("post_image", SqlDbType.VarChar).Value = userPostImagePath;
            command.Parameters.Add("dated", SqlDbType.DateTime).Value = formattedShareDateTime;

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            postHeading_TextBox.Text = postText_TextBox.Text = "";
            Commons.ShowAlertMsg(" Dear " + Session["firstname"] + "\n Your post shared succesfully ! 😎✔");
        }
        else
        {
            // Code for post without image selected

            formattedShareDateTime = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt");
            //string[] splitDateTime = formattedShareDateTime.Split(' ');

            //string saveFormatDateTime = splitDateTime[0] + "_" + splitDateTime[1].Replace(':', '-') + "_" + splitDateTime[2];

            //string dated = splitDateTime[0] + " at " + splitDateTime[1] + splitDateTime[2];

            cmdText = "INSERT INTO posts_mst(UserID, post_heading, post_text, post_image, dated) VALUES(@UserID, @post_heading, @post_text, @post_image, @dated)";

            SqlConnection connection = new SqlConnection(Commons.GetConnectionStringFor_user_creds);
            SqlCommand command = new SqlCommand(cmdText, connection);

            command.Parameters.Add("UserID", SqlDbType.Decimal).Value = Session["UserID"].ToString();
            command.Parameters.Add("post_heading", SqlDbType.NVarChar).Value = postHeading_TextBox.Text.Trim().Replace(Environment.NewLine, "<br />");
            command.Parameters.Add("post_text", SqlDbType.NVarChar).Value = postText_TextBox.Text.Trim().Replace(Environment.NewLine, "<br />");
            command.Parameters.Add("post_image", SqlDbType.VarChar).Value = "NoImage";      // Since image is NOT selected
            command.Parameters.Add("dated", SqlDbType.DateTime).Value = formattedShareDateTime;

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            postHeading_TextBox.Text = postText_TextBox.Text = "";
        }

        Response.Redirect("ShareSomething.aspx");
    }

    private bool ValidateShareInputs()
    {
        if (postText_TextBox.Text.ToString().Trim() == "")
        {
            Commons.ShowAlertMsg(" ❌ Post text should not be empty ! ❌ ");
            postText_TextBox.Focus();
            return false;
        }
        /*
        else if (Commons.CheckQuotes(postText_TextBox.Text))
        {
            Commons.ShowAlertMsg(" ❌ Post text should not contain any kind of quotes ! ❌ ");
            postText_TextBox.Focus();
            return false;
        }
        */
        return true;
    }
}
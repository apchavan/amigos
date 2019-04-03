using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Forgot_password_Forgot_password : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }   // Method 'Page_Load(object sender, EventArgs e)' closed.

    protected void signupBtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Signup/Signup.aspx");
    }   // Method 'signupBtn_Click(object sender, EventArgs e)' closed.

    #region Methods for validations
    // Method to check email whether available or not
    private bool CheckEmail(string email)
    {
        string cmdText = "SELECT email FROM user_creds WHERE (email = '" + email + "')";
        DataTable dt_emailAvailable = SQLHelper.FillDataTable(cmdText);

        if (dt_emailAvailable.Rows.Count > 0)
            return true;

        return false;
    }   // Method 'CheckEmail(string email)' closed.

    // Method to check security answer is correct or not
    private bool CheckSecurityAnswer(string email, string answer)
    {
        string cmdText = "SELECT secans FROM user_creds WHERE (email = '" + email + "')";
        DataTable dt_securityAnswer = SQLHelper.FillDataTable(cmdText);

        if (dt_securityAnswer.Rows.Count > 0 && answer == dt_securityAnswer.Rows[0]["secans"].ToString())
            return true;

        return false;
    }   // Method 'CheckSecurityAnswer(string answer)' closed.
    #endregion

    #region Methods for next, back buttons in 'forgotPassword_MultiView'
    protected void nextEmail_Button_Click(object sender, EventArgs e)
    {
        if (inputEmail_TextBox.Text.Trim() == "")
        {
            Response.Write("<div class='container fixed-bottom'>" +
                           "<div class='alert alert-danger alert-dismissible fade show'>" +
                           "<button type='button' class='close' data-dismiss='alert'>&times;</button>" +
                           "<strong> Email can not be empty ! </strong>" +
                           "</div></div>");
            inputEmail_TextBox.Focus();
            return;
        }   // 'if (inputEmail_TextBox.Text.Trim() == "")' closed.

        if (!CheckEmail(inputEmail_TextBox.Text.Trim()))
        {
            Response.Write("<div class='container fixed-bottom'>" +
                           "<div class='alert alert-danger alert-dismissible fade show'>" +
                           "<button type='button' class='close' data-dismiss='alert'>&times;</button>" +
                           "<strong> Invalid email ! </strong>" +
                           "</div></div>");
            inputEmail_TextBox.Focus();
            return;
        }   // 'if (!CheckEmail(inputEmail_TextBox.Text.Trim()))' closed.

        string cmdText = "SELECT secque FROM user_creds WHERE (email = '" + inputEmail_TextBox.Text.Trim() + "')";
        DataTable dt_secQue = SQLHelper.FillDataTable(cmdText);

        secQue_Label.Text = "<strong>\"" + dt_secQue.Rows[0]["secque"].ToString() + "\"</strong>";
        forgotPassword_MultiView.ActiveViewIndex++;
    }   // Method 'nextEmail_Button_Click(object sender, EventArgs e)' closed.

    protected void backSecurity_Button_Click(object sender, EventArgs e)
    {

        forgotPassword_MultiView.ActiveViewIndex--;
    }   // Method 'backSecurity_Button_Click(object sender, EventArgs e)' closed.

    protected void nextSecurity_Button_Click(object sender, EventArgs e)
    {
        if (securityAnswer_TextBox.Text.Trim() == "")
        {
            Response.Write("<div class='container fixed-bottom'>" +
                           "<div class='alert alert-danger alert-dismissible fade show'>" +
                           "<button type='button' class='close' data-dismiss='alert'>&times;</button>" +
                           "<strong> Answer can not be empty ! </strong>" +
                           "</div></div>");
            securityAnswer_TextBox.Focus();
            return;
        }   // 'if (securityAnswer_TextBox.Text.Trim() == "")' closed.

        if (!CheckSecurityAnswer(inputEmail_TextBox.Text.Trim(), securityAnswer_TextBox.Text.Trim()))
        {
            Response.Write("<div class='container fixed-bottom'>" +
                           "<div class='alert alert-danger alert-dismissible fade show'>" +
                           "<button type='button' class='close' data-dismiss='alert'>&times;</button>" +
                           "<strong> Wrong answer ! </strong>" +
                           "</div></div>");
            securityAnswer_TextBox.Focus();
            return;
        }   // 'if (!CheckEmail(securityAnswer_TextBox.Text.Trim()))' closed.

        string cmdText = "SELECT upassword FROM user_creds WHERE " + 
                         "(email = '" + inputEmail_TextBox.Text.Trim() + "' AND " + 
                         "secans = '" + securityAnswer_TextBox.Text.Trim() + "')";
        DataTable dt_upassword = SQLHelper.FillDataTable(cmdText);

        password_Label.Text = "<strong>Your password is: </strong><h5><i>" + 
                              dt_upassword.Rows[0]["upassword"].ToString() + "</i></h5>";

        Response.Write("<div class='container fixed-bottom'>" +
                           "<div class='alert alert-info alert-dismissible fade show'>" +
                           "<button type='button' class='close' data-dismiss='alert'>&times;</button>" +
                           "<strong> <u>NOTE:</u> Please clear all of browser history after session to avoid security risks ! </strong>" +
                           "</div></div>");

        forgotPassword_MultiView.ActiveViewIndex++;
    }   // Method 'nextSecurity_Button_Click(object sender, EventArgs e)' closed.

    protected void login_Button_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/LandingPage/LandingPage.aspx");
    }   // Method 'login_Button_Click(object sender, EventArgs e)' closed.

    #endregion
}   // class 'Forgot_password_Forgot_password' closed.
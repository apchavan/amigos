using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LandingPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void signupBtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Signup/Signup.aspx");
    }

    protected void goBtn_Click(object sender, EventArgs e)
    {
        // Handle user login here...
        try
        {
            if (Commons.CheckQuotes(inputEmail.Text))
            {
                Commons.ShowAlertMsg(" ❌ Any quotes in email is NOT allowed !!! ❌ ");
                inputEmail.Focus();
                return;
            }

            string cmdText = "SELECT UserID, RoleID, upassword, firstname, lastname, email, dob, active " +
                             "FROM user_creds WHERE (email = '" + inputEmail.Text + "')";

            DataTable dt = new DataTable();
            dt = SQLHelper.FillDataTable(cmdText);

            if (dt.Rows.Count <= 0)
            {
                Commons.ShowAlertMsg(" ❌ INVALID email !!! ❌ ");
                inputEmail.Focus();
                return;
            }

            //string passwordText = dt.Rows[0]["upassword"].ToString();

            if (inputPassword.Text.ToString() == dt.Rows[0]["upassword"].ToString())
            {
                if (Convert.ToBoolean(dt.Rows[0]["active"]))
                {
                    Session["UserID"] = dt.Rows[0]["UserID"].ToString();
                    Session["RoleID"] = dt.Rows[0]["RoleID"].ToString();
                    Session["firstname"] = dt.Rows[0]["firstname"].ToString();
                    
                    // Set user current 'islogin' status to 1 (True) 
                    cmdText = "UPDATE user_creds SET islogin = 1 WHERE (UserID = " + Session["UserID"].ToString() + ")";
                    SQLHelper.ExecuteNonQuery(cmdText);
                    Response.Redirect("~/Home/Home.aspx");
                }   // 'if( Convert.ToBoolean(dt.Rows[0]["active"]) )' closed.
                else
                {
                    Commons.ShowAlertMsg(" ⚠ Account for " + inputEmail.Text + " is BLOCKED by administrator ! ⚠ ");
                    return;
                }   // 'else' closed.
            }   // 'if( inputPassword.Text.ToString() == dt.Rows[0]["upassword"].ToString() )' closed.
            else
            {
                Commons.ShowAlertMsg(" ❌ INVALID password !!! ❌ ");
                inputPassword.Focus();
                return;
            }   // 'else' closed. 

        }   // 'try' closed.
        catch (Exception ex)
        {

            throw ex;
        }
    }   // 'goBtn_Click(object sender, EventArgs e)' closed.

}

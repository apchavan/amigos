using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

public partial class Signup_Signup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void nextBtn_Click(object sender, EventArgs e)
    {
        //UserDetailStatus uds = ValidateUserDetails_FirstView();
        if (ValidateUserDetails_FirstView())
            signup_MultiView.ActiveViewIndex++;
    }   // Method 'nextBtn_Click(object sender, EventArgs e)' closed.

    protected void backBtn_Click(object sender, EventArgs e)
    {
        signup_MultiView.ActiveViewIndex--;
    }   // Method 'backBtn_Click(object sender, EventArgs e)' closed.

    protected void signupBtn_Click(object sender, EventArgs e)
    {
        //UserDetailStatus uds = ValidateUserDetails_SecondView();
        if (ValidateUserDetails_SecondView())
            CheckAndSave(); // Sign-up handle
    }   // Method 'signupBtn_Click(object sender, EventArgs e)' closed.

    private void CheckAndSave()
    {
        // Check if email already signed up
        string cmdText = "SELECT UserID FROM user_creds WHERE email = '" + emailTextBox.Text + "'";
        DataTable dt = new DataTable();

        dt = SQLHelper.FillDataTable(cmdText);

        if (dt.Rows.Count > 0)
        {
            Commons.ShowAlertMsg("A user with email credential " + emailTextBox.Text + " already exist !!! Please provide different email. ");
            emailTextBox.Focus();
            return;
        }   // 'if (dt.Rows.Count > 0)' closed.

        // Otherwise save account
        SaveAccount();
    }   // Method 'CheckAndSave()' closed.

    private void SaveAccount()
    {
        string gender = "";
        if (femaleRadioButton.Checked)
            gender = "Female";
        else if (maleRadioButton.Checked)
            gender = "Male";

        //DateTime datetime_dob = DateTime.Parse(dobTextBox.Text);

        //string dobText = datetime_dob.ToString().Split(' ')[0];
        string dobText = dobTextBox.Text.ToString();
        dobText = dobText.Replace("-", ""); // Convert dd-MM-yyyy to ddMMyyyy
        dobText = dobText.Substring(4) + dobText.Substring(2, 2) + dobText.Substring(0, 2); // Convert ddMMyyyy to yyyyMMdd 
        

        string cmdText = "INSERT INTO user_creds(RoleID, firstname, lastname, mobileno, dob, email, upassword, secque, secans, gender, active, islogin) " +
                        "VALUES (2, '" + firstNameTextBox.Text + "', '" + lastNameTextBox.Text + "', " + Convert.ToInt64(mobileTextBox.Text) +
                        ", CAST('" + dobText + "' AS DATE), '" + emailTextBox.Text + "', '" + passwordTextBox.Text.ToString() + "', '" + questionTextBox.Text +
                        "', '" + answerTextBox.Text + "', '" + gender + "', 1, 0)";

        SQLHelper.ExecuteNonQuery(cmdText);

        Commons.ShowAlertMsg(" Dear '" + firstNameTextBox.Text + " " + lastNameTextBox.Text + "', account is successfully created 😎! ");

        Response.Redirect("~/SuccessSignup/SuccessSignup.aspx");
    }   // Method 'SaveAccount()' closed.


    // Methods to validate filled user details
    private bool ValidateUserDetails_FirstView()
    {
        if (firstNameTextBox.Text.ToString().Trim() == "")
        {
            Commons.ShowAlertMsg(" ❌ First name should not be empty ! ❌ ");
            firstNameTextBox.Focus();
            return false;
        }
        else if (Commons.CheckQuotes(firstNameTextBox.Text))
        {
            Commons.ShowAlertMsg(" ❌ First name should not contain any kind of quotes ! ❌ ");
            firstNameTextBox.Focus();
            return false;
        }

        else if (lastNameTextBox.Text.ToString().Trim() == "")
        {
            Commons.ShowAlertMsg(" ❌ Last name should not be empty ! ❌ ");
            lastNameTextBox.Focus();
            return false;
        }
        else if (Commons.CheckQuotes(lastNameTextBox.Text))
        {
            Commons.ShowAlertMsg(" ❌ Last name should not contain any kind of quotes ! ❌ ");
            lastNameTextBox.Focus();
            return false;
        }

        else if (mobileTextBox.Text.ToString().Trim() == "")
        {
            Commons.ShowAlertMsg(" ❌ Mobile number should not be empty ! ❌ ");
            mobileTextBox.Focus();
            return false;
        }
        else if (Commons.CheckQuotes(mobileTextBox.Text))
        {
            Commons.ShowAlertMsg(" ❌ Mobile number should ONLY contain numbers ! ❌ ");
            mobileTextBox.Focus();
            return false;
        }
        else if (mobileTextBox.Text.Length != 10)
        {
            Commons.ShowAlertMsg(" ❌ Mobile number should be 10 digit numbers ! ❌ ");
            mobileTextBox.Focus();
            return false;
        }
        else if (!CheckMobileNumber(mobileTextBox.Text.ToString()))
        {
            Commons.ShowAlertMsg(" ❌ Mobile number should only 10 digit number ! NO Alphabet or special characters allowed ! ❌ ");
            mobileTextBox.Focus();
            return false;
        }

        else if (dobTextBox.Text.Length == 0)
        {
            Commons.ShowAlertMsg(" Please select the date of birth using 🔽 !");
            dobTextBox.Focus();
            return false;
        }

        return true;
    }   // Method 'ValidateUserDetails_FirstView()' closed.

    private bool ValidateUserDetails_SecondView()
    {
        if (emailTextBox.Text.ToString().Trim() == "")
        {
            Commons.ShowAlertMsg(" ❌ Email should not be empty ! ❌ ");
            emailTextBox.Focus();
            return false;
        }
        else if (Commons.CheckQuotes(emailTextBox.Text))
        {
            Commons.ShowAlertMsg(" ❌ Email should not contain any kind of quotes ! ❌ ");
            emailTextBox.Focus();
            return false;
        }
        else if (!emailTextBox.Text.Contains("@") || !emailTextBox.Text.Contains(".")) // Check for email in proper format. 
        {
            Commons.ShowAlertMsg(" ❌ Email address 📧 is of INVALID format ! ❌ ");
            emailTextBox.Focus();
            return false;
        }

        else if (passwordTextBox.Text.ToString().Trim() == "")
        {
            Commons.ShowAlertMsg(" ❌ Password should not be empty ! ❌ ");
            passwordTextBox.Focus();
            return false;
        }
        else if (Commons.CheckQuotes(passwordTextBox.Text))
        {
            Commons.ShowAlertMsg(" ❌ Password should not contain any kind of quotes ! ❌ ");
            passwordTextBox.Focus();
            return false;
        }

        else if (confirmPasswordTextBox.Text.ToString().Trim() == "")
        {
            Commons.ShowAlertMsg(" ❌ Confirm password should not be empty ! ❌ ");
            confirmPasswordTextBox.Focus();
            return false;
        }
        else if (Commons.CheckQuotes(confirmPasswordTextBox.Text))
        {
            Commons.ShowAlertMsg(" ❌ Confirm password should not contain any kind of quotes ! ❌ ");
            confirmPasswordTextBox.Focus();
            return false;
        }

        else if (questionTextBox.Text.ToString().Trim() == "")
        {
            Commons.ShowAlertMsg(" ❌ Security question should not be empty ! ❌ ");
            questionTextBox.Focus();
            return false;
        }

        else if (Commons.CheckQuotes(questionTextBox.Text))
        {
            Commons.ShowAlertMsg(" ❌ Security question should not contain any kind of quotes ! ❌ ");
            questionTextBox.Focus();
            return false;
        }

        else if (answerTextBox.Text.ToString().Trim() == "")
        {
            Commons.ShowAlertMsg(" ❌ Answer to security question should not be empty ! ❌ ");
            answerTextBox.Focus();
            return false;
        }

        else if (Commons.CheckQuotes(answerTextBox.Text))
        {
            Commons.ShowAlertMsg(" ❌ Answer to security question should not contain any kind of quotes ! ❌ ");
            answerTextBox.Focus();
            return false;
        }

        if (!maleRadioButton.Checked && !femaleRadioButton.Checked)
        {
            Commons.ShowAlertMsg(" ❌ Please select your gender ! ❌ ");
            return false;
        }

        //Check for password & confirm password match
        if (!passwordTextBox.Text.ToString().Equals(confirmPasswordTextBox.Text.ToString()))
        {
            Commons.ShowAlertMsg(" ❌ Password and confirm password NOT matched, please type both exactly the same !!! ❌ ");
            passwordTextBox.Focus();
            return false;
        }

        return true;
    }   // Method 'ValidateUserDetails_SecondView()' closed.

    // Method for mobile number checker containing numbers only 
    private bool CheckMobileNumber(string mobileno)
    {
        foreach (char digitChar in mobileno)
        {
            // Checking for Digits 
            if (digitChar >= 48 && digitChar <= 57)
                continue;

            // Checking for Alphabet 
            else if ((digitChar >= 65 && digitChar <= 122))
                return false;

            // Otherwise Special Character 
            else
                return false;
        }   // 'foreach (char digitChar in mobileno)' closed.
        return true;
    }   // Method 'CheckMobileNumber(string mobileno)' closed.

}   // class 'class Signup_Signup' closed.
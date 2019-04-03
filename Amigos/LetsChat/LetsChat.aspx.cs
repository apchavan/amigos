using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LetsChat_LetsChat : System.Web.UI.Page
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
            LoadChatFriendsList();

        if (Session["chatUserID"].ToString() != "")
            LoadChatMessagesList();

    }   // Method 'Page_Load(object sender, EventArgs e)' closed.

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        Commons.ClearCookies();
    }

    private void LoadChatFriendsList()
    {
        string cmdText = "SELECT friends.from_UserID, friends.to_UserID, friends.confirmed, user_creds.firstname, user_creds.lastname, " +
                         "user_profile.photo, user_profile.profession, user_profile.at FROM friends LEFT JOIN " +
                         "user_creds ON friends.from_UserID = user_creds.UserID LEFT JOIN " +
                         "user_profile ON friends.from_UserID = user_profile.UserID " +
                         "WHERE (friends.to_UserID = " + Session["UserID"].ToString() +
                         " OR friends.from_UserID = " + Session["UserID"].ToString() +
                         ") AND friends.confirmed = 1";

        DataTable dt_friendsList = SQLHelper.FillDataTable(cmdText);

        if (dt_friendsList.Rows.Count <= 0)
        {
            heading_Label.Text = "<strong> 😊 Currently, you do not have any friends ... 😊<br /> Connect people you know by searching to begin chat !</strong>";
            chatWindow.Visible = false;
            return;
        }

        heading_Label.Text = "<h5> 😊 Select friend from list to chat ... 😊<br /></h5>";

        DataTable dt_ChatFriendsList = new DataTable();

        dt_ChatFriendsList.Columns.Add("UserID");
        dt_ChatFriendsList.Columns.Add("firstname");
        dt_ChatFriendsList.Columns.Add("lastname");
        dt_ChatFriendsList.Columns.Add("photo");

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

            dt_ChatFriendsList.Rows.Add(
                                        dt_row.Rows[0]["UserID"].ToString(), dt_row.Rows[0]["firstname"].ToString(),
                                        dt_row.Rows[0]["lastname"].ToString(), dt_row.Rows[0]["photo"].ToString()
                                        );
        }

        chatFriends_DataList.DataSource = dt_ChatFriendsList;
        chatFriends_DataList.DataBind();
    }

    // Method to load all chat messages for selected friend
    private void LoadChatMessagesList()
    {
        if (Session["chatUserID"].ToString().Trim() == "" || Session["UserID"].ToString() == "")
            return;
        string cmdText = "SELECT sender_UserID, receiver_UserID, chat_message, CAST(chat_date_time AS DATE) AS chat_date, chat_date_time " +
                         "FROM user_chat " +
                         "WHERE (sender_UserID = " +
                         Session["UserID"].ToString() + " AND receiver_UserID = " + Session["chatUserID"].ToString() +
                         ") OR " + "(sender_UserID = " + Session["chatUserID"].ToString() +
                         " AND receiver_UserID = " + Session["UserID"].ToString() + ") ORDER BY chat_date ASC, chat_date_time ASC";

        DataTable dt_allChats = SQLHelper.FillDataTable(cmdText);

        if (dt_allChats.Rows.Count <= 0)
        {
            dt_allChats.Reset();
            chatMessages_DataList.DataSource = null;
            chatMessages_DataList.DataBind();
            return;
        }

        DataTable dt_ChatMessagesList = new DataTable();

        dt_ChatMessagesList.Columns.Add("photo");
        dt_ChatMessagesList.Columns.Add("sender_UserID");
        dt_ChatMessagesList.Columns.Add("receiver_UserID");
        dt_ChatMessagesList.Columns.Add("chat_date_time");
        dt_ChatMessagesList.Columns.Add("chat_message");

        for (int i = 0; i < dt_allChats.Rows.Count; ++i)
        {
            if (dt_allChats.Rows[i]["sender_UserID"].ToString() == Session["UserID"].ToString())
            {
                cmdText = "SELECT photo FROM user_profile WHERE (UserID = " + dt_allChats.Rows[i]["receiver_UserID"].ToString() + ")";
            }

            else if (dt_allChats.Rows[i]["receiver_UserID"].ToString() == Session["UserID"].ToString())
            {
                cmdText = "SELECT photo FROM user_profile WHERE (UserID = " + dt_allChats.Rows[i]["sender_UserID"].ToString() + ")";
            }


            DataTable dt_photo = SQLHelper.FillDataTable(cmdText);

            string photoPath = "";

            if (dt_photo.Rows.Count > 0)
                photoPath = dt_photo.Rows[0]["photo"].ToString().Trim();
            else
                photoPath = "~/Images/no_image.jpg";

            dt_ChatMessagesList.Rows.Add(
                                         photoPath,
                                         dt_allChats.Rows[i]["sender_UserID"].ToString(),
                                         dt_allChats.Rows[i]["receiver_UserID"].ToString(),
                                         dt_allChats.Rows[i]["chat_date_time"].ToString(),
                                         dt_allChats.Rows[i]["chat_message"].ToString()
                                         );
        }

        chatMessages_DataList.DataSource = dt_ChatMessagesList;
        chatMessages_DataList.DataBind();

        if (!chat_Timer.Enabled)
            chat_Timer.Enabled = true;
    }   // Method 'LoadChatMessagesList()' closed.

    protected void chatFriends_DataList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        // Load specific chat window for selected friend name.
        if (e.CommandName == "otherUserName_LinkBtn_CommandName")
        {
            Session["chatUserID"] = "";
            //sendMessage_TextBox.Enabled = sendMessage_Btn.Enabled = true;

            LinkButton otherUserName_LinkBtn = (LinkButton)e.Item.FindControl("otherUserName_LinkBtn");


            string cmdText = "SELECT UserID FROM user_creds WHERE (firstname = '" + otherUserName_LinkBtn.Text.ToString().Split(' ')[0] +
                             "' AND lastname = '" + otherUserName_LinkBtn.Text.ToString().Split(' ')[1] + "')";

            DataTable dt_UserIDResult = SQLHelper.FillDataTable(cmdText);

            Session["chatUserID"] = dt_UserIDResult.Rows[0]["UserID"].ToString();

            // Set selected friend name to label control to show message to be sent to that friend
            chatFriendName_Label.Text = otherUserName_LinkBtn.Text.ToString().Split(' ')[0] + " " + otherUserName_LinkBtn.Text.ToString().Split(' ')[1];

            // Load all friends
            LoadChatFriendsList();

            // Load all previous chat messages for selected friend
            LoadChatMessagesList();

            if (!chat_Timer.Enabled)
                chat_Timer.Enabled = true;
        }   // 'if (e.CommandName == "otherUserName_LinkBtn_CommandName")' closed.
    }       // Method 'chatFriends_DataList_ItemCommand(object source, DataListCommandEventArgs e)' closed.

    protected void chatFriends_DataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ShowDefaultProfileImageForNull(e);
            sendMessage_TextBox.Enabled = sendMessage_Btn.Enabled = true;
            DataTable dt_friendsList = (DataTable)chatFriends_DataList.DataSource;

            if (
                (dt_friendsList.Rows[e.Item.ItemIndex]["UserID"].ToString() == Session["chatUserID"].ToString()) &&
                (sendMessage_TextBox.Enabled && sendMessage_Btn.Enabled == true)
                )
                ((Label)e.Item.FindControl("chatSelection_Label")).Text = "<font size='1'> (✔) </font>";
            else
                ((Label)e.Item.FindControl("chatSelection_Label")).Text = "";
        }
    }

    protected void chatMessages_DataList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ShowDefaultProfileImageForNull(e);

            DataTable dt_allChats = (DataTable)chatMessages_DataList.DataSource;

            Image otherUserProfile_Image = (Image)e.Item.FindControl("otherUserProfile_Image");

            Label sentMessage_Label = (Label)e.Item.FindControl("sentMessage_Label");
            Label sentMessageTime_Label = (Label)e.Item.FindControl("sentMessageTime_Label");

            Label receivedMessage_Label = (Label)e.Item.FindControl("receivedMessage_Label");
            Label receivedMessageTime_Label = (Label)e.Item.FindControl("receivedMessageTime_Label");

            Control receivedMessageRow_Div = (Control)e.Item.FindControl("receivedMessageRow_Div");
            Control sentMessageRow_Div = (Control)e.Item.FindControl("sentMessageRow_Div");

            if (dt_allChats.Rows[e.Item.ItemIndex]["sender_UserID"].ToString() == Session["UserID"].ToString())
            {
                sentMessage_Label.Text = dt_allChats.Rows[e.Item.ItemIndex]["chat_message"].ToString();
                sentMessageTime_Label.Text = dt_allChats.Rows[e.Item.ItemIndex]["chat_date_time"].ToString();

                // Hide the un-necessary row for received messages
                receivedMessage_Label.Text = receivedMessageTime_Label.Text = "";

                receivedMessage_Label.Visible = receivedMessageTime_Label.Visible =
                                                otherUserProfile_Image.Visible = receivedMessageRow_Div.Visible = false;
            }
            else if (dt_allChats.Rows[e.Item.ItemIndex]["receiver_UserID"].ToString() == Session["UserID"].ToString())
            {
                receivedMessage_Label.Text = dt_allChats.Rows[e.Item.ItemIndex]["chat_message"].ToString();
                receivedMessageTime_Label.Text = dt_allChats.Rows[e.Item.ItemIndex]["chat_date_time"].ToString();

                // Hide the un-necessary row for sent messages
                sentMessage_Label.Text = sentMessageTime_Label.Text = "";
                sentMessage_Label.Visible = sentMessageTime_Label.Visible = sentMessageRow_Div.Visible = false;
            }
        }   // 'if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)' closed.
    }   // Method 'chatMessages_DataList_ItemDataBound(object sender, DataListItemEventArgs e)' closed.


    // Method for show default profile if user profile is not set
    private void ShowDefaultProfileImageForNull(DataListItemEventArgs e)
    {
        Image otherUserProfile_Image = (Image)e.Item.FindControl("otherUserProfile_Image");

        if (otherUserProfile_Image.ImageUrl.ToString().Trim() == "")
            otherUserProfile_Image.ImageUrl = "~/Images/no_image.jpg";
    }

    protected void sendMessage_Btn_Click(object sender, EventArgs e)
    {
        if (sendMessage_TextBox.Text.Trim() == "" || Session["chatUserID"].ToString() == "")
        {
            sendMessage_TextBox.Text = "";
            return;
        }   // 'if (sendMessage_TextBox.Text.Trim() == "" || Session["chatUserID"].ToString() == "")' closed.

        if (!chat_Timer.Enabled)
            chat_Timer.Enabled = true;

        string cmdText = "INSERT INTO user_chat(sender_UserID, receiver_UserID, chat_date_time, chat_message) " +
                     "VALUES(@sender_UserID, @receiver_UserID, @chat_date_time, @chat_message)";

        SqlConnection connection = new SqlConnection(Commons.GetConnectionStringFor_user_creds);
        SqlCommand command = new SqlCommand(cmdText, connection);

        command.Parameters.Add("@sender_UserID", SqlDbType.Decimal).Value = Session["UserID"].ToString();
        command.Parameters.Add("@receiver_UserID", SqlDbType.Decimal).Value = Session["chatUserID"].ToString();
        command.Parameters.Add("@chat_date_time", SqlDbType.DateTime).Value = DateTime.Now;
        command.Parameters.Add("@chat_message", SqlDbType.NVarChar).Value = sendMessage_TextBox.Text.Replace(Environment.NewLine, "<br />").Trim();

        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();

        // Load chat list again after message is sent
        LoadChatMessagesList();

        // Clear 'sendMessage_TextBox' TextBox
        sendMessage_TextBox.Text = "";
    }

    protected void chat_Timer_Tick(object sender, EventArgs e)
    {
        LoadChatMessagesList();
    }

}

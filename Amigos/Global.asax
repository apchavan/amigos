<%@ Application Language="C#" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup

    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started
        Session["UserID"] = "";
        Session["RoleID"] = "";
        Session["accountMasterPage"] = "~/AccountMaster/UserAccountMaster.master";
        Session["firstname"] = "";
        Session["chatUserID"] = "";
    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
        Session["UserID"] = "";
        Session["RoleID"] = "";
        Session["accountMasterPage"] = "";
        Session["firstname"] = "";
        Session["chatUserID"] = "";

        Session.Clear();
        Session.RemoveAll();
        Session.Abandon();
    }

</script>

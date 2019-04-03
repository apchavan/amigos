using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

/// <summary>
/// Summary description for Commons
/// </summary>
public class Commons
{
    public Commons()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    // String handling methods
    public static bool CheckQuotes(string word)
    {
        if (word.Contains("'"))
            return true;
        if (word.Contains("\""))
            return true;
        
        return false;
    }   // 'CheckQuotes(string word)' closed.

    // Method for showing 'alert' on webpage
    public static void ShowAlertMsg(string msg)
    {
        HttpContext.Current.Response.Write("<script> alert('" + msg + "'); </script>");
    }   // 'ShowAlertMsg(string msg)' closed.

    // Properties to return respective connection strings
    public static string GetConnectionStringFor_user_creds
    {
        get
        {
            return (WebConfigurationManager.ConnectionStrings["user_creds_connect"].ConnectionString);
        }
    }

    public static void ClearSession()
    {
        HttpContext.Current.Session["UserID"] = "";
        HttpContext.Current.Session["RoleID"] = "";
        HttpContext.Current.Session["firstname"] = "";
        HttpContext.Current.Session["chatUserID"] = "";

        HttpContext.Current.Session.Clear();
        HttpContext.Current.Session.Abandon();
        HttpContext.Current.Session.RemoveAll();
        
    }
    public static void ClearCookies()
    {
        HttpContext.Current.Request.Cookies.Clear();
        HttpContext.Current.Request.Cookies.Remove("searchName");
        HttpContext.Current.Request.Cookies.Remove("otherUserIDs");
    }
}
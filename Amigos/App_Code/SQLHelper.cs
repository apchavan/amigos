using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for SQLHelper
/// </summary>
public class SQLHelper
{
    public SQLHelper()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    // Methods for database operations 
    public static void ExecuteNonQuery(string cmdText)
    {
        try
        {
            SqlConnection connection = new SqlConnection(Commons.GetConnectionStringFor_user_creds);
            SqlCommand cmd = new SqlCommand(cmdText, connection);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static DataTable FillDataTable(string cmdText)
    {
        try
        {
            SqlConnection connection = new SqlConnection(Commons.GetConnectionStringFor_user_creds);
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = new SqlCommand(cmdText, connection);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}

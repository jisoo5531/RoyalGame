using System;
using UnityEngine;
using MySql.Data.MySqlClient;

public class DatabaseManager : MonoBehaviour
{
    #region private º¯¼ö
    private static DatabaseManager instance;

    private string serverEndPoint = "unity-mysql-rds.ct8w8icaeo26.ap-northeast-2.rds.amazonaws.com";
    private string databaseName = "RoyaleDatabase";
    string port = "3306";
    private string userName = "root";
    private string password = "abcdgh3076";

    private string connStr;
    #endregion

    public int userId;
    public MySqlConnection conn;

    public static DatabaseManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("DatabaseManager").AddComponent<DatabaseManager>();
            }
            return instance;
        }
    }

    void Awake()
    {
        connStr = string.Format("server={0};port={1};uid={2};pwd={3};database={4};", serverEndPoint, port, userName, password, databaseName);
        DontDestroyOnLoad(gameObject);
        OpenConnection();
    }

    void OnDestroy()
    {
        CloseConnection();
    }

    private void OpenConnection()
    {
        if (conn == null)
        {
            conn = new MySqlConnection(connStr);
        }

        bool connected = false;
        int retryCount = 0;
        int maxRetries = 5;
        int retryDelay = 2000;

        while (!connected && retryCount < maxRetries)
        {
            try
            {
                conn.Open();
                connected = true;
                Debug.Log("Database connection opened");
            }
            catch (Exception ex)
            {
                retryCount++;
                Debug.LogWarning($"Failed to open database connection (Attempt {retryCount}/{maxRetries}): " + ex.Message);
                System.Threading.Thread.Sleep(retryDelay);
            }
        }

        if (!connected)
        {
            Debug.LogError("Failed to open database connection after multiple attempts.");
        }
    }

    public void CloseConnection()
    {
        if (conn != null)
        {
            conn.Close();
            Debug.Log("Database connection closed");
        }
    }
    private void OnApplicationQuit()
    {
        conn.Close();
    }

    public bool connection_check(MySqlConnection con)
    {
        if (con.State != System.Data.ConnectionState.Open)
        {
            con.Open();
            if (con.State != System.Data.ConnectionState.Open)
            {
                return false;
            }
        }
        return true;
    }
}

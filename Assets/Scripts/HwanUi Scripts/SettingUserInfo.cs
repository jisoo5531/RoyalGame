using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingUserInfo : MonoBehaviour
{
    #region public º¯¼ö
    public TMP_Text goldAmount;
    public TMP_Text jewelAmount;

    public TMP_Text userName;
    public TMP_Text trophyAmount;
    #endregion

    private void Awake()
    {
        SelectUser(DatabaseManager.Instance.userId);
    }

    private void SelectUser(int userId)
    {
        string selectUserInfo = string.Format("SELECT * FROM USER WHERE userId = {0}", userId);

        MySqlCommand cmd = DatabaseManager.Instance.DBConnection(selectUserInfo);

        if (cmd != null)
        {
            userName.text = GetStringData(cmd, "userName");
            trophyAmount.text = GetStringData(cmd, "currentTrophy");
            goldAmount.text = GetStringData(cmd, "gold");
            jewelAmount.text = GetStringData(cmd, "jewel");
        }
    }
    string GetStringData(MySqlCommand cmd, string column)
    {
        string data = string.Empty;

        using (MySqlDataReader reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                data = reader[column].ToString();
            }
            reader.Close();
        }

        return data;
    }
}

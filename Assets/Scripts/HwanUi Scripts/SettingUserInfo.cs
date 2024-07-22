using MySql.Data.MySqlClient;
using System;
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
        try
        {
            string selectUserInfo = $"SELECT * FROM USER WHERE userId = {userId}";

            using (MySqlConnection conn = DatabaseManager.Instance.DBConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(selectUserInfo, conn))
                {
                    if (cmd != null)
                    {
                        userName.text = GetStringData(cmd, "userName");
                        trophyAmount.text = GetStringData(cmd, "currentTrophy");
                        goldAmount.text = GetStringData(cmd, "gold");
                        jewelAmount.text = GetStringData(cmd, "jewel");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            print(ex.Message);
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

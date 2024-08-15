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

    public TMP_Text detailUserName;
    public TMP_Text maxTrophyCount;
    public TMP_Text currentTrophyCount;
    public TMP_Text battleCount;
    public TMP_Text haveCardCount;
    public TMP_Text victoryCount;
    public TMP_Text defeatCount;
    #endregion

    private void Awake()
    {
        SelectUser(DatabaseManager.Instance.userId);
    }

    private void Start()
    {
        StartManager.m_Instance.gold = int.Parse(goldAmount.text);
        StartManager.m_Instance.jewel = int.Parse(jewelAmount.text);
    }

    private void SelectUser(int userId)
    {
        try
        {
            if (!DatabaseManager.Instance.connection_check(DatabaseManager.Instance.conn))
            {
                return;
            }

            string selectUserInfo = $"SELECT * FROM USER WHERE userId = {userId}";

            using (MySqlCommand cmd = new MySqlCommand(selectUserInfo, DatabaseManager.Instance.conn))
            {
                if (cmd != null)
                {
                    userName.text = GetStringData(cmd, "userName");
                    detailUserName.text = GetStringData(cmd, "userName");
                    trophyAmount.text = GetStringData(cmd, "currentTrophy");
                    currentTrophyCount.text = GetStringData(cmd, "currentTrophy");
                    maxTrophyCount.text = GetStringData(cmd, "maxTrophy");
                    goldAmount.text = GetStringData(cmd, "gold");
                    jewelAmount.text = GetStringData(cmd, "jewel");
                    battleCount.text = GetStringData(cmd, "battleCount");
                    haveCardCount.text = $"{GetStringData(cmd, "currentCardCount")} / {GetStringData(cmd, "maxCardCount")}";
                    victoryCount.text = GetStringData(cmd, "victoryCount");
                    defeatCount.text = GetStringData(cmd, "defeatCount");
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
        }

        return data;
    }
}

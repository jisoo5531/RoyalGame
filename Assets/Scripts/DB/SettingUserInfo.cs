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
            if (!DatabaseManager.Instance.Connection_Check(DatabaseManager.Instance.conn))
            {
                return;
            }

            string selectUserInfo = $"SELECT * FROM USER WHERE userId = {userId}";

            using (MySqlCommand cmd = new MySqlCommand(selectUserInfo, DatabaseManager.Instance.conn))
            {
                if (cmd != null)
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userName.text = reader["userName"].ToString();
                            detailUserName.text = reader["userName"].ToString();
                            trophyAmount.text = reader["currentTrophy"].ToString();
                            currentTrophyCount.text = reader["currentTrophy"].ToString();
                            maxTrophyCount.text = reader["maxTrophy"].ToString();
                            goldAmount.text = reader["gold"].ToString();
                            jewelAmount.text = reader["jewel"].ToString();
                            battleCount.text = reader["battleCount"].ToString();
                            haveCardCount.text = $"{reader["currentCardCount"]} / {reader["maxCardCount"]}";
                            victoryCount.text = reader["victoryCount"].ToString();
                            defeatCount.text = reader["defeatCount"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }
}

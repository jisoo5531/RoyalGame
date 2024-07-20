using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordFind : MonoBehaviour
{
    public TMP_InputField nickname;

    public static string userPassword;

    public Button findBtn;

    void Update()
    {
        findBtn.interactable = CheckTextLength(nickname.text.Length);
    }

    private bool CheckTextLength(int nicknameLength)
    {
        return nicknameLength >= 4;
    }

    public void FindPasswordClick()
    {
        if(CheckEqualsUserName(nickname.text))
        {

        }
        else
        {

        }
    }

    private bool CheckEqualsUserName(string nickname)
    {
        try
        {
            string selectName = string.Format("SELECT count(*), password FROM USER WHERE userName = '{0}'", nickname);

            MySqlCommand cmd = DatabaseManager.Instance.DBConnection(selectName);
            cmd.CommandText = selectName;
            if (cmd != null)
            {
                userPassword = GetPassword(cmd);
                int rowCount = GetRowCount(cmd);

                return rowCount > 0;
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Select Query execution error: " + ex.Message);
        }
        return false;
    }
    string GetPassword(MySqlCommand cmd)
    {
        string password = string.Empty;

        using (MySqlDataReader reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                password = reader["password"].ToString();
            }
            reader.Close();
        }

        return password;
    }

    int GetRowCount(MySqlCommand cmd)
    {
        int count = 0;

        using (MySqlDataReader reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                count = reader.GetInt32(0);
            }
            reader.Close();
        }

        return count;
    }
}

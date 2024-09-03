using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordFind : MonoBehaviour
{
    #region public º¯¼ö
    public TMP_InputField nickname;
    public Button findBtn;

    public string userPassword;

    public GameObject findPasswordFail_UI;
    public GameObject findPasswordSuccess_UI;
    public GameObject findPassword;

    public GameObject signInUI;
    #endregion

    void Update()
    {
        findBtn.interactable = CheckTextLength(nickname.text.Length);
    }

    private bool CheckTextLength(int nicknameLength)
    {
        return nicknameLength >= 4;
    }
    public void FindPassword_SignInClick()
    {
        nickname.text = string.Empty;
        signInUI.SetActive(true);
        findPassword.SetActive(false);
    }

    public void FindPasswordClick()
    {
        findPassword.SetActive(false);
        if (CheckEqualsUserName(nickname.text))
        {
            findPasswordSuccess_UI.SetActive(true);
        }
        else
        {
            findPasswordFail_UI.SetActive(true);
        }
        nickname.text = string.Empty;
    }

    private bool CheckEqualsUserName(string nickname)
    {
        try
        {
            if (!DatabaseManager.Instance.Connection_Check(DatabaseManager.Instance.conn))
            {
                return false;
            }

            string selectName = $"SELECT count(*), password FROM USER WHERE userName = '{nickname}'";

            using (MySqlCommand cmd = new MySqlCommand(selectName, DatabaseManager.Instance.conn))
            {
                cmd.CommandText = selectName;
                if (cmd != null)
                {
                    int count = 0;
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            count = reader.GetInt32(0);
                            userPassword = reader["password"].ToString();
                        }
                        return count > 0;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Select Query execution error: " + ex.Message);
        }
        return false;
    }
}

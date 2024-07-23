using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUp : MonoBehaviour
{
    #region public 변수
    public TMP_InputField nickname;
    public TMP_InputField password;

    public Button loginBtn;

    public GameObject signInUI;
    #endregion

    private void Start()
    {
        loginBtn.interactable = false;
    }

    void Update()
    {
        loginBtn.interactable = CheckTextLength(nickname.text.Length, password.text.Length);
    }
    public void SignInClick()
    {
        nickname.text = string.Empty;
        password.text = string.Empty;
        signInUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private bool CheckTextLength(int nicknameLength, int passwordLength)
    {
        return (nicknameLength >= 4 && passwordLength >= 4);
    }

    public void LoginClick()
    {
        if (!CheckDuplicateName(nickname.text))
        {
            InsertUserData(nickname.text, password.text);
            loginBtn.interactable = false;
            PhotonConnManager.instance.userName = nickname.text;
            PhotonConnManager.instance.Connection();
        }
        else
        {
            print("이름 중복");
        }
    }

    private void InsertUserData(string name, string password)
    {
        try
        {
            string insertQuery = $"INSERT INTO USER (userName, password, battleCount, victoryCount, defeatCount, maxTrophy, currentTrophy, gold, " +
            "jewel, maxCardCount, currentCardCount) VALUES (@userName, @password, @battleCount, @victoryCount, @defeatCount, @maxTrophy, " +
            "@currentTrophy, @gold, @jewel, @maxCardCount, @currentCardCount); SELECT LAST_INSERT_ID();";

            using (MySqlConnection conn = DatabaseManager.Instance.DBConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                {
                    if (cmd != null)
                    {
                        cmd.Parameters.AddWithValue("@userName", name);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.Parameters.AddWithValue("@battleCount", 0);
                        cmd.Parameters.AddWithValue("@victoryCount", 0);
                        cmd.Parameters.AddWithValue("@defeatCount", 0);
                        cmd.Parameters.AddWithValue("@maxTrophy", 0);
                        cmd.Parameters.AddWithValue("@currentTrophy", 0);
                        cmd.Parameters.AddWithValue("@gold", 0);
                        cmd.Parameters.AddWithValue("@jewel", 1000);
                        cmd.Parameters.AddWithValue("@maxCardCount", 8);
                        cmd.Parameters.AddWithValue("@currentCardCount", 0);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DatabaseManager.Instance.userId = reader.GetInt32(0);
                                Debug.Log("데이터 삽입 성공");
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning("데이터 삽입 실패");
                    }
                }
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Insert Query execution error: " + ex.Message);
        }
    }

    private bool CheckDuplicateName(string name)
    {
        try
        {
            string nameSelect = $"SELECT count(*) FROM USER WHERE userName = '{name}'";

            using (MySqlConnection conn = DatabaseManager.Instance.DBConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(nameSelect, conn))
                {
                    if (cmd != null)
                    {
                        int result = GetRowCount(cmd);

                        return result > 0;
                    }
                }
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Select Query execution error: " + ex.Message);
        }
        return true;
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

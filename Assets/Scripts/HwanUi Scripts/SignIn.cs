using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignIn : MonoBehaviour
{
    public TMP_InputField nickname;
    public TMP_InputField password;

    public Button loginBtn;

    private void Start()
    {
        loginBtn.interactable = false;
    }

    void Update()
    {
        if (nickname.text.Length < 4 || password.text.Length < 4)
            loginBtn.interactable = false;


        if (nickname.text.Length >= 4 && password.text.Length >= 4)
            loginBtn.interactable = true;

    }

    public void LoginClick()
    {
        if (!CheckUserInfo(nickname.text, password.text))
        {
            print("로그인 되었습니다");
        }
        else
        {
            print("로그인 실패");
        }
    }

    private void InsertUserData(string name, string password)
    {
        try
        {
            string insertQuery = "INSERT INTO USER (userName, password, battleCount, victoryCount, defeatCount, maxTrophy, currentTrophy, gold, " +
                "jewel, maxCardCount, currentCardCount) VALUES (@userName, @password, @battleCount, @victoryCount, @defeatCount, @maxTrophy, " +
                "@currentTrophy, @gold, @jewel, @maxCardCount, @currentCardCount)";

            MySqlCommand cmd = DatabaseManager.Instance.CreateCommand(insertQuery);

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
                cmd.Parameters.AddWithValue("@jewel", 0);
                cmd.Parameters.AddWithValue("@maxCardCount", 8);
                cmd.Parameters.AddWithValue("@currentCardCount", 0);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Debug.Log("데이터 삽입 성공");
                }
                else
                {
                    Debug.LogWarning("데이터 삽입 실패");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Insert Query execution error: " + ex.Message);
        }
    }

    private bool CheckUserInfo(string name, string password)
    {
        try
        {
            string nameSelect = string.Format("SELECT count(*) FROM USER WHERE userName = '{0}'", name);

            MySqlCommand cmd = DatabaseManager.Instance.CreateCommand(nameSelect);

            if (cmd != null)
            {
                object result = cmd.ExecuteScalar();
                int rowCount = Convert.ToInt32(result);

                print(rowCount);
                return rowCount > 0;
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Select Query execution error: " + ex.Message);
        }
        return true;
    }
}

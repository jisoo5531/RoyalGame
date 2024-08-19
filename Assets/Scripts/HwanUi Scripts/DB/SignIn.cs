using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignIn : MonoBehaviour
{
    #region public 변수
    public TMP_InputField nickname;
    public TMP_InputField password;

    public Button loginBtn;

    public GameObject signUpUI;
    public GameObject findPasswordUI;

    public GameObject SignInFailUI;
    #endregion

    private bool isClick = false;

    private void Start()
    {
        loginBtn.interactable = false;
    }

    void Update()
    {
        if (!isClick)
        {
            loginBtn.interactable = CheckTextLength(nickname.text.Length, password.text.Length);
        }
    }

    public void SignUpClick()
    {
        nickname.text = string.Empty;
        password.text = string.Empty;
        signUpUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void FindPasswordClick()
    {
        nickname.text = string.Empty;
        password.text = string.Empty;
        findPasswordUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private bool CheckTextLength(int nicknameLength, int passwordLength)
    {
        return (nicknameLength >= 4 && passwordLength >= 4);
    }

    public void LoginClick()
    {
        if (CheckUserInfo(nickname.text, password.text))
        {
            print("로그인 되었습니다");
            isClick = true;
            loginBtn.interactable = false;
            PhotonConnManager.instance.userName = nickname.text;
            PhotonConnManager.instance.Connection();
        }
        else
        {
            nickname.text = string.Empty;
            password.text = string.Empty;
            SignInFailUI.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    private bool CheckUserInfo(string name, string password)
    {
        try
        {
            if (!DatabaseManager.Instance.Connection_Check(DatabaseManager.Instance.conn))
            {
                return false;
            }

            string userInfoSelect = $"SELECT count(*), userID FROM USER WHERE userName = '{name}' AND password = '{password}'";

            using (MySqlCommand cmd = new MySqlCommand(userInfoSelect, DatabaseManager.Instance.conn))
            {
                if (cmd != null)
                {
                    int result = GetRowCount(cmd);
                    DatabaseManager.Instance.userId = GetUserId(cmd);

                    return result > 0;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Select Query execution error: " + ex.Message);
        }
        return false;
    }
    int GetUserId(MySqlCommand cmd)
    {
        int userId = 0;

        using (MySqlDataReader reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                userId = reader.GetInt32(1);
            }
        }

        return userId;
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
        }

        return count;
    }
}

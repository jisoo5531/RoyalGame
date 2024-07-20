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
    #endregion

    private void Start()
    {
        loginBtn.interactable = false;
    }

    void Update()
    {
        loginBtn.interactable = CheckTextLength(nickname.text.Length, password.text.Length);
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
            loginBtn.interactable = false;
            PhotonConnManager.instance.userName = nickname.text;
            PhotonConnManager.instance.Connection();
        }
        else
        {
            print("회원 정보가 없거나, 아이디 혹은 비밀번호가 잘못 되었습니다.");
        }
    }

    private bool CheckUserInfo(string name, string password)
    {
        try
        {
            string userInfoSelect = string.Format("SELECT count(*) FROM USER WHERE userName = '{0}' AND password = '{1}'", name, password);

            MySqlCommand cmd = DatabaseManager.Instance.DBConnection(userInfoSelect);

            if (cmd != null)
            {
                int result = GetRowCount(cmd);

                return result > 0;
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Select Query execution error: " + ex.Message);
        }
        return false;
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

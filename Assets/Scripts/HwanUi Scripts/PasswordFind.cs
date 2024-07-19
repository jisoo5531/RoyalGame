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
    public TMP_InputField password;

    public Button findBtn;

    void Update()
    {
        findBtn.interactable = CheckTextLength(nickname.text.Length, password.text.Length);
    }

    private bool CheckTextLength(int nicknameLength, int passwordLength)
    {
        return (nicknameLength >= 4 && passwordLength >= 4);
    }

    private bool CheckEqualsUserName(string nickname)
    {

        try
        {
            string selectName = string.Format("SELECT count(*) FROM USER WHERE userName = '{0}'", nickname);

            MySqlCommand cmd = DatabaseManager.Instance.CreateCommand(selectName);

            if (cmd != null)
            {
                object result = cmd.ExecuteScalar();
                int rowCount = Convert.ToInt32(result);

                return rowCount > 0;
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Select Query execution error: " + ex.Message);
        }
        return false;
    }
}

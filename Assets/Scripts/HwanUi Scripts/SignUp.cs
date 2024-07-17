using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class SignUp : MonoBehaviour
{
    public TextMeshPro nickname;
    public TextMeshPro password;

    public Button loginBtn;

    private void Start()
    {
        loginBtn.interactable = false;
    }

    void Update()
    {
        if (int.Parse(nickname.text) < 4 || int.Parse(password.text) < 4)
            loginBtn.interactable = false;


        if (int.Parse(nickname.text) >= 4 && int.Parse(password.text) >= 4)
            loginBtn.interactable = true;

    }

    public void LoginClick()
    {
        if(CheckDuplicateName(nickname.text))
        {

        }
    }

    private void InsertUserData(string name, string password)
    {

    }

    private bool CheckDuplicateName(string name)
    {
        try
        {
            string nameSelect = string.Format("SELECT count(*) FROM USER WHERE userName = {0}", name);

            MySqlCommand cmd = DatabaseManager.Instance.CreateCommand(nameSelect);

            if (cmd != null)
            {
                object result = cmd.ExecuteScalar();
                int rowCount = Convert.ToInt32(result);

                return rowCount > 0;
            }
        }
        catch(Exception ex)
        {
            Debug.LogError("Query execution error: " + ex.Message);
        }
        return false;
    }
}

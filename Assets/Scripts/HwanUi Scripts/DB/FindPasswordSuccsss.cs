using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FindPasswordSuccsss : MonoBehaviour
{
    public TMP_Text passwordText;
    public PasswordFind passwordFind;

    private void Start()
    {
        if (passwordFind.userPassword != string.Empty)
        {
            passwordText.text = passwordFind.userPassword;
        }
    }
}

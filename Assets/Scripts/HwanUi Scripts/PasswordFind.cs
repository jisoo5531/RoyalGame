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
}

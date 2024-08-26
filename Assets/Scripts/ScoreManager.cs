using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int enemyCount = 0;
    public int allyCount = 0;

    public TextMeshProUGUI enemyText;
    public TextMeshProUGUI allyText;

    private void Awake()
    {
        instance = this;
    }

    public void SettingScore()
    {
        enemyText.text = enemyCount.ToString();
        allyText.text = allyCount.ToString();
    }
}

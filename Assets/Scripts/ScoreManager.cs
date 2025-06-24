using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int enemyCount = 0;
    public int allyCount = 0;

    public TextMeshProUGUI enemyText;
    public TextMeshProUGUI allyText;

    private void Awake()
    {
        Instance = this;
    }

    public void SettingScore()
    {
        enemyText.text = enemyCount.ToString();
        allyText.text = allyCount.ToString();
    }
}

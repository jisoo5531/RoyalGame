using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTimeUI : MonoBehaviour
{
    public TMP_Text gametimeUI;

    float setTime = 180;

    int min;
    float sec;

    void Update()
    {
        setTime -= Time.deltaTime;

        if (setTime >= 60f)
        {
            min = (int)setTime / 60;
            sec = setTime % 60;
            gametimeUI.text = min + " : " + (int)sec;
        }
        if (setTime < 60f)
        {
            gametimeUI.text = "0 : " + (int)setTime;
        }

        if (setTime <= 0)
        {
            gametimeUI.text = "0 : 00";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class TimeManager : MonoBehaviourPunCallbacks
{
    public static TimeManager instance;
    public TMP_Text gametimeUI;
    public TMP_Text overTimeUI;

    public bool isGameStart = false;

    int time;
    int min;
    int sec;

    private void Awake()
    {
        instance = this;
    }

    public void GameStart()
    {
        if (photonView.IsMine)
        {
            time = 180;

            StartCoroutine(TimerCoroution());
        }
    }

    IEnumerator TimerCoroution()
    {
        while (true)
        {
            if (time > 0)
            {
                time -= 1;
            }
            else
            {
                yield break;
            }

            photonView.RPC("ShowTimer", RpcTarget.All, time);

            yield return new WaitForSeconds(1f);
        }
    }


    [PunRPC]
    void ShowTimer(int number)
    {
        if (number >= 60f)
        {
            min = number / 60;
            sec = number % 60;
            gametimeUI.text = min + " : " + sec;
        }
        else
        {
            gametimeUI.text = $"0 : + {number}";
        }

        if (number <= 0)
        {
            gametimeUI.text = "0 : 00";
        }
    }

}

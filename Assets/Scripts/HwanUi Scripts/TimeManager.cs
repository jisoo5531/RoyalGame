using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class TimeManager : MonoBehaviourPunCallbacks
{
    public static TimeManager instance;
    private double startTime = 0;
    private float countdownDuration = 2f;
    private float gameCountdownDuration = 180f;
    public TMP_Text gametimeUI;
    public TMP_Text overTimeUI;
    private int time;

    public bool isGameStart = false;

    int min;
    float sec;

    private void Awake()
    {
        instance = this;
    }


    public void GameStart()
    {
        //if (photonView.IsMine)
        //{
        //    photonView.RPC("StartGameCountdown", RpcTarget.All);
        //}
        if (PhotonNetwork.IsMasterClient)
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
                Debug.Log("타이머 종료");
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
            gametimeUI.text = min + " : " + (int)sec;
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

    [PunRPC]
    private void StartGameCountdown()
    {

        StartCoroutine(GameCountDown());
    }

    private IEnumerator GameCountDown()
    {
        double endTime = startTime + gameCountdownDuration;
        print(PhotonNetwork.Time+",  "+ endTime);
        while (PhotonNetwork.Time < endTime)
        {
            print(PhotonNetwork.Time);
            double remainingTime = endTime - PhotonNetwork.Time;
            int secondsRemaining = Mathf.CeilToInt((float)remainingTime);
            if (secondsRemaining >= 60f)
            {
                min = secondsRemaining / 60;
                sec = secondsRemaining % 60;
                gametimeUI.text = min + " : " + (int)sec;
            }
            else
            {
                gametimeUI.text = "0 : " + secondsRemaining;
            }

            if (secondsRemaining <= 0)
            {
                gametimeUI.text = "0 : 00";
            }
            yield return new WaitForSeconds(1f);
        }
    }

}

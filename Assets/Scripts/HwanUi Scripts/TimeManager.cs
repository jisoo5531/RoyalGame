using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class TimeManager : MonoBehaviourPunCallbacks
{
    public static TimeManager instance;
    private float timer;
    private float countdownDuration = 2f;
    private float gameCountdownDuration = 180f;
    public TMP_Text gametimeUI;
    public TMP_Text overTimeUI;

    int min;
    float sec;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        photonView.RPC("CountdownBeforeStartGame", RpcTarget.All);
        //CountdownBeforeStartGame();
    }

    public void CountdownBeforeGame()
    {
        //photonView.RPC("CountdownBeforeStartGame", RpcTarget.All);
        CountdownBeforeStartGame();
    }


    [PunRPC]
    private void CountdownBeforeStartGame()
    {
        timer = (float)PhotonNetwork.Time;
        StartCoroutine(CountdownTimer());
    }

    private IEnumerator CountdownTimer()
    {
        float endTime = timer + countdownDuration;
        while (PhotonNetwork.Time < endTime)
        {
            yield return null;
        }
        GameManager.instance.GameStart();
        Countdown();
    }

    public void Countdown()
    {
        photonView.RPC("StartGameCountdown", RpcTarget.All);
    }

    [PunRPC]
    private void StartGameCountdown()
    {
        timer = Time.time;
        StartCoroutine(GameCountDown());
    }

    private IEnumerator GameCountDown()
    {
        while (Time.time < timer + gameCountdownDuration)
        {
            float remainingTime = timer + gameCountdownDuration - Time.time;
            int secondsRemaining = Mathf.CeilToInt(remainingTime);

            if (secondsRemaining >= 60f)
            {
                min = (int)secondsRemaining / 60;
                sec = secondsRemaining % 60;
                gametimeUI.text = min + " : " + (int)sec;
            }
            if (secondsRemaining < 60f)
            {
                gametimeUI.text = "0 : " + (int)secondsRemaining;
            }

            if (secondsRemaining <= 0)
            {
                gametimeUI.text = "0 : 00";
            }
            yield return new WaitForSeconds(1f);
        }
    }

}

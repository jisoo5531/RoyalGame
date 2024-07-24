using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class TimeManager : MonoBehaviourPunCallbacks
{
    public static TimeManager instance;
    private double startTime;
    private float countdownDuration = 2f;
    private float gameCountdownDuration = 180f;
    public TMP_Text gametimeUI;
    public TMP_Text overTimeUI;
    private AudioSource audioSource;


    public bool isGameStart = false;

    int min;
    float sec;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startTime = PhotonNetwork.Time + countdownDuration;
            photonView.RPC("SetStartTime", RpcTarget.All, startTime);
            SoundStart();
        }
    }

    private void SoundStart()
    {
        audioSource.Play();
    }

    [PunRPC]
    private void SetStartTime(double networkStartTime)
    {
        startTime = networkStartTime;
        StartCoroutine(CountdownBeforeStartGame());   
    }

    private IEnumerator CountdownBeforeStartGame()
    {
        while (PhotonNetwork.Time < startTime)
        {
            yield return null;
        }
        photonView.RPC("NotifyGameStart", RpcTarget.All);
    }

    [PunRPC]
    private void NotifyGameStart()
    {
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
        StartCoroutine(GameCountDown());
    }

    private IEnumerator GameCountDown()
    {
        double endTime = startTime + gameCountdownDuration;
        while (PhotonNetwork.Time < endTime)
        {
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

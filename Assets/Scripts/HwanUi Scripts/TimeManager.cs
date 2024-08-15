using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class TimeManager : MonoBehaviourPunCallbacks
{
    #region public 변수
    public static TimeManager instance;
    public TMP_Text gametimeUI;
    public TMP_Text overTimeUI;
    public Animator animator;
    public GameObject suddenDeath;
    public GameObject elixirTwiceUI;

    public bool isGameStart = false;
    public bool isOverTiem = false;
    #endregion

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
                if (!isOverTiem)
                {
                    isOverTiem = true;
                    photonView.RPC("StartOverTime", RpcTarget.All);
                    time = 120;
                }
                else
                {
                    photonView.RPC("GameEnd", RpcTarget.All);
                    yield break;
                }
            }

            if(PhotonNetwork.IsMasterClient)
            {
                if(MasterManager.instance.towers.Count == 0)
                {
                    photonView.RPC("GameEnd", RpcTarget.All);
                    yield break;
                }
            }
            else
            {
                if (NonMasterManager.instance.towers.Count == 0)
                {
                    photonView.RPC("GameEnd", RpcTarget.All);
                    yield break;
                }
            }

            photonView.RPC("ShowTimer", RpcTarget.All, time);

            yield return new WaitForSeconds(1f);
        }
    }

    [PunRPC]
    private void GameEnd()
    {
        animator.SetTrigger("GameEnd");
        GameManager.instance.isGameEnd = true;
    }

    [PunRPC]
    void StartOverTime()
    {
        overTimeUI.text = "오버타임";
        overTimeUI.color = Color.red;
        gametimeUI.color = Color.red;
        suddenDeath.SetActive(true);
        elixirTwiceUI.SetActive(true);
        Invoke("SuddenDeathLifeTime", 4f);
    }

    private void SuddenDeathLifeTime()
    {
        suddenDeath.SetActive(false);
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
            gametimeUI.text = $"0 : {number}";
        }
    }

}

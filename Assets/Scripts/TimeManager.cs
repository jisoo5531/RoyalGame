using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;

public class TimeManager : MonoBehaviourPunCallbacks
{
    #region public 변수
    public static TimeManager instance;
    public TMP_Text gametimeUI;
    public TMP_Text overTimeUI;
    public Animator animator;
    public GameObject suddenDeath;
    public GameObject elixirTwiceUI;
    public SoundBattle soundBattle;

    public bool isGameStart = false;
    public bool isOverTiem = false;
    public bool isTimeZero = false;
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
                if (!CheckEndCondition())
                {
                    if (!isOverTiem)
                    {
                        isTimeZero = true;
                        isOverTiem = true;
                        photonView.RPC("StartOverTime", RpcTarget.All);
                        time = 120;
                    }
                    else
                    {
                        GameEnd();
                        yield break;
                    }
                }
                else
                {
                    GameEnd();
                    yield break;
                }
            }

            if(PhotonNetwork.IsMasterClient)
            {
                if(MasterManager.instance.towers.Count == 0)
                {
                    GameEnd();
                    yield break;
                }
            }
            else
            {
                if (NonMasterManager.instance.towers.Count == 0)
                {
                    GameEnd();
                    yield break;
                }
            }

            photonView.RPC("ShowTimer", RpcTarget.All, time);

            yield return new WaitForSeconds(1f);
        }
    }

    private bool CheckEndCondition()
    {
        if(ScoreManager.instance.allyCount != ScoreManager.instance.enemyCount)
        {
            return true;
        }
        return false;
    }

    public void GameEnd()
    {
        photonView.RPC("PunRPC_GameEnd", RpcTarget.All);
    }

    [PunRPC]
    private void PunRPC_GameEnd()
    {
        animator.SetTrigger("GameEnd");
        GameManager.instance.isGameEnd = true;
    }

    [PunRPC]
    void StartOverTime()
    {
        soundBattle.CheckTime(0, true);
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
        soundBattle.CheckTime(number, false);
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

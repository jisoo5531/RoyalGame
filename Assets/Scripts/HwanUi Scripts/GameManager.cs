using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region pubilc º¯¼ö
    public static GameManager instance;

    public Transform firstCamera;
    public Transform secondCamera;

    public Transform firstLight;
    public Transform secondLight;

    public GameObject cameraPrefab;
    public GameObject lightPrefab;

    public GameObject vsUI;
    #endregion
    private void Awake()
    {
        instance = this;

        if (photonView.IsMine)
        {
            Instantiate(cameraPrefab, firstCamera.position, firstCamera.rotation);
            Instantiate(lightPrefab, firstLight.position, firstLight.rotation);
        }
        else
        {
            Instantiate(cameraPrefab, secondCamera.position, secondCamera.rotation);
            Instantiate(lightPrefab, secondLight.position, secondLight.rotation);
        }
    }

    private void Start()
    {
        //if (PhotonNetwork.IsMasterClient)
        //{
            //TimeManager.instance.CountdownBeforeGame();
       // }
    }

    public void GameStart()
    {
        vsUI.SetActive(false);
    }
}

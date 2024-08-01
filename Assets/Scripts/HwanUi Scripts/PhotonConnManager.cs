using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonConnManager : MonoBehaviourPunCallbacks
{
    public static PhotonConnManager instance;
    public Animator fadeIn;

    public string userName = string.Empty;

    private readonly string gameVersion = "1";

    private void Awake()
    {
        instance = this;
    }

    public void Connection()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "kr";
        PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "75d8d2d9-2e37-43cc-895d-c607d2fe84e1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        try
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
    public override void OnConnectedToMaster()
    {
        try
        {
            if (userName != string.Empty)
            {
                PhotonNetwork.LocalPlayer.NickName = userName;
            }
            PhotonNetwork.JoinLobby();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public override void OnJoinedLobby()
    {
        fadeIn.SetTrigger("FadeIn");
        //StartCoroutine(PreloadLoadingScene());
    }

    IEnumerator PreloadLoadingScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("Loading");
        op.allowSceneActivation = false;

        yield return new WaitForSeconds(0.35f);

        op.allowSceneActivation = true;
        SceneManager.LoadScene("Loading");
        //Loading.LoadScene("Lobby", false);
    }
}


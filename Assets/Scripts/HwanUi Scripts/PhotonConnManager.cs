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

    public string userName = string.Empty;

    private void Awake()
    {
        instance = this;
    }

    public void Connection()
    {
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
        Loading.LoadScene("Lobby");
    }
}


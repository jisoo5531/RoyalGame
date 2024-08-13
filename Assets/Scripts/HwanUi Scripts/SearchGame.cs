using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using MySql.Data.MySqlClient;
using System.Text;
using UnityEngine.SceneManagement;

public class SearchGame : MonoBehaviourPunCallbacks
{
    public GameObject lobbyUI;
    public GameObject matchingUI;

    public GameObject warningUI;

    private int maxPlayerCount = 2;

    public void MatchingClick()
    {
        StringBuilder battleCard = new StringBuilder();
        try
        {
            if (StartManager.m_Instance.battleCardList.Count < 8 || !PhotonNetwork.IsConnected)
            {
                warningUI.SetActive(true);
                return;
            }
            lobbyUI.SetActive(false);
            matchingUI.SetActive(true);

            for (int i = 0; i < StartManager.m_Instance.battleCardList.Count; i++)
            {
                battleCard.Append(StartManager.m_Instance.battleCardList[i]);
                if (i < StartManager.m_Instance.battleCardList.Count - 1)
                {
                    battleCard.Append(",");
                }
            }

            StartManager.m_Instance.UpdateUserCard(battleCard.ToString());


            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayerCount;                                
            roomOptions.IsVisible = true;
            roomOptions.IsOpen = true;
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "maxTime", 300 } };
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "maxTime" };

            string roomName = "test1";
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }
    public override void OnJoinedRoom()
    {
        try
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            //PhotonNetwork.LoadLevel("Battle");
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }

    [PunRPC]
    private void LoadScene()
    {
        //Loading.LoadScene("Battle");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)  
    {
        try
        {
            if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                PhotonNetwork.LoadLevel("Battle");
               // photonView.RPC("LoadScene", RpcTarget.All);
            }

        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        print($"{otherPlayer.NickName}가 방을 떠났습니다");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning(message);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning(message);
    }
}

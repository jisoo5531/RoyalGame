using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using MySql.Data.MySqlClient;

public class SearchGame : MonoBehaviourPunCallbacks
{
    public GameObject lobbyUI;
    public GameObject matchingUI;

    public void MatchingClick()
    {

        try
        {
            if (StartManager.m_Instance.battleCardList.Count < 8)
            {
                // 팝업창
                return;
            }
            lobbyUI.SetActive(false);
            matchingUI.SetActive(true);

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "maxTime", 300 } };
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "maxTime" };

            string roomName = "test";

            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
    public override void OnJoinedRoom()
    {
        print("입장 성공");
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        try
        {
            print($"{newPlayer.NickName}가 방에 입장했습니다");

            if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                PhotonNetwork.LoadLevel("Battle");
            }
        }
        catch(Exception ex)
        {
            print(ex.Message);
        }
        //int newPlayerTrophy = GetPlayerTrophy(newPlayer.UserId);

        //int[] trophies = new int[PhotonNetwork.CurrentRoom.PlayerCount];
        //int i = 0;
        //foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        //{
        //    trophies[i++] = GetPlayerTrophy(player.NickName);
        //}

        //bool isAllowed = true;
        //foreach (int trophy in trophies)
        //{
        //    if (Mathf.Abs(trophy - newPlayerTrophy) > 100)
        //    {
        //        isAllowed = false;
        //        break;
        //    }
        //}

        //if (isAllowed)
        //{
        //    if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
        //    {
        //        PhotonNetwork.LoadLevel("Battle");
        //    }
        //}
        //else
        //{
        //    newPlayer.kick
        //    PhotonNetwork.CurrentRoom.RemovePlayer(player);
        //}
    }

    //private int GetPlayerTrophy(string userName)
    //{
    //    int trophy = 0;

    //    using (MySqlConnection connection = DatabaseManager.Instance.DBConnection())
    //    {
    //        connection.Open();

    //        string query = "SELECT currentTrophy FROM USER WHERE userName = @userName";
    //        using (MySqlCommand command = new MySqlCommand(query, connection))
    //        {
    //            command.Parameters.AddWithValue("@userName", userName);
    //            using (MySqlDataReader reader = command.ExecuteReader())
    //            {
    //                if (reader.Read())
    //                {
    //                    trophy = reader.GetInt32(0);
    //                }
    //            }
    //        }

    //        connection.Close();
    //    }

    //    return trophy;
    //}

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

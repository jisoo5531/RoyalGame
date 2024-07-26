using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities.IO.Pem;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
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

    public TMP_Text[] playerNames;
    public TMP_Text[] playerTrophy;

    public GameObject[] enemyTowers;

    public GameObject[] currentAllyTowers = new GameObject[3];
    public GameObject[] currentEnemyTowers = new GameObject[3];

    public Transform[] myTowersTranform;
    public Transform[] enemyTowersTranform;

    public Transform[] myTowersHp;
    public Transform[] enemyTowersHp;

    public Transform[] AllyTowerHpInAlly;
    public Transform[] EnemyTowerHpInAlly;

    public Transform[] AllyTowerHpInEnemy;
    public Transform[] EnemyTowerHpInEnemy;

    public Sprite[] hpSprite;

    public Material[] allyMaterial;

    public GridController grid;

    public IsMineManager isMineManager;
    #endregion
    private void Awake()
    {
        instance = this;
        if (!PhotonNetwork.IsMasterClient)
        {
            Instantiate(cameraPrefab, firstCamera.position, firstCamera.rotation);
            Instantiate(lightPrefab, firstLight.position, firstLight.rotation);
        }
        else
        {
            Instantiate(cameraPrefab, secondCamera.position, secondCamera.rotation);
            Instantiate(lightPrefab, secondLight.position, secondLight.rotation);
        }

        int[] playerKeys = PhotonNetwork.CurrentRoom.Players.Keys.ToArray();

        for (int i = 0; i < playerKeys.Length; i++)
        {
            int key = playerKeys[i];
            Player player = PhotonNetwork.CurrentRoom.Players[key];
            playerTrophy[i].text = SelectTrophy(player.NickName).ToString();
            playerNames[i].text = player.NickName;
        }
    }
    private void Start()
    {
        try
        {
            if (PhotonNetwork.IsConnected)
            {
                GameObject mineManager = PhotonNetwork.Instantiate("IsMineManager", Vector3.zero, Quaternion.identity);
                isMineManager = mineManager.GetComponent<IsMineManager>();
            }
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }

    public bool PositionCheck()
    {
        int localNum = PhotonNetwork.LocalPlayer.ActorNumber;

        if (localNum % 2 == 0)
            return true;


        return false;
    }

    private int SelectTrophy(string name)
    {
        try
        {
            string nameSelect = $"SELECT currentTrophy FROM USER WHERE userName = '{name}'";

            using (MySqlConnection conn = DatabaseManager.Instance.DBConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(nameSelect, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int trophy = reader.GetInt32(0);

                            return trophy;
                        }
                    }
                }
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Select Query execution error: " + ex.Message);
        }
        return 0;
    }


    public void GameStart()
    {
        vsUI.SetActive(false);
    }
}

using MySql.Data.MySqlClient;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region pubilc º¯¼ö
    public static GameManager instance;

    public Transform flares;

    public Transform firstCamera;
    public Transform secondCamera;

    public Transform firstLight;
    public Transform secondLight;

    public GameObject flaresPrefab;

    public GameObject cameraPrefab;
    public GameObject lightPrefab;

    public TMP_Text[] playerNames;
    public TMP_Text[] playerTrophy;

    public GameObject[] enemyTowers;

    public GameObject[] currentEnemyTowers = new GameObject[3];

    public Transform[] myTowersTranform;
    public Transform[] enemyTowersTranform;

    public Transform[] allyTowerHp;
    public Transform[] enemyTowerHp;

    public Transform allyFlares;
    public Transform enemyFlares;

    public Sprite[] hpSprite;

    public Material[] allyMaterial;

    public GridController grid;

    public UnitSpawner unitSpawner;

    public bool isGameEnd = false;
    public bool isStart = false;

    public GameObject winUI;
    public GameObject loseUI;

    public GameObject[] winCrownCount;
    public GameObject[] loseCrownCount;

    public TextMeshProUGUI trophyText;
    public TextMeshProUGUI winGoldText;

    public TextMeshProUGUI loseGoldText;
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            Instantiate(cameraPrefab, firstCamera.position, firstCamera.rotation);
            Instantiate(lightPrefab, firstLight.position, firstLight.rotation);
            Instantiate(flaresPrefab, enemyFlares.position, Quaternion.identity);
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            Instantiate(cameraPrefab, secondCamera.position, secondCamera.rotation);
            Instantiate(lightPrefab, secondLight.position, secondLight.rotation);
            Instantiate(flaresPrefab, allyFlares.position, Quaternion.identity);
        }
        unitSpawner.mainCamera = Camera.main;

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
                CreateManager();
            }
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }

    private void CreateManager()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("MasterManager", Vector3.one, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("NonMasterManager", Vector3.one, Quaternion.identity);
        }
    }

    private int SelectTrophy(string name)
    {
        try
        {
            if (!DatabaseManager.Instance.Connection_Check(DatabaseManager.Instance.conn))
            {
                return 0;
            }

            string nameSelect = $"SELECT userID, currentTrophy FROM USER WHERE userName = '{name}'";

            using (MySqlCommand cmd = new MySqlCommand(nameSelect, DatabaseManager.Instance.conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int userId = reader.GetInt32(0);
                        int trophy = reader.GetInt32(1);

                        if (!name.Equals(PhotonNetwork.NickName))
                        {
                            DatabaseBattleRecordModel.enemyID = userId;
                        }

                        return trophy;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Select Query execution error: " + ex.Message);
        }
        return 0;
    }

    private void Update()
    {
        if (isGameEnd && !isStart)
        {
            isStart = true;
            Invoke("GameResult", 1.5f);
        }
    }

    private void GameResult()
    {
        if (ScoreManager.instance.allyCount > ScoreManager.instance.enemyCount)
        {
            winUI.SetActive(true);
            for (int i = 0; i < ScoreManager.instance.allyCount; i++)
            {
                winCrownCount[i].SetActive(true);
            }
            int trophy = int.Parse(trophyText.text);
            int gold = int.Parse(winGoldText.text);
            DatabaseBattleRecordModel.UpdateUser(1, 1, 0, trophy, gold);
        }
        else
        {
            loseUI.SetActive(true);
            for (int i = 0; i < ScoreManager.instance.allyCount; i++)
            {
                loseCrownCount[i].SetActive(true);
            }
            int trophy = int.Parse(trophyText.text);
            int gold = int.Parse(loseGoldText.text);
            DatabaseBattleRecordModel.UpdateUser(1, 0, 1, 0, gold);
        }
    }

    public void LobbySceneLoad_Click()
    {
        DatabaseBattleRecordModel.GetBattleCard();
        DatabaseBattleRecordModel.InsertGameRecord(ScoreManager.instance.allyCount, ScoreManager.instance.enemyCount);

        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
        Loading.LoadScene("Lobby");
    }
}

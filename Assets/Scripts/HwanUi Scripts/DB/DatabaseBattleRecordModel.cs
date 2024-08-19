using MySql.Data.MySqlClient;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DatabaseBattleRecordModel : MonoBehaviour
{
    public static int enemyID;
    public static string allyBattleCard;
    public static string enemyBattleCard;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static void UpdateUser(int battleCount, int victoryCount, int defeatCount, int currentTrophy, int gold)
    {
        string updateUser = string.Empty;

        try
        {
            if (!DatabaseManager.Instance.Connection_Check(DatabaseManager.Instance.conn))
            {
                return;
            }

            updateUser = $"UPDATE USER SET " +
             $"battleCount = battleCount + {battleCount}, " +
             $"victoryCount = victoryCount + {victoryCount}, " +
             $"defeatCount = defeatCount + {defeatCount}, " +
             $"currentTrophy = currentTrophy + {currentTrophy}, " +
             $"gold = gold + {gold}, " +
             $"maxTrophy = CASE WHEN (currentTrophy + {currentTrophy}) > maxTrophy THEN (currentTrophy + {currentTrophy}) ELSE maxTrophy END " +
             $"WHERE userID = {DatabaseManager.Instance.userId}";

            using (MySqlCommand cmd = new MySqlCommand(updateUser, DatabaseManager.Instance.conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }

    public static void InsertGameRecord(int allyCrownCount, int enemyCrownCount)
    {
        try
        {
            if (!DatabaseManager.Instance.Connection_Check(DatabaseManager.Instance.conn))
            {
                return;
            }

            string insertQuery = $"INSERT INTO GAME_RECORD(userId, user_CrownCount, user_CurrentBattleCard, " +
                $"enemy_UserId, enemy_CrownCount, enemy_CurrentBattleCard) " +
                $"VALUES(@userID, @allyCrownCount, @allyBattleCard, @enemyID, @enemyCrownCount, @enemyBattleCard);";

            using (MySqlCommand cmd = new MySqlCommand(insertQuery, DatabaseManager.Instance.conn))
            {
                if (cmd != null)
                {
                    cmd.Parameters.AddWithValue("@userID", DatabaseManager.Instance.userId);
                    cmd.Parameters.AddWithValue("@allyCrownCount", allyCrownCount);
                    cmd.Parameters.AddWithValue("@allyBattleCard", allyBattleCard);
                    cmd.Parameters.AddWithValue("@enemyID", enemyID);
                    cmd.Parameters.AddWithValue("@enemyCrownCount", enemyCrownCount);
                    cmd.Parameters.AddWithValue("@enemyBattleCard", enemyBattleCard);

                    cmd.ExecuteReader();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public static void GetBattleCard()
    {
        allyBattleCard = SelectUser(DatabaseManager.Instance.userId);
        enemyBattleCard = SelectUser(enemyID);
    }

    private static string SelectUser(int userId)
    {
        try
        {
            if (!DatabaseManager.Instance.Connection_Check(DatabaseManager.Instance.conn))
            {
                return null;
            }

            string userSelect = $"SELECT currentBattleCard FROM USER WHERE userID = '{userId}'";

            using (MySqlCommand cmd = new MySqlCommand(userSelect, DatabaseManager.Instance.conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string currentBattleCard = reader.GetString(0);

                        return currentBattleCard;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        return null;
    }

    public static List<(int userId, int recordId)> SelectBattleRecord(int userId)
    {
        List<(int userId, int recordId)> userIDList = new List<(int userId, int recordId)>();
        try
        {
            if (!DatabaseManager.Instance.Connection_Check(DatabaseManager.Instance.conn))
            {
                return null;
            }

            string userSelect = $"SELECT enemy_UserId, game_RecordID FROM GAME_RECORD WHERE userId = '{userId}'";

            using (MySqlCommand cmd = new MySqlCommand(userSelect, DatabaseManager.Instance.conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int enemyUserID = reader.GetInt32(0);
                        int recordId = reader.GetInt32(1);

                        userIDList.Add((enemyUserID, recordId));
                    }
                    return userIDList;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
        return null;
    }

    public static GameResultUserInfo SelectAllyResultInfo(int userId, int recordID)
    {
        GameResultUserInfo userGameInfo = null;
        try
        {
            if (!DatabaseManager.Instance.Connection_Check(DatabaseManager.Instance.conn))
            {
                return null;
            }

            string userCardQuery = $"SELECT gr.user_CurrentBattleCard AS battleCard, gr.user_CrownCount AS crownCount, u.userName AS name " +
                       $"FROM GAME_RECORD gr " +
                       $"JOIN USER u ON gr.userId = u.userID " +
                       $"WHERE gr.userId = {userId} AND gr.game_RecordID = {recordID}";

            using (MySqlCommand cmd = new MySqlCommand(userCardQuery, DatabaseManager.Instance.conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userGameInfo = new GameResultUserInfo(
                            reader.GetString("name"),
                            reader.GetInt32("crownCount"),
                            reader.GetString("battleCard")
                            );
                    }
                    return userGameInfo;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        return null;
    }

    public static GameResultUserInfo SelectEnemyResultInfo(int userId, int recordID)
    {
        GameResultUserInfo userGameInfo = null;
        try
        {
            if (!DatabaseManager.Instance.Connection_Check(DatabaseManager.Instance.conn))
            {
                return null;
            }

            string userCardQuery = $"SELECT gr.enemy_CurrentBattleCard AS battleCard, gr.enemy_CrownCount AS crownCount, u.userName AS name " +
                       $"FROM GAME_RECORD gr " +
                       $"JOIN USER u ON gr.enemy_UserId = u.userID " +
                       $"WHERE gr.userId = {userId} AND gr.game_RecordID = {recordID}";

            using (MySqlCommand cmd = new MySqlCommand(userCardQuery, DatabaseManager.Instance.conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        userGameInfo = new GameResultUserInfo(
                            reader.GetString("name"),
                            reader.GetInt32("crownCount"),
                            reader.GetString("battleCard")
                            );
                    }
                    return userGameInfo;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        return null;
    }

    public static List<GameResultCardInfo> SelectCurrentBattleCard(string card, int userId, int recordID, bool isAlly)
    {
        List<GameResultCardInfo> userGameInfoList = new List<GameResultCardInfo>();
        try
        {
            string[] cardIds = card.Split(',');

            foreach (string cardId in cardIds)
            {
                string userSelect = string.Empty;

                if (isAlly)
                {
                    userSelect = $"SELECT " +
                        $"c.cardID AS id, " +
                        $"c.grade AS grade, " +
                        $"COALESCE((SELECT m.level FROM MAGIC m WHERE m.userID = {userId} AND m.cardID = {cardId}), " +
                        $"(SELECT dt.level FROM DEFENSE_TOWER dt WHERE dt.userID = {userId} AND dt.cardID = {cardId}), " +
                        $"(SELECT un.level FROM UNIT un WHERE un.userID = {userId} AND un.cardID = {cardId})" +
                        $") AS selectedLevel " +
                        $"FROM GAME_RECORD gr, CARD c " +
                        $"WHERE gr.userId = {userId} AND gr.game_RecordID = {recordID}";
                }
                else
                {
                    userSelect = $"SELECT " +
                        $"c.cardID AS id, " +
                        $"c.grade AS grade, " +
                        $"COALESCE((SELECT m.level FROM MAGIC m WHERE m.userID = {userId} AND m.cardID = {cardId}), " +
                        $"(SELECT dt.level FROM DEFENSE_TOWER dt WHERE dt.userID = {userId} AND dt.cardID = {cardId}), " +
                        $"(SELECT un.level FROM UNIT un WHERE un.userID = {userId} AND un.cardID = {cardId})" +
                        $") AS selectedLevel " +
                        $"FROM GAME_RECORD gr, CARD c " +
                        $"WHERE gr.enemy_UserId = {userId} AND gr.game_RecordID = {recordID}";
                }

                using (MySqlCommand cmd = new MySqlCommand(userSelect, DatabaseManager.Instance.conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GameResultCardInfo userGameInfo = new GameResultCardInfo(
                                reader.GetInt32("id"),
                                reader.GetString("grade"),
                                reader.GetInt32("selectedLevel")
                            );

                            userGameInfoList.Add(userGameInfo);
                        }
                        return userGameInfoList;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
        return null;
    }
}

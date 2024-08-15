using MySql.Data.MySqlClient;
using Photon.Pun;
using System;
using UnityEngine;

public class DatabaseModel : MonoBehaviour
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
            if (!DatabaseManager.Instance.connection_check(DatabaseManager.Instance.conn))
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
            if (!DatabaseManager.Instance.connection_check(DatabaseManager.Instance.conn))
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

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Debug.Log("데이터 삽입 성공");
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("데이터 삽입 실패");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Insert Query execution error: " + ex.Message);
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
            if (!DatabaseManager.Instance.connection_check(DatabaseManager.Instance.conn))
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
            Debug.LogWarning("Select Query execution error: " + ex.Message);
        }

        return null;
    }
}

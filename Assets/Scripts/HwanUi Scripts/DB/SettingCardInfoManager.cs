using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingCardInfoManager : MonoBehaviour
{
    #region public º¯¼ö
    public static SettingCardInfoManager instance;
    public List<CharacterData> charData = new List<CharacterData>();
    public GameObject[] slots;
    #endregion

    private void Awake()
    {
        instance = this;
    }

    public void InsertNewCard(int index, int damage, int hp, float attackSpeed, int moveSpeed)
    {
        string insertCard = string.Empty;
        try
        {
            if (!DatabaseManager.Instance.connection_check(DatabaseManager.Instance.conn))
            {
                return;
            }

            if (index != 2 && index != 7)
            {
                insertCard = $"INSERT INTO UNIT(userID, cardID, damage, level, hp, attackSpeed, moveSpeed, currentCardCount, maxCardCount, spawnTime) VALUES(@userId, @cardID, {damage}, 1, {hp}, {attackSpeed}, {moveSpeed}, 3, 2, 1)";
            }
            else if (index == 2)
            {
                insertCard = $"INSERT INTO MAGIC(userID, cardID, Level, currentCardCount, maxCardCount, unit_Damage, tower_Damage) VALUES(@userId, @cardID, 1, 1, 2, 310, 100)";
            }
            else if (index == 7)
            {
                insertCard = $"INSERT INTO DEFENSE_TOWER(userID, cardID, damage, level, hp, lifeTime, currentCardCount, maxCardCount, spawnTime, attackSpeed) VALUES(@userId, @cardID, 26, 1, 1000, 30, 1, 2, 3.5, 1.6)";
            }

            using (MySqlCommand cmd = new MySqlCommand(insertCard, DatabaseManager.Instance.conn))
            {
                cmd.CommandText = insertCard;
                cmd.Parameters.AddWithValue("@userId", DatabaseManager.Instance.userId);
                cmd.Parameters.AddWithValue("@cardID", index + 1);

                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }

    public void UpdateCard(int cardId, int upAmount)
    {
        string updateCard = string.Empty;
        try
        {
            if (!DatabaseManager.Instance.connection_check(DatabaseManager.Instance.conn))
            {
                return;
            }

            if (cardId != 3 && cardId != 8)
            {
                updateCard = $"UPDATE UNIT SET currentCardCount = currentCardCount + @amount WHERE userID = {DatabaseManager.Instance.userId} AND cardID = {cardId}";
            }
            else if (cardId == 3)
            {
                updateCard = $"UPDATE MAGIC SET currentCardCount = currentCardCount + @amount WHERE userID = {DatabaseManager.Instance.userId} AND cardID = {cardId}";
            }
            else if (cardId == 8)
            {
                updateCard = $"UPDATE DEFENSE_TOWER SET currentCardCount = currentCardCount + @amount WHERE userID = {DatabaseManager.Instance.userId} AND cardID = {cardId}";
            }

            using (MySqlCommand cmd = new MySqlCommand(updateCard, DatabaseManager.Instance.conn))
            {
                cmd.Parameters.AddWithValue("@amount", upAmount);

                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }

    public void UpdateUserInfo(int amount)
    {
        string updateUserCard = string.Empty;
        try
        {
            if (!DatabaseManager.Instance.connection_check(DatabaseManager.Instance.conn))
            {
                return;
            }

            updateUserCard = $"UPDATE USER SET currentCardCount = {amount} WHERE userID = {DatabaseManager.Instance.userId}";

            using (MySqlCommand cmd = new MySqlCommand(updateUserCard, DatabaseManager.Instance.conn))
            {
                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} row(s) updated.");
            }
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }

    public bool IsNewbie()
    {
        try
        {
            if (!DatabaseManager.Instance.connection_check(DatabaseManager.Instance.conn))
            {
                return true;
            }

            string nameSelect = $"SELECT currentCardCount FROM USER WHERE userID = '{DatabaseManager.Instance.userId}'";

            using (MySqlCommand cmd = new MySqlCommand(nameSelect, DatabaseManager.Instance.conn))
            {
                if (cmd != null)
                {
                    int result = GetCardCount(cmd);

                    return result < 8;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Select Query execution error: " + ex.Message);
        }
        return true;
    }

    int GetCardCount(MySqlCommand cmd)
    {
        int count = 0;

        using (MySqlDataReader reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                count = reader.GetInt32(0);
            }
        }

        return count;
    }
}

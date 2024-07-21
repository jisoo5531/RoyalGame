using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingCardInfoManager : MonoBehaviour
{
    public static SettingCardInfoManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void InsertNewCard(int index)
    {
        string insertCard = string.Empty;
        try
        {
            if (index != 3 && index != 8)
            {
                insertCard = $"INSERT INTO UNIT(userID, cardID, damage, level, hp, attackSpeed, moveSpeed, currentCardCount, maxCardCount, spawnTime) VALUES({DatabaseManager.Instance.userId}, {index}, 86, 1, 759, 1.2, 3, 1, 2, 1)";
            }
            else if (index == 3)
            {
                insertCard = $"INSERT INTO MAGIC(userID, cardID, Level, currentCardCount, maxCardCount, unit_Damage, tower_Damage) VALUES({DatabaseManager.Instance.userId}, {index}, 1, 1, 2, 310, 100)";
            }
            else if (index == 8)
            {
                insertCard = $"INSERT INTO DEFENSE_TOWER(userID, cardID, damage, level, hp, lifeTime, currentCardCount, maxCardCount, spawnTime, attackSpeed) VALUES({DatabaseManager.Instance.userId}, {index}, 26, 1, 1000, 30, 1, 2, 3.5, 1.6)";
            }

            using (MySqlCommand cmd = DatabaseManager.Instance.DBConnection(insertCard))
            {
                int rowsAffected = cmd.ExecuteNonQuery();

                print((rowsAffected > 0) ? "성공" : "실패");
            }
        }
        catch(Exception ex)
        {
            print(ex.Message);
        }
    }

    public void UpdateCard(int cardId, int upAmount)
    {
        string updateCard = string.Empty;
        try
        {
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

            using (MySqlCommand cmd = DatabaseManager.Instance.DBConnection(updateCard))
            {
                cmd.Parameters.AddWithValue("@amount", upAmount);

                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} row(s) updated.");
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
            updateUserCard = $"UPDATE UNIT SET currentCardCount = {amount} WHERE userID = {DatabaseManager.Instance.userId}";

            using (MySqlCommand cmd = DatabaseManager.Instance.DBConnection(updateUserCard))
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
}

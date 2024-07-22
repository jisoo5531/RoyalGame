using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SettingUnitSetting : MonoBehaviour
{
    private void SelectUnitData(int cardId)
    {
        string selectData = string.Empty;

        selectData = $"SELECT CARD.cardID, CARD.name, CARD.cost, CARD.grade, CARD.range, CARD.detectRange, " +
            $"UNIT.level, UNIT.damage, CARD.type, UNIT.attackSpeed, UNIT.moveSpeed, UNIT.hp, UNIT.spawnTime " +
            $"FROM USER, CARD, UNIT WHERE USER.userID = {cardId} AND USER.userID = UNIT.userID AND CARD.cardID = UNIT.cardID";

        using (MySqlConnection conn = DatabaseManager.Instance.DBConnection())
        {
            using (MySqlCommand cmd = new MySqlCommand(selectData, conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        UnitInfoData cardInfo = new UnitInfoData
                        {
                            cardId = id,
                            cardName = reader.GetString(1),
                            cost = reader.GetInt32(2),
                            grade = reader.GetString(3),
                            range = reader.GetFloat(4),
                            detectRange = reader.GetFloat(5),
                            level = reader.GetInt32(6),
                            damage = reader.GetInt32(7),
                            type = reader.GetString(8),
                            attackSpeed = reader.GetFloat(9),
                            moveSpeed = reader.GetInt32(10),
                            hp = reader.GetInt32(11),
                            spawnTime = reader.GetInt32(12),
                        };
                        UI_Manager.m_Instance.UnitDatas.Add(cardInfo);
                    }
                }
            }
            conn.Close();
        }
    }

    private void SelectTowerData(int cardId)
    {
        string selectData = string.Empty;

        selectData = $"SELECT CARD.cardID, CARD.name, CARD.cost, CARD.grade, CARD.range, CARD.detectRange, "+
            "DEFENSE_TOWER.level, DEFENSE_TOWER.damage, CARD.type, DEFENSE_TOWER.attackSpeed,  DEFENSE_TOWER.hp, DEFENSE_TOWER.spawnTime"+
            "FROM USER, CARD, DEFENSE_TOWER WHERE USER.userID = 14 AND USER.userID = UNIT.userID AND CARD.cardID = UNIT.cardID";

        using (MySqlConnection conn = DatabaseManager.Instance.DBConnection())
        {
            using (MySqlCommand cmd = new MySqlCommand(selectData, conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        UnitInfoData cardInfo = new UnitInfoData
                        {
                            cardId = id,
                            cardName = reader.GetString(1),
                            cost = reader.GetInt32(2),
                            grade = reader.GetString(3),
                            range = reader.GetFloat(4),
                            detectRange = reader.GetFloat(5),
                            level = reader.GetInt32(6),
                            damage = reader.GetInt32(7),
                            type = reader.GetString(8),
                            attackSpeed = reader.GetFloat(9),
                            moveSpeed = reader.GetInt32(10),
                            hp = reader.GetInt32(11),
                            spawnTime = reader.GetInt32(12),
                        };
                        UI_Manager.m_Instance.UnitDatas.Add(cardInfo);
                    }
                }
            }
            conn.Close();
        }
    }
}

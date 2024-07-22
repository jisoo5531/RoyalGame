using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUnitSetting : MonoBehaviour
{
    private void SelectUnitData(int cardId)
    {
        string selectData = string.Empty;

        selectData = $"SELECT CARD.cardID, CARD.name, CARD.cost, CARD.grade, CARD.range, CARD.detectRange, " +
            $"UNIT.level, UNIT.damage, CARD.type, UNIT.moveSpeed, UNIT.hp, UNIT.spawnTime " +
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
                            moveSpeed = reader.GetInt32(9),
                            hp = reader.GetInt32(10),

                        };
                        //allCharacters.Add(cardInfo);
                    }
                }
            }
            conn.Close();
        }
    }
}

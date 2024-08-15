using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpdateUnit : MonoBehaviour, IPointerClickHandler
{
    public GameObject upgradeUI;
    public Upgrade upgrade;
    public SettingUnit settingUnit;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (upgrade.collection.cardId != 3 && upgrade.collection.cardId != 8)
        {
            upgrade.Setinfo(settingUnit.GetUnitData(DatabaseManager.Instance.userId, upgrade.collection.cardId), false);
        }
        else if (upgrade.collection.cardId == 3)
        {
            upgrade.Setinfo(settingUnit.GetMagicData(DatabaseManager.Instance.userId, upgrade.collection.cardId), false);
        }
        else if (upgrade.collection.cardId == 8)
        {
            upgrade.Setinfo(settingUnit.GetTowerData(DatabaseManager.Instance.userId, upgrade.collection.cardId), false);
        }
        SelectUnitInfo();
        upgradeUI.SetActive(false);
    }

    public void SelectUnitInfo()
    {
        try
        {
            if (!DatabaseManager.Instance.connection_check(DatabaseManager.Instance.conn))
            {
                return;
            }

            string selectCardInfo = $"SELECT CARD.cardID, CARD.cost, CARD.name, CARD.grade, CARD.type, " +
                $"CASE WHEN UNIT.currentCardCount IS NOT NULL THEN UNIT.currentCardCount " +
                $"WHEN DEFENSE_TOWER.currentCardCount IS NOT NULL THEN DEFENSE_TOWER.currentCardCount " +
                $"WHEN MAGIC.currentCardCount IS NOT NULL THEN MAGIC.currentCardCount END AS currentCardCount, " +
                $"CASE WHEN UNIT.maxCardCount IS NOT NULL THEN UNIT.maxCardCount " +
                $"WHEN DEFENSE_TOWER.maxCardCount IS NOT NULL THEN DEFENSE_TOWER.maxCardCount " +
                $"WHEN MAGIC.maxCardCount IS NOT NULL THEN MAGIC.maxCardCount END AS maxCardCount, " +
                $"CASE WHEN UNIT.level IS NOT NULL THEN UNIT.level WHEN DEFENSE_TOWER.level IS NOT NULL THEN DEFENSE_TOWER.level " +
                $"WHEN MAGIC.level IS NOT NULL THEN MAGIC.level END AS level FROM CARD " +
                $"LEFT JOIN UNIT ON CARD.cardID = UNIT.cardID AND UNIT.userID = {DatabaseManager.Instance.userId} " +
                $"LEFT JOIN MAGIC ON CARD.cardID = MAGIC.cardID AND MAGIC.userID = {DatabaseManager.Instance.userId} " +
                $"LEFT JOIN DEFENSE_TOWER ON CARD.cardID = DEFENSE_TOWER.cardID AND DEFENSE_TOWER.userID = {DatabaseManager.Instance.userId} WHERE CARD.cardID = {upgrade.collection.cardId}";


            using (MySqlCommand cmd = new MySqlCommand(selectCardInfo, DatabaseManager.Instance.conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader.GetInt32(5) != 0 && reader?.GetInt32(5) != null)
                        {
                            int id = reader.GetInt32(0);
                            CharacterData characterData = new CharacterData
                            {
                                cardId = id,
                                cost = reader.GetInt32(1),
                                name = reader.GetString(2),
                                grade = reader.GetString(3),
                                type = reader.GetString(4),
                                currentCardCount = reader.GetInt32(5),
                                maxCardCount = reader.GetInt32(6),
                                level = reader.GetInt32(7),
                                img = CardInfoManager.instance.characterImgs[id - 1],
                            };
                            upgrade.collection.InitUI(characterData);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }
}

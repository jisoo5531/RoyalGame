using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SettingUnit : MonoBehaviour
{
    public void SelectUnitData(int userId, int cardId)
    {
        string selectData = string.Empty;

        selectData = $"SELECT CARD.cardID, CARD.name, CARD.cost, CARD.grade, CARD.range, CARD.detectRange, " +
            $"UNIT.level, UNIT.damage, CARD.type, UNIT.attackSpeed, UNIT.moveSpeed, UNIT.hp, UNIT.spawnTime, " +
            $"CARD.targeting FROM USER, CARD, UNIT WHERE USER.userID = {userId} AND CARD.cardID = {cardId} AND USER.userID = UNIT.userID AND CARD.cardID = UNIT.cardID";

        if (!DatabaseManager.Instance.Connection_Check(DatabaseManager.Instance.conn))
        {
            return;
        }

        using (MySqlCommand cmd = new MySqlCommand(selectData, DatabaseManager.Instance.conn))
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
                        target = reader.GetString(13),
                        img = UI_Manager.m_Instance.unitSprites[cardId - 1],
                        prefab = UI_Manager.m_Instance.unitPrefab[cardId - 1],
                    };
                    UI_Manager.m_Instance.UnitDatas.Add(cardInfo);
                }
            }
        }
    }

    public void SelectTowerData(int userId, int cardId)
    {
        string selectData = string.Empty;

        if (!DatabaseManager.Instance.Connection_Check(DatabaseManager.Instance.conn))
        {
            return;
        }

        selectData = $"SELECT CARD.cardID, CARD.name, CARD.cost, CARD.grade, " +
            $"CARD.range, DEFENSE_TOWER.level, DEFENSE_TOWER.damage, CARD.type, " +
            $"DEFENSE_TOWER.attackSpeed,  DEFENSE_TOWER.hp, " +
            $"DEFENSE_TOWER.spawnTime, DEFENSE_TOWER.lifeTime, CARD.targeting FROM " +
            $"USER, CARD, DEFENSE_TOWER WHERE USER.userID = {userId} AND CARD.cardID = {cardId} AND " +
            $"USER.userID = DEFENSE_TOWER.userID AND CARD.cardID = DEFENSE_TOWER.cardID";

        using (MySqlCommand cmd = new MySqlCommand(selectData, DatabaseManager.Instance.conn))
        {
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    DEFENSETOWERInfoData cardInfo = new DEFENSETOWERInfoData
                    {
                        cardId = id,
                        cardName = reader.GetString(1),
                        cost = reader.GetInt32(2),
                        grade = reader.GetString(3),
                        range = reader.GetFloat(4),
                        level = reader.GetInt32(5),
                        damage = reader.GetInt32(6),
                        type = reader.GetString(7),
                        attackSpeed = reader.GetFloat(8),
                        hp = reader.GetInt32(9),
                        spawnTime = reader.GetFloat(10),
                        lifeTime = reader.GetInt32(11),
                        target = reader.GetString(12),
                        img = UI_Manager.m_Instance.unitSprites[cardId - 1],
                        prefab = UI_Manager.m_Instance.unitPrefab[cardId - 1]
                    };
                    UI_Manager.m_Instance.UnitDatas.Add(cardInfo);
                }
            }
        }
    }

    public void SelectMagicData(int userId, int cardId)
    {
        string selectData = string.Empty;

        if (!DatabaseManager.Instance.Connection_Check(DatabaseManager.Instance.conn))
        {
            return;
        }

        selectData = $"SELECT CARD.cardID, CARD.name, CARD.cost, " +
            $"CARD.grade, CARD.range, MAGIC.level, MAGIC.unit_Damage, " +
            $"MAGIC.tower_Damage, CARD.type FROM USER, CARD, " +
            $"MAGIC WHERE USER.userID = {userId} AND CARD.cardID = {cardId} AND USER.userID = MAGIC.userID " +
            $"AND CARD.cardID = MAGIC.cardID";

        using (MySqlCommand cmd = new MySqlCommand(selectData, DatabaseManager.Instance.conn))
        {
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    MAGICInfoData cardInfo = new MAGICInfoData
                    {
                        cardId = id,
                        cardName = reader.GetString(1),
                        cost = reader.GetInt32(2),
                        grade = reader.GetString(3),
                        range = reader.GetFloat(4),
                        level = reader.GetInt32(5),
                        damage = reader.GetInt32(6),
                        tower_Damage = reader.GetInt32(7),
                        type = reader.GetString(8),
                        img = UI_Manager.m_Instance.unitSprites[cardId - 1],
                        prefab = UI_Manager.m_Instance.unitPrefab[cardId - 1]
                    };
                    UI_Manager.m_Instance.UnitDatas.Add(cardInfo);
                }
            }
        }
    }

    public UnitInfoData GetUnitData(int userId, int cardId)
    {
        string selectData = string.Empty;

        if (!DatabaseManager.Instance.Connection_Check(DatabaseManager.Instance.conn))
        {
            return null;
        }

        selectData = $"SELECT CARD.cardID, CARD.name, CARD.cost, CARD.grade, CARD.range, CARD.detectRange, " +
            $"UNIT.level, UNIT.damage, CARD.type, UNIT.attackSpeed, UNIT.moveSpeed, UNIT.hp, UNIT.spawnTime, CARD.cardDesc, " +
            $"CARD.targeting, UNIT.currentCardCount, UNIT.maxCardCount FROM USER, CARD, UNIT WHERE USER.userID = {userId} AND CARD.cardID = {cardId} AND USER.userID = UNIT.userID AND CARD.cardID = UNIT.cardID";

        using (MySqlCommand cmd = new MySqlCommand(selectData, DatabaseManager.Instance.conn))
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
                        desc = reader.GetString(13),
                        target = reader.GetString(14),
                        currentCardCount = reader.GetInt32(15),
                        maxCardCount = reader.GetInt32(16),
                        img = CardInfoManager.instance.characterImgs[cardId - 1]
                    };
                    return cardInfo;
                }
            }
        }
        return null;
    }

    public DEFENSETOWERInfoData GetTowerData(int userId, int cardId)
    {
        if (!DatabaseManager.Instance.Connection_Check(DatabaseManager.Instance.conn))
        {
            return null;
        }

        string selectData = string.Empty;

        selectData = $"SELECT CARD.cardID, CARD.name, CARD.cost, CARD.grade, " +
            $"CARD.range, DEFENSE_TOWER.level, DEFENSE_TOWER.damage, CARD.type, " +
            $"DEFENSE_TOWER.attackSpeed,  DEFENSE_TOWER.hp, " +
            $"DEFENSE_TOWER.spawnTime, DEFENSE_TOWER.lifeTime, CARD.cardDesc, CARD.targeting, DEFENSE_TOWER.currentCardCount, DEFENSE_TOWER.maxCardCount FROM " +
            $"USER, CARD, DEFENSE_TOWER WHERE USER.userID = {userId} AND CARD.cardID = {cardId} AND " +
            $"USER.userID = DEFENSE_TOWER.userID AND CARD.cardID = DEFENSE_TOWER.cardID";

        using (MySqlCommand cmd = new MySqlCommand(selectData, DatabaseManager.Instance.conn))
        {
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    DEFENSETOWERInfoData cardInfo = new DEFENSETOWERInfoData
                    {
                        cardId = id,
                        cardName = reader.GetString(1),
                        cost = reader.GetInt32(2),
                        grade = reader.GetString(3),
                        range = reader.GetFloat(4),
                        level = reader.GetInt32(5),
                        damage = reader.GetInt32(6),
                        type = reader.GetString(7),
                        attackSpeed = reader.GetFloat(8),
                        hp = reader.GetInt32(9),
                        spawnTime = reader.GetFloat(10),
                        lifeTime = reader.GetInt32(11),
                        desc = reader.GetString(12),
                        target = reader.GetString(13),
                        currentCardCount = reader.GetInt32(14),
                        maxCardCount = reader.GetInt32(15),
                        img = CardInfoManager.instance.characterImgs[cardId - 1]
                    };
                    return cardInfo;
                }
            }
        }
        return null;
    }

    public MAGICInfoData GetMagicData(int userId, int cardId)
    {
        if (!DatabaseManager.Instance.Connection_Check(DatabaseManager.Instance.conn))
        {
            return null;
        }

        string selectData = string.Empty;

        selectData = $"SELECT CARD.cardID, CARD.name, CARD.cost, " +
            $"CARD.grade, CARD.range, MAGIC.level, MAGIC.unit_Damage, " +
            $"MAGIC.tower_Damage, CARD.type, CARD.cardDesc, MAGIC.currentCardCount, MAGIC.maxCardCount FROM USER, CARD, " +
            $"MAGIC WHERE USER.userID = {userId} AND CARD.cardID = {cardId} AND USER.userID = MAGIC.userID " +
            $"AND CARD.cardID = MAGIC.cardID";
        using (MySqlCommand cmd = new MySqlCommand(selectData, DatabaseManager.Instance.conn))
        {
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    MAGICInfoData cardInfo = new MAGICInfoData
                    {
                        cardId = id,
                        cardName = reader.GetString(1),
                        cost = reader.GetInt32(2),
                        grade = reader.GetString(3),
                        range = reader.GetFloat(4),
                        level = reader.GetInt32(5),
                        damage = reader.GetInt32(6),
                        tower_Damage = reader.GetInt32(7),
                        type = reader.GetString(8),
                        desc = reader.GetString(9),
                        currentCardCount = reader.GetInt32(10),
                        maxCardCount = reader.GetInt32(11),
                        img = CardInfoManager.instance.characterImgs[cardId - 1]
                    };
                    return cardInfo;
                }
            }
        }
        return null;
    }
}

using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CardInfoManager : MonoBehaviour
{
    public static CardInfoManager instance;

    public Sprite[] characterImgs;
    public Image[] collectionsImage;
    public GameObject[] displaySelectedUnit_UI;
    public List<CharacterInfo> allCharacters;
    public SettingChest openChest;

    private void Awake()
    {
        instance = this;
        allCharacters = new List<CharacterInfo>();
    }

    public void OpenFirstChestClick()
    {
        SelectCardInfoInEpicChest();
    }

    public void SelectCardInfoInEpicChest()
    {
        try
        {
            if (!DatabaseManager.Instance.connection_check(DatabaseManager.Instance.conn))
            {
                return;
            }

            allCharacters.Clear();
            string selectCardInfo = $"SELECT cardID, name, grade FROM CARD";

            using (MySqlCommand cmd = new MySqlCommand(selectCardInfo, DatabaseManager.Instance.conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int characterID = reader.GetInt32(0) - 1;
                        if (characterID >= 0 && characterID < characterImgs.Length)
                        {
                            CharacterInfo cardInfo = new CharacterInfo
                            {
                                characterID = characterID,
                                characterSprite = characterImgs[characterID],
                                CharacterName = reader.GetString(1),
                                CharacterGrade = reader.GetString(2),
                            };
                            allCharacters.Add(cardInfo);
                        }
                    }
                    if (openChest != null)
                    {
                        openChest.OpenEpicChest();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }

    public void SelectCardInfoInChest()
    {
        try
        {
            if (!DatabaseManager.Instance.connection_check(DatabaseManager.Instance.conn))
            {
                return;
            }

            allCharacters.Clear();
            string selectCardInfo = $"SELECT CARD.cardID, CARD.name, CARD.grade, " +
                $"CASE WHEN UNIT.currentCardCount IS NOT NULL THEN UNIT.currentCardCount " +
                $"WHEN DEFENSE_TOWER.currentCardCount IS NOT NULL THEN DEFENSE_TOWER.currentCardCount " +
                $"WHEN MAGIC.currentCardCount IS NOT NULL THEN MAGIC.currentCardCount END AS currentCardCount, " +
                $"CASE WHEN UNIT.maxCardCount IS NOT NULL THEN UNIT.maxCardCount " +
                $"WHEN DEFENSE_TOWER.maxCardCount IS NOT NULL THEN DEFENSE_TOWER.maxCardCount " +
                $"WHEN MAGIC.maxCardCount IS NOT NULL THEN MAGIC.maxCardCount END AS maxCardCount, " +
                $"CASE WHEN UNIT.level IS NOT NULL THEN UNIT.level WHEN DEFENSE_TOWER.level IS NOT NULL THEN DEFENSE_TOWER.level " +
                $"WHEN MAGIC.level IS NOT NULL THEN MAGIC.level END AS level FROM CARD " +
                $"LEFT JOIN UNIT ON CARD.cardID = UNIT.cardID LEFT JOIN MAGIC ON CARD.cardID = MAGIC.cardID " +
                $"LEFT JOIN DEFENSE_TOWER ON CARD.cardID = DEFENSE_TOWER.cardID";


            using (MySqlCommand cmd = new MySqlCommand(selectCardInfo, DatabaseManager.Instance.conn))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CharacterInfo cardInfo = new CharacterInfo
                        {
                            characterID = reader.GetInt32(0),
                            characterSprite = characterImgs[reader.GetInt32(0)],
                            CharacterName = reader.GetString(1),
                            CharacterGrade = reader.GetString(2),
                            CharacterCurrentCardCount = reader.GetInt32(3),
                            CharacterMaxCardCount = reader.GetInt32(4),
                            CharacterLevel = reader.GetInt32(5)
                        };
                        allCharacters.Add(cardInfo);
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

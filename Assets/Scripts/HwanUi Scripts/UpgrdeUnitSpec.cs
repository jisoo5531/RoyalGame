using MySql.Data.MySqlClient;
using Photon.Pun.Demo.Cockpit;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgrdeUnitSpec : MonoBehaviour
{
    Upgrade upgrade;
    private bool isMagic = false;

    #region public º¯¼ö
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardCount;
    public Slider slider;
    public Image cardImg;
    public GameObject hpUI;
    public GameObject towerDamageUI;

    public TextMeshProUGUI hpText;
    public TextMeshProUGUI towerDamageText;
    public TextMeshProUGUI damageText;

    public TextMeshProUGUI addHpText;
    public TextMeshProUGUI addTowerDamageText;
    public TextMeshProUGUI addDamageText;

    public SettingUserInfo userInfo;
    #endregion


    private void Awake()
    {
        upgrade = GetComponent<Upgrade>();
    }

    public int SelectCardId(string tableName, int userId, int cardId)
    {
        string selectId = string.Empty;
        int id = 0;

        try
        {
            selectId = $"SELECT user_CardID FROM {tableName} WHERE userID = {userId} AND cardID = {cardId}";
            using (MySqlConnection conn = DatabaseManager.Instance.DBConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(selectId, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            id = reader.GetInt32(0);
                        }
                    }
                }
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
        return id;
    }

    public void UpgradeClick()
    {
        int upgradeCost = upgrade.upgradeCost;
        UpdateUnitData(upgrade.damage, upgrade.tower_damage, upgrade.hp, upgradeCost);
    }


    public void UpdateUnitData(int damageAmount, int towerDamageAmount, int hpAmount, int goldAmount)
    {
        string updateUnit = string.Empty;
        int id = 0;
        try
        {
            if (upgrade.unitdata.cardId != 3 && upgrade.unitdata.cardId != 8)
            {
                id = SelectCardId("UNIT", DatabaseManager.Instance.userId, upgrade.unitdata.cardId);
                isMagic = false;
                updateUnit = $"UPDATE UNIT SET level = level + 1, currentCardCount = currentCardCount - maxCardCount, maxCardCount = maxCardCount * level, damage = damage + {damageAmount}, hp = hp + {hpAmount} WHERE user_CardID = {id}";
            }
            else if (upgrade.unitdata.cardId == 3)
            {
                id = SelectCardId("MAGIC", DatabaseManager.Instance.userId, upgrade.unitdata.cardId);
                isMagic = true;
                updateUnit = $"UPDATE MAGIC SET level = level + 1, currentCardCount = currentCardCount - maxCardCount, maxCardCount = maxCardCount * level, unit_Damage = unit_Damage + {damageAmount}, tower_Damage = tower_Damage + {towerDamageAmount} WHERE user_CardID = {id}";
            }
            else if (upgrade.unitdata.cardId == 8)
            {
                id = SelectCardId("DEFENSE_TOWER", DatabaseManager.Instance.userId, upgrade.unitdata.cardId);
                updateUnit = $"UPDATE DEFENSE_TOWER SET level = level + 1, currentCardCount = currentCardCount - maxCardCount, maxCardCount = maxCardCount * level, damage = damage + {damageAmount}, hp = hp + {hpAmount} WHERE user_CardID = {id}";
                isMagic = false;
            }

            using (MySqlConnection conn = DatabaseManager.Instance.DBConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(updateUnit, conn))
                {
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        cardName.text = upgrade.unitdata.cardName;
                        cardCount.text = $"{upgrade.unitdata.currentCardCount} / {upgrade.unitdata.maxCardCount}";
                        slider.value = 100f;
                        cardImg.sprite = CardInfoManager.instance.characterImgs[upgrade.unitdata.cardId - 1];
                        damageText.text = upgrade.unitdata.damage.ToString();
                        addDamageText.text = $"+ {damageAmount}";
                        if (isMagic)
                        {
                            if (upgrade.unitdata is MAGICInfoData magicInfo)
                            {
                                hpUI.SetActive(false);
                                towerDamageUI.SetActive(true);
                                towerDamageText.text = magicInfo.tower_Damage.ToString();
                                addTowerDamageText.text = $"+ {towerDamageAmount}";
                            }
                        }
                        else
                        {
                            if (upgrade.unitdata is UnitInfoData unitInfo)
                            {
                                hpText.text = unitInfo.hp.ToString();
                            }
                            else if (upgrade.unitdata is DEFENSETOWERInfoData defenseTowerInfo)
                            {
                                hpText.text = defenseTowerInfo.hp.ToString();
                            }
                            addHpText.text = $"+ {hpAmount}";
                            StartManager.m_Instance.gold -= goldAmount;
                            userInfo.goldAmount.text = StartManager.m_Instance.gold.ToString();

                            hpUI.SetActive(true);
                            towerDamageUI.SetActive(false);
                        }
                    }
                }
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }
}

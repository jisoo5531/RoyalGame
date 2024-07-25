using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    private static StartManager instance;
    public static StartManager m_Instance { get { return instance; } }        
    
    [SerializeField] private GameObject[] displaySelectedUnit_UI;
    /// <summary>
    /// 배틀덱의 8개의 선택한 유닛들
    /// </summary>
    [HideInInspector] public GameObject[] m_displaySelectedUnit_UI { get { return displaySelectedUnit_UI; } }
    
    [SerializeField] private CharacterData[] unitDatas;
    /// <summary>
    /// 유닛 데이터
    /// </summary>
    [HideInInspector] public CharacterData[] m_unitDatas { get { return unitDatas; } }

    [SerializeField] private List<CharacterData> selectedUnits; 
    /// <summary>
    /// 인스펙터 창으로 테스트하기 위해 보여지는 선택 유닛들
    /// </summary>
    [HideInInspector] public List<CharacterData> m_selectedUnits { get { return selectedUnits; } }
        
    public Image[] collectionsImage;
    public List<int> battleCardList = new List<int>();
    public List<int> battleCardCostList = new List<int>();
    public TMP_Text costAvg;
    private float avg = 0f;

    public int gold = 0;
    public int jewel = 0;
    
    
    private int currentDisplayIndex = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);            
        }
        else
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < 8; i++)
        {
            selectedUnits.Add(null);
        }
        
    }

    public void InitList()
    {
        battleCardList.Clear();
        battleCardCostList.Clear();
        selectedUnits.Clear();

        for (int i = 0; i < 8; i++)
        {
            selectedUnits.Add(null);
        }
    }

    /// <summary>
    /// 컬렉션 탭 SO에 맞춰 이미지 세팅
    /// </summary>
    public void InitializeCollectionImage()
    {
        for (int i = 0; i < collectionsImage.Length; i++)
        {
            collectionsImage[i].sprite = SettingCardInfoManager.instance.charData[i].img;
            if (collectionsImage[i].sprite != null)
            {
                collectionsImage[i].ImageTransparent(1f);
            }            
        }
    }

    /// <summary>
    /// Collection 탭 유닛 선택
    /// </summary>
    /// <param name="index"></param>
    public void OnClickUseUnit(int index)
    {
        avg = 0f;
        for (int i = 0; i < 8; i++)
        {

            if (selectedUnits[i] == null)
            {
                currentDisplayIndex = i;
                selectedUnits[currentDisplayIndex] = SettingCardInfoManager.instance.charData[index];
                battleCardList.Add(SettingCardInfoManager.instance.charData[index].cardId);
                battleCardCostList.Add(SettingCardInfoManager.instance.charData[index].cost);
                for (int j = 0; j< battleCardCostList.Count; j++)
                {
                    avg += battleCardCostList[j];
                }
                avg /= battleCardCostList.Count;
                avg = Mathf.Floor(avg * 10.0f) / 10.0f;

                if (avg % 1 != 0)
                {
                    costAvg.text = avg.ToString();
                }
                else
                {
                    costAvg.text = avg.ToString() + ".0";
                }

                Image unitImage = displaySelectedUnit_UI[currentDisplayIndex].transform.GetChild(0).GetComponent<Image>();
                unitImage.ImageTransparent(1f);

                unitImage.sprite = SettingCardInfoManager.instance.charData[index].img;
                break;
            }
        }
     
    }       
    /// <summary>
    /// Collection 탭 유닛 제거
    /// </summary>
    /// <param name="index"></param>
    public void OnClickNotUseUnit(int index)
    {
        avg = 0f;
        for (int i = 0; i < 8; i++)
        {
            if (selectedUnits[i] != null && selectedUnits[i].name != null && selectedUnits[i].name.Equals(SettingCardInfoManager.instance.charData[index].name))
            {
                selectedUnits[i] = null;
                currentDisplayIndex = i;
                battleCardList.Remove(SettingCardInfoManager.instance.charData[index].cardId);
                battleCardCostList.Remove(SettingCardInfoManager.instance.charData[index].cost);

                if (battleCardCostList.Count > 0)
                {
                    for (int j = 0; j < battleCardCostList.Count; j++)
                    {
                        avg += battleCardCostList[j];
                    }
                    avg /= battleCardCostList.Count;
                    avg = Mathf.Floor(avg * 10.0f) / 10.0f;

                    if (avg % 1 != 0)
                    {
                        costAvg.text = avg.ToString();
                    }
                    else
                    {
                        costAvg.text = avg.ToString() + ".0";
                    }
                }
                else
                {
                    avg = 0f;
                    costAvg.text = avg.ToString() + ".0";
                }


                Image unitImage = displaySelectedUnit_UI[currentDisplayIndex].transform.GetChild(0).GetComponent<Image>();
                unitImage.sprite = null;
                unitImage.ImageTransparent(0f);
                break;
            }   
        }           

    }

    public void UpdateUserCard(string battleCard)
    {
        string updateUserBattleCard = string.Empty;
        try
        {
            updateUserBattleCard = $"UPDATE USER SET currentBattleCard = '{battleCard}', gold = {gold}, jewel = {jewel} WHERE userID = {DatabaseManager.Instance.userId}";

            using (MySqlConnection conn = DatabaseManager.Instance.DBConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(updateUserBattleCard, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }
}
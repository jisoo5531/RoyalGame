using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour
{
    private static UnitManager instance;
    public static UnitManager m_Instance { get { return instance; } }    
    
    [SerializeField] private GameObject[] displaySelectedUnit_UI;
    /// <summary>
    /// 게임 상에 보여지는 8개의 선택한 유닛들
    /// </summary>
    [HideInInspector] public GameObject[] m_displaySelectedUnit_UI { get { return displaySelectedUnit_UI; } }
    
    [SerializeField] private UnitData_SO[] unitDatas;
    /// <summary>
    /// 유닛 데이터
    /// </summary>
    [HideInInspector] public UnitData_SO[] m_unitDatas { get { return unitDatas; } }

    [SerializeField] private List<UnitData_SO> selectedUnits; 
    /// <summary>
    /// 인스펙터 창으로 테스트하기 위해 보여지는 선택 유닛들
    /// </summary>
    [HideInInspector] public List<UnitData_SO> m_selectedUnits { get { return selectedUnits; } }
    
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
    /// <summary>
    /// Collection 탭 유닛 선택
    /// </summary>
    /// <param name="index"></param>
    public void OnClickUseUnit(int index)
    {        
        for (int i = 0; i < 8; i++)
        {

            if (selectedUnits[i] == null)
            {
                currentDisplayIndex = i;
                break;
            }
        }
        Debug.Log(currentDisplayIndex);
        selectedUnits[currentDisplayIndex] = unitDatas[index];        
                
        Image unitImage = displaySelectedUnit_UI[currentDisplayIndex].transform.GetChild(0).GetComponent<Image>();
        unitImage.ImageTransparent(1f);

        unitImage.sprite = unitDatas[index].iconSprite;        
    }    
    /// <summary>
    /// Collection 탭 유닛 제거
    /// </summary>
    /// <param name="index"></param>
    public void OnClickNotUseUnit(int index)
    {
        for (int i = 0; i < 8; i++)
        {
            if (selectedUnits[i] != null && (selectedUnits[i].name == unitDatas[index].name))
            {
                selectedUnits[i] = null;
                currentDisplayIndex = i;
                break;
            }
        }        
        
        Debug.Log($"배열 개수 : {selectedUnits.Count}");

        Image unitImage = displaySelectedUnit_UI[currentDisplayIndex].transform.GetChild(0).GetComponent<Image>();
        unitImage.sprite = null;
        unitImage.ImageTransparent(0f);
    }    
}
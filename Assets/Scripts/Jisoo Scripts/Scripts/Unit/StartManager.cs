using System.Collections;
using System.Collections.Generic;
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
    

    [SerializeField] private List<UnitData> selectedUnits; 
    /// <summary>
    /// 인스펙터 창으로 테스트하기 위해 보여지는 선택 유닛들
    /// </summary>
    [HideInInspector] public List<UnitData> m_selectedUnits { get { return selectedUnits; } }
        
    public Image[] collectionsImage;
    
    
    private int currentDisplayIndex = 0;

    private void Awake()
    {
        InitializeCollectionImage();

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
    /// 컬렉션 탭 유닛 데이터에 맞춰 이미지 세팅
    /// </summary>
    private void InitializeCollectionImage()
    {        
        for (int i = 0; i < collectionsImage.Length; i++)
        {
            collectionsImage[i].sprite = GameManager.m_Instance.uniData[i].iconSprite;
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
        for (int i = 0; i < 8; i++)
        {

            if (selectedUnits[i] == null)
            {
                currentDisplayIndex = i;
                break;
            }
        }
        Debug.Log(currentDisplayIndex);
        selectedUnits[currentDisplayIndex] = GameManager.m_Instance.uniData[index];        
                
        Image unitImage = displaySelectedUnit_UI[currentDisplayIndex].transform.GetChild(0).GetComponent<Image>();
        unitImage.ImageTransparent(1f);

        unitImage.sprite = GameManager.m_Instance.uniData[index].iconSprite;        
    }    
    /// <summary>
    /// Collection 탭 유닛 제거
    /// </summary>
    /// <param name="index"></param>
    public void OnClickNotUseUnit(int index)
    {
        for (int i = 0; i < 8; i++)
        {
            if (selectedUnits[i] != null && (selectedUnits[i].unitInfo.unitName == GameManager.m_Instance.uniData[index].unitInfo.unitName))
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
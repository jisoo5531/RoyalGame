using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_DisplayUnit : MonoBehaviour
{
    public Image[] unitImage;

    private List<UnitData_SO> shuffledUnit;

    private GameObject[] unitSpawnButtons;       // 유닛 생성하기 위해 보여지는 게임 상에 보여지는 이미지
    //private GameObject[] waitUnitsDisplay;       // TODO : 대기 유닛들 (테스트용, 나중에 지우기)
    private TextMeshProUGUI[] unitElixirText;    // 유닛 엘릭서 UI 텍스트

    //private List<UnitData> availableUnit = new List<UnitData>();
    //private Queue<UnitData> waitUnitsQueue = new Queue<UnitData>();

    //private int selectSlotNumber;

    private UI_Elixir elixir;

    private void Start()
    {
        shuffledUnit = UI_Manager.m_Instance.m_shuffledUnit;
        unitElixirText = UI_Manager.m_Instance.m_UI_unitElixirText;
        unitSpawnButtons = UI_Manager.m_Instance.UI_unitSpawnButtons;

        elixir = GetComponent<UI_Elixir>();

        UI_DisplayInitialUnits(unitSpawnButtons);

        StartCoroutine(UI_UnitElixir_Slider());
    }


    IEnumerator UI_UnitElixir_Slider()
    {
        while (true)
        {
            yield return null;
            for (int i = 0; i < unitSpawnButtons.Length; i++)
            {
                //Image unitImage = unitSpawnButtons[i].transform.GetChild(0).GetChild(0).GetComponent<Image>();

                unitImage[i].fillMethod = Image.FillMethod.Radial360;
                unitImage[i].fillOrigin = (int)Image.Origin360.Top;


                unitImage[i].fillAmount = elixir.currentElixir / UI_Manager.m_Instance.m_UI_availableUnit[i].cost;
                //unitImage.fillAmount = Mathf.Lerp(unitImage.fillAmount, 1, Time.time);
            }
        }
    }

    /// <summary>
    /// 게임 시작 후 초기 UI 세팅
    /// </summary>
    /// <param name="unitButtons">4개의 버튼 UI</param>
    public void UI_DisplayInitialUnits(GameObject[] unitButtons)
    {
        for (int i = 0; i < shuffledUnit.Count; i++)
        {
            if (i < 4)
            {
                UI_SetDisplayUnit(shuffledUnit[i], i, unitElixirText[i]);
            }
            else
            {                
                // waitUnitQueue 시각화 테스트
                //UI_SetDisplayUnit(shuffledUnit[i], UI_Manager.m_Instance.waitUnitsDisplay[i - 4]);
            }
        }
        UI_Manager.m_Instance.UI_nextUnitDisplay.transform.GetChild(0).GetComponent<Image>().sprite = shuffledUnit[4].iconSprite;
    }
    /// <summary>
    /// 유닛 소환때마다 UI 바꾸기
    /// </summary>
    /// <param name="unitData">소환 대상 유닛 정보</param>
    /// <param name="number">몇 번째 버튼에 있는 유닛인지</param>
    public void UI_ChangeDisplayUnit(UnitData_SO unitData, int number)
    {
        //unitSpawnButtons[number].transform.GetChild(1).GetComponent<Image>().sprite = unitData.iconSprite;
        UI_SetDisplayUnit(unitData, number, unitElixirText[number]);

        UnitData_SO nextUnitData =  UI_Manager.m_Instance.m_UI_waitUnitsQueue.Peek();
        UI_Manager.m_Instance.UI_nextUnitDisplay.transform.GetChild(0).GetComponent<Image>().sprite = nextUnitData.iconSprite;

        //int index = 0;
        //foreach (UnitData_SO unit in UI_Manager.m_Instance.m_UI_waitUnitsQueue)
        //{
        //    UI_Manager.m_Instance.waitUnitsDisplay[index++].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = unit.iconSprite;
        //}

    }
    /// <summary>
    /// UI Set
    /// </summary>
    /// <param name="unitData"></param>
    /// <param name="unitButton"></param>
    /// <param name="elixirText">유닛 코스트</param>
    private void UI_SetDisplayUnit(UnitData_SO unitData, int number, TextMeshProUGUI elixirText = null)
    {
        //GameObject unitUI = unitButton.transform.GetChild(1).gameObject;
        unitImage[number].sprite = unitData.iconSprite;
        //unitButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = unitData.iconSprite;
        if (elixirText != null)
        {
            elixirText.text = unitData.cost.ToString();
        }
    }
}


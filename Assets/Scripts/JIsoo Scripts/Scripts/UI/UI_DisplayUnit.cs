using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_DisplayUnit : MonoBehaviour
{
    public Image[] unitImage;

    private List<AllCardData> shuffledUnit;

    private GameObject[] unitSpawnButtons;       // 유닛 생성하기 위해 보여지는 게임 상에 보여지는 이미지
    private TextMeshProUGUI[] unitElixirText;    // 유닛 엘릭서 UI 텍스트

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
                unitImage[i].type = Image.Type.Filled;
                unitImage[i].fillMethod = Image.FillMethod.Radial360;
                unitImage[i].fillOrigin = (int)Image.Origin360.Top;

                unitImage[i].fillAmount = elixir.currentElixir / UI_Manager.m_Instance.m_UI_availableUnit[i].cost;
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
        }
        UI_Manager.m_Instance.UI_nextUnitDisplay.transform.GetChild(0).GetComponent<Image>().sprite = shuffledUnit[4].img;
    }
    /// <summary>
    /// 유닛 소환때마다 UI 바꾸기
    /// </summary>
    /// <param name="unitData">소환 대상 유닛 정보</param>
    /// <param name="number">몇 번째 버튼에 있는 유닛인지</param>
    public void UI_ChangeDisplayUnit(AllCardData unitData, int number)
    {
        UI_SetDisplayUnit(unitData, number, unitElixirText[number]);
        AllCardData nextUnitData =  UI_Manager.m_Instance.m_UI_waitUnitsQueue.Peek();
        UI_Manager.m_Instance.UI_nextUnitDisplay.transform.GetChild(0).GetComponent<Image>().sprite = nextUnitData.img;

    }
    /// <summary>
    /// UI Set
    /// </summary>
    /// <param name="unitData"></param>
    /// <param name="unitButton"></param>
    /// <param name="elixirText">유닛 코스트</param>
    private void UI_SetDisplayUnit(AllCardData unitData, int number, TextMeshProUGUI elixirText = null)
    {
        unitImage[number].sprite = unitData.img;
        if (elixirText != null)
        {
            elixirText.text = unitData.cost.ToString();
        }
    }
}


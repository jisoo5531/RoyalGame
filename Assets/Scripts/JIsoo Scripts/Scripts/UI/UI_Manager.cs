using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    #region 전역변수

    private static UI_Manager instance;
    public static UI_Manager m_Instance { get { return instance; } }

    private List<UnitData_SO> shuffledUnit;
    [HideInInspector] public List<UnitData_SO> m_shuffledUnit { get { return shuffledUnit; } }


    private List<UnitData_SO> UI_availableUnit = new List<UnitData_SO>();
    /// <summary>
    /// 현재 생성 버튼에 있는 생성 가능 유닛
    /// </summary>
    [HideInInspector] public List<UnitData_SO> m_UI_availableUnit { get { return UI_availableUnit; } }


    private Queue<UnitData_SO> UI_waitUnitsQueue = new Queue<UnitData_SO>();
    /// <summary>
    /// 생성 가능 유닛이 아닌 대기 중인 유닛
    /// </summary>
    [HideInInspector] public Queue<UnitData_SO> m_UI_waitUnitsQueue { get { return UI_waitUnitsQueue; } }


    /// <summary>
    /// 4개의 유닛 생성 버튼
    /// </summary>
    public GameObject[] UI_unitSpawnButtons;

    [SerializeField] private TextMeshProUGUI[] UI_unitElixirText;
    /// <summary>
    /// 유닛 엘릭서 코스트 UI 텍스트 배열
    /// </summary>
    [HideInInspector] public TextMeshProUGUI[] m_UI_unitElixirText { get { return UI_unitElixirText; } }

    [Space(20)]
    public GameObject UI_nextUnitDisplay;    
    [Space(20)]
    /// <summary>
    /// 몇번째 유닛을 선택했는지
    /// </summary>
    public int selectSlotNumber;


    public Transform SelectedUnitPanel;

    public SpawnSlot focusedSlot;           // 어떤 슬롯에 커서를 대고 있는지

    public SpawnSlot selectedSlot;          // TODO : 어떤 슬롯이 선택되었는지 (선택된 슬롯 하이라이트할 때 사용)
    public GameObject[] slotOutLine;

    private UI_Elixir elixir;

    #endregion
    private void Awake()
    {
        instance = this;

        shuffledUnit = StartSelectUnitManager.m_Instance.m_selectedUnits;
        shuffledUnit.Shuffle();

        elixir = GetComponent<UI_Elixir>();

        InitialUnitSet();


        StartCoroutine(CheckSpawnUnit());
    }

    private void InitialUnitSet()
    {
        for (int i = 0; i < shuffledUnit.Count; i++)
        {
            if (i < 4)
            {
                UI_availableUnit.Add(shuffledUnit[i]);
            }
            else
            {
                UI_waitUnitsQueue.Enqueue(shuffledUnit[i]);
            }
        }
    }
    IEnumerator CheckSpawnUnit()
    {
        while (true)
        {
            yield return null;

            // 현재 보유 엘릭서가 충분하다면
            if (CheckSpawnPossible(selectSlotNumber))
            {
                UnitSpawner.instance.isElixirEnough = true;

                if (UnitSpawner.instance.spawnComplete)
                {
                    UnitData_SO spawnedUnit = UI_availableUnit[selectSlotNumber];

                    elixir.ElixirMinus(spawnedUnit.cost);

                    UI_availableUnit.RemoveAt(selectSlotNumber);
                    UI_availableUnit.Insert(selectSlotNumber, UI_waitUnitsQueue.Dequeue());
                    UI_waitUnitsQueue.Enqueue(spawnedUnit);

                    GetComponent<UI_DisplayUnit>().UI_ChangeDisplayUnit(UI_availableUnit[selectSlotNumber], selectSlotNumber);
                    selectedSlot = null;
                    ActiveSlotOutLine();

                    UnitSpawner.instance.spawnComplete = false;
                }
            }
        }
    }
    /// <summary>
    /// SpawnManager에게 현재 몇번째 슬롯을 선택했는지 전달
    /// </summary>
    /// <param name="number"></param>
    public void OnClickSpawnUnit(int number)
    {        
        selectSlotNumber = number;

        UnitSpawner.instance.SelectUnit(UI_availableUnit[number]);        
    }
    /// <summary>
    /// 생성이 가능한지 체크 (엘릭서 코스트 체크)
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public bool CheckSpawnPossible(int number)
    {
        if (elixir.IsSpawnUnitPossible(UI_availableUnit[number].cost))
        {
            return true;
        }
        return false;
    }

    public void ActiveSlotOutLine()
    {
        foreach (GameObject slot in slotOutLine)
        {
            slot.SetActive(false);
        }
        if (selectedSlot != null)
        {
            selectedSlot.SelectedOutLine.SetActive(true);
        }        
    }
}

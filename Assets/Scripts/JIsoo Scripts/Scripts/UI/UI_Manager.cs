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


    private int selectSlotNumber;
    /// <summary>
    /// 몇번째 유닛을 선택했는지
    /// </summary>
    [HideInInspector] public int m_selectSlotNumber { get { return selectSlotNumber; } }


    public GameObject[] waitUnitsDisplay;     // TODO : 대기 유닛들 (테스트용, 나중에 지우기)

    public Transform SelectedUnitPanel;

    public SpawnSlot focusedSlot;    
    public SpawnSlot selectedSlot;

    private UI_Elixir elixir;

    #endregion
    private void Awake()
    {
        instance = this;

        shuffledUnit = UnitManager.m_Instance.m_selectedUnits;
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
            if (elixir.IsSpawnUnitPossible(UI_availableUnit[selectSlotNumber].cost))
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
                    UnitSpawner.instance.spawnComplete = false;
                }
            }
        }
    }
    public void OnClickSpawnUnit(int number)
    {
        selectSlotNumber = number;
        UnitSpawner.instance.SelectUnit(UI_availableUnit[number]);
    }
}

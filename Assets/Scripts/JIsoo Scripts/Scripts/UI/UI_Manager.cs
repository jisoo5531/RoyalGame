using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class UI_Manager : MonoBehaviourPunCallbacks
{
    private static UI_Manager instance;
    public static UI_Manager m_Instance { get { return instance; } }

    private List<AllCardData> shuffledUnit;
    [HideInInspector] public List<AllCardData> m_shuffledUnit { get { return shuffledUnit; } }

    public List<AllCardData> UnitDatas = new List<AllCardData>();

    private List<AllCardData> UI_availableUnit = new List<AllCardData>();
    /// <summary>
    /// 현재 생성 버튼에 있는 생성 가능 유닛
    /// </summary>
    [HideInInspector] public List<AllCardData> m_UI_availableUnit { get { return UI_availableUnit; } }


    private Queue<AllCardData> UI_waitUnitsQueue = new Queue<AllCardData>();
    /// <summary>
    /// 생성 가능 유닛이 아닌 대기 중인 유닛
    /// </summary>
    [HideInInspector] public Queue<AllCardData> m_UI_waitUnitsQueue { get { return UI_waitUnitsQueue; } }


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

    public SpawnSlot focusedSlot;

    public SpawnSlot selectedSlot;
    public GameObject[] slotOutLine;

    private UI_Elixir elixir;

    private SettingUnit settingUnit;
    public Sprite[] unitSprites;
    public GameObject[] unitPrefab;

    private void Awake()
    {
        instance = this;

        settingUnit = GetComponent<SettingUnit>();

        for (int i = 0; i< StartManager.m_Instance.battleCardList.Count; i++)
        {
            if (StartManager.m_Instance.battleCardList[i] != 3 && StartManager.m_Instance.battleCardList[i] != 8)
            {
                settingUnit.SelectUnitData(DatabaseManager.Instance.userId, StartManager.m_Instance.battleCardList[i]);
            }
            else if(StartManager.m_Instance.battleCardList[i] == 3)
            {
                settingUnit.SelectMagicData(DatabaseManager.Instance.userId, StartManager.m_Instance.battleCardList[i]);
            }
            else if (StartManager.m_Instance.battleCardList[i] == 8)
            {
                settingUnit.SelectTowerData(DatabaseManager.Instance.userId, StartManager.m_Instance.battleCardList[i]);
            }
        }

        shuffledUnit = UnitDatas;
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

            if (CheckSpawnPossible(selectSlotNumber))
            {
                UnitSpawner.instance.isElixirEnough = true;

                if (UnitSpawner.instance.spawnComplete)
                {
                    AllCardData spawnedUnit = UI_availableUnit[selectSlotNumber];

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

        if (UI_availableUnit[number].cardId != 3)
        {
            GameManager.instance.spawnLimits.EnableTowerLimit();
        }
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

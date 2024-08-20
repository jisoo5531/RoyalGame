using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

/// <summary>
/// 유닛 드래그, 클릭을 통한 생성
/// </summary>
public class SpawnSlot : MonoBehaviourPunCallbacks,
    IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    #region public 변수
    public LayerMask otherTargetLayer;
    public LayerMask fireballTargetLayer;
    public GameObject iconImage;
    public GameObject SelectedOutLine;

    public int selectedNumber;
    public UnitSpawner unitSpawner;
    public RectTransform mainCard;
    public GameObject dragUnit = null;
    #endregion

    #region private 변수
    private AllCardData unitData;
    private bool isSpawn = false;
    private string unitName;
    private bool isMaster = false;
    #endregion


    private void Start()
    {
        isMaster = PhotonNetwork.IsMasterClient;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameManager.instance.isGameEnd) return;

        UI_Manager.m_Instance.selectedSlot = this;
        selectedNumber = int.Parse(name);
        if (false == UI_Manager.m_Instance.CheckSpawnPossible(selectedNumber))
        {
            return;
        }

        UI_Manager.m_Instance.ActiveSlotOutLine();
        iconImage.GetComponent<RectTransform>().SetParent(UI_Manager.m_Instance.SelectedUnitPanel);
        UI_Manager.m_Instance.selectedSlot = this;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (false == UI_Manager.m_Instance.CheckSpawnPossible(selectedNumber))
        {
            return;
        }

        if (unitData != null && unitData.cardId != 3)
        {
            GameManager.instance.spawnLimits.EnableTowerLimit();
        }
        MoveImage(eventData);

        MoveModel(eventData);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (false == UI_Manager.m_Instance.CheckSpawnPossible(selectedNumber))
        {
            return;
        }

        if (true == isSpawn)
        {
            GameManager.instance.spawnLimits.DisableTower();
            UI_Manager.m_Instance.OnClickSpawnUnit(selectedNumber);
            UnitSpawner.instance.spawnComplete = true;

            GameObject unitObj = AllySpawnManager.Instance.InitCreateUnit(unitName, dragUnit.transform.position, unitData, dragUnit);
            if (isMaster)
            {
                if (unitObj.TryGetComponent<DeffenseTower>(out DeffenseTower deffense))
                {
                    MasterManager.instance.AddTower(unitObj.GetComponent<PhotonView>().ViewID);
                }
                else if (unitObj.TryGetComponent<MovableUnit>(out MovableUnit movable))
                {
                    MasterManager.instance.AddUnit(unitObj.GetComponent<PhotonView>().ViewID);
                }
            }
            else
            {
                if (unitObj.TryGetComponent<DeffenseTower>(out DeffenseTower deffense))
                {
                    NonMasterManager.instance.AddTower(unitObj.GetComponent<PhotonView>().ViewID);
                }
                else if (unitObj.TryGetComponent<MovableUnit>(out MovableUnit movable))
                {
                    NonMasterManager.instance.AddUnit(unitObj.GetComponent<PhotonView>().ViewID);
                }
            }

            UnitSpawner.instance.selectedUnit = null;
        }

        UI_Manager.m_Instance.selectedSlot = null;

        iconImage.gameObject.SetActive(true);

        iconImage.GetComponent<RectTransform>().SetParent(transform);
        iconImage.GetComponent<RectTransform>().SetSiblingIndex(0);

        iconImage.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        isSpawn = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UI_Manager.m_Instance.selectedSlot = this;
        selectedNumber = int.Parse(name);

        if (false == UI_Manager.m_Instance.CheckSpawnPossible(selectedNumber))
        {
            return;
        }
        UI_Manager.m_Instance.ActiveSlotOutLine();
        UI_Manager.m_Instance.OnClickSpawnUnit(selectedNumber);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI_Manager.m_Instance.focusedSlot = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI_Manager.m_Instance.focusedSlot = null;
    }

    /// <summary>
    /// 드래그 중 이미지 옮기기
    /// </summary>
    /// <param name="y_Pos"></param>
    private void MoveImage(PointerEventData eventData)
    {
        if (eventData.position.y > 200f)
        {
            iconImage.gameObject.SetActive(false);
            return;
        }
        iconImage.gameObject.SetActive(true);

        iconImage.GetComponent<RectTransform>().position = eventData.position;
    }

    /// <summary>
    /// 드래그 중 유닛 모델 옮기기
    /// </summary>
    /// <param name="y_Pos"></param>
    private void MoveModel(PointerEventData eventData)
    {
        // TODO : Slot Background 밖에서 (맵에서) - y 좌표 알맞게 수정
        if (eventData.position.y < 180f)
        {
            if (dragUnit == null)
            {
                return;
            }
            Destroy(dragUnit);
            isSpawn = false;
            return;
        }
        unitData = UI_Manager.m_Instance.m_UI_availableUnit[selectedNumber];
        GameObject unitPrefab = unitData.prefab;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("skdsklsdklsd");
            if (unitData.cardId != 3)
            {
                CheckUnitId(hit, unitPrefab, otherTargetLayer);
            }
            else
            {
                CheckUnitId(hit, unitPrefab, fireballTargetLayer);
            }
        }
    }

    private void CheckUnitId(RaycastHit hit, GameObject unitPrefab, LayerMask targetLayer)
    {
        Debug.Log(hit.collider.gameObject.layer+",  "+targetLayer+",  "+targetLayer.value);
        if ((targetLayer | (1 << hit.collider.gameObject.layer)) == targetLayer)
        {
            Debug.Log("eeeeeeeeee");
            if (false == isSpawn)
            {
                if (!PhotonNetwork.IsMasterClient)
                {
                    dragUnit = Instantiate(unitPrefab, hit.point, Quaternion.Euler(0, 180, 0));
                }
                else
                {
                    dragUnit = Instantiate(unitPrefab, hit.point, Quaternion.identity);
                }
                dragUnit.GetComponent<DragUnitInfo>().unitName.text = unitData.cardName;
                dragUnit.GetComponent<DragUnitInfo>().unitLevel.text = $"Lv. {unitData.level}";
                unitName = unitPrefab.name;
                isSpawn = true;
            }
            dragUnit.transform.position = hit.point;
        }
    }
}

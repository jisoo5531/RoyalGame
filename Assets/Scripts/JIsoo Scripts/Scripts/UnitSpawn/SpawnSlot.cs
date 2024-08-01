using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine.TextCore.Text;

/// <summary>
/// 유닛 드래그, 클릭을 통한 생성
/// </summary>
public class SpawnSlot : MonoBehaviourPunCallbacks,
    IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public LayerMask targetLayer;
    public GameObject iconImage;
    public GameObject SelectedOutLine;

    public int selectedNumber;
    public RectTransform mainCard;

    public GameObject dragUnit = null;

    private AllCardData unitData;
    private bool isSpawn = false;
    private string unitName;

    public static Vector3 spawnPoint;

    public UnitSpawner unitSpawner;
    public IsMineManager isMineManager;

    public void OnBeginDrag(PointerEventData eventData)
    {
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
            UI_Manager.m_Instance.OnClickSpawnUnit(selectedNumber);
            UnitSpawner.instance.spawnComplete = true;

            // TODO : 유닛 유형(유닛, 방어타워) 등에 맞게 수정
            GameObject unit = null;
            if (IsFirstPlayer())
            {
                unit = PhotonNetwork.Instantiate(unitName, dragUnit.transform.position, Quaternion.identity);
            }
            else
            {
                unit = PhotonNetwork.Instantiate(unitName, dragUnit.transform.position, Quaternion.Euler(0, 180, 0));
            }
            unit.UnitClassification(unitData);

            unit.layer = 11;
            foreach (Transform child in unit.transform)
            {
                child.gameObject.layer = 11;
            }

            Destroy(dragUnit);

            if (unitData is UnitInfoData unitInfo)
            {
                if (unit.TryGetComponent<MovableUnit>(out MovableUnit mu))
                {
                    mu.moveDelay = unitInfo.spawnTime;
                    mu.isMove = false;
                    mu.isSpawn = true;
                }
            }
            else if (unitData is DEFENSETOWERInfoData defenseTowerInfo)
            {
                if (unit.TryGetComponent<MovableUnit>(out MovableUnit mu))
                {
                    mu.moveDelay = defenseTowerInfo.spawnTime;
                    mu.isMove = false;
                    mu.isSpawn = true;
                }
            }
            //IsMineManager.instance.AddUnit(unit.GetComponent<PhotonView>().ViewID);

            UnitSpawner.instance.selectedUnit = null;

            
        }

        UI_Manager.m_Instance.selectedSlot = null;

        iconImage.gameObject.SetActive(true);

        iconImage.GetComponent<RectTransform>().SetParent(transform);
        iconImage.GetComponent<RectTransform>().SetSiblingIndex(0);

        iconImage.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        isSpawn = false;
    }

    private bool IsFirstPlayer()
    {
        int num = PhotonNetwork.LocalPlayer.ActorNumber;

        return num % 2 == 0;
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
        // TODO : Slot Background 안에서만 - y 좌표 알맞게 수정
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
            if ((targetLayer | (1 << hit.collider.gameObject.layer)) == targetLayer)
            {
                spawnPoint = hit.point;
                if (false == isSpawn)
                {
                    if (IsFirstPlayer())
                    {
                        dragUnit = Instantiate(unitPrefab, hit.point, Quaternion.identity);
                    }
                    else
                    {
                        dragUnit = Instantiate(unitPrefab, hit.point, Quaternion.Euler(0, 180, 0));
                    }
                    dragUnit.GetComponent<DragUnitInfo>().unitName.text = unitData.cardName;
                    dragUnit.GetComponent<DragUnitInfo>().unitName.text = unitData.level.ToString();
                    unitName = unitPrefab.name;
                    isSpawn = true;
                }
                dragUnit.transform.position = hit.point;
            }
        }
    }
}

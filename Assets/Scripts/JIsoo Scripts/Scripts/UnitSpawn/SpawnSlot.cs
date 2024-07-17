using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 유닛 드래그, 클릭을 통한 생성
/// </summary>
public class SpawnSlot : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public LayerMask targetLayer;
    //public Image iconImage;
    public GameObject iconImage;
    public int selectedNumber;
    public RectTransform mainCard;

    public GameObject dragUnit = null;

    private UnitData_SO unitData;
    private bool isSpawn = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        UI_Manager.m_Instance.selectedSlot = this;
        selectedNumber = int.Parse(name);
        if (false == UI_Manager.m_Instance.CheckSpawnPossible(selectedNumber))
        {
            return;
        }

        // 이미지 드래그 시작        
        iconImage.GetComponent<RectTransform>().SetParent(UI_Manager.m_Instance.SelectedUnitPanel);
        //iconImage.rectTransform.SetParent(UI_Manager.m_Instance.SelectedUnitPanel);
        UI_Manager.m_Instance.selectedSlot = this;

        //Debug.Log($"드래그 시작 {selectedNumber}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 이미지 드래그        
        //Debug.Log($"현재 {eventData.position}");

        if (false == UI_Manager.m_Instance.CheckSpawnPossible(selectedNumber))
        {
            return;
        }

        MoveImage(eventData);

        // 드래그 중 이미지가 아닌 유닛 프리팹으로 옮기기
        MoveModel(eventData);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (false == UI_Manager.m_Instance.CheckSpawnPossible(selectedNumber))
        {
            return;
        }

        // 드래그 끝났을 때 포지션이 맵이면 생성        
        if (true == isSpawn)
        {
            UI_Manager.m_Instance.OnClickSpawnUnit(selectedNumber);
            UnitSpawner.instance.spawnComplete = true;

            // TODO : 유닛 유형(유닛, 방어타워) 등에 맞게 수정
            dragUnit.UnitClassification(unitData);

            dragUnit.UnitTransparent(1f);
            if(dragUnit.TryGetComponent<MovableUnit>(out MovableUnit mu))
            {
                print("생성");
                mu.isMove = true;
            }

            UnitSpawner.instance.selectedUnit = null;
        }

        UI_Manager.m_Instance.selectedSlot = null;

        iconImage.gameObject.SetActive(true);

        //iconImage.rectTransform.SetParent(transform);
        iconImage.GetComponent<RectTransform>().SetParent(transform);

        //iconImage.rectTransform.anchoredPosition = Vector2.zero;
        iconImage.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;


        dragUnit = null;
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
        //iconImage.rectTransform.position = eventData.position;
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
                if (false == isSpawn)
                {
                    dragUnit = Instantiate(unitPrefab, hit.point, Quaternion.identity);
                    dragUnit.UnitTransparent(0.5f);

                    isSpawn = true;
                }
                dragUnit.transform.position = hit.point;
            }
        }
    }
}

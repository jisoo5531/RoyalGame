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
    public Image iconImage;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 이미지 드래그 시작
        iconImage.rectTransform.SetParent(UI_Manager.m_Instance.SelectedUnitPanel);
        UI_Manager.m_Instance.selectedSlot = this;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 이미지 드래그

        iconImage.rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 끝났을 때 포지션이 맵이면 생성        

        UI_Manager.m_Instance.selectedSlot = null;

        iconImage.rectTransform.SetParent(transform.GetChild(0));
        

        iconImage.rectTransform.anchoredPosition = Vector2.zero;
        //iconImage.rectTransform.anchoredPosition
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI_Manager.m_Instance.focusedSlot = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI_Manager.m_Instance.focusedSlot = null;
    }
}

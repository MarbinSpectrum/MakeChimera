using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//해당 UI에게 스크롤뷰 드래그 속성을 추가한다. 
public class ScrollViewDragObj : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private ScrollRect scrollRect;
    public void OnBeginDrag(PointerEventData e) => scrollRect.OnBeginDrag(e);
    public void OnDrag(PointerEventData e) => scrollRect.OnDrag(e);
    public void OnEndDrag(PointerEventData e) => scrollRect.OnEndDrag(e);
}

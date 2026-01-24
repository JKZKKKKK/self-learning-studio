using UnityEngine;
using UnityEngine.EventSystems; // 必須引用這個

public class CardDragHandle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private CardDrag2D card;

    void Start()
    {
        card = GetComponentInParent<CardDrag2D>();
    }

    // 當滑鼠按下 (New Input System 專用)
    public void OnPointerDown(PointerEventData eventData)
    {
        if (card != null)
        {
            Debug.Log("偵測到點擊！");
            card.StartDrag();
        }
    }

    // 當滑鼠放開
    public void OnPointerUp(PointerEventData eventData)
    {
        if (card != null)
        {
            card.isDragging = false; // 這裡可以直接控制，或是呼叫 card.EndDrag()
        }
    }
}
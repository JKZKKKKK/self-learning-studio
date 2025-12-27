using UnityEngine;
using UnityEngine.InputSystem; // 引用新版套件

public class CardDrag2D : MonoBehaviour
{
    public bool isDragging;
    private Vector2 offset;

    public void StartDrag()
    {
        isDragging = true;
        Vector2 mousePos = GetMouseWorldPosition();
        offset = (Vector2)transform.position - mousePos;
    }

    void Update()
    {
        if (!isDragging) return;

        // 跟隨滑鼠
        Vector2 mousePos = GetMouseWorldPosition();
        transform.position = mousePos + offset;
    }

    Vector2 GetMouseWorldPosition()
    {
        // 新版獲取滑鼠位置的方法：Mouse.current.position.ReadValue()
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        mouseScreenPos.z = -Camera.main.transform.position.z; 
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        return new Vector2(worldPos.x, worldPos.y);
    }
}
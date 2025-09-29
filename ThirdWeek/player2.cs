using UnityEngine;
using UnityEngine.InputSystem; // 新輸入系統命名空間

public class Player2 : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;

    void Update()
    {
        // 用新 InputSystem 的鍵盤輸入
        if (Keyboard.current.dKey.isPressed)
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        }
        else if (Keyboard.current.aKey.isPressed)
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Floor1")
        {
            Debug.Log("撞到了第一種階梯");
        }
        else if (other.gameObject.tag == "Floor2")
        {
            Debug.Log("撞到第二種階梯");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "DeathLine")
        {
            Debug.Log("你輸了");
        }
    }
}

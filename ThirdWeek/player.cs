using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    void Start()
    {
        // 初始化
        Debug.Log("Player script attached!");

    }

    void Update()
    {
        
        // 每幀執行
        //transform.Translate(0, moveSpeed*Time.deltaTime, 0);
    }
    
}


using UnityEngine;

public class AccountSession : MonoBehaviour
{
    public static AccountSession Instance;   // Singleton

    public AccountData currentAccount;       // 你登入後的資料放這
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 跨場景不刪除
        }
        else
        {
            Destroy(gameObject); // 避免重複
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;

[System.Serializable]
public class AccountData
{
    public string username;
    public string password;
    public int level_rank = 0;
    public int level_difficulty = 0;//最高
    public int level_selected = 0;//可選的
    public int score = 0;
    public float playing_level = 0;

    public string lastLoginTime;
}

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public int quantity;
}   

public class login : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] Text messageText;
    [SerializeField] GameObject messageBlock;

    private string savePath;

    void Start()
    {
        // 設定儲存路徑
        savePath = Path.Combine(Application.persistentDataPath, "accounts", "user.json");

        // 如果資料夾不存在就建立
        string folderPath = Path.GetDirectoryName(savePath);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log("✅ 建立資料夾：" + folderPath);
        }
        // 如果 user.json 不存在就建立一個空檔案
        if (!File.Exists(savePath))
        {
            AccountData emptyData = new AccountData();
            string json = JsonUtility.ToJson(emptyData, true);
            File.WriteAllText(savePath, json);
            Debug.Log("🆕 建立新的 user.json 檔案：" + savePath);
        }
        else
        {
            Debug.Log("📂 已存在 user.json：" + savePath);
        }
    }


    public void Login()
    {
        AccountData data = new AccountData();
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            messageText.text = "請輸入帳號與密碼";
            return;
        }

        List<AccountData> accounts = LoadAccounts();

        foreach (var acc in accounts)
        {
            if (acc.username == username && acc.password == password)
            {
                // 將玩家資料塞到 Session 供跨場景使用
                AccountSession.Instance.currentAccount.username = acc.username;
                AccountSession.Instance.currentAccount.password = acc.password;
                AccountSession.Instance.currentAccount.level_rank = acc.level_rank;
                AccountSession.Instance.currentAccount.level_difficulty = acc.level_difficulty;
                AccountSession.Instance.currentAccount.lastLoginTime = acc.lastLoginTime;
                
                messageText.text = "登入成功！";
                SceneManager.LoadScene("game-list");
                return;
            }
        }

        messageText.text = "帳號或密碼錯誤！";
    }


    public void OnSignUp()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            messageText.text = "請輸入帳號與密碼";
            return;
        }

        List<AccountData> accounts = LoadAccounts();

        foreach (var acc in accounts)
        {
            if (acc.username == username)
            {
                messageText.text = "此帳號已存在！";
                return;
            }
        }

        accounts.Add(new AccountData
        {
            username = username,
            password = password,
            level_rank = 0, 
            level_difficulty = 0,
            level_selected = 0,
            playing_level = 0,       
            lastLoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        });
        SaveAccounts(accounts);
        messageText.text = "註冊成功！";
        Debug.Log("帳號創建\n名稱 : " + username + "\n時間" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        // 設定儲存路徑
        savePath = Path.Combine(Application.persistentDataPath, "accounts", username, "inventory.json");

        // 如果資料夾不存在就建立
        string folderPath = Path.GetDirectoryName(savePath);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log("✅ 建立資料夾：" + folderPath);
        }

        // 如果 user.json 不存在就建立一個空檔案
        if (!File.Exists(savePath))
        {
            InventoryData inventory = new InventoryData();
            string json = JsonUtility.ToJson(inventory, true);
            File.WriteAllText(savePath, json);
            Debug.Log("🆕 建立新的 inventory.json 檔案：" + savePath);
        }
        else
        {
            Debug.Log("📂 已存在 inventory.json：" + savePath);
        }
        //Invoke("back", 2f);  // 2秒後執行
        if (AccountSession.Instance != null && AccountSession.Instance.currentAccount != null)
        {
            AccountSession.Instance.currentAccount.username = username;
            AccountSession.Instance.currentAccount.password = password;
            AccountSession.Instance.currentAccount.level_rank = 0;
            AccountSession.Instance.currentAccount.level_difficulty = 0;
            AccountSession.Instance.currentAccount.lastLoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        else
        {
            Debug.LogWarning("⚠️ AccountSession.Instance 尚未初始化，無法儲存 Session。請確保場景中有 AccountSession 物件。");
        }
        SceneManager.LoadScene("game-list");
        return;

    }

    [Serializable]
    public class InventoryData
    {
        public List<InventoryItem> items = new List<InventoryItem>();
    }
    private List<AccountData> LoadAccounts()
    {
        if (!File.Exists(savePath))
            return new List<AccountData>();

        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<AccountList>(json).accounts;
    }

    private void SaveAccounts(List<AccountData> accounts)
    {
        AccountList wrapper = new AccountList { accounts = accounts };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }

    [System.Serializable]
    private class AccountList
    {
        public List<AccountData> accounts;
    }
    public void back()
    {
        SceneManager.LoadScene("account");
    }
}

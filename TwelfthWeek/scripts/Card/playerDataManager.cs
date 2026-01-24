using UnityEngine;
using System.IO;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    public PlayerData playerData = new PlayerData();

    string savePath,path;

    string playername = AccountSession.Instance.currentAccount.username;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        savePath = Path.Combine(Application.persistentDataPath,"accounts",playername, "playerData.json");
        Load();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("存檔完成：" + savePath);
    }

    public void Load()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("沒有存檔，建立新資料");
            playerData = new PlayerData();
            return;
        }

        string json = File.ReadAllText(savePath);
        playerData = JsonUtility.FromJson<PlayerData>(json);
        Debug.Log("讀檔完成");
    }
    public void AddTestCard()
    {
        PlayerCard newCard = new PlayerCard();
        newCard.cardId = 1;
        newCard.level = 1;
        newCard.star = 0;

        PlayerDataManager.Instance.playerData.cards.Add(newCard);
        PlayerDataManager.Instance.Save();
    }

}

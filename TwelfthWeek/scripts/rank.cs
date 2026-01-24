using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class rank : MonoBehaviour
{
    int level_rank;

    // Rank 名稱表
    List<string> rankNames = new List<string>()
    {
        "Rookie", 
        "Bronze",
        "Iron",
        "Silver",
        "Gold",
        "Platinum",
        "Diamond",
        "Master",
        "Grandmaster",
        "Legend"
    };

    [SerializeField] Text rankbox;       // 用 TMP 比較好
    [SerializeField] List<GameObject> rankIcons;    // 10 個 icon 放進這裡（依照順序）

    void Update() 
    {
        level_rank = AccountSession.Instance.currentAccount.level_rank;
        UpdateRankUI();
        UpdateRankIcon(level_rank);
    }

    public void UpdateRankUI()
    {
        level_rank = AccountSession.Instance.currentAccount.level_rank;

        // 更新 Rank 名稱
        rankbox.text = rankNames[level_rank];

        // 更新 Rank Icon
        UpdateRankIcon(level_rank);
    }

    void UpdateRankIcon(int rank)
    {
        // 全部關掉
        foreach (var icon in rankIcons)
        {
            icon.SetActive(false);
        }

        // 只開啟對應的那一個
        rankIcons[rank].SetActive(true);
    }
}

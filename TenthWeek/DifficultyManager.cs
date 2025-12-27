using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;
public class DifficultyManager : MonoBehaviour
{
    bool can_start = false;
    public TMP_Dropdown dropdown;

    // 所有難度列表
    List<string> gm = new List<string>() {
        "Select Gamemode",
        "Easy",
        "Normal",
        "Hard",
        "Expert",
        "Master"
    };

    void Start()
    {
        dropdown.ClearOptions();

        // 讀取玩家的最大可選難度
        int unlockedDifficulty = AccountSession.Instance.currentAccount.level_difficulty;

        // 🔥 限制顯示的選項數量
        unlockedDifficulty = Mathf.Clamp(unlockedDifficulty, 0, gm.Count - 1);

        List<string> availableOptions = gm.GetRange(0, unlockedDifficulty + 2);

        // 加入可用難度
        dropdown.AddOptions(availableOptions);

        // 設定當前顯示值（預設為已解鎖的最高難度）

        dropdown.value = unlockedDifficulty;
        dropdown.RefreshShownValue();

        dropdown.onValueChanged.AddListener(OnDifficultyChanged);
    }

    void OnDifficultyChanged(int index)
    {
        Debug.Log("Selected difficulty: " + gm[index]);
        if (index == 0) {
            can_start = false;
        }  else {
            AccountSession.Instance.currentAccount.level_selected = index;
            can_start = true;
        }
    }
    public void bt ()
    {
        if (can_start == true) {
            //Invoke("loading", 0.1f);
            AccountSession.Instance.currentAccount.playing_level = 1.1f ;
            SceneManager.LoadScene("prepare");
        } else if (can_start == false) {
            // return
        }
    }
}

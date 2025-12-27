using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class ml_manager : MonoBehaviour
{
    [SerializeField] Text mode_text;
    [SerializeField] Text level;
    int mode = AccountSession.Instance.currentAccount.level_selected;
    float playing_level = AccountSession.Instance.currentAccount.playing_level;
    List<string> modeNames = new List<string>()
    {
        "Easy",
        "Normal",
        "Hard",
        "Expert",
        "Master"
    };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float pl = playing_level-Mathf.Floor(playing_level);
        string pltext = (int)playing_level + "-" + pl*10;
        mode_text.text = modeNames[mode-1];
        level.text = pltext;
    }

}

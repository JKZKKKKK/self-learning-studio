using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections; 
using TMPro; 
using Unity.VisualScripting;
using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
public class chat : MonoBehaviour
{
    [SerializeField] List<GameObject> chatboxs;
    [SerializeField] List<Text> chattexts;
    [SerializeField] Text question_text;
    [SerializeField] float typingSpeed = 0.05f;
    [SerializeField] float blink_waiting = 0.5f;

    public int count = 0;
    public int dialogue_count = 0;
    public bool IsTyping ;
    private Coroutine blinkCoroutine;
    private Coroutine typingCoroutine;
 
     
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        allStatus(false);
        ClearDialogue();
        question.Instance.active(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnterKey();
    }

    void CheckEnterKey()
    {
        List<string> gm = new List<string>() {
                    "Select Gamemode",
                    "easy",
                    "normal",
                    "hard",
                    "expert",
                    "master"
                };
        if (Keyboard.current.enterKey.wasPressedThisFrame) {
            count += 1;
            dialogue_count += 1;
            string role ;
            bool reply;
            if (count > 4) { 
                allStatus(false);
                foreach (var element in chattexts) {
                    element.text = ""; // 清空文字內容
                    element.enabled = false;   
                }
                count = 1; // 重置計數器
                // 必須停止當前正在打字的協程，否則舊文字會繼續跑出來
                ClearDialogue();
            }
            chatboxs[count-1].SetActive(true);
            chattexts[count -1].enabled = true;
            Debug.Log("Count :" + count);
            Debug.Log("dialogue_count :"+dialogue_count);
            if (dialogue_count  == 1) {
                //chattexts[dialogue_count-1].text = "what is it ?!!!!!";
                StartCoroutine(TypeText("what is it ?!!!!!",chattexts[dialogue_count-1]));
            } else  if (dialogue_count  < 9) {
                if (count%2 == 0){ 
                    role = "AI";
                    reply =false;
                } else { 
                    role = "player";
                    reply =true;
                }
                //假設
                string difficulty = gm[AccountSession.Instance.currentAccount.level_selected];
                float playing_level = AccountSession.Instance.currentAccount.playing_level;
                int level = AccountSession.Instance.currentAccount.level_selected;
                if (count - 1 < chattexts.Count) {
                    Text targetText = chattexts[count - 1];
                    
                    // 安全檢查：確保 Text 物件不是 Null
                    if (targetText != null) {
                        if (OllamaConnector_bus.Instance != null) {
                            OllamaConnector_bus.Instance.prompt_combine(level, playing_level, difficulty, (dialogue_count/2)-1, reply, targetText, role);
                        } else {
                            Debug.LogError("場景中找不到 OllamaConnector_bus 實例！");
                        }
                    } else {
                        Debug.LogError($"[Chat] chattexts 列表第 {count - 1} 格是空的！請檢查 Inspector。");
                    }
                }
            } else if (dialogue_count  == 9)  {
                allStatus(false);
                foreach (var element in chattexts) {
                    element.text = "";
                }
                question.Instance.active(true);
                question.Instance.active_uninteract(false);
                role = "AI";
                string difficulty = gm[AccountSession.Instance.currentAccount.level_selected];
                float playing_level = AccountSession.Instance.currentAccount.playing_level;
                int level = AccountSession.Instance.currentAccount.level_selected;
                question.Instance.question_text(question_text,level,playing_level,difficulty,role);
                
            }
        }
    }
    IEnumerator TypeText(string fullText,Text textbox)
    {
        textbox.text = "";
        IsTyping = true;
        
        // 停止舊的閃爍
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);

        foreach (char c in fullText)
        {
            textbox.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        IsTyping = false;

        // 打完字後啟動閃爍游標
        //blinkCoroutine = StartCoroutine(BlinkCursor(textbox));
    }

        public void ClearDialogue()
    {
        // 1. 停止所有相關的協程
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
//
        // 2. 重置狀態變數
        IsTyping = false;
//
        // 3. 直接清空 UI 文字
        foreach (var box in chattexts)
        {
            if (box != null) box.text = "";
        }
        question_text.text = "";
        
        
        Debug.Log("[SYS] Dialogue Cleared & Coroutines Stopped.");
    }
    void allStatus (bool tf)
    {
        foreach (var box in chatboxs)
        {
            box.SetActive(tf);
            
        }
    }
}

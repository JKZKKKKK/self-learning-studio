using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using JetBrains.Annotations;



// 定義用來接收 JSON 的類別
[Serializable]
public class PromptLevel
{
    public int level;
    public float playing_level;
    public string title;
    public string systemPrompt;
    public string userPrompt;
    public string[] dialogueSteps;
    public string restrict;
    public string subject;
    public string learningGoal;
    public string difficulty;
}

[Serializable]
public class PromptData
{
    public List<PromptLevel> levels;
}

[Serializable]
public class OllamaResponse
{

    public string model;
    public string response;
    public bool done;
}

public class OllamaConnector : MonoBehaviour
{
    [SerializeField] float typingSpeed = 0.05f;
    [SerializeField] float blink_waiting = 0.5f;
    private string url = "http://localhost:11434/api/generate";
    public Text right_textbox;
    public Text left_textbox;
    public GameObject right_dialogue;
    public GameObject left_dialogue;
    public PromptData promptData;
    private Coroutine typingCoroutine;
    private Coroutine blinkCoroutine;
    private bool IsTyping = false;
    public int i = 0; 
    public int times = 0;
    public int entertimes =0;
    //暫時假設
    public int level =1;
    public float playing_level =1.1f;
    public string difficulty = "easy";

    void Start()
    {
        ClearDialogue();
        ClearDialogueBox();
        ActiveDialogue_Box(right_dialogue);
        
        prompt_combine(level, playing_level,difficulty,true);
        
    }

    void Update()
    {
        PromptLevel level2 = GetLevelContext(level, playing_level, difficulty);
        CheckEnterKey(level, playing_level,difficulty,level2.dialogueSteps.Length);
    }

    void prompt_combine(int level, float playing_level, string difficulty, bool lr)
    {
        PromptLevel level2 = GetLevelContext(level, playing_level, difficulty);
        Debug.Log("this is information "+
            "title " + level2.title+
            " systemPrompt "+ level2.systemPrompt+
            " dialogueStep" + level2.dialogueSteps[times]+
            " restrict " + level2.restrict+
            " subject " + level2.subject+
            " learningGoal " + level2.learningGoal+
            " difficulty " + level2.difficulty+
            "According to the information reply.And each period should be on a separate line.");
        StartCoroutine(AskOllama(
            "this is information "+
            "title " + level2.title+
            " systemPrompt "+ level2.systemPrompt+
            " dialogueStep" + level2.dialogueSteps[times]+
            " restrict " + level2.restrict+
            " subject " + level2.subject+
            " learningGoal " + level2.learningGoal+
            " difficulty " + level2.difficulty+
            "According to the information reply.And each period should be on a separate line.",lr));
    }

    IEnumerator AskOllama(string prompt, bool dialogueToward)
    {
        // 為了防止 JSON 內容有特殊換行字元導致錯誤，稍微處理一下字串
        string safePrompt = prompt.Replace("\"", "\\\"").Replace("\n", "\\n");
        string jsonPayload = "{\"model\": \"llama3.1\", \"prompt\": \"" + safePrompt + "\", \"stream\": false}";
        loading(false,dialogueToward);
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                loading(true,dialogueToward);
                OllamaResponse res = JsonUtility.FromJson<OllamaResponse>(request.downloadHandler.text);
                string aiReply = res.response;

                Debug.Log("【AI 回答】: " + aiReply);

                // --- 這裡開始處理「字串轉陣列」的邏輯 ---
                // 假設我們在 Prompt 裡要求 AI 用 "|" 分隔句子
                //string[] splitReplies = aiReply.Split(new char[] { '|', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                if (dialogueToward == false)
                {
                    // 停止之前的打字效果
                    if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                    
                    // 呼叫修正後的打字協程 (我們傳入第一段 splitReplies[0])
                    typingCoroutine = StartCoroutine(TypeText_left("Me : \n" + aiReply));
                }       
                else
                {
                    if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                    
                    // 呼叫修正後的打字協程 (我們傳入第一段 splitReplies[0])
                    typingCoroutine = StartCoroutine(TypeText_right("AI : \n" + aiReply));
                }
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
    public PromptLevel GetLevelContext(int level, float playing_level, string difficulty)

    {

        string path = Path.Combine(

            Application.streamingAssetsPath,

            "Json",

            "prompt_levels.json"

        );

        //Debug.Log(path);



        // 讀 JSON

        string json = File.ReadAllText(path);

        promptData = JsonUtility.FromJson<PromptData>(json);



        // 找出符合條件的關卡

        PromptLevel result = promptData.levels.Find(l =>

            l.level == level &&

            Mathf.Approximately(l.playing_level, playing_level) &&

            l.difficulty == difficulty

        );

        if (result == null)

            Debug.LogWarning("找不到符合條件的關卡！");
        return result;

    }


    // 修正：參數從 string[] 改回 string
    IEnumerator TypeText_left(string fullText)
    {
        left_textbox.text = "";
        IsTyping = true;
        
        // 停止舊的閃爍
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);

        foreach (char c in fullText)
        {
            left_textbox.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        IsTyping = false;

        // 打完字後啟動閃爍游標
        blinkCoroutine = StartCoroutine(BlinkCursor(left_textbox));
    }
    IEnumerator TypeText_right(string fullText)
    {
        right_textbox.text = "";
        IsTyping = true;
        
        // 停止舊的閃爍
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);

        foreach (char c in fullText)
        {
            right_textbox.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        IsTyping = false;

        // 打完字後啟動閃爍游標
        blinkCoroutine = StartCoroutine(BlinkCursor(right_textbox));
    }

    IEnumerator BlinkCursor(Text gm)
    {
        gm.text += "\n>";
        while (true)
        {
            gm.text += "_";
            yield return new WaitForSeconds(blink_waiting);
            gm.text = gm.text.TrimEnd('_');
            yield return new WaitForSeconds(blink_waiting);
        }
    }
    void loading(bool ai_result,  bool dialogueToward)
    {
        if (ai_result == false)
        {
            if (dialogueToward == false)
            {
                typingCoroutine = StartCoroutine(TypeText_left("loading......."));
            }       
            else
            {
                typingCoroutine = StartCoroutine(TypeText_right("loading......."));
            }
        } else
        {
            ClearDialogue();
        }

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

        // 2. 重置狀態變數
        IsTyping = false;

        // 3. 直接清空 UI 文字
        if (left_textbox != null) left_textbox.text = "";
        if (right_textbox != null) right_textbox.text = "";
        
        Debug.Log("[SYS] Dialogue Cleared & Coroutines Stopped.");
    }
    void ClearDialogueBox()
    {
        left_dialogue.SetActive(false);
        right_dialogue.SetActive(false);
    }
    void ActiveDialogue_Box(GameObject go)
    {

        go.SetActive(true);

    }
    void CheckEnterKey(int level, float playing_levels, string difficulty,int arrayLenght)
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            entertimes +=1;
            Debug.Log("Enter 被按下！");
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            // 停止閃爍游標
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
            }
            // 清空文字
            ClearDialogue();

            // times計數
            times +=1;
            if (times == arrayLenght)
            {
                playing_level += 0.1f;
            } 
            else if (times < arrayLenght)
            {
                ClearDialogue();
                ClearDialogueBox();
                ActiveDialogue_Box(left_dialogue);
                prompt_combine(level, playing_levels,difficulty,false);

            }
        }
    }
}  
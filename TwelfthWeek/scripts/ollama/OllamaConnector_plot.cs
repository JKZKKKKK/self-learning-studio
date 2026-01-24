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
using NUnit.Framework.Interfaces;



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

[Serializable]
public class AiRespondMemory
{
    public string time;
    public string role;
    public string content;
    public int level;
    public string difficulty;
}

[Serializable]
public class AiMemoryData
{
    public List<AiRespondMemory> memories = new List<AiRespondMemory>();
}


public class OllamaConnector_plot : MonoBehaviour
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

    public static OllamaConnector_plot Instance;
    private bool IsTyping = false;
    public int i = 0; 
    public int times = 0;
    public int entertimes =0;
    //暫時假設
    public int level = AccountSession.Instance.currentAccount.level_selected;
    public float playing_level = AccountSession.Instance.currentAccount.playing_level;

    public string difficulty ;
    
    public string role;
    public string savePath;
    
    void Start()
    {
        List<string> gm = new List<string>() {
            "Select Gamemode",
            "easy",
            "Normal",
            "Hard",
            "Expert",
            "Master"
        };
        string difficulty = gm[AccountSession.Instance.currentAccount.level_selected];

        Debug.Log("level : "+level+"playing_leveling"+ playing_level+"difficulty : "+difficulty);
        ClearDialogue();
        ClearDialogueBox();
        ActiveDialogue_Box(right_dialogue);
        
        prompt_combine(level, playing_level,difficulty,true,false);
        
    }
    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        PromptLevel level2 = GetLevelContext(level, playing_level, difficulty);
        CheckEnterKey(level, playing_level,difficulty,level2.dialogueSteps.Length);
    }

    void prompt_combine(int level, float playing_level, string difficulty, bool lr,bool reply)
    {
        PromptLevel level2 = GetLevelContext(level, playing_level, difficulty);
        if (!reply)
        {
            Debug.Log("this is information "+
                "title " + level2.title+
                " systemPrompt "+ level2.systemPrompt+
                " dialogueStep" + level2.dialogueSteps[times/2]+
                " restrict " + level2.restrict+
                " subject " + level2.subject+
                " learningGoal " + level2.learningGoal+
                " difficulty " + level2.difficulty+
                "According to the information reply.And each period should be on a separate line.");
            StartCoroutine(AskOllama(
                "this is information "+
                "title " + level2.title+
                " systemPrompt "+ level2.systemPrompt+
                " dialogueStep" + level2.dialogueSteps[times/2]+
                " restrict " + level2.restrict+
                " subject " + level2.subject+
                " learningGoal " + level2.learningGoal+
                " difficulty " + level2.difficulty+
                "According to the information reply.And each period should be on a separate line.",lr));
        } else
        {
            AiRespondMemory result =  LoadLatestMemory();
            Debug.Log("this is information "+
                "Generation time " + result.time+
                " content" + result.content+
                " level " + result.level+
                " difficulty " + result.difficulty+
                "According to the information reply which You need to put yourself in the player's mindset and generate the content the player might say..And each period should be on a separate line.You are the player character inside the game world.Respond as natural spoken dialogue, not narration.Do NOT explain game mechanics.Do NOT mention levels, difficulty, systems, or UI.Do NOT ask multiple questions.Sound curious but restrained, like someone experiencing this for the first time.Only output the dialogue itself.Generate 4 to 5 rows");
            StartCoroutine(AskOllama(
                "this is information "+
                "Generation time " + result.time+
                " content" + result.content+
                " level " + result.level+
                " difficulty " + result.difficulty+
                "According to the information reply which You need to put yourself in the player's mindset and generate the content the player might say..And each period should be on a separate line.You are the player character inside the game world.Respond as natural spoken dialogue, not narration.Do NOT explain game mechanics.Do NOT mention levels, difficulty, systems, or UI.Do NOT ask multiple questions.Sound curious but restrained, like someone experiencing this for the first time.Only output the dialogue itself.Generate 4 to 5 rows",lr));

        }
        
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
                //memory
                if (dialogueToward)
                {
                    role = "AI";
                } else {
                    role = "player";
                }
                SaveMemory(role,res.response);
 
                Debug.Log("【AI 回答】: " + aiReply);

                // --- 這裡開始處理「字串轉陣列」的邏輯 ---
                // 假設我們在 Prompt 裡要求AI 用 "|" 分隔句子
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

    void replyDialogue(int level, float playing_levels, string difficulty)
    {
        ClearDialogue();
        ClearDialogueBox();
        ActiveDialogue_Box(left_dialogue);
        prompt_combine(level, playing_levels,difficulty,false,true);
    }

    void SaveMemory(string role, string content)
    {
        string username = AccountSession.Instance.currentAccount.username;//暫時
        savePath = Path.Combine(
            Application.persistentDataPath,
            "accounts",
            username,
            "memory.json"
        );

        // exist
        Directory.CreateDirectory(Path.GetDirectoryName(savePath));

        AiMemoryData data;

        // 舊記憶
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            data = JsonUtility.FromJson<AiMemoryData>(json);
        }
        else
        {
            data = new AiMemoryData();
        }

        // 新記憶
        AiRespondMemory memory = new AiRespondMemory
        {
            time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            role = role,
            content = content,
            level = level,
            difficulty = difficulty
        };

        data.memories.Add(memory);

        // 寫回 JSON
        string output = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, output);

        Debug.Log("💾 Memory Saved");
    }

    //Get latest Memory
    public AiRespondMemory LoadLatestMemory()
    {
        string username = AccountSession.Instance.currentAccount.username;//暫時
        string path = Path.Combine(
            Application.persistentDataPath,
            "accounts",
            username,
            "memory.json"
        );

        if (!File.Exists(path))
        {
            Debug.LogWarning("memory.json 不存在");
            return null;
        }

        string json = File.ReadAllText(path);
        AiMemoryData data = JsonUtility.FromJson<AiMemoryData>(json);

        if (data == null || data.memories == null || data.memories.Count == 0)
        {
            Debug.LogWarning("memory.json 是空的");
            return null;
        }

        // 取最後一筆（最新）
        return data.memories[data.memories.Count - 1];
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
                if (playing_level == 1.2f) {
                    SceneManager.LoadScene("waiting_for_busstop");
                }
            } 
            else if (times < arrayLenght)
            {
                if ( times%2 ==0 )
                {
                    ClearDialogue();
                    ClearDialogueBox();
                    ActiveDialogue_Box(right_dialogue);
                    prompt_combine(level, playing_levels,difficulty,true,false);
                } else
                {
                    replyDialogue(level, playing_levels,difficulty);
                }
                

            }
        }
    }
}  
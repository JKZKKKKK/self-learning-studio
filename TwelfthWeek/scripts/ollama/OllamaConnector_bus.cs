using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

[Serializable]
public class PromptLevel2 // 統一命名，解決 CS0029
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
public class PromptData2
{
    public List<PromptLevel> levels;
}

public class OllamaConnector_bus : MonoBehaviour
{
    [SerializeField] float typingSpeed = 0.05f;
    [SerializeField] float blink_waiting = 0.5f;
    public static OllamaConnector_bus Instance;
    public PromptData2 promptData;
    private Coroutine typingCoroutine;
    private Coroutine blinkCoroutine;
    public bool IsTyping;
    private string url = "http://localhost:11434/api/generate";

    void Awake() { Instance = this; }

    public void prompt_combine(int level, float playing_level, string difficulty, int dialogue_count, bool reply, Text text, string role)
    {
        PromptLevel levelData = GetLevelContext(level, playing_level, difficulty);
        if (levelData == null) return;

        string finalPrompt = "";
        if (!reply)
        {
            // 確保索引不越界
            int index = Mathf.Clamp(dialogue_count, 0, levelData.dialogueSteps.Length - 1);
            finalPrompt = $"[Identity]: {levelData.systemPrompt}\n[Situation]: {levelData.dialogueSteps[index]}\n[Constraint]: {levelData.restrict}. Respond naturally in english";
        }
        else
        {
            AiRespondMemory mem = LoadLatestMemory();
            finalPrompt = $"[Context]: {mem?.content}\n[Task]: Act as the player. Respond with a natural, short sentence (4-5 words) in english";
        }
        StartCoroutine(AskOllama_bus(finalPrompt, text, role, level, difficulty));
    }

    public string GetQuestionPrompt(int level, float playing_level, string difficulty)
    {
        PromptLevel data = GetLevelContext(level, playing_level, difficulty);
        AiRespondMemory mem = LoadLatestMemory();
        return $"[Goal]: {data?.learningGoal}\n[Last Event]: {mem?.content}\nGenerate a challenging choice-based question in english";
    }

    public IEnumerator AskOllama_bus(string prompt, Text textbox, string role, int level, string difficulty)
    {
        if (textbox == null) yield break;
        IsTyping = true;
        string jsonPayload = "{\"model\": \"llama3.1\", \"prompt\": \"" + prompt.Replace("\"", "\\\"").Replace("\n", "\\n") + "\", \"stream\": false}";
        
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            loading(false,textbox);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                OllamaResponse res = JsonUtility.FromJson<OllamaResponse>(request.downloadHandler.text);
                SaveMemory(role, res.response, level, difficulty);
                loading(true,textbox);
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                typingCoroutine = StartCoroutine(TypeText(res.response, textbox));
            }
            IsTyping = false;
        }
    }

    IEnumerator TypeText(string fullText, Text text)
    {
        text.text = "";
        foreach (char c in fullText)
        {
            if (text == null) yield break;
            text.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(BlinkCursor(text));
    }

    IEnumerator BlinkCursor(Text gm) // 修正 228/288 行崩潰
    {
        if (gm == null) yield break;
        gm.text += "\n>";
        while (gm != null && gm.gameObject.activeInHierarchy)
        {
            gm.text += "_";
            yield return new WaitForSeconds(blink_waiting);
            if (gm == null) yield break;
            gm.text = gm.text.TrimEnd('_');
            yield return new WaitForSeconds(blink_waiting);
        }
    }
    void SaveMemory(string role,string content, int level,string difficulty)
    {
        List<string> gm = new List<string>() {
            "Select Gamemode",
            "easy",
            "normal",
            "hard",
            "expert",
            "master"
        };
        string username = AccountSession.Instance.currentAccount.username;//暫時
        string savePath = Path.Combine(
            Application.persistentDataPath,
            "accounts",
            username,
            "memory_bus_station.json"
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

        Debug.Log("💾 bus sataion Memory Saved \nPath :" +savePath);
    }
    public AiRespondMemory LoadLatestMemory()
    {
        string username = "Ryan";//暫時
        string path = Path.Combine(
            Application.persistentDataPath,
            "accounts",
            username,
            "memory_bus_station.json"
        );

        if (!File.Exists(path))
        {
            Debug.LogWarning("memory_bus_station.json 不存在");
            return null;
        }

        string json = File.ReadAllText(path);
        AiMemoryData data = JsonUtility.FromJson<AiMemoryData>(json);

        if (data == null || data.memories == null || data.memories.Count == 0)
        {
            Debug.LogWarning("memory_bus_station.json 是空的");
            return null;
        }

        // 取最後一筆（最新）
        return data.memories[data.memories.Count - 1];
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

        promptData = JsonUtility.FromJson<PromptData2>(json);



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
    void loading(bool ai_result, Text text)
    {
        if (ai_result == false)
        {
            
            typingCoroutine = StartCoroutine(TypeText("loading.......",text));

        }
    }
    

}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class question : MonoBehaviour
{
    [SerializeField] GameObject box;
    [SerializeField] Button allow_button;
    [SerializeField] Button deny_button;
    [SerializeField] Text text;

    public static question Instance;
    public bool interactable = false;

    private void Awake() { Instance = this; }

    void Update()
    {
        // 當 AI 沒在說話且開啟了互動開關時，啟用按鈕
        if (OllamaConnector_bus.Instance != null && !OllamaConnector_bus.Instance.IsTyping && interactable)
        {
            active_uninteract(true);
        }
    }
    public void question_text(Text text, int level, float playing_level, string difficulty, string role)
    {
        // 獲取 Prompt
        string result = OllamaConnector_bus.Instance.GetQuestionPrompt(level, playing_level, difficulty);
        
        // 啟動 AI 請求
        StartCoroutine(OllamaConnector_bus.Instance.AskOllama_bus(result, text, role, level, difficulty));
        
        interactable = true; // 開啟 Update 裡的按鈕啟用檢查
    }

    public void allow_selection()
    {
        SendChoice("Positive/Yes");
    }

    public void deny_selection()
    {
        SendChoice("Negative/No");
    }

    private void SendChoice(string answer)
    {
        Debug.Log("Button Clicked: " + answer);
        AiRespondMemory latestMemory = OllamaConnector_bus.Instance.LoadLatestMemory();
        if (latestMemory == null) return;

        string prompt = $"[Situation]: {latestMemory.content}\n[Player Choice]: {answer}\nGenerate the consequence in english";
        
        // 關鍵修正：必須使用 StartCoroutine
        StartCoroutine(OllamaConnector_bus.Instance.AskOllama_bus(prompt, text, "AI", latestMemory.level, latestMemory.difficulty));
        active_uninteract(false); // 點擊後禁用，避免重複點擊
    }

    public void active_uninteract(bool tf)
    {
        allow_button.interactable = tf;
        deny_button.interactable = tf;
    }

    public void active(bool tf)
    {
        box.SetActive(tf);
        allow_button.image.enabled = tf;
        deny_button.image.enabled = tf;
        interactable = tf;
    }
}
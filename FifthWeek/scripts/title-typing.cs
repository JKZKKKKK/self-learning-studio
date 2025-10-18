using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections; 
using TMPro; 
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using System.Net.NetworkInformation; // 如果你用 TextMeshPro
public class blackboard : MonoBehaviour {

    [SerializeField] Text enterbox;
    [SerializeField] GameObject progress_bar;
    [SerializeField] string[] fullTexts;
    [SerializeField] float typingSpeed = 0.05f;
    [SerializeField] float blink_waiting = 0.5f;
    [SerializeField] float progress_bar_waiting = 0.05f;


    private Coroutine typingCoroutine;
    private Coroutine blinkCoroutine;
    private int progressValue = 2;
    private bool IsTyping = false;
    int i = 0;  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enterbox.color = Color.white;
        StartTyping();
        UpdateProgressBar();
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnterKey();
    }
    void StartTyping()
    {
        typingCoroutine = StartCoroutine(TypeText());
    }
    IEnumerator TypeText()
    {
        enterbox.text = "";
        IsTyping = true;
        foreach (char c in fullTexts[i])
        {
            enterbox.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        IsTyping = false;

        // 打完字後啟動閃爍游標
        blinkCoroutine = StartCoroutine(BlinkCursor());
    }

    IEnumerator BlinkCursor()
    {
        enterbox.text += "\n>";
        while (true)
        {
            enterbox.text += "_";
            yield return new WaitForSeconds(blink_waiting);
            enterbox.text = enterbox.text.TrimEnd('_');
            yield return new WaitForSeconds(blink_waiting);
        }
    }

    void CheckEnterKey()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            Debug.Log("Enter 被按下！");
            // 停止正在打字
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            // 停止閃爍游標
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
            }
            Debug.Log(fullTexts.Length);
            Debug.Log(i);
            // 清空文字
            enterbox.text = "";
            if (fullTexts.Length - 2 > i)
            {
                i += 1;
                ModifyProgressBar(2);
            }
            else if (fullTexts.Length - 2 == i)
            {
                i += 1;
                enterbox.color = new Color(0f, 9547169f, 0.687027f);
                ModifyProgressBar(2);
            }
            else
            {
                SceneManager.LoadScene("game-list");
            }
            // 從頭開始
            StartTyping();
        }
    }
    void ModifyProgressBar(int num)
    {
        progressValue = Mathf.Clamp(progressValue + num, 0, progress_bar.transform.childCount);
        UpdateProgressBar();
    }

    void UpdateProgressBar()
    {
        for (int i = 0; i < progress_bar.transform.childCount; i++)
        {
            Renderer rend = progress_bar.transform.GetChild(i).GetComponent<Renderer>();

            // 啟用或關閉可見性（不會停用物件本身）
            if (rend != null)
                rend.enabled = i < progressValue;
            if (i == progressValue - 1)
            {// 剛亮起的那一格
                StartCoroutine(FlashBar(rend));
            }
        }
    }

    IEnumerator FlashBar(Renderer rend)
    {
        for (int i = 0; i < 3; i++)
        {
            rend.enabled = false;
            yield return new WaitForSeconds(progress_bar_waiting);
            rend.enabled = true;
            yield return new WaitForSeconds(progress_bar_waiting);
        }
    }



}
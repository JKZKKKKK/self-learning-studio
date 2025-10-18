using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections; 
using TMPro; 
using Unity.VisualScripting; // 如果你用 TextMeshPro

public class typing_box : MonoBehaviour
{
    [SerializeField] Text enterbox;
    [SerializeField] string fullText = "Please press Enter to continue:";
    [SerializeField] float typingSpeed = 0.05f;
    [SerializeField] float blink_waiting = 0.5f;

    private Coroutine typingCoroutine;
    private Coroutine blinkCoroutine;
    private bool IsTyping = false;

    void Start()
    {
        StartTyping();
    }

    void Update()
    {
        CheckEnterKey();
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

            // 清空文字
            enterbox.text = "";

            // 從頭開始
            StartTyping();
        }
    }

    void StartTyping()
    {
        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        enterbox.text = "";
        IsTyping = true;

        foreach (char c in fullText)
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
        while (true)
        {
            enterbox.text += "_";
            yield return new WaitForSeconds(blink_waiting);
            enterbox.text = enterbox.text.TrimEnd('_');
            yield return new WaitForSeconds(blink_waiting);
        }
    }
}

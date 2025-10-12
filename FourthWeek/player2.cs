using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // 新 輸入系統命名空間
using UnityEngine.SceneManagement;


public class Player2 : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] int Hp;
    [SerializeField] GameObject HpBar;
    [SerializeField] Text scoreText;
    [SerializeField] GameObject replayButton;
    int score;
    float scoreTime;
    GameObject currentFloor;
    SpriteRenderer render;
    Animator anim;
    AudioSource deathSource;

    void Start()
    {
        Hp = 10;
        score = 0;
        scoreTime = 0f;
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        deathSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 用新 InputSystem 的鍵盤輸入
        if (Keyboard.current.dKey.isPressed)
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
            render.flipX = false;
            anim.SetBool("run", true);
        }
        else if (Keyboard.current.aKey.isPressed)
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
            render.flipX = true;
            anim.SetBool("run", true);
        }
        else
        {
            anim.SetBool("run", false);
        }
        UpdateScore();  
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Normal")
        {
            if (other.contacts[0].normal == new Vector2(0f,1f))
            {
                //Debug.Log("撞到了階梯");
                currentFloor = other.gameObject;
                ModifyHp(1);
                other.gameObject.GetComponent<AudioSource>().Play();
            }
            
        }
        else if (other.gameObject.tag == "Nails")
        {
            if (other.contacts[0].normal == new Vector2(0f,1f))
            {
                //Debug.Log("撞到了尖刺");
                currentFloor = other.gameObject;
                ModifyHp(-3);
                anim.SetTrigger("hurt");
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if (other.gameObject.tag == "ceiling")
        {
            currentFloor.GetComponent<BoxCollider2D>().enabled = false;//把腳下的BoxCollider2D關掉
            ModifyHp(-3);
            other.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "DeathLine")
        {
            die();
        }
    }

    void ModifyHp(int num)
    {
        Hp += num;//Hp = Hp + num
        if (Hp > 10)
        {
            Hp = 10;
        }
        else if (Hp < 0)
        {
            Hp = 0;
            die();
        }
        UpdateHpBar();
    }

    void UpdateHpBar()
    {
        for (int i = 0; i < HpBar.transform.childCount; i++)
        {
            if (Hp > i)
            {
                HpBar.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                HpBar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void UpdateScore()
    {
        scoreTime += Time.deltaTime;
        if (scoreTime > 2f)
        {
            score++;
            scoreTime = 0f;
            scoreText.text = "地下" + score.ToString() + "層";
        }
    }

    void die()
    {
        deathSource.Play();
        Time.timeScale = 0f;//將遊戲的執行時間變成N倍
        replayButton.SetActive(true);
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

}

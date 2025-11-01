using UnityEngine;
using UnityEngine.SceneManagement;

public class account : MonoBehaviour
{
    [SerializeField] GameObject loginButton;
    [SerializeField] GameObject signinButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void login()
    {
        SceneManager.LoadScene("login");
    }
    public void signin()
    {
        SceneManager.LoadScene("signin");
    }
}

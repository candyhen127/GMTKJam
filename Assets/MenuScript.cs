using System.Collections;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class MenuScript : MonoBehaviour
{
    public static MenuScript Instance;
    public Player player;
    public bool paused;
    public bool truepaused;
    
    public GameObject gameOverScreen;
    public GameObject pauseScreen;

    public GameObject winScreen;
    public GameObject nuke;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        //player = GameObject.Find("Robot").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //don't allow input when paused
        if(paused == true){return;}
        //timeleft = player.battery;
        //spawntimer += Time.deltaTime;
        /*
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("p"))
        {
            pauseGame();
        }
        */
    }
/*
    public void pauseGame()
    {
        if (truepaused)
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
            truepaused = false;
        } else
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0;
            truepaused = true;
        }
    }
*/
    public void MainMenu()
    {
        StartCoroutine(RestartRoutine("Title"));
    }

    public void GoToShop()
    {
        StartCoroutine(RestartRoutine("ShopScene"));
    }

    public void StartRun()
    {
        StartCoroutine(RestartRoutine("MainScene"));
    }

    public IEnumerator RestartRoutine(String scene)
    {
        
       // StartCoroutine(MenuManager.Instance.AudioFade(true));
        yield return new WaitForSecondsRealtime(0.1f);
        //MenuManager.Instance.StartCoroutine(MenuManager.Instance.FadeImage(false));
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(scene);
        Time.timeScale = 1;
    }

    public void EndRun()
    {
        
            truepaused = true;
            //MenuManager.Instance.StartCoroutine(MenuManager.Instance.flash(Color.red));
            GameManager.Instance.globalScrap += player.scrap;
            for (int i = 0; i < player.inventory.Count; i++)
            {
                player.inventory[i].numCollected ++;
            }
        StartCoroutine(GameOverRoutine());
        
    }

    public IEnumerator GameOverRoutine()
    {
        //StartCoroutine(MenuManager.Instance.AudioFade(true));
        //canvas.GameOver();
        gameOverScreen.SetActive(true);
        for(float i = 1f; i>=0; i-=Time.unscaledDeltaTime)
        {
            Time.timeScale = i;
            yield return null;
        }
        Time.timeScale = 0;
        
        
    
    }

    public void WinGame()
    {
        
            truepaused = true;
            //MenuManager.Instance.StartCoroutine(MenuManager.Instance.flash(Color.white));
        Instantiate(nuke, player.transform.position, Quaternion.identity);
        StartCoroutine(WinRoutine());
        
    }

    public IEnumerator WinRoutine()
    {
        //StartCoroutine(MenuManager.Instance.AudioFade(true));
        //canvas.GameOver();
        winScreen.SetActive(true);
        for(float i = 1f; i>=0; i-=Time.unscaledDeltaTime)
        {
            Time.timeScale = i;
            yield return null;
        }
        Time.timeScale = 0;
        
        
    
    }
}

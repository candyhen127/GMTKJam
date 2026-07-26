using System.Collections;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Microsoft.Unity.VisualStudio.Editor;

public class MenuScript : MonoBehaviour
{
    public static MenuScript Instance;
    public Player player;
    public bool isPaused;
    public bool truepaused;


    public GameObject winScreen;

    public GameObject settingsPanel;

    public ShopManager shop;

    public GameObject gameOverScreen;
    public GameObject pauseScreen;

    public GameObject trueGameOverScreen;
    public TextMeshProUGUI truegameoverblurb;

    public UnityEngine.UI.Image fadeout;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;

        truepaused = false;
        StartCoroutine(FadeImage(true));
        if (GameManager.Instance == null) {return;}
        
        GameManager.Instance.player = player;
        GameManager.Instance.gameOverScreen = gameOverScreen;
        GameManager.Instance.pauseScreen = pauseScreen;
        GameManager.Instance.settingsPanel = settingsPanel;
        //player = GameObject.Find("Robot").GetComponent<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (truepaused == true) {return;}
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

        public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pauseScreen != null) 
            pauseScreen.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseScreen != null) 
            pauseScreen.SetActive(false);

        if (settingsPanel != null) 
            settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        if (settingsPanel != null) 
            settingsPanel.SetActive(true);

        if (pauseScreen != null) 
            pauseScreen.SetActive(false);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) 
            settingsPanel.SetActive(false);

        if (pauseScreen != null) 
            pauseScreen.SetActive(true);
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
        StartCoroutine(RestartRoutine("Title Screen"));
    }

    public void GoToShop()
    {
        StartCoroutine(RestartRoutine("ShopScene"));
    }

    public void StartRun()
    {
        if (shop != null)
        {
            if (shop.head.bodypart == "blank" || shop.leftArm.bodypart == "blank" || shop.rightArm.bodypart == "blank" || shop.leftLeg.bodypart == "blank" || shop.rightLeg.bodypart == "blank")
            {
                return;
            }
            shop.LockInBuild();
        }
        if (GameManager.Instance != null)
        {
            
        GameManager.Instance.totalRuns++;
        }
        Debug.Log("start");
        StartCoroutine(RestartRoutine("MainScene"));
    }

    public IEnumerator RestartRoutine(String scene)
    {
        // StartCoroutine(MenuManager.Instance.AudioFade(true));
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(FadeImage(false));
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(scene);
        Time.timeScale = 1;
    }

    public void EndRun()
    {
        truepaused = true;
        //MenuManager.Instance.StartCoroutine(MenuManager.Instance.flash(Color.red));
        if (GameManager.Instance != null && player != null)
        {
            GameManager.Instance.globalScrap += player.scrap;
            GameManager.Instance.totalScrap += player.scrap;
            
                int sum = 0;
            if (player.inventory != null)
            {
                for (int i = 0; i < player.inventory.Count; i++)
                {
                    if (player.inventory[i] != null)
                    {
                        player.inventory[i].numCollected++;
                        sum++;
                    }
                }
                
                        GameManager.Instance.totalParts+= sum;
            }
            String s = "Scrap Collected: " + player.scrap + "\nParts Collected: " + sum;
            Debug.Log(s);
        truegameoverblurb.text = s;
        }
        StartCoroutine(GameOverRoutine());
    }

    public IEnumerator GameOverRoutine()
    {
        //StartCoroutine(MenuManager.Instance.AudioFade(true));
        //canvas.GameOver();
        if (gameOverScreen != null) gameOverScreen.SetActive(true);
        for (float i = 1f; i >= 0; i -= Time.unscaledDeltaTime)
        {
            Time.timeScale = i;
            yield return null;
        }
        Time.timeScale = 0;
    }

    public void TrueGameOver()
    {
        trueGameOverScreen.SetActive(true);
        String s = "Total Runs: " + GameManager.Instance.totalRuns + 
                    "\nTotal Scrap Collected: " + GameManager.Instance.totalScrap + 
                    "\nTotal Parts Collected: " + GameManager.Instance.totalParts;
        truegameoverblurb.text = s;
    }

    public void WinGame()
    {
        truepaused = true;
        //MenuManager.Instance.StartCoroutine(MenuManager.Instance.flash(Color.white));

        String s = "Total Runs: " + GameManager.Instance.totalRuns + 
                    "\nTotal Scrap Collected: " + GameManager.Instance.totalScrap + 
                    "\nTotal Parts Collected: " + GameManager.Instance.totalParts;

                    Debug.Log(s);
        
        StartCoroutine(WinRoutine());
    }

    public IEnumerator WinRoutine()
    {
        //StartCoroutine(MenuManager.Instance.AudioFade(true));
        //canvas.GameOver();
        if (winScreen != null) winScreen.SetActive(true);
        for (float i = 1f; i >= 0; i -= Time.unscaledDeltaTime)
        {
            Time.timeScale = i;
            yield return null;
        }
        Time.timeScale = 0;
    }

    public IEnumerator FadeImage(bool b)    //true = enter, false = leave;
    {
        Debug.Log("fadeout " + b);
        fadeout.enabled = true;
        if(b)
        {
            fadeout.color = new Color(0, 0, 0, 1);
            for(float i = 1; i>=0; i-=Time.fixedDeltaTime*2)
            {
                fadeout.color = new Color(0, 0, 0, i);
                yield return null;
            }
            fadeout.color = new Color(0, 0, 0, 0);
            fadeout.enabled = false;

            
            
        }
        else
        {
            
            fadeout.color = new Color(0, 0, 0, 0);
            for(float i = 0; i<=1; i+=Time.fixedDeltaTime*2)
            {
                fadeout.color = new Color(0, 0, 0, i);
                yield return null;
            }
            fadeout.color = new Color(0, 0, 0, 1);
        }
        
    }
}
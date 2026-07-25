using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices.WindowsRuntime;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Player player;
    public bool paused;
    public bool truepaused;
    //public TextMeshProUGUI timer;

    public float timeleft = 600;
    public float startTimeLeft = 600;
    public float halftime = 300;
    float spawntimer;
    public float spawnInterval;
    public float baseSpawnInterval = 2.2f;
    public float spawnDistance;
    public int maxEnemiesAtSpawn = 1;
    public int baseMaxEnemies = 1;

    public int difficultyUpMinute = 9;
    public int difficulty = 1;

    public List<GameObject> enemyPrefabs; 

    public GameObject gameOverScreen;
    public GameObject pauseScreen;

    public GameObject winScreen;

    public GameObject settingsPanel;
    public GameObject nuke;

    public bool won = false;

    public float spawnIntervalScale = 1.075f;
    public float maxEnemiesScale = 0.25f;

    public TextMeshProUGUI batteryText;
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI depthTextUI;

    public float startYPosition = 0f;
    public int startDepthMeters = 0;


    //public int[] quadrants = {0, 1, 2, 3};


    // Start is called before the first frame update
    void Start()
    {   
        //update start depth meters
        if (player != null)
        {
            startYPosition = player.transform.position.y;
            startDepthMeters = (int)startYPosition;
        }
        //startTimeLeft = MenuManager.Instance.startTimeLeft;
        Time.timeScale = 1;
        timeleft = startTimeLeft;
        Instance = this;
        spawntimer = 0f;
        spawnInterval = baseSpawnInterval;
        maxEnemiesAtSpawn = baseMaxEnemies;
    }

    // Update is called once per frame
    void Update()
    {
        // Allow ESC or P to toggle pause state
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (truepaused)
            {
                ResumeGame();
            }
            else
            {
                pauseGame();
            }
        }
    }

    void FixedUpdate()
    {
        if (timeleft <= 0 && !won)
        {
            //WinGame();
            //won = true;
            
            
            //return;
        }
        if (won)
        {
            //MenuManager.Instance.aud.pitch = 1f;
            return;
        }
        int minutes = (int)(timeleft/60);
        int seconds = (int)(timeleft%60);
        String secondstring = seconds.ToString();
        if(seconds < 10)
        {
            secondstring = "0" + secondstring;
        }

        //battery % based on timeleft / startTimeLeft
        float batteryPercent = Mathf.Clamp((timeleft / startTimeLeft) * 100f, 0, 100);
        
        if (batteryText != null) 
            batteryText.text = "battery: " + (int)batteryPercent + "%";

        if (player != null && depthTextUI != null)
        {
            // Calculate distance descended (how far below startYPosition the player is)
            float distanceDescended = startYPosition - player.transform.position.y;
            
            // count positive downward movement (so jumping up doesn't decrease depth)
            distanceDescended = Mathf.Max(0, distanceDescended);

            // Curr Depth = Starting Depth + Distance Travelled Down
            int currentDepth = startDepthMeters + Mathf.FloorToInt(distanceDescended);

            depthTextUI.text = -currentDepth + " m";
        }
            
        //timer.text = minutes +":" + secondstring;
        
        if (timeleft < 60)
        {
            //timer.color = Color.red;
            
            //MenuManager.Instance.aud.pitch = Mathf.Lerp(1.01f, 1.3f, 60 - timeleft);;
        } else
        {
            //timer.color = Color.white;
            //MenuManager.Instance.aud.pitch = 1f;
        }
        
    }


    public void SpawnEnemy(int number)
    {
        for(int i = 0; i < 4; i++) {
            float angle = UnityEngine.Random.Range(0f, Mathf.PI / 2) + i * (Mathf.PI / 2);
            
            // Calculate the spawn position
            Vector3 spawnOffset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * spawnDistance;
            Vector3 spawnPos = player.transform.position + spawnOffset;

            // Instantiate (or pull from object pool)
            for(int j = 0; j < number; j++) {
                GameObject enemy;
                if (timeleft < halftime)
                {
                    enemy = enemyPrefabs[(int)UnityEngine.Random.Range(0, 1.5f)];
                } else
                {
                    enemy = enemyPrefabs[0];
                }
                Instantiate(enemy, spawnPos + new Vector3(i * 0.25f, 0, 0), Quaternion.identity);
            }
        }
        //quadrants = quadrants.OrderBy(x => UnityEngine.Random.value).ToArray();
    }

    public void pauseGame()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
        truepaused = true;
        paused = true;
    }

    public void ResumeGame()
    {
        // 1. Hide UI screens
        if (pauseScreen != null) pauseScreen.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        // 2. Reset time and state flags
        Time.timeScale = 1f;
        truepaused = false;
        paused = false;
    }
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    public void loseGame()
    {
        
            truepaused = true;
            //MenuManager.Instance.StartCoroutine(MenuManager.Instance.flash(Color.red));
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

    public void RestartGame()
    {
        StartCoroutine(RestartRoutine("SampleScene"));
    }

    public IEnumerator RestartRoutine(String scene)
    {
        
       // StartCoroutine(MenuManager.Instance.AudioFade(true));
        yield return new WaitForSecondsRealtime(0.1f);
        //MenuManager.Instance.StartCoroutine(MenuManager.Instance.FadeImage(false));
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(scene);
    }

    public void MainMenu()
    {
        StartCoroutine(RestartRoutine("Title"));
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

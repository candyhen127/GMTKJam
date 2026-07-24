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
    //public TextMeshProUGUI timer;

    public float globalScrap;
    public List<Part> allParts;


    public GameObject gameOverScreen;
    public GameObject pauseScreen;

    public GameObject winScreen;

    public GameObject settingsPanel;
    public GameObject nuke;

    public bool won = false;

    public Part head;
    public Part leftArm;
    public Part rightArm;
    public Part leftLeg;
    public Part rightLeg;

    public float baseMaxbattery;
    public float baseMoveSpeed = 3;
    public float baseJumpHeight = 3;
    public float baseDefense = 1;

    public float leftbaseAttackSpeed = 0.5f;
    public float leftbaseDamage = 10;
    public float rightbaseAttackSpeed = 0.5f;
    public float rightbaseDamage = 10;
    //public int baseProjectiles = 1;


    public TextMeshProUGUI batteryText;
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI depthTextUI;

    public float startYPosition = 0f;
    public int startDepthMeters = 0;


    //public int[] quadrants = {0, 1, 2, 3};


    // Start is called before the first frame update
    void Awake()
    {   
        if(Instance != null )
        {
            Destroy(gameObject);
            return;
        }
        //update start depth meters
        if (player != null)
        {
            startYPosition = player.transform.position.y;
            startDepthMeters = (int)startYPosition;
        }
        //startTimeLeft = MenuManager.Instance.startTimeLeft;
        
        
        foreach(Part p in allParts)
        {
            p.numCollected = 0;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // Update is called once per frame
    void Update()
    {

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

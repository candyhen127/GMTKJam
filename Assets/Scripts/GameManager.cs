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




    
}

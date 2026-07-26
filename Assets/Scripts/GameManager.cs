using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Player player;

    public int globalScrap;

    public int totalScrap;
    public int totalParts;
    public int totalRuns;
    public int runsToCorrupt = 5;
    public List<Part> allParts;

    public List<Part> allArms;
    public List<Part> allHeads;
    public List<Part> allLegs;

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

    public int headLevel = 0;
    public int leftArmLevel = 0;
    public int rightArmLevel = 0;
    public int leftLegLevel = 0;
    public int rightLegLevel = 0;

    public float baseHeadbattery;
    public float baseLeftArmbattery;
    public float baseRightArmbattery;
    public float baseLeftLegbattery;
    public float baseRightLegbattery;
    
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

    public bool isPaused = false;

    public bool leverPulled;

    //public int[] quadrants = {0, 1, 2, 3};

    // Start is called before the first frame update
    void Awake()
    {   
        if (Instance != null)
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
        
        foreach (Part p in allParts)
        {
            if (p != null) p.numCollected = 0;
        }

        
        allParts[0].numCollected = 3;
        allParts[1].numCollected = 6;
        allParts[2].numCollected = 6;

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event when the script becomes active
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks or null reference errors
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log($"Scene loaded: {scene.name}");
        
        // Put your scene setup logic here (e.g., finding the local player)
        //InitializeNewScene();
        if (SceneManager.GetActiveScene().name == "Title Screen")
        {
            Destroy(gameObject);
            return;
        }
        Time.timeScale = 1f;
        isPaused = false;

        if (player != null)
        {
            startYPosition = player.transform.position.y;
            startDepthMeters = (int)startYPosition;
        }
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
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
    }



    public void loseGame()
    {
        StartCoroutine(GameOverRoutine());
    }

    public IEnumerator GameOverRoutine()
    {
        if (gameOverScreen != null) gameOverScreen.SetActive(true);

        for (float i = 1f; i >= 0; i -= Time.unscaledDeltaTime)
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

    public IEnumerator RestartRoutine(string scene)
    {
        Time.timeScale = 1f;
        yield return new WaitForSecondsRealtime(0.1f);
        SceneManager.LoadScene(scene);
    }

    public void MainMenu()
    {
        StartCoroutine(RestartRoutine("Title"));
    }

    public void WinGame()
    {
        won = true;
        if (nuke != null && player != null)
        {
            Instantiate(nuke, player.transform.position, Quaternion.identity);
        }
        StartCoroutine(WinRoutine());
    }

    public IEnumerator WinRoutine()
    {
        if (winScreen != null) winScreen.SetActive(true);

        for (float i = 1f; i >= 0; i -= Time.unscaledDeltaTime)
        {
            Time.timeScale = i;
            yield return null;
        }
        Time.timeScale = 0;
    }
}
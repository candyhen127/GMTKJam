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
        
        
    }


  







    
}

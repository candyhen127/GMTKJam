using System;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public Part head;
    public Part leftArm;
    public Part rightArm;
    public Part leftLeg;
    public Part rightLeg;

    public int headLevel = 1;
    public int leftArmLevel = 1;
    public int rightArmLevel = 1;
    public int leftLegLevel = 1;
    public int rightLegLevel = 1;

public TextMeshProUGUI scrapText;
    public TextMeshProUGUI headLevelText;
    public TextMeshProUGUI leftArmLevelText;
    public TextMeshProUGUI rightArmLevelText;
    public TextMeshProUGUI leftLegLevelText;
    public TextMeshProUGUI rightLegLevelText;

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


    public float damageScaling;
    public float attackSpeedScaling;
    public float defenseScaling;
    public float batteryScaling;
    public float moveSpeedScaling;
    public float jumpHeightScaling;
    public int scrapScaling;

    public int baseScrap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set batterys
        baseHeadbattery = GameManager.Instance.baseHeadbattery;
        baseLeftArmbattery = GameManager.Instance.baseLeftArmbattery;
        baseRightArmbattery = GameManager.Instance.baseRightArmbattery;
        baseRightLegbattery = GameManager.Instance.baseRightLegbattery;
        baseLeftLegbattery = GameManager.Instance.baseLeftLegbattery;

        //Set base arms
        leftbaseAttackSpeed = GameManager.Instance.leftbaseAttackSpeed;
        rightbaseAttackSpeed = GameManager.Instance.rightbaseAttackSpeed;
        leftbaseDamage = GameManager.Instance.leftbaseDamage;
        rightbaseDamage = GameManager.Instance.rightbaseDamage;

        //Set base legs
        baseJumpHeight = GameManager.Instance.baseJumpHeight;
        baseMoveSpeed = GameManager.Instance.baseMoveSpeed;

        //Set base heads
        baseDefense = GameManager.Instance.baseDefense;
        spendScrap(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spendScrap(int scrap)
    {
        GameManager.Instance.globalScrap -= scrap;
        scrapText.text = scrap.ToString();
    }

    public void Upgrade(String part)
    {
        if (part == "head")
        {
            headLevel++;
            headLevelText.text = "Level " + headLevel;
            baseDefense += defenseScaling;
            baseHeadbattery += batteryScaling;

            spendScrap(baseScrap + headLevel * scrapScaling);
        }
        if (part == "leftarm")
        {
            leftArmLevel++;
            leftArmLevelText.text = "Level " + leftArmLevel;
            leftbaseAttackSpeed += attackSpeedScaling;
            leftbaseDamage += damageScaling;
            baseLeftArmbattery += batteryScaling;
            
            spendScrap(baseScrap + leftArmLevel * scrapScaling);
        }
        if (part == "rightarm")
        {
            rightArmLevel++;
            rightArmLevelText.text = "Level " + rightArmLevel;
            rightbaseAttackSpeed += attackSpeedScaling;
            rightbaseDamage += damageScaling;
            baseRightArmbattery += batteryScaling;
            
            spendScrap(baseScrap + rightArmLevel * scrapScaling);
        }
        if (part == "leftleg")
        {
            leftLegLevel++;
            leftLegLevelText.text = "Level " + leftLegLevel;
            baseMoveSpeed += moveSpeedScaling;
            baseJumpHeight += jumpHeightScaling;
            baseLeftLegbattery += batteryScaling;
            
            spendScrap(baseScrap + leftLegLevel * scrapScaling);
        }
        if (part == "rightleg")
        {
            rightLegLevel++;
            rightLegLevelText.text = "Level " + rightLegLevel;
            baseMoveSpeed += moveSpeedScaling;
            baseJumpHeight += jumpHeightScaling;
            baseRightLegbattery += batteryScaling;
            
            spendScrap(baseScrap + rightLegLevel * scrapScaling);
        }
    }



    public void LockInBuild()
    {
        head.numCollected--;
        leftArm.numCollected--;
        rightArm.numCollected--;
        leftLeg.numCollected--;
        rightLeg.numCollected--;

        //Set parts
        GameManager.Instance.head = head;
        GameManager.Instance.leftArm=leftArm;
        GameManager.Instance.rightArm=rightArm;
        GameManager.Instance.leftLeg=leftLeg;
        GameManager.Instance.rightLeg=rightLeg;

        //Set batterys
        GameManager.Instance.baseHeadbattery = baseHeadbattery;
        GameManager.Instance.baseLeftArmbattery = baseLeftArmbattery;
        GameManager.Instance.baseRightArmbattery = baseRightArmbattery;
        GameManager.Instance.baseRightLegbattery = baseRightLegbattery;
        GameManager.Instance.baseLeftLegbattery = baseLeftLegbattery;

        //Set base arms
        GameManager.Instance.leftbaseAttackSpeed = leftbaseAttackSpeed;
        GameManager.Instance.rightbaseAttackSpeed = rightbaseAttackSpeed;
        GameManager.Instance.leftbaseDamage = leftbaseDamage;
        GameManager.Instance.rightbaseDamage = rightbaseDamage;

        //Set base legs
        GameManager.Instance.baseJumpHeight = baseJumpHeight;
        GameManager.Instance.baseMoveSpeed = baseMoveSpeed;

        //Set base heads
        GameManager.Instance.baseDefense = baseDefense;
    }
}

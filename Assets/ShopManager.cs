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

    public int headCost = 3;
    public int leftArmCost = 3;
    public int rightArmCost = 3;
    public int leftLegCost = 3;
    public int rightLegCost = 3;

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

        headLevel = GameManager.Instance.headLevel;
        leftArmLevel = GameManager.Instance.leftArmLevel;
        rightArmLevel = GameManager.Instance.rightArmLevel;
        leftLegLevel = GameManager.Instance.leftLegLevel;
        rightLegLevel = GameManager.Instance.rightLegLevel;

        headCost = baseScrap + headLevel * scrapScaling;
        leftArmCost = baseScrap + leftArmLevel * scrapScaling;
        rightArmCost = baseScrap + rightArmLevel * scrapScaling;
        leftLegCost = baseScrap + leftLegLevel * scrapScaling;
        rightLegCost = baseScrap + rightLegLevel * scrapScaling;

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
        Upgrade("");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPart (String slot, Part part)
    {
        if (slot == "head")
        {
            if (head != null)
            {
                head.numCollected++;
            }
            
            
            head = part;
            if (head.numCollected == 0)
            {
                return;
            }
            head.numCollected--;
        }
        if (slot == "leftarm")
        {
            if (leftArm != null)
            {
                leftArm.numCollected++;
            }
            
            leftArm = part;
            if (leftArm.numCollected == 0)
            {
                return;
            }
            leftArm.numCollected--;
        }
        if (slot == "rightarm")
        {
            if (rightArm != null)
            {
                rightArm.numCollected++;
            }
            
            rightArm = part;
            if (rightArm.numCollected == 0)
            {
                return;
            }
            rightArm.numCollected--;
        }
        if (slot == "leftleg")
        {
            if (leftLeg != null)
            {
                leftLeg.numCollected++;
            }
            
            leftLeg = part;
            if (leftLeg.numCollected == 0)
            {
                return;
            }
            leftLeg.numCollected--;
        }
        if (slot == "rightleg")
        {
            if (rightLeg != null)
            {
                rightLeg.numCollected++;
            }
            
            rightLeg = part;
            if (rightLeg.numCollected == 0)
            {
                return;
            }
            rightLeg.numCollected--;
        }
    }

    public void spendScrap(int scrap)
    {
        GameManager.Instance.globalScrap -= scrap;
        scrapText.text = GameManager.Instance.globalScrap.ToString() + " scraps";
    }

    public void Upgrade(String part)
    {
        if (part == "head")
        {
            if (GameManager.Instance.globalScrap - headCost < 0)
            {
                return;
            }
            spendScrap(headCost);
            headCost += scrapScaling;
            headLevel++;
            baseDefense += defenseScaling;
            baseHeadbattery += batteryScaling;

        }
        if (part == "leftarm")
        {
            if (GameManager.Instance.globalScrap - leftArmCost < 0)
            {
                return;
            }
            spendScrap(leftArmCost);
            leftArmCost += scrapScaling;
            leftArmLevel++;
            leftbaseAttackSpeed += attackSpeedScaling;
            leftbaseDamage += damageScaling;
            baseLeftArmbattery += batteryScaling;
            
        }
        if (part == "rightarm")
        {
            if (GameManager.Instance.globalScrap - rightArmCost < 0)
            {
                return;
            }
            spendScrap(rightArmCost);
            rightArmCost += scrapScaling;
            rightArmLevel++;
            rightbaseAttackSpeed += attackSpeedScaling;
            rightbaseDamage += damageScaling;
            baseRightArmbattery += batteryScaling;
            
        }
        if (part == "leftleg")
        {
            if (GameManager.Instance.globalScrap - leftLegCost < 0)
            {
                return;
            }
            spendScrap(leftLegCost);
            leftLegCost += scrapScaling;
            leftLegLevel++;
            baseMoveSpeed += moveSpeedScaling;
            baseJumpHeight += jumpHeightScaling;
            baseLeftLegbattery += batteryScaling;
            
        }
        if (part == "rightleg")
        {
            if (GameManager.Instance.globalScrap - rightLegCost < 0)
            {
                return;
            }
            spendScrap(rightLegCost);
            rightLegCost += scrapScaling;
            rightLegLevel++;
            baseMoveSpeed += moveSpeedScaling;
            baseJumpHeight += jumpHeightScaling;
            baseRightLegbattery += batteryScaling;
            
        }
        
            headLevelText.text = "Level " + headLevel + " (Cost: " + headCost + ")";
            leftArmLevelText.text = "Level " + leftArmLevel + " (Cost: " + leftArmCost + ")";
            rightArmLevelText.text = "Level " + rightArmLevel + " (Cost: " + rightArmCost + ")";
            leftLegLevelText.text = "Level " + leftLegLevel + " (Cost: " + leftLegCost + ")";
            rightLegLevelText.text = "Level " + rightLegLevel + " (Cost: " + rightLegCost + ")";
    }



    public void LockInBuild()
    {
        //head.numCollected--;
        //leftArm.numCollected--;
        //rightArm.numCollected--;
        //leftLeg.numCollected--;
        //rightLeg.numCollected--;

        //Set parts
        GameManager.Instance.head = head;
        GameManager.Instance.leftArm=leftArm;
        GameManager.Instance.rightArm=rightArm;
        GameManager.Instance.leftLeg=leftLeg;
        GameManager.Instance.rightLeg=rightLeg;

        GameManager.Instance.headLevel = headLevel;
        GameManager.Instance.leftArmLevel = leftArmLevel;
        GameManager.Instance.rightArmLevel = rightArmLevel;
        GameManager.Instance.leftLegLevel = leftLegLevel;
         GameManager.Instance.rightLegLevel = rightLegLevel;

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

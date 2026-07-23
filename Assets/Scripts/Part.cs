using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Part", order = 1)]
public class Part : ScriptableObject
{
    public String name;
    public String bodypart; //head, body, leftArm, rightArm, leftLeg, rightLeg

    public float battery;

    //leg
    public float moveSpeed;
    
    //head
    public float playerBattery;
    
    //arm
    public float damage;
    public GameObject bulletPrefab;

    //body
    public float defense;     
}

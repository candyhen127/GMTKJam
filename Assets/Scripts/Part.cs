using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Part", order = 1)]
public class Part : ScriptableObject
{
    public String name;
    public String bodypart; //head, body, leftArm, rightArm, leftLeg, rightLeg

    public Sprite icon;

    public float battery;

    [Header("Leg")]
    public float moveSpeed;
    
    [Header("Head")]
    public float playerBattery;
    
    [Header("Arm")]
    public float damage;
    public GameObject bulletPrefab;
    public int projectiles;
    public float bulletSpeed;
    public float attackSpeed;

    [Header("Body")]
    public float defense;     
}

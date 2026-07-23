using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public int level;
    public float battery;
    public float maxbattery = 100;
    public float baseMaxbattery = 100;
    public float moveSpeed;
    public float baseMoveSpeed = 3;
    public float jumpHeight;
    public float baseJumpHeight = 3;
    public float defense = 1;
    public float baseDefense = 1;
    public float knockback = 1;
    public bool knockedBack;
    public float invinceTime = 1.4f;
    public bool invince;
    
   
    public Gun arm1;

    public SimplePlayerMovement movement;

    [Header("Parts")]
    public int scrap;
    public List<Part> inventory;
    public Part head;
    //public Part body;
    public Part leftArm;
    public Part rightArm;
    public Part leftLeg;
    public Part rightLeg;

    public float headBattery;
    //public float bodyBattery;
    public float leftArmBattery;
    public float rightArmBattery;
    public float leftLegBattery;
    public float rightLegBattery;

    public bool headEquipped = true;
    public bool leftArmEquipped = true;
    public bool rightArmEquipped = true;
    public bool leftLegEquipped = true;
    public bool rightLegEquipped = true;


    public Rigidbody2D rb;
    public Collider2D collider2d;
    public Animator animator;

    [Header("UI Elements")]
    public Camera cam;
    public GameObject canvas;
    public TextMeshProUGUI damagenum;
    //public UnityEngine.UI.Image batterybar;
    
    public TextMeshProUGUI scrapText;
    
    
    public GameObject LevelUpPopup;

    [Header("SFX")]
    public AudioSource basic;


    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        maxbattery = baseMaxbattery + head.playerBattery;
        battery = maxbattery;
        headBattery = head.battery;
        leftArmBattery = leftArm.battery;
        rightArmBattery = rightArm.battery;
        leftLegBattery = leftLeg.battery;
        rightArmBattery = rightArm.battery;
        moveSpeed = baseMoveSpeed + leftLeg.moveSpeed + rightLeg.moveSpeed;
        jumpHeight = baseJumpHeight + leftLeg.jumpHeight + rightLeg.jumpHeight;
        movement.moveSpeed = moveSpeed;
        movement.jumpHeight = jumpHeight;
        //defense = baseDefense + body.defense;
    }
    

    // Update is called once per frame
    void Update()
    {
        //don't allow input when paused
        if(GameManager.Instance.paused == true || GameManager.Instance.truepaused){return;}

        //player movement input
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");

        //use wheel spell & spin for next spell
        if (Input.GetKey("space"))
        {
            
        }
    }

    void FixedUpdate()
    {
        
        //camera follows player
        cam.transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, cam.transform.position.z);

        //don't allow input when paused
        if(GameManager.Instance.paused == true || GameManager.Instance.truepaused)
        {
            //rb.velocity = Vector2.zero;
            return;
        }
        if (knockedBack)
        {
            return;
        }

        battery -= Time.deltaTime;
        headBattery -= Time.deltaTime;
        leftArmBattery -= Time.deltaTime;
        rightArmBattery -= Time.deltaTime;
        leftLegBattery -= Time.deltaTime;
        rightLegBattery -= Time.deltaTime;
        if (headBattery <= 0 && headEquipped)
        {
            Eject("head");
        } else
        {
            
        }
        if (leftArmBattery <= 0 && leftArmEquipped)
        {

            Eject("leftArm");
        }
        if (rightArmBattery <= 0 && rightArmEquipped)
        {
            Eject("rightArm");
        }
        if (leftLegBattery <= 0 && leftLegEquipped)
        {
            Eject("leftLeg");
        }
        if (rightLegBattery <= 0 && rightLegEquipped)
        {
            Eject("rightLeg");
        }
    }

    

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(GameManager.Instance.truepaused == true){return;}
        if(GameManager.Instance.paused == true){return;}
        if (knockedBack){return;}

        if (collision.gameObject.tag == "Enemy" && !invince){
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            TakeDamage(e.damage);

            Vector2 direction = rb.position - e.rb.position;

            StartCoroutine(KnockbackRoutine(direction, 0.2f));
            StartCoroutine(InvinceRoutine(invinceTime));
        }
    }

    private IEnumerator KnockbackRoutine(Vector2 direction, float duration)
    {
        knockedBack = true;
        // Clear current velocity to ensure a clean, consistent push
        rb.linearVelocity = Vector2.zero;

        Vector2 knockbackForce = direction.normalized * knockback;
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);

        // Wait out the stun duration
        yield return new WaitForSeconds(duration);

        knockedBack = false;
    }

    private IEnumerator InvinceRoutine(float duration)
    {
        
        invince = true;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = new Color(1, 1, 1, 0.5f);
        
        
        // Wait out the stun duration
        yield return new WaitForSeconds(duration);
        renderer.color = new Color(1, 1, 1, 1f);
        

        invince = false;
    }

    public void TakeDamage(float damage)
    {
        //spawnDamageNum(damage, false);
        //damaged.Play();
        damage = damage / (1 + 0.1f * defense);

        headBattery -= damage;
        //bodyBattery -= damage;
        leftArmBattery -= damage;
        rightArmBattery -= damage;
        leftLegBattery -= damage;
        rightLegBattery -= damage;
        
    }

    public void Eject(String p)
    {
        if (p == "head")
        {
            battery -= head.battery;
            headEquipped = false;
        }
        if (p == "leftArm")
        {
            leftArmEquipped = false;
        }
        if (p == "rightArm")
        {
            rightArmEquipped = false;
        }
        if (p == "leftLeg")
        {
            moveSpeed -= leftLeg.moveSpeed;
            jumpHeight -= leftLeg.jumpHeight;
            movement.moveSpeed = moveSpeed;
            movement.jumpHeight = jumpHeight;
            leftLegEquipped = false;
        }
        if (p == "rightLeg")
        {
            moveSpeed -= rightLeg.moveSpeed;
            jumpHeight -= rightLeg.jumpHeight;
            movement.moveSpeed = moveSpeed;
            movement.jumpHeight = jumpHeight;
            rightLegEquipped = false;
        }
    }

    public void PlayerHeal(float damage)
    {
        //spawnDamageNum(damage, true);
        
        battery += damage;
        if (battery > maxbattery)
        {
            battery = maxbattery;
        }
    }

    /*
    public void spawnDamageNum(float damage, bool heal)
    {
        TextMeshProUGUI x = Instantiate(damagenum, canvas.transform, false);
        x.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        x.gameObject.GetComponent<damageNum>().dnum = damage;
        x.gameObject.GetComponent<damageNum>().heal = heal;
    }
    */


    public void UpdateScrapCount()
    {
        scrapText.text = scrap.ToString();
    }

    
    public void GetScrap(int points)
    {
        if(GameManager.Instance.paused == true ||  GameManager.Instance.truepaused == true){return;}
        //getxp.Play();
        scrap += points;
        UpdateScrapCount();
    }
    
    

    

}

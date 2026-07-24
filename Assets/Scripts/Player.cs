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
    
   
    public Gun leftGun;
    public Gun rightGun;

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

    public GameObject headSprite;
    public GameObject leftArmSprite;
    public GameObject rightArmSprite;
    public GameObject leftLegSprite;
    public GameObject rightLegSprite;

    public Transform groundCheck2;  //groundcheck for when legs are gone

    public GameObject dumpedPart;
    public float dumpForce = 8;
    public Color redtint = new Color32(255, 163, 163, 255);
    public float redtime = 3;


    public Rigidbody2D rb;
    public Collider2D collider2d;
    public Animator animator;

    [Header("UI Elements")]
    public Camera cam;
    public GameObject canvas;
    public TextMeshProUGUI damagenum;
    //public UnityEngine.UI.Image batterybar;
    
    public TextMeshProUGUI scrapText;


    public TextMeshProUGUI headTimer;
    public TextMeshProUGUI leftArmTimer;
    public TextMeshProUGUI rightArmTimer;
    public TextMeshProUGUI leftLegTimer;
    public TextMeshProUGUI rightLegTimer;
    
    
    public GameObject LevelUpPopup;

    [Header("SFX")]
    public AudioSource basic;


    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        maxbattery = baseMaxbattery + head.playerBattery;
        headSprite.GetComponent<SpriteRenderer>().sprite = head.icon;

        battery = maxbattery;
        headBattery = head.battery;
        leftArmBattery = leftArm.battery;
        rightArmBattery = rightArm.battery;
        leftLegBattery = leftLeg.battery;
        rightLegBattery = rightLeg.battery;

        moveSpeed = baseMoveSpeed + leftLeg.moveSpeed + rightLeg.moveSpeed;
        jumpHeight = baseJumpHeight + leftLeg.jumpHeight + rightLeg.jumpHeight;
        leftLegSprite.GetComponent<SpriteRenderer>().sprite = leftLeg.icon;
        rightLegSprite.GetComponent<SpriteRenderer>().sprite = rightLeg.icon;

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


        if (headBattery <= redtime)
        {
            headSprite.GetComponent<SpriteRenderer>().color = redtint;
            headTimer.color = Color.red;
        } 
        if (leftArmBattery <= redtime)
        {
            leftArmSprite.GetComponent<Gun>().sprite.color = redtint;
            leftArmTimer.color = Color.red;
        }
        if (rightArmBattery <= redtime)
        {
            rightArmSprite.GetComponent<Gun>().sprite.color = redtint;
            rightArmTimer.color = Color.red;
        }
        if (leftLegBattery <= redtime)
        {
            leftLegSprite.GetComponent<SpriteRenderer>().color = redtint;
            leftLegTimer.color = Color.red;
        }
        if (rightLegBattery <= redtime)
        {
            rightLegSprite.GetComponent<SpriteRenderer>().color = redtint;
            rightLegTimer.color = Color.red;
        }

        

        if (headBattery <= 0)
        {
            headBattery = 0;
            if (headEquipped)
            {
                
            Eject("head");
            }
        } 
        if (leftArmBattery <= 0)
        {

            leftArmBattery = 0;
            if (leftArmEquipped)
            {
                    
            Eject("leftArm");
            }
        }
        if (rightArmBattery <= 0)
        {
            rightArmBattery = 0;
            if (rightArmEquipped)
            {
                
            Eject("rightArm");
            }
        }
        if (leftLegBattery <= 0)
        {
            
            leftLegBattery = 0;
            if (leftLegEquipped)
            {
                Eject("leftLeg");
            }
        }
        if (rightLegBattery <= 0)
        {
            rightLegBattery = 0;
            if (rightLegEquipped)
            {
                
            Eject("rightLeg");
            }
        }

        headTimer.text = headBattery.ToString("F2");
        leftArmTimer.text = leftArmBattery.ToString("F2");
        rightArmTimer.text = rightArmBattery.ToString("F2");
        leftLegTimer.text = leftLegBattery.ToString("F2");
        rightLegTimer.text = rightLegBattery.ToString("F2");
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
        Sprite icon = head.icon;
        float d = 0;
        if (p == "head")
        {
            battery -= head.battery;
            headEquipped = false;
            icon = head.icon;
            d = head.battery;
            headSprite.SetActive(false);
        }
        if (p == "leftArm")
        {
            leftArmEquipped = false;
            icon = leftArm.icon;
            d = leftArm.battery;
            leftArmSprite.GetComponent<Gun>().sprite.enabled = false;
        }
        if (p == "rightArm")
        {
            rightArmEquipped = false;
            icon = rightArm.icon;
            d = rightArm.battery;
            rightArmSprite.GetComponent<Gun>().sprite.enabled = false;
        }
        if (p == "leftLeg")
        {
            moveSpeed -= leftLeg.moveSpeed;
            jumpHeight -= leftLeg.jumpHeight;
            movement.moveSpeed = moveSpeed;
            movement.jumpHeight = jumpHeight;
            leftLegEquipped = false;
            icon = leftLeg.icon;
            d = leftLeg.battery;
            leftLegSprite.SetActive(false);
        }
        if (p == "rightLeg")
        {
            moveSpeed -= rightLeg.moveSpeed;
            jumpHeight -= rightLeg.jumpHeight;
            movement.moveSpeed = moveSpeed;
            movement.jumpHeight = jumpHeight;
            rightLegEquipped = false;
            icon = rightLeg.icon;
            d = rightLeg.battery;
            rightLegSprite.SetActive(false);
        }
        if (!leftLegEquipped && !rightLegEquipped)
        {
            movement.groundCheck = groundCheck2;
        }

        GameObject bullet = Instantiate(dumpedPart, transform.position, leftGun.shootPoint.rotation);
        bullet.GetComponent<SpriteRenderer>().sprite = icon;
        bullet.GetComponent<Bullet>().damage = d;
        bullet.GetComponent<Rigidbody2D>().linearVelocity = leftGun.shootPoint.up * dumpForce;
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

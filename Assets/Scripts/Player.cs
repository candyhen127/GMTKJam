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
    public float health;
    public float maxHealth = 100;
    public float baseMaxHealth = 100;
    public float moveSpeed;
    public float baseMoveSpeed = 3;
    public float knockback = 1;
    public bool knockedBack;
    public float invinceTime = 0.4f;

    public int lightningsInARow = 1;
    public int nonAdjFreezes = 0;
    
   
    public Gun arm1;

    public SimplePlayerMovement movement;

    public Rigidbody2D rb;
    public Collider2D collider2d;
    public Animator animator;

    [Header("UI Elements")]
    public Camera cam;
    public GameObject canvas;
    public TextMeshProUGUI damagenum;
    public UnityEngine.UI.Image healthbar;
    
    
    public GameObject LevelUpPopup;

    [Header("SFX")]
    public AudioSource basic;


    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        maxHealth = baseMaxHealth;
        health = maxHealth;
        moveSpeed = baseMoveSpeed;
        UpdateHealthBar();
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

        
        
    }

    

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(GameManager.Instance.truepaused == true){return;}
        if(GameManager.Instance.paused == true){return;}
        if (knockedBack){return;}

        if (collision.gameObject.tag == "Enemy"){
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
        
        collider2d.isTrigger = true;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = new Color(1, 1, 1, 0.5f);
        
        
        // Wait out the stun duration
        yield return new WaitForSeconds(duration);
        renderer.color = new Color(1, 1, 1, 1f);
        

        collider2d.isTrigger = false;
    }

    public void TakeDamage(float damage)
    {
        //spawnDamageNum(damage, false);
        //damaged.Play();
        health -= damage;
        UpdateHealthBar();
        if (health <= 0)
        {
            GameManager.Instance.loseGame();
        }
    }

    public void PlayerHeal(float damage)
    {
        //spawnDamageNum(damage, true);
        
        health += damage;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        UpdateHealthBar();
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

    public void UpdateHealthBar()
    {
        //healthbar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1f);
    }

    /*
    public void GetXP(float points)
    {
        if(GameManager.Instance.paused == true ||  GameManager.Instance.truepaused == true){return;}
        getxp.Play();
        xp += points;
        UpdateLevelBar();
    }
    */
    

    

}

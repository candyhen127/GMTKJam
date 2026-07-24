using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;
//using Pathfinding;

public class Enemy : MonoBehaviour
{
    public Player player;
    public Rigidbody2D rb;
    public float health = 80;
    public float maxHealth = 80;
    public float damage = 10;
    public float moveSpeed = 10;
    public Animator animator;
    new public SpriteRenderer renderer;

    public bool willexplode = false;
    public bool willexplode2;
    public bool dead;
    public GameObject explosionprefab;

    public GameObject scrapDrop;
    public int drops;
    public GameObject partDrop;


    public float burndamage = 0;
    public Coroutine burnroutine;
    public bool frozen;
    public Coroutine deathroutine;

    public TextMeshProUGUI damagenum;
    public GameObject canvas;

    public AudioSource hit;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Robot").GetComponent<Player>();
        canvas = GameObject.Find("Canvas");

        //GetComponent<AIDestinationSetter>().target = player.GetComponent<Transform>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) {
            //gameObject.GetComponent<AIPath>().maxSpeed = 0;
            return;}
        if(health < 1)
        {
            Die();
        }
        //don't allow input when paused
        if(MenuScript.Instance.paused == true || frozen){
            //gameObject.GetComponent<AIPath>().maxSpeed = 0;
            //GetComponent<Animator>().speed = 0;
            rb.mass = 2;
            return;
        }
        else
        {
            //gameObject.GetComponent<AIPath>().maxSpeed = moveSpeed;
            //GetComponent<Animator>().speed = 1;
            rb.mass = 1;
        }
        if(willexplode2)
        {
            if(Vector3.Distance(this.transform.position, player.transform.position) < 25f)
            {
                willexplode = false;
                //animator.SetTrigger("Explode");
                //gameObject.GetComponent<AIPath>().maxSpeed = 0;

                
               

                deathroutine = StartCoroutine(delayedDeath(0.4f));
                
            }
        }
        
        
    }

    void FixedUpdate()
    {
        if (dead) {return;}
        Vector2 direction = rb.position - player.rb.position;
        float angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
        if (angle < 90 && angle > -90)
        {
            renderer.flipX = false;
        } else
        {
            renderer.flipX = true;
        }
    }

    public virtual void Die()
    {
        if (dead)
        {
            return;
        }
        if(deathroutine != null)
        {
            StopCoroutine(deathroutine);
        }
        if(willexplode)
        {
            explosion();
            
        }
        dead = true;
        
        //gameObject.GetComponent<AIPath>().maxSpeed = 0;
        for (int i = 0; i < drops; i++)
        {
            Instantiate(scrapDrop, transform.position, Quaternion.identity);
        }

            Instantiate(partDrop, transform.position, Quaternion.identity);

        if (!MenuScript.Instance.truepaused)
        {
                
        //hit.Play();
        }
        //GetComponent<Animator>().SetTrigger("Die");
        GetComponent<Collider2D>().enabled = false;
        rb.gravityScale = 0;
        //Destroy(transform.GetChild(0).gameObject);
        Destroy(this.gameObject, 0.4f);
    }
    void explosion()
    {
        GameObject e = Instantiate(explosionprefab, this.transform.position, Quaternion.identity);
        if(e.GetComponent<AOEdamage>() != null)
            {
                e.GetComponent<AOEdamage>().setvars(damage, 0.5f, 24);
                e.GetComponent<AOEdamage>().source = "Explosion";
            }
    }

    public IEnumerator delayedDeath(float time)
    {
        yield return new WaitForSeconds(time);
            explosion();
            /*
            if(e.GetComponent<MultiBullet>() != null)
            {
                e.GetComponent<MultiBullet>().damage = gun.damage;
                e.GetComponent<MultiBullet>().bulletSpeed = gun.bulletSpeed;
            }*/
        Die();
    }

    public virtual void TakeDamage(float damage)
    {
        bool crit = false;
        
        //Debug.Log("hit");
        if (!MenuScript.Instance.truepaused)
        {
        //TextMeshProUGUI x = Instantiate(damagenum, canvas.transform, false);
        //x.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        //x.gameObject.GetComponent<damageNum>().dnum = damage;
        //x.gameObject.GetComponent<damageNum>().crit = crit;
        }
        health -= damage;  
        StartCoroutine(FlashRoutine(0.25f));
        //aud.Play();
    }

    public virtual void EnemyHeal(float h)
    {
        
        health += h;
        if(health > maxHealth)
        {
            health = maxHealth;
        }

        //TextMeshProUGUI x = Instantiate(damagenum, canvas.transform, false);
        //x.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        //x.gameObject.GetComponent<damageNum>().dnum = h;
        //x.gameObject.GetComponent<damageNum>().heal = true;
    }

    private IEnumerator FlashRoutine(float duration)
    {
        
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0.5f);
        
        
        // Wait out the stun duration
        yield return new WaitForSeconds(duration);
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1f);
        

    }

    public void Freeze()
    {
        if (frozen) {return;}
        float duration = 2f;
        StartCoroutine(FreezeRoutine(duration));
    }

    private IEnumerator FreezeRoutine(float duration)
    {
        
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        
        renderer.color = new Color(0.625f, 1, 1, renderer.color.a);
        frozen = true;
        
        // Wait out the stun duration
        yield return new WaitForSeconds(duration);


        renderer.color = new Color(1, 1, 1, renderer.color.a);
        frozen = false;
        

    }

    public void burnMethod(float burndamage1) {
        burndamage += burndamage1;
        if(burnroutine == null)
        {
            burnroutine = StartCoroutine(burn(burndamage, 5));
        }
    }
    

    public IEnumerator burn(float damage, int stacks)
    {
        burndamage = damage;
        int i = 0;
        
        while(i <= stacks-1)
        {
            renderer.color = new Color32(255, 76, 76, 255);
            yield return new WaitForSeconds(1f);
            
            TakeDamage(burndamage);

            renderer.color = new Color32(255, 0, 0, 255);
            yield return new WaitForSeconds(0.1f);
            renderer.color = new Color32(255, 76, 76, 255);
            i++;
        }
        renderer.color = new Color32(255, 255, 255, 255);
        burndamage = 0;
        StopCoroutine(burnroutine);
        burnroutine = null;
        yield break;
    }
}

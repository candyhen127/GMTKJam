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

    public GameObject bulletPrefab;
    public int projectiles = 1;
    public int destroy = 3;
    public Transform leftshootPoint;
    public Transform rightshootPoint;
    public Coroutine shootroutine;

    
    public float defatkspd = 1f;
    public int shots = 3;
    public int shotcount = 0;
    public int shooting = 0;

    public bool willexplode = false;
    public bool willexplode2;
    public bool dead;
    public GameObject explosionprefab;

    public GameObject scrapDrop;
    public int drops;
    public GameObject partDrop;
    public List<Part> parts;


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

        getshoot(damage);
        //don't allow input when paused
        if(MenuScript.Instance.paused == true){
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

    public void shootProjectile()
    {
        Transform shootPoint;
        if (transform.position.x > player.transform.position.x)
        {
            shootPoint = rightshootPoint;
        } else
        {
            shootPoint = leftshootPoint;
        }
        for(float x = 0-(((float)projectiles/2)-0.5f); x <= (((float)projectiles)/2-0.5f)+0.1f; x+= 1)
            {
                Quaternion q = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z+(x*(15)));
                GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation * q);
                
                //bullet.GetComponent<Bullet>().bulletSpeed += bulletSpeed;

                ///float temp = d / (1 + 0.3f * (projectiles - 1));

                
                //bullet.GetComponent<Bullet>().damage += temp;
                bullet.GetComponent<Bullet>().StartCoroutine(bullet.GetComponent<Bullet>().bulletDestroy(destroy));
                
            }
    }

    void getshoot(float d)
    {
        
            if(shooting == 1) {return;}
            
            //gunanimator.Play("GunFire");
            //aud.Play();
            //Debug.Log("shot");
            shootProjectile();
                shotcount ++;
                if (shotcount == shots)
                {
                    shotcount = 0;
            
                    shootroutine = this.StartCoroutine(FireRateRoutine(defatkspd));
                } else
                {
            
                    shootroutine = this.StartCoroutine(FireRateRoutine(0.1f));
                }
        
    }


    IEnumerator FireRateRoutine(float sec)
    {
        if(shooting == 0)
        {
            shooting = 1;
            yield return new WaitForSeconds(sec);
            shooting = 0;
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

            GameObject partdrop = Instantiate(partDrop, transform.position, Quaternion.identity);
            partdrop.GetComponent<ScrapDrop>().part = parts[UnityEngine.Random.Range(0, parts.Count)];

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

    
}

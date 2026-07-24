using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float bulletSpeed;
    Rigidbody2D rb;
    public int pierce;
    public int bounce;
    public GameObject explosionprefab;
    public float pulse;
    public List<GameObject> pierced;
    public bool dontDestroy;

    public int bonusOnBounce;
    public int fireDamage;
    public bool freeze;
    public float spread;
    public float destroy;
    public float spinForce;

    public bool straight = true;

    void Start()
    {
        
        rb = gameObject.GetComponent<Rigidbody2D>();
        if(pulse != 0)
        {
            StartCoroutine(makeTrail());
        }

        if (spread != 0) {
            Transform transform = gameObject.GetComponent<Transform>();
            Quaternion q = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + UnityEngine.Random.Range(-3, 3) * spread);
            transform.rotation = transform.rotation * q;
        }
        if (!straight)
        {
            rb.AddTorque(spinForce); 
        }
    }

    void Update()
    {
        if (straight)
        {
            
            rb.linearVelocity = transform.up * bulletSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>(), false);
        
            if(explosionprefab != null)
            {
                
                if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Destructible" || collision.gameObject.tag == "Bullet")
                {
                    if(pierce == 0)
                    {
                        explode();
                    }
                    else
                    {
                        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
                        pierced.Add(collision.gameObject);
                        pierce--;
                        Enemy e = collision.gameObject.GetComponent<Enemy>();
                        //Boss b = collision.gameObject.GetComponent<Boss>();
                        
                        if(e != null)
                        {
                            e.TakeDamage(damage/3);
                            if (fireDamage != 0) {
                                e.burnMethod(fireDamage);
                            }
                            if (freeze)
                            {
                                e.Freeze();
                            }
                        }/*
                        else if(b != null)
                        {
                            b.TakeDamage(damage/3, "Bullet");
                            if (fireDamage != 0) {
                                b.burnMethod(fireDamage);
                            }
                        }*/
                    }
                }
                else if(bounce > 0)
                {
                    bounceMethod(collision);
                }
                else
                {
                    explode();
                }
                
            }
            
            else if(collision.gameObject.tag == "Enemy")
            {
                Enemy e = collision.gameObject.GetComponent<Enemy>();
                //Boss b = collision.gameObject.GetComponent<Boss>();
                
                if(e != null)
                {
                    e.TakeDamage(damage);
                    if (fireDamage != 0) {
                        e.burnMethod(fireDamage);
                    }
                    if (freeze)
                    {
                        e.Freeze();
                    }
                }/*
                else if(b != null)
                {
                    b.TakeDamage(damage, "Bullet");
                    if (fireDamage != 0) {
                        b.burnMethod(fireDamage);
                    }
                }
                */
                
                if(pierce == 0)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
                    
                    pierced.Add(collision.gameObject);
                    pierce--;
                    
                }
                
            }
            /*
            else if(collision.gameObject.tag == "Destructible")
            {
                collision.gameObject.GetComponent<Destructible>().TakeDamage(damage);
                if(pierce == 0)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
                    pierced.Add(collision.gameObject);
                    pierce--;
                }
            }
            */
            
            else if(bounce > 0)
            {
                bounceMethod(collision);
            }
            else if(!dontDestroy)
            {
                Destroy(this.gameObject);
            }
    }

    public IEnumerator bulletDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        if(explosionprefab != null)
            {
                //explode();
            }
        Destroy(this.gameObject);
    }

    
    IEnumerator makeTrail()
    {
        while(true)
        {
            
        GameObject e = Instantiate(explosionprefab, this.transform.position, Quaternion.identity);
            
        e.GetComponent<AOEdamage>().damage = damage;
        yield return new WaitForSeconds(pulse);
        }
        
    }
    

    private void bounceMethod(Collision2D collision) {
        Vector3 v = Vector3.Reflect(transform.up, collision.contacts[0].normal);
        float rot = Mathf.Atan2(-v.x, v.y) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, rot);
        foreach (GameObject g in pierced)
        {
            if(g != null)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), g.GetComponent<Collider2D>(), false);
            }
                        
        }
        bounce --;
        damage += bonusOnBounce;
    }
    
    private void explode() {
        GameObject e = Instantiate(explosionprefab, this.transform.position, Quaternion.identity);
                        if(e.GetComponent<AOEdamage>() != null)
                        {
                            e.GetComponent<AOEdamage>().setvars(damage, 0.5f, 2);
                        }
                        /*
                        else if(e.GetComponent<MultiBullet>() != null)
                        {
                            e.GetComponent<MultiBullet>().setvars(damage, bulletSpeed+10);
                            e.GetComponent<Transform>().rotation = this.transform.rotation; 
                        }
                        */
                        //e.transform.localScale = transform.localScale;
                        Destroy(this.gameObject);
    }
    
}

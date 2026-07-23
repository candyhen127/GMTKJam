using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AOEdamage : MonoBehaviour
{
    public float damage;
    public float destroy;
    public float radius;
    public bool hitplayer = false;
    public bool hitenemy = true;
    public bool heal = false;
    public String source;
    public float pulse = 100;
    public AudioSource aud;

    public void setvars(float da, float de, float r)
    {
        damage = da;
        destroy = de;
        radius = r;
    }

    void Start()
    {
        if(aud != null)
        {
            aud.Play();
        }
        hit();
        if(destroy > 0)
        {
            Destroy(gameObject, destroy);
        }
        
    }

    void Update()
    {
        
    }

    public void hit()
    {
        Collider2D[] objectsinrange = Physics2D.OverlapCircleAll(gameObject.transform.position, radius); //*transform.localScale.x
        foreach(Collider2D c in objectsinrange)
        {
            if(!heal)
            {
                if(hitenemy)
                {
                    Enemy e = c.GetComponent<Enemy>();
                    //Enemybullet bu = c.GetComponent<Enemybullet>();
                    if(e != null)
                    {
                        e.TakeDamage(damage);
                    }
                    /*
                    else if(bu != null)
                    {
                        bu.TakeDamage(damage, "Bullet");
                    }
                    */
                }
                if(hitplayer)
                {
                    Player p = c.GetComponent<Player>();
                    if(p != null)
                    {
                        p.TakeDamage(damage);
                    }
                }
                if(!hitplayer && !hitenemy){return;}
            }
            else
            {
                if(hitenemy)
                {
                    Enemy e = c.GetComponent<Enemy>();
                    if(e != null)
                    {
                        e.EnemyHeal(damage/100*e.maxHealth);
                    }
                }
                if(hitplayer)
                {
                    Player p = c.GetComponent<Player>();
                    if(p != null)
                    {
                        p.PlayerHeal(damage/100*p.maxHealth);
                    }
                }
            }
            
        }
        StartCoroutine(wait());
    }

    public IEnumerator wait()
    {
        yield return new WaitForSeconds(pulse);
        hit();
    }
}

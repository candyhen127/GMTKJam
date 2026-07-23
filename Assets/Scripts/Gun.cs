using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gun : MonoBehaviour
{

    Vector2 mousePos;

    [SerializeField]
    private Player player;

    [SerializeField]
    private Transform rightHand;
    [SerializeField]
    private Transform leftHand;
    [SerializeField]
    private Transform shootPoint;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject bulletPrefab2;
    [SerializeField]
    private Transform center;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Transform transform;
    [SerializeField]
    private SpriteRenderer sprite;
    
    [SerializeField]
    private float pangle;
    [SerializeField] 
    private float angle;


    public float bulletSpeed = 2f;
    public float baseBulletSpeed = 10;
    public float attackSpeed = 0.1f;
    public float defatkspd = 0.1f;
    public float baseAttackSpeed = 0.5f;


    public float damage;
    public float baseDamage = 10;
    public int projectiles;
    public int baseProjectiles = 1;
    public float spread;
    public float destroy;
    public int shooting = 0;
    Coroutine shootroutine;


    
    public float bulletSpeed2 = 2f;
    public float baseBulletSpeed2 = 10;
    public float defatkspd2 = 0.1f;
    public float baseAttackSpeed2 = 0.5f;


    public float damage2;
    public float baseDamage2 = 10;
    public int projectiles2;
    public int baseProjectiles2 = 1;
    public float spread2;
    public float destroy2;
    public int shooting2 = 0;
    Coroutine shootroutine2;


    [SerializeField]
    private SpriteRenderer playerSprite;
    [SerializeField]
    private Animator playerAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damage = baseDamage + player.rightArm.damage;
        projectiles = baseProjectiles + player.rightArm.projectiles;
        bulletSpeed = baseBulletSpeed + player.rightArm.bulletSpeed;
        defatkspd = baseAttackSpeed + player.rightArm.attackSpeed;
        bulletPrefab = player.rightArm.bulletPrefab;

        damage2 = baseDamage2 + player.leftArm.damage;
        projectiles2 = baseProjectiles2 + player.leftArm.projectiles;
        bulletSpeed2 = baseBulletSpeed2 + player.leftArm.bulletSpeed;
        defatkspd2 = baseAttackSpeed2 + player.leftArm.attackSpeed;
        bulletPrefab2 = player.leftArm.bulletPrefab;
        

        attackSpeed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if(pangle > 90 || pangle < -90)
        {
            sprite.flipY = true;
        }
        else if(pangle <=90 || pangle >= -90)
        {
            sprite.flipY = false;
        }
        

        if(Input.GetMouseButton(0))
        {
            //if(ammo - 1 >= 0)
            //{
                getshoot(damage);
            //}
        }
        if(Input.GetMouseButton(1))
        {
            //if(ammo - 1 >= 0)
            //{
                getshoot2(damage2);
            //}
        }
        if(Input.GetKeyDown("r")) {
            
            //ammo = maxAmmo;
        }
        //ammoCounter.text = ammo.ToString() + "/" + maxAmmo.ToString();
        //playerAnimator.SetFloat("AmmoCount", ammo/(float)maxAmmo);
    }

    void FixedUpdate()
    {
        
        Vector2 direction = mousePos - (Vector2) transform.position;
        angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
        //Vector2 centerPos = (center.x, center.y);
        Vector2 pdirection;
        pdirection.x = mousePos.x - center.position.x;
        pdirection.y = mousePos.x - center.position.y;
        pangle = Mathf.Atan2(pdirection.y, pdirection.x)*Mathf.Rad2Deg;

        
        transform.rotation = Quaternion.Euler(0f, 0f, angle);;
        if(pangle > 90 || pangle < -90)
        {
            transform.position = leftHand.position;
            playerSprite.flipX = true;
        }
        else if (pangle <=90 || pangle >= -90)
        {
            transform.position = rightHand.position;
            playerSprite.flipX = false;
        }
    }

    public void shootProjectile(GameObject bulletPrefab, float d)
    {
        //player.basic.Play();
        for(float x = 0-(((float)projectiles/2)-0.5f); x <= (((float)projectiles)/2-0.5f)+0.1f; x+= 1)
            {
                Quaternion q = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z+(x*(spread)));
                GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation * q);
                
                bullet.GetComponent<Bullet>().bulletSpeed += bulletSpeed;

                float temp = d / (1 + 0.3f * (projectiles - 1));

                
                bullet.GetComponent<Bullet>().damage += temp;
                bullet.GetComponent<Bullet>().StartCoroutine(bullet.GetComponent<Bullet>().bulletDestroy(destroy));
                
            }
    }

    void getshoot(float d)
    {
        
            if(shooting == 1 || !player.rightArmEquipped) {return;}
            
            //gunanimator.Play("GunFire");
            //aud.Play();
            //Debug.Log("shot");
            shootProjectile(bulletPrefab, d);
                
            shootroutine = this.StartCoroutine(FireRateRoutine());
        
    }

    public void shootProjectile2(GameObject bulletPrefab, float d)
    {
        //player.basic.Play();
        for(float x = 0-(((float)projectiles2/2)-0.5f); x <= (((float)projectiles2)/2-0.5f)+0.1f; x+= 1)
            {
                Quaternion q = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z+(x*(spread2)));
                GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation * q);
                
                bullet.GetComponent<Bullet>().bulletSpeed += bulletSpeed2;

                float temp = d / (1 + 0.3f * (projectiles2 - 1));

                
                bullet.GetComponent<Bullet>().damage += temp;
                bullet.GetComponent<Bullet>().StartCoroutine(bullet.GetComponent<Bullet>().bulletDestroy(destroy2));
                
            }
    }

    void getshoot2(float d)
    {
        
            if(shooting2 == 1 || !player.leftArmEquipped) {return;}
            
            //gunanimator.Play("GunFire");
            //aud.Play();
            //Debug.Log("shot2");
            shootProjectile(bulletPrefab2, d);
                
            shootroutine2 = this.StartCoroutine(FireRateRoutine2());
        
    }

    IEnumerator FireRateRoutine()
    {
        if(shooting == 0)
        {
            shooting = 1;
            yield return new WaitForSeconds(defatkspd/attackSpeed);
            shooting = 0;
        } 
    }

    IEnumerator FireRateRoutine2()
    {
        if(shooting2 == 0)
        {
            shooting2 = 1;
            yield return new WaitForSeconds(defatkspd2/attackSpeed);
            shooting2 = 0;
        } 
    }
}

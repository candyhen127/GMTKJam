using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gun : MonoBehaviour
{

    Vector2 mousePos;

    [SerializeField]
    private Transform rightHand;
    [SerializeField]
    private Transform leftHand;
    [SerializeField]
    private Transform shootPoint;
    [SerializeField]
    private GameObject bulletPrefab;
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


    public float damage;
    public float baseDamage = 10;
    public int projectiles;
    public int baseProjectiles = 1;
    public float spread;
    public float destroy;
    int shooting = 0;
    Coroutine shootroutine;


    [SerializeField]
    private SpriteRenderer playerSprite;
    [SerializeField]
    private Animator playerAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damage = baseDamage;
        projectiles = baseProjectiles;
        bulletSpeed = baseBulletSpeed;
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
        

        if(Input.GetButton("Fire1"))
        {
            //if(ammo - 1 >= 0)
            //{
                getshoot(damage);
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
        
            if(shooting == 1) {return;}
            //gunanimator.Play("GunFire");
            //aud.Play();
            Debug.Log("shot");
            shootProjectile(bulletPrefab, d);
                
            shootroutine = this.StartCoroutine(FireRateRoutine());
        
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
}

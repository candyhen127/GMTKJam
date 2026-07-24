using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPDrop : MonoBehaviour
{
    public bool floating;
    
    public int points = 1;
    public bool isPart;
    public Part part;
    public float duration = 0.25f;
    public AnimationCurve interp;
    public Player player;
    public Rigidbody2D rb;
    public float force;
    // Start is called before the first frame update
    void Start()
    {
        if (floating) {return;}
        Vector2 knockbackForce = Random.insideUnitCircle.normalized * force;
        rb.AddForce(knockbackForce, ForceMode2D.Impulse);
        if (isPart)
        {
            GetComponent<SpriteRenderer>().sprite = part.icon;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("trig");
        if (collision.tag == "Player")
        {
            //Debug.Log("triggered");
            player = collision.gameObject.GetComponent<Player>();
            StartCoroutine(GoToPlayer());
        }
    }

    private IEnumerator GoToPlayer()
    {
        Vector2 startPos = transform.position;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            // Evaluate the animation curve for smooth deceleration easing
            float curveValue = interp.Evaluate(t);
            
            // Interpolate smoothly between start angle and end angle
            Vector3 currentpos = Vector3.LerpUnclamped(startPos, player.transform.position, curveValue);
            
            transform.position = currentpos;
            yield return null;
        }
        if (isPart)
        {
            player.inventory.Add(part);
        } else {
            player.GetScrap(points);
        }
        Destroy(gameObject);
    }
}


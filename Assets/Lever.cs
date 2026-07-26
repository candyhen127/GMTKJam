using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Lever : MonoBehaviour
{

    public GameObject door;
    public GameObject lever;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.Instance.leverPulled)
        {
            
            door.SetActive(false);
            lever.transform.rotation = Quaternion.Euler(0, 0, -90f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.leverPulled) {return;}
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.leverPulled = true;
            door.SetActive(false);
            StartCoroutine(pullLever());
        }
    }

    IEnumerator pullLever()
    {
        float angle;
        for(float i = 0; i < 1; i += Time.deltaTime)
        {
             angle = Mathf.Lerp(0, -90, i);
             lever.transform.rotation = Quaternion.Euler(0, 0, angle);

            yield return null;
        } 
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Cinemachine;

public class damagenum : MonoBehaviour
{

    public float dnum;
    public bool heal;
    public bool crit;

    public bool isPlayer;

    public Vector3 targetWorldPos;
    private Vector2 dynamicScreenOffset;
    private Camera mainCam;

    public Vector2 screenFloatSpeed = new Vector2(0, 1f);


    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        if (isPlayer)
        {
            
        gameObject.GetComponent<TextMeshProUGUI>().text = TimeSpan.FromSeconds(dnum).ToString(@"mm\:ss");
        } else
        {
            gameObject.GetComponent<TextMeshProUGUI>().text = ((int) dnum).ToString();
        }
        if(heal)
        {
            gameObject.GetComponent<TextMeshProUGUI>().color = new Color(0, 1, 0, 1);
        }
        else if (crit)
        {
            gameObject.GetComponent<TextMeshProUGUI>().color = new Color32(243, 97, 255, 255);
        }
        else
        {
            gameObject.GetComponent<TextMeshProUGUI>().color = new Color(1, 10f/dnum, 10f/dnum, 1);
        }

        gameObject.GetComponent<TextMeshProUGUI>().fontSize = 9+(.1f*dnum);
        StartCoroutine(destro());
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = transform.position + new Vector3(0, 0.3f*GameObject.Find("Canvas").GetComponent<RectTransform>().localScale.x, 0);
    }

    IEnumerator destro()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        // Hooking into Cinemachine core loop to eliminate position lag
        CinemachineCore.CameraUpdatedEvent.AddListener(UpdateUIPosition);
    }

    private void OnDisable()
    {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(UpdateUIPosition);
    }

    private void UpdateUIPosition(ICinemachineCamera vcam)
    {
        if (mainCam == null) return;

        // 1. Calculate base screen position from the player's world position
        Vector3 screenPoint = mainCam.WorldToScreenPoint(targetWorldPos);

        // 2. Check if the text is behind the camera (safeguard for 2D/3D boundaries)
        if (screenPoint.z < 0) return;

        // 3. Apply standard UI floating motion over time (independent of world physics)
        dynamicScreenOffset += screenFloatSpeed * Time.deltaTime;

        // 4. Assign the final position directly to the RectTransform
        transform.position = (Vector2)screenPoint + dynamicScreenOffset;
    }
}

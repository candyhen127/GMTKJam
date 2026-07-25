using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour
{
    public bool start = false; // Flag to start the shake effect
    public AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f); // Animation curve for the shake effect
    public float duration = 0.25f; // Duration of the shake effect
    public float smoothing = 0.2f; // How quickly the shake settles back to normal

    private float shakeTimer;
    private Vector3 shakeOffset;

    public void TriggerShake()
    {
        if (shakeTimer > 0f) return;
        start = true;
        shakeTimer = duration;
    }

    private void LateUpdate()
    {
        if (start)
        {
            start = false;
            shakeTimer = duration;
        }

        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;
            float strength = curve.Evaluate(1f - Mathf.Clamp01(shakeTimer / duration));
            Vector3 targetOffset = Random.insideUnitSphere * strength * .9f;
            shakeOffset = Vector3.Lerp(shakeOffset, targetOffset, smoothing);
            transform.position += shakeOffset;
        }
        else if (shakeOffset.sqrMagnitude > 0.0001f)
        {
            shakeOffset = Vector3.Lerp(shakeOffset, Vector3.zero, smoothing);
            transform.position += shakeOffset;
        }
    }
}

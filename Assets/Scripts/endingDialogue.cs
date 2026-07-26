using UnityEngine;
using UnityEngine.Events;

// Attach to a trigger collider (2D or 3D) at Elena's location. Plays the reunion
// lines when the player reaches her, pauses like the intro does, then fires
// onDialogueComplete once the box closes -- hook up whatever "the end" means for
// you in the Inspector (load a credits scene, play an animation, fade to black,
// call a GameManager method, etc.) without needing to touch this script again.
public class EndingDialogue : MonoBehaviour
{
    public string playerTag = "Player";

    [TextArea(2, 4)]
    public string[] endingLines = new string[]
    {
        "Elena?",
        "...",
        "I'm here. I found you."
    };

    [Tooltip("Called after the ending dialogue box closes.")]
    public UnityEvent onDialogueComplete;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other) => TryTrigger(other.gameObject);
    private void OnTriggerEnter(Collider other) => TryTrigger(other.gameObject);

    private void TryTrigger(GameObject obj)
    {
        if (hasTriggered || !obj.CompareTag(playerTag)) return;
        if (DialogueManager.Instance == null) return;

        hasTriggered = true;
        if (MenuScript.Instance != null) MenuScript.Instance.truepaused = true;

        DialogueManager.Instance.OnDialogueEnd += HandleEnd;
        DialogueManager.Instance.StartDialogue(endingLines);
    }

    private void HandleEnd()
    {
        DialogueManager.Instance.OnDialogueEnd -= HandleEnd;
        if (MenuScript.Instance != null) MenuScript.Instance.truepaused = false;
        onDialogueComplete?.Invoke();
    }
}
using UnityEngine;

// Drop this on a trigger collider (2D or 3D, either works) placed at a checkpoint
// as the player descends toward Elena. Plays a short line once when the player
// passes through. Doesn't pause the game by default -- meant for quick story
// beats mid-descent, not a full stop like the intro/ending.
//
// Place several of these at different depths in the level, each with its own
// "lines". If you'd rather have some variety without hand-writing dozens of
// triggers, fill in "lineSets" instead and enable "useRandomSet" -- it'll pick
// one at random each time (handy for reusable ambient triggers).
public class DepthDialogueTrigger : MonoBehaviour
{
    [Tooltip("Tag used to identify the player.")]
    public string playerTag = "Player";

    [TextArea(2, 4)]
    [Tooltip("Used when useRandomSet is false -- always plays these lines.")]
    public string[] lines = new string[]
    {
        "Depth marker... her signal's still coming through down here."
    };

    [Tooltip("If true, ignores 'lines' and picks one random set from 'lineSets' instead.")]
    public bool useRandomSet = false;

    public DialogueLineSet[] lineSets;

    [Tooltip("If true, pauses the game like the intro/ending dialogue. Leave false for quick ambient lines while still moving.")]
    public bool pauseGame = false;

    [Tooltip("If false, this trigger can fire again on repeat visits.")]
    public bool onlyOnce = true;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other) => TryTrigger(other.gameObject);
    private void OnTriggerEnter(Collider other) => TryTrigger(other.gameObject);

    private void TryTrigger(GameObject obj)
    {
        if ((onlyOnce && hasTriggered) || !obj.CompareTag(playerTag)) return;
        if (DialogueManager.Instance == null || DialogueManager.Instance.IsDialogueActive) return;

        hasTriggered = true;

        string[] chosenLines = lines;
        if (useRandomSet && lineSets != null && lineSets.Length > 0)
        {
            chosenLines = lineSets[Random.Range(0, lineSets.Length)].lines;
        }

        if (pauseGame)
        {
            Time.timeScale = 0f;
            if (GameManager.Instance != null) GameManager.Instance.isPaused = true;
            DialogueManager.Instance.OnDialogueEnd += HandleEnd;
        }

        DialogueManager.Instance.StartDialogue(chosenLines);
    }

    private void HandleEnd()
    {
        DialogueManager.Instance.OnDialogueEnd -= HandleEnd;
        Time.timeScale = 1f;
        if (GameManager.Instance != null) GameManager.Instance.isPaused = false;
    }
}

// Wrapper so lineSets shows up nicely in the Inspector as a list of line arrays.
[System.Serializable]
public class DialogueLineSet
{
    [TextArea(2, 4)]
    public string[] lines;
}
using System.Collections;
using UnityEngine;

// Plays the opening lore sequence, but only when IntroFlag.ShouldPlayIntro
// was set to true (by the New Game button) right before MainScene loaded.
// Attach this to an empty GameObject in MainScene.
public class IntroDialogue : MonoBehaviour
{
    [Tooltip("Optional: a full-screen black Image/panel to show behind the dialogue text during the intro. Leave empty in the Inspector if you just want the bottom dialogue box with no full-screen cover.")]
    public GameObject blackBackdrop;

    [TextArea(2, 4)]
    public string[] introLines = new string[]
    {
        "Two years since the diagnosis. I found someone who'd upload her off the books.",

        "It worked. Elena's on a drive smaller than my thumb.",

        "They raided the place, called it contraband, sent it to the Dump.",

        "Nothing comes back from the Dump. Nothing left to lose, either.",

        "SYSTEM: Transfer complete. Descent initiated.",

        "Coming, Elena."
    };

    void Start()
    {
        if (!IntroFlag.ShouldPlayIntro)
        {
            return;
        }

        IntroFlag.ShouldPlayIntro = false;

        // GameManager persists across scene loads, so this stays true even after
        // going to the shop and back -- guarantees the intro only ever plays once.
        if (GameManager.Instance != null && GameManager.Instance.hasPlayedIntro)
        {
            return;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.hasPlayedIntro = true;
        }

        StartCoroutine(PlayIntroNextFrame());
    }

    private IEnumerator PlayIntroNextFrame()
    {
        // Wait one frame so GameManager's own Start/OnSceneLoaded logic
        // (which sets Time.timeScale = 1) has already run before we pause.
        yield return null;

        if (DialogueManager.Instance == null)
        {
            yield break;
        }

        if (MenuScript.Instance != null)
        {
            MenuScript.Instance.truepaused = true;
        }

        if (blackBackdrop != null)
        {
            blackBackdrop.SetActive(true);
        }

        DialogueManager.Instance.OnDialogueEnd += HandleIntroEnd;
        DialogueManager.Instance.StartDialogue(introLines);
    }

    private void HandleIntroEnd()
    {
        DialogueManager.Instance.OnDialogueEnd -= HandleIntroEnd;

        if (MenuScript.Instance != null)
        {
            MenuScript.Instance.truepaused = false;
        }

        if (blackBackdrop != null)
        {
            blackBackdrop.SetActive(false);
        }
    }
}
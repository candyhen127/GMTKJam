using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    [SerializeField] private GameObject textPanel; // Updated from chudPanel
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI promptText; // e.g. "> press [E]"

    [Header("Typewriter Settings")]
    [SerializeField] private float textSpeed = 0.03f; // Seconds per character

    private Queue<string> sentences = new Queue<string>();
    private bool isTyping = false;
    private string currentSentence;

    // True while a dialogue box is on screen (typing or waiting for the player to advance)
    public bool IsDialogueActive => textPanel != null && textPanel.activeSelf;

    // Fires right after the dialogue box closes (e.g. so an intro script can un-pause the game)
    public event System.Action OnDialogueEnd;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Hide text panel at game start
        if (textPanel != null) textPanel.SetActive(false);
    }

    void Update()
    {
        // Quick test key: Press 'T' anytime while playing to test the dialogue!
        if (Input.GetKeyDown(KeyCode.T))
        {
            string[] testLines = new string[]
            {
                "Arthur... can you hear me?",
                "System battery dropping...",
                "We need to get deeper."
            };
            StartDialogue(testLines);
        }

        // Advance dialogue on Space/E click
        if (textPanel != null && textPanel.activeSelf && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = currentSentence;
                isTyping = false;
            }
            else
            {
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(string[] lines)
    {
        textPanel.SetActive(true);
        sentences.Clear();

        foreach (string line in lines)
        {
            sentences.Enqueue(line);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        if (promptText != null) promptText.text = "...";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(textSpeed);
        }

        isTyping = false;
        if (promptText != null) promptText.text = "> press [E] to continue";
    }

    public void EndDialogue()
    {
        textPanel.SetActive(false);
        OnDialogueEnd?.Invoke();
    }

    public void TestDialogueFromInspector()
    {
        string[] sampleLines = new string[]
        {
            "SYSTEM WARNING: Battery below 20%.",
            "Memory corruption detected in Sector 4.",
            "Elena's signal acquired..."
        };

        StartDialogue(sampleLines);
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject textPanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI promptText;

    public float typingSpeed = 0.03f;

    private Queue<string> sentences = new Queue<string>();
    private string currentSentence;
    private bool isTyping = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Don't disable textPanel here while debugging—keep it visible!
    }

    private void Update()
    {
        // Press T to trigger test dialogue
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("T key pressed! Triggering dialogue...");
            string[] testLines = new string[] 
            {
                "Arthur... is that you?",
                "My core memory sectors are degrading.",
                "Please keep coming down..."
            };
            StartDialogue(testLines);
        }

        // Advance dialogue on E or Space
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
        Debug.Log("StartDialogue called with " + lines.Length + " lines.");

        if (textPanel != null)
        {
            textPanel.SetActive(true);
        }

        if (dialogueText != null)
        {
            dialogueText.gameObject.SetActive(true);
        }

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

        // Hide prompt while typing the new sentence
        if (promptText != null) 
        {
            promptText.gameObject.SetActive(false);
        }

        currentSentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed); 
        }

        isTyping = false;

        // Show prompt when typing finishes!
        if (promptText != null)
        {
            promptText.text = "[E] Next >";
            promptText.gameObject.SetActive(true);
        }
    }

    public void EndDialogue()
    {
        Debug.Log("Dialogue finished.");
        if (textPanel != null)
        {
            textPanel.SetActive(false);
        }
    }
}
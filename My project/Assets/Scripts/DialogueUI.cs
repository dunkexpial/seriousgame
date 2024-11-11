using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    // Reference to the dialogue box (UI) and the text that will be displayed in the interface.
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private PlayerMovement Playermovement;

    // Indicates whether the dialogue box is open or not.
    public bool isOpen { get; private set; }

    private ResponseHandler responseHandler;
    private TypeWriterEffect typeWriterEffect;

    private void Start() 
    {
        typeWriterEffect = GetComponent<TypeWriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();

        // Initialize the dialogue box as closed.
        CloseDialogueBox();
    }

    // Function responsible for showing the dialogue box and starting the dialogue process.
    public void ShowDialogue(DialogueObject dialogueObject)
    {
        isOpen = true;  // Set that the dialogue is open.
        dialogueBox.SetActive(true);  // Activate the dialogue box.
        
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }
    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        // For each line of dialogue, go through the array of lines.
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];

            // Show the typing effect for the current line.
            yield return RunTypingEffect(dialogue);

            // Set the full line of text after the typing effect finishes.
            textLabel.text = dialogue;

            // If it's the last line of the dialogue and there are responses, break the loop.
            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

            yield return null;  // Wait for the next frame.

            // If the space key is pressed, skip the dialogue.
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        // If there are responses, show the response options, otherwise close the dialogue box.
        if (dialogueObject.HasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            CloseDialogueBox();  
        }
    }

    // Coroutine that controls the typing effect for each line of dialogue. Just fru fru shit
    private IEnumerator RunTypingEffect(string dialogue)
    {
        typeWriterEffect.Run(dialogue, textLabel);

        // While the typing effect is running, wait for it to finish or for the player to press Space to skip the dialogue.
        while (typeWriterEffect.isRunning)
        {
            yield return null;  // Wait for the next frame.

            // If the Space key is pressed, stop the typing effect and display the full line.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                typeWriterEffect.Stop();
            }
        }
    }

    // Function that closes the dialogue box and resets the displayed text.
    private void CloseDialogueBox()
    {
        isOpen = false;  // Set that the dialogue is closed.
        dialogueBox.SetActive(false);  
        textLabel.text = "";  // Clear the displayed text.
    }

    public bool IsDialogueActive()
    {
        return dialogueBox.activeSelf;  // Returns true if the dialogue box is active.
    }
}

// I don't blame you if don't understand, i hardly know how i did this shit

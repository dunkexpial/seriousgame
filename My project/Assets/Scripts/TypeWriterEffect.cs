using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This class simulates a typewriter effect for text in an interface.
public class TypeWriterEffect : MonoBehaviour
{
    // Speed of the typing effect
    [SerializeField] private float TypeWriterSpeed = 50f;
    
    // This property indicates whether the typing effect is currently running.
    // The GET accessor allows other classes to read the value of isRunning,
    // while the PRIVATE SET accessor restricts modifications to this value 
    // to within this class only, ensuring encapsulation.
    public bool isRunning { get; private set; }
    private Coroutine typingCoroutine;

    // Public method to start the typing effect.
    public void Run(string textToType, TMP_Text textLabel)
    {
        // Starts the coroutine that types the provided text.
        typingCoroutine = StartCoroutine(routine: TypeText(textToType, textLabel));
    }

    // Public method to stop the typing effect.
    public void Stop()
    {
        // Stops the typing coroutine and sets isRunning to false.
        StopCoroutine(typingCoroutine);
        isRunning = false;
    }

    // Coroutine responsible for displaying the text letter by letter.
    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        // Sets isRunning to true, indicating that typing is in progress.
        isRunning = true;
        
        // Clears the initial text of the label.
        textLabel.text = "";

        // Variables to control time and character index.
        float t = 0; // Accumulated time
        int charIndex = 0; // Current character index

        // While the character index is less than the length of the text to be typed...
        while (charIndex < textToType.Length)
        {
            // Accumulates time based on typing speed.
            t += Time.deltaTime * TypeWriterSpeed; 

            // Updates the character index based on the accumulated time.
            charIndex = Mathf.FloorToInt(t);
            
            // Ensures that the index does not exceed the length of the text.
            charIndex = Mathf.Clamp(value: charIndex, min: 0, max: textToType.Length);

            // Updates the label text to show only the characters that have been "typed".
            textLabel.text = textToType.Substring(startIndex: 0, length: charIndex);

            // Waits until the next frame to continue execution.
            yield return null;
        }

        // Sets isRunning to false when typing ends.
        isRunning = false;
    }
}


// When you look into an abyss for a long time, the abyss looks back at you.
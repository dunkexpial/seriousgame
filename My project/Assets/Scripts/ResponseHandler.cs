using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// This class manages the responses that appear when a dialogue has options for the player to choose.
public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;   
    [SerializeField] private RectTransform responseButtonTemplate;
    [SerializeField] private RectTransform responseContainer;

    // Reference to the DialogueUI class, which manages the dialogue interface.
    private DialogueUI dialogueUI;

    // Temporary list to store the created response buttons.
    List<GameObject> tempResponseButtons = new List<GameObject>();

    private void Start()
    {
        // Gets the reference to the DialogueUI class when the script starts.
        dialogueUI = GetComponent<DialogueUI>();
    }
    public void ShowResponses(Response[] responses)
    {
        // Initializes the width of the response container, which will be adjusted later.
        float containerWidth = 0;

        // For each provided response, creates a corresponding button.
        foreach (Response response in responses)
        {
            // Instantiates a new response button from the template.
            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            
            // Activates the instantiated button.
            responseButton.gameObject.SetActive(true);
            
            // Sets the button text as the text of the response.
            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;

            // Adds a listener for the button click, calling OnPickedResponse when clicked.
            responseButton.GetComponent<Button>().onClick.AddListener(call: () => OnPickedResponse(response));

            // Adds the button to the temporary list of response buttons.
            tempResponseButtons.Add(responseButton);
        }

        // Adjusts the width of the response container 
        responseContainer.sizeDelta = new Vector2(containerWidth, responseContainer.sizeDelta.y);

        // Makes the response box visible to the player.
        responseBox.gameObject.SetActive(true); 
    }

    // Method called when the player chooses a response.
    private void OnPickedResponse(Response response)
    {
        // Hides the response box when a response is chosen.
        responseBox.gameObject.SetActive(false); 

        
        foreach (GameObject button in tempResponseButtons)
        {
            // Destroys all temporary response buttons that were created.
            Destroy(button);
        }
        
        // Clears the temporary button list to free memory.
        tempResponseButtons.Clear();

        // Shows the next dialogue associated with the response chosen by the player.
        dialogueUI.ShowDialogue(response.DialogueObject);
    }
}


// At least i tried, in the end i did
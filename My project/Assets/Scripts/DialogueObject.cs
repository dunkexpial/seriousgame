using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows the creation of a ScriptableObject instance from the Unity menu
// Enables creating a Dialogue Object via the Unity menu, how can i say... in the Dialogue Data folder you can create a
// Dialogue Object, basically is the dialogue of the professor
[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    // Stores a list of dialogue lines as an array of strings
    [SerializeField] [TextArea] private string[] dialogue;

    // Stores a list of possible responses [Option to choose the dialogue], represented by the array of Response objects
    [SerializeField] private Response[] responses;

    // Returns the array of dialogue lines
    public string[] Dialogue => dialogue;

    // Returns true if there are any responses associated with the dialogue
    public bool HasResponses => Responses != null && Responses.Length > 0;

    // Returns the array of possible responses
    public Response[] Responses => responses;

}

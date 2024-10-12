using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// This is the Response class that represents a response a player can choose during a dialogue.
[System.Serializable] // Attribute that allows this class to be serialized, making it easier to edit in the Unity Inspector.
public class Response 
{
    // This field stores the text of the response that will be displayed to the player.
    [SerializeField] private string responseText;

    // This field stores a dialogue object associated with the response, which may contain more dialogue.
    [SerializeField] private DialogueObject dialogueObject;

    // Property that allows access to the response text from outside the class.
    public string ResponseText => responseText;

    // Property that allows access to the dialogue object associated with this response.
    public DialogueObject DialogueObject => dialogueObject;
}

//Please help me, it was fun in the beginning, now it's torture
// This is an interface called IInteractable.
// An interface defines a set of methods that other classes must implement.
// In this case, we are stating that any class using this interface must have a method called Interact.
public interface Interactable 
// A public interface is like a set of rules that tells classes what they must do. 
// It defines certain methods that those classes have to include, but it doesn't tell them how to do it. 
// This way, different classes can have their own way of doing things while still following the same rules.

{
    // The Interact method allows a player to interact with an object.
    // The 'player' parameter represents the player attempting to interact.
    // Classes that implement this interface need to define how this interaction occurs.
    void Interact(playermovement player);
}


// I don't blame you if don't understand, i hardly know how i did this shit
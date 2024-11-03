using UnityEngine;

public class LayerCollisionIgnorer : MonoBehaviour
{
    // Replace these with your actual layer names or indexes
    public int layer1 = 8; // Example: Layer 8
    public int layer2 = 9; // Example: Layer 9

    void Start()
    {
        // Ignore collisions between the specified layers
        Physics2D.IgnoreLayerCollision(layer1, layer2, true);
    }
}

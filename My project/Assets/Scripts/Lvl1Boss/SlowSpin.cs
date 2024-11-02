using UnityEngine;

public class SlowSpin : MonoBehaviour
{
    public float spinSpeed = 180f; // Rotation speed in degrees per second

    void Update()
    {
        // Rotate around the Z-axis
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
    }
}

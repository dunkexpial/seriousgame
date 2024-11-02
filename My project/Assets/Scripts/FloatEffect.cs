using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    public float floatAmplitude = 2f;
    public float floatFrequency = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Calculate new Y position
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        
        // Apply the new position
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}

using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    public float floatAmplitude = 2f;
    public float floatFrequency = 2f;

    private Vector3 startLocalPos;

    void Start()
    {
        // Store the initial local position relative to the parent
        startLocalPos = transform.localPosition;
    }

    void Update()
    {
        // Calculate new Y position relative to the parent
        float newY = startLocalPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        // Apply the new local position
        transform.localPosition = new Vector3(startLocalPos.x, newY, startLocalPos.z);
    }
}

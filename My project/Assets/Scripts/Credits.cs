using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScroller : MonoBehaviour
{
    private RectTransform rectTransform;

    public float scrollSpeed = 40f;
    public float smoothness = 0.5f;

    private Vector2 targetPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        targetPosition = rectTransform.anchoredPosition; 
    }

    void Update()
    {
        targetPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, smoothness);
    }
}

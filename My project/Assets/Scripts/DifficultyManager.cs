using UnityEngine;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    public Button[] difficultyButtons;
    public float difficulty;
    public float reverseDifficulty;

    void Start()
    {
        difficulty = PlayerPrefs.GetFloat("Difficulty", 1f);
        reverseDifficulty = PlayerPrefs.GetFloat("ReverseDifficulty", 1f);

        for (int i = 0; i < difficultyButtons.Length; i++)
        {
            int buttonIndex = i;
            difficultyButtons[buttonIndex].onClick.AddListener(() => SetDifficulty(buttonIndex));
        }
    }

    void SetDifficulty(int index)
    {
        switch (index)
        {
            case 0:
                difficulty = 0.75f;
                reverseDifficulty = 1.5f;
                break;
            case 1:
                difficulty = 1.0f;
                reverseDifficulty = 1.0f;
                break;
            case 2:
                difficulty = 1.5f;
                reverseDifficulty = 0.75f;
                break;
            case 3:
                difficulty = 1.75f;
                reverseDifficulty = 0.5f;
                break;
            case 4:
                difficulty = 2.0f;
                reverseDifficulty = 0.25f;
                break;
        }

        PlayerPrefs.SetFloat("Difficulty", difficulty);
        PlayerPrefs.SetFloat("ReverseDifficulty", reverseDifficulty);

        Debug.Log("Normal Difficulty set to: " + difficulty);
        Debug.Log("Reverse Difficulty set to: " + reverseDifficulty);
    }
}
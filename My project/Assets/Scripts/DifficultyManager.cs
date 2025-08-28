using UnityEngine;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour
{
    public Button[] difficultyButtons;
    public float difficulty;
    public float reverseDifficulty;

    [Header("Selector")]
    public RectTransform selector; // The object to move to the selected button

    void Start()
    {
        // Initialize PlayerPrefs if not set
        if (!PlayerPrefs.HasKey("Difficulty"))
        {
            PlayerPrefs.SetFloat("Difficulty", 1f);
            PlayerPrefs.SetFloat("ReverseDifficulty", 1f);
            PlayerPrefs.Save();
        }

        // Load saved difficulty
        difficulty = PlayerPrefs.GetFloat("Difficulty");
        reverseDifficulty = PlayerPrefs.GetFloat("ReverseDifficulty");

        // Assign button click listeners
        for (int i = 0; i < difficultyButtons.Length; i++)
        {
            int buttonIndex = i;
            difficultyButtons[buttonIndex].onClick.AddListener(() => SetDifficulty(buttonIndex));
        }

        // Move selector to the button that matches current difficulty
        MoveSelectorToCurrentDifficulty();
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

        MoveSelectorToButton(index);
    }

    private void MoveSelectorToCurrentDifficulty()
    {
        // Determine which button matches the saved difficulty
        int index = 1; // default to normal
        if (Mathf.Approximately(difficulty, 0.75f)) index = 0;
        else if (Mathf.Approximately(difficulty, 1f)) index = 1;
        else if (Mathf.Approximately(difficulty, 1.5f)) index = 2;
        else if (Mathf.Approximately(difficulty, 1.75f)) index = 3;
        else if (Mathf.Approximately(difficulty, 2f)) index = 4;

        MoveSelectorToButton(index);
    }

    private void MoveSelectorToButton(int index)
    {
        if (selector != null && index >= 0 && index < difficultyButtons.Length)
        {
            // Move selector to the button's position
            selector.position = difficultyButtons[index].transform.position;
        }
    }
}

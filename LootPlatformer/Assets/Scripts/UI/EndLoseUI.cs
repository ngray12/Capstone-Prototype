using UnityEngine;
using UnityEngine.UI;

public class EndLoseUI : MonoBehaviour
{
    [Header("UI Buttons")]
    public Button restartButton;
    public Button mainMenuButton;
    public Button levelSelectButton;

    private void Awake()
    {
        // Hook buttons into GameManager
        if (restartButton != null)
            restartButton.onClick.AddListener(() => GameManager.Instance.RestartLevel());

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(() => GameManager.Instance.LoadMainMenu());

        if (levelSelectButton != null)
            levelSelectButton.onClick.AddListener(() => GameManager.Instance.LoadLevelSelect());
    }

    private void OnEnable()
    {
        // Freeze time while Lose UI is active
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        // Resume time when closed
        Time.timeScale = 1f;
    }
}
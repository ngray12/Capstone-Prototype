using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndWinUI : MonoBehaviour
{
    public Button restartButton;
    public Button levelSelectButton;
    public Button mainMenuButton;
    public Button nextLevelButton;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI globalaScoreText;
    public TextMeshProUGUI timerText;

    private void Awake()
    {
        // Hook up buttons to GameManager actions
        if (restartButton != null)
            restartButton.onClick.AddListener(() => GameManager.Instance.RestartLevel());

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(() => GameManager.Instance.LoadMainMenu());

        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(() => GameManager.Instance.NextLevel());

        if (levelSelectButton != null)
            levelSelectButton.onClick.AddListener(() => GameManager.Instance.LoadLevelSelect());
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {  
        Time.timeScale = 1f;
    }


}
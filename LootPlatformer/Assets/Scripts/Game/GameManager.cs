using CMIYC;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public OptionsManager OptionsManager { get; private set; }
    public AudioManager AudioManager { get; private set; }
    public EnemySpawner EnemySpawner { get; private set; }

    [Header("Gameplay Stats")]
    public int score = 0;
    public int globalScore = 0;
    public float timeElapsed = 0f;
    public bool gameActive = true;
    public bool gamePaused = false;
    public bool gameWin = false;
    public bool inGame = false;

    [Header("UI ITEMS")]
    public GameObject endScene_UI_LOSE;
    public EndLoseUI endLoseUI;
    public GameObject endScene_UI_WIN;
    public EndWinUI endWinUI;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI globalScoreText;
    public TextMeshProUGUI timerText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
            
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameWin = false;

        Scene current = SceneManager.GetActiveScene();

        if(current.name != "Main Menu" && current.name != "Level Select")
            inGame = true;

        if (inGame)
        {
            // Refresh UI references in the new scene
            endLoseUI = FindObjectOfType<EndLoseUI>(true);
            endWinUI = FindObjectOfType<EndWinUI>(true);

            if (endLoseUI != null)
                endScene_UI_LOSE = endLoseUI.gameObject;

            if (endWinUI != null)
            {
                endScene_UI_WIN = endWinUI.gameObject;
                scoreText = endWinUI.scoreText;
                globalScoreText = endWinUI.globalaScoreText;
                timerText = endWinUI.timerText;

                if (endWinUI.restartButton != null)
                {
                    endWinUI.restartButton.onClick.RemoveAllListeners();
                    endWinUI.restartButton.onClick.AddListener(RestartLevel);
                }

                if (endWinUI.mainMenuButton != null)
                {
                    endWinUI.mainMenuButton.onClick.RemoveAllListeners();
                    endWinUI.mainMenuButton.onClick.AddListener(LoadMainMenu);
                }

                if (endWinUI.levelSelectButton != null)
                {
                    endWinUI.levelSelectButton.onClick.RemoveAllListeners();
                    endWinUI.levelSelectButton.onClick.AddListener(LoadLevelSelect);
                }

                if (endWinUI.nextLevelButton != null)
                {
                    endWinUI.nextLevelButton.onClick.RemoveAllListeners();
                    endWinUI.nextLevelButton.onClick.AddListener(NextLevel);
                }
            }
        }
    }
    private void InitializeManagers()
    {
        OptionsManager = GetComponentInChildren<OptionsManager>();
        AudioManager = GetComponentInChildren<AudioManager>();
        


        if (OptionsManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/OptionsManager");
            if (prefab == null)
            {
                Debug.Log("$OptionsManager prefab not found");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                OptionsManager = GetComponentInChildren<OptionsManager>();
            }
        }

        if (AudioManager == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/AudioManager");
            if (prefab == null)
            {
                Debug.Log("$AudioManager prefab not found");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                AudioManager = GetComponentInChildren<AudioManager>();
            }
        }

    }

   
    private void Update()
    {
        if (!gameActive) return;

        if (inGame)
        {
            // Track time
            timeElapsed += Time.deltaTime;

            int minutes = Mathf.FloorToInt(timeElapsed / 60f);
            int seconds = Mathf.FloorToInt(timeElapsed % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    public void AddScore(int ammount)
    {
        score += ammount;
        Debug.Log("Score: " + score);
    }

    public void RemoveScore(int ammount)
    {
        score -= ammount;
        Debug.Log("Score: " + score);
    }

    public void EndScene()
    {
        gameActive = false;

        PlayerController2D player = FindObjectOfType<PlayerController2D>();
        if (player != null)
        { 
            player.Freeze();
        }

        if (gameWin && endScene_UI_WIN != null)
        {
            scoreText.text = "Score: " + score.ToString();
            globalScoreText.text ="Global Score:" + globalScore.ToString();

            int minutes = Mathf.FloorToInt(timeElapsed / 60f);
            int seconds = Mathf.FloorToInt(timeElapsed % 60f);
            int millis = Mathf.FloorToInt((timeElapsed * 10f) % 10f);
            timerText.text = $"Time: {minutes:00}:{seconds:00}.{millis}";

            endScene_UI_WIN.SetActive(true);
        }


        if (!gameWin && endScene_UI_LOSE != null)
        {
            endScene_UI_LOSE.SetActive(true);
        }
    }
    public void LoadMainMenu()
    {
        ResetGameState();
        SceneManager.LoadScene("Main Menu"); 
    }

    public void LoadLevelSelect()
    {
        ResetGameState();
        SceneManager.LoadScene("Level Select");
    }

    public void NextLevel()
    {
        ResetGameState();
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            SceneManager.LoadScene("Level Select");
        }
    }

    public void RestartLevel()
    {
        ResetGameState();

        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    private void ResetGameState()
    {
        score = 0;
        timeElapsed = 0f;
        gameActive = true;
    }

}

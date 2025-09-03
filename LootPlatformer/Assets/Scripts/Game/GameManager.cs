using CMIYC;
using System.Collections;
using System.Collections.Generic;
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

    [Header("UI ITEMS")]
    public GameObject endScene_UI;

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

    private void InitializeManagers()
    {
        OptionsManager = GetComponentInChildren<OptionsManager>();
        AudioManager = GetComponentInChildren<AudioManager>();
        EnemySpawner = GetComponentInChildren<EnemySpawner>();


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

        if (EnemySpawner == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/EnemySpawner");
            if (prefab == null)
            {
                Debug.Log("$EnemySpawner prefab not found");
            }
            else
            {
                Instantiate(prefab, transform.position, Quaternion.identity, transform);
                EnemySpawner = GetComponentInChildren<EnemySpawner>();
            }
        }
    }

   
    private void Update()
    {
        if (!gameActive) return;

        // Track time
        timeElapsed += Time.deltaTime;
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

        if (endScene_UI != null)
        {
            endScene_UI.SetActive(true);
        }
    }
    public void LoadMainMenu()
    {
        ResetGameState();
        SceneManager.LoadScene("Main Menu"); 
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

using CMIYC;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public float spawnDelay = 4f;
    public float behindDistance = 5f;
    public float yOffset = 3f;
    public float randomXRange = 2f;
    public float maxChaseDistance = 20f;
    public LayerMask groundMask;

    private PlayerController2D playerController;
    private GameObject currentEnemy;
    private bool gameActive = true;
    private bool spawning = false;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController2D>();
        if (playerController == null)
        {
            Debug.LogError("EnemySpawner: No PlayerController2D found in scene!");
            return;
        }

        ScheduleSpawn(spawnDelay);
    }

    void Update()
    {
        if (!gameActive) return;

        // Schedule spawn if there is no current enemy
        if (currentEnemy == null && !spawning)
        {
            ScheduleSpawn(spawnDelay);
        }
        // Check if enemy is too far and needs respawn
        else if (currentEnemy != null && !spawning)
        {
            EnemyChase chase = currentEnemy.GetComponent<EnemyChase>();
            if (chase != null && chase.IsTooFarFrom(playerController.transform, maxChaseDistance))
            {
                Destroy(currentEnemy);
                currentEnemy = null;
                ScheduleSpawn(spawnDelay);
            }
        }
    }

    public void StopRespawning()
    {
        gameActive = false;
        if (currentEnemy != null)
        {
            Destroy(currentEnemy);
            currentEnemy = null;
        }
    }

    private void ScheduleSpawn(float delay)
    {
        spawning = true;
        Invoke(nameof(SpawnEnemy), delay);
    }

    private void SpawnEnemy()
    {
        spawning = false;

        if (!gameActive || enemyPrefab == null || playerController == null) return;

        Transform player = playerController.transform;

        // Determine spawn side and randomize horizontal offset
        float randomX = Random.Range(-randomXRange, randomXRange);
        Vector3 offset = (playerController.facingRight ? Vector3.left : Vector3.right) * behindDistance;
        offset.x += randomX;
        offset.y += yOffset;

        Vector3 spawnPos = player.position + offset;

        // Snap to ground using raycast
        RaycastHit2D hit = Physics2D.Raycast(spawnPos, Vector2.down, 20f, groundMask);
        if (hit.collider != null)
        {
            spawnPos = new Vector3(hit.point.x, hit.point.y + 0.5f, 0f);
        }

        // Instantiate enemy
        currentEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        // Assign player to enemy chase script
        EnemyChase chase = currentEnemy.GetComponent<EnemyChase>();
        if (chase != null)
        {
            chase.player = player;
        }
    }
}

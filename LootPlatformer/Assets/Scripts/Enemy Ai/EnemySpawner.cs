using CMIYC;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public PlayerController2D playerController;
    public float spawnDelay = 4f;
    public float behindDistance = 5f;
    public float yOffset = 3f;
    public LayerMask groundMask;
    public float maxChaseDistance = 20f;
    private GameObject currentEnemy;
    private bool gameActive = true;
    private bool spawning = false;

    


    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController2D>();
        spawning = true;
        Invoke(nameof(SpawnEnemy), spawnDelay);
    }

    private void Update()
    {
        if (!gameActive) return;

        if (currentEnemy == null && !spawning)
        {
            spawning = true;
            Invoke(nameof(SpawnEnemy), spawnDelay);
        }
        else if (currentEnemy != null && !spawning)
        {
            EnemyChase chase = currentEnemy.GetComponent<EnemyChase>();
            if (chase !=null && chase.IsTooFarFrom(playerController.transform,maxChaseDistance))
            {
                Destroy(currentEnemy);
                currentEnemy = null;

                Invoke(nameof(SpawnEnemy), spawnDelay);
            }
        }
    }

    public void StopRespawning()
    {
        gameActive = false;

        if (currentEnemy != null && currentEnemy.name == "RespawnPlaceholder")
        {
            Destroy(currentEnemy);
            currentEnemy = null;
        }
    }

    void SpawnEnemy()
    {
        spawning = false;

        if (!gameActive || enemyPrefab == null || playerController == null) return;

        Transform player = playerController.transform;

        // Determine spawn side based on player facing
        Vector3 offset = playerController.facingRight ? Vector3.left * behindDistance : Vector3.right * behindDistance;

        offset.y += 3f; // raise above ground for safety

        Vector3 spawnPos = player.position + offset;

        // Snap to ground
        RaycastHit2D hit = Physics2D.Raycast(spawnPos, Vector2.down, 20f, groundMask);
        if (hit.collider != null)
            spawnPos = new Vector3(hit.point.x, hit.point.y + 0.5f, player.position.z);

        // Spawn enemy
        currentEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        EnemyChase chase = currentEnemy.GetComponent<EnemyChase>();
        if (chase != null)
            chase.player = player;
    }
    
}

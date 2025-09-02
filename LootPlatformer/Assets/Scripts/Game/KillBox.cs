using CMIYC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemy falls
        EnemyChase enemy = collision.GetComponent<EnemyChase>();
        if (enemy != null)
        {
            Debug.Log("GUARD DIED");
            Destroy(enemy.gameObject);
            return;
        }

        // Player falls
        PlayerController2D player = collision.GetComponent<PlayerController2D>();
        if (player != null)
        {
            Debug.Log("Player fell! Game Over!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

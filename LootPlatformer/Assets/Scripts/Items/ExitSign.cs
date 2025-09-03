using CMIYC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSign : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("PLAYER WINS");



            EnemyChase enemy = FindObjectOfType<EnemyChase>();
            if (enemy != null)
            {
                enemy.StopChase();
            }

            EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
            if (spawner != null)
            {
                spawner.StopRespawning();
            }

            GameManager.Instance.gameWin = true;
            GameManager.Instance.EndScene();
        }
    }
}

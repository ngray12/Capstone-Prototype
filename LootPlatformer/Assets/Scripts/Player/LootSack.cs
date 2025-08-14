using CMIYC;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LootSack : MonoBehaviour
{
    public Transform sackVisual;       // The sack object that grows
    public float sizePerWeight = 0.05f;
    public PlayerController2D player;  // Reference to player to adjust speed

    private List<Treasure> treasures = new List<Treasure>();
    private float totalWeight = 0f;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropLastTreasure();
        }
    }

    public void AddTreasure(Treasure t)
    {
        treasures.Add(t);
        totalWeight += t.encumberanceValue;

        UpdateSackSize();
        player.AddEncumberance(totalWeight);
    }

    private void UpdateSackSize()
    {
        float newScale = 1 + (totalWeight * sizePerWeight);
        sackVisual.localScale = Vector3.one * newScale;
    }

    private void DropLastTreasure()
    {
        if (treasures.Count == 0) return;

        Treasure t = treasures[treasures.Count - 1];
        treasures.RemoveAt(treasures.Count - 1);
        totalWeight -= t.encumberanceValue;

        Treasure dropped = Instantiate(t,player.transform.position + Vector3.left *0.5f, Quaternion.identity);
        dropped.gameObject.SetActive(true);
        
        UpdateSackSize();
        player.AddEncumberance(totalWeight);

    }
}

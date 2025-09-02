using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMIYC
{
    public class Treasure : MonoBehaviour
    {
        public float encumbranceValue;
        public GameObject prefab; 

        private bool canBePickedUp = false;
        public float pickupDelay = .5f;

        private void Start()
        {
            if (prefab == null) prefab = gameObject;
            SetPickupDelay(pickupDelay);
        }

        public void SetPickupDelay(float seconds)
        {
            canBePickedUp = false ;
            Invoke(nameof(EnablePickup), seconds);
        }

        private void EnablePickup()
        {
            canBePickedUp = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!canBePickedUp) return;

            PlayerController2D player = collision.GetComponent<PlayerController2D>();
            if (player != null && player.lootSack != null)
            {
                // Add THIS treasure to the sack
                player.lootSack.AddTreasure(this);

                // Destroy the pickup object in the scene
                gameObject.SetActive(false);
            }
        }
    }
}

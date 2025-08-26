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
            PlayerController2D player = collision.GetComponent<PlayerController2D>();
            if (player != null && player.lootSack != null)
            {
                player.lootSack.AddTreasure(this);
                Destroy(gameObject); 
            }
        }
    }
}

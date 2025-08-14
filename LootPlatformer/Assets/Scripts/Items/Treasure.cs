using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMIYC
{
    public class Treasure : MonoBehaviour
    {
        public float encumberanceValue;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerController2D player = collision.GetComponent<PlayerController2D>();
            if (player != null) 
            { 
                
                player.AddEncumberance(encumberanceValue);
                Destroy(gameObject);
            }

        }
    }
}

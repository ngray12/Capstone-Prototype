using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMIYC
{
    public class LootSack : MonoBehaviour
    {
        [Header("Visuals")]
        public Transform sackVisual;
        public CapsuleCollider2D sackCollider;
        public float sizePerWeight = 0.05f;

        [Header("Sway")]
        public float swayAmount = 10f;
        public float swaySpeed = 5f;

        [Header("Encumbrance")]
        public float totalEncumbrance = 0f;   // field instead of property
        public float maxEncumbrance = 10f;

        private PlayerController2D player;
        private float currentTilt;

        [SerializeField]
        private List<Treasure> treasures = new List<Treasure>();

        private void Start()
        {
            player = GetComponentInParent<PlayerController2D>();
        }

        public void AddTreasure(Treasure t)
        {
            treasures.Add(t);
            totalEncumbrance += t.encumbranceValue;
            totalEncumbrance = Mathf.Clamp(totalEncumbrance, 0f, maxEncumbrance);

            UpdateSackSize();

            if (player != null)
                player.SetEncumbrance(totalEncumbrance);
        }

        public bool HasTreasure()
        {
            return treasures.Count > 0;
        }

        public void DropLastTreasure()
        {
            if (treasures.Count == 0) return;

            Treasure t = treasures[treasures.Count - 1];
            treasures.RemoveAt(treasures.Count - 1);
            totalEncumbrance -= t.encumbranceValue;
            totalEncumbrance = Mathf.Max(totalEncumbrance, 0f);

            if (t.prefab != null && player != null)
            {
                Vector3 dropOffset = player.facingRight ? Vector3.left : Vector3.right;
                dropOffset *= 1.0f;
                GameObject dropped = Instantiate(t.prefab, player.transform.position + dropOffset, Quaternion.identity);
                dropped.SetActive(true);

                Treasure droppedTreasure = dropped.GetComponent<Treasure>();
                if (droppedTreasure != null)
                {
                    droppedTreasure.SetPickupDelay(0.5f);
                }
            }

            UpdateSackSize();

            if (player != null)
                player.SetEncumbrance(totalEncumbrance);
        }

        private void UpdateSackSize()
        {
            if (sackVisual == null) return;

            float yScale = 1f + totalEncumbrance * sizePerWeight;        // taller with weight
            float xScale = 1f + totalEncumbrance * sizePerWeight * 0.5f; // less wide growth

            sackVisual.localScale = new Vector3(xScale, yScale, 1f);

            if (sackCollider != null)
            {
                sackCollider.size = new Vector2(xScale, yScale);
                sackCollider.offset = new Vector2(0f, -yScale / 2f);
            }
        }

        public void ApplySway()
        {
            if (player == null || sackVisual == null) return;

            float horizontalVelocity = player.body.velocity.x;
            float facingFactor = player.facingRight ? 1f : -1f;

            float targetTilt = -horizontalVelocity * swayAmount * facingFactor;

            currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * swaySpeed);

            sackVisual.localRotation = Quaternion.Euler(0f, 0f, currentTilt);
        }
    }
}

using UnityEngine;

namespace CMIYC
{
    [RequireComponent(typeof(Collider2D))]
    public class CashPickup : MonoBehaviour
    {
        public LootItem loot;
        public bool destroyOnPickUp = true;

        private void Reset()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            int value = loot != null ? loot.value : 100;
            float weight = loot != null ? loot.weight : 5f;
            GameManager.Instance.AddCash(value, weight);
            if (destroyOnPickUp) Destroy(gameObject);
        }
    }
}

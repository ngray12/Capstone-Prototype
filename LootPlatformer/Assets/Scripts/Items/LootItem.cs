using UnityEngine;

namespace CMIYC
{
    [CreateAssetMenu(menuName = "CMIYC/Loot Item")]
    public class LootItem : ScriptableObject
    {
        public string displayName = "Cash Bundle";
        public int value = 100;
        public float weight = 5f;
        public Sprite icon;
    }
}

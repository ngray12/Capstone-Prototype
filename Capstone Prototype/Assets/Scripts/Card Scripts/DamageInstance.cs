using UnityEngine;

namespace Cardlike
{
    [System.Serializable]
    public class DamageInstance
    {
        public int damageAmmount;
        public DamageType damageType;
    }
    public enum DamageType
    {
        Physical, Fire, Ice, Lightning, Poison, Holy,
    }

}
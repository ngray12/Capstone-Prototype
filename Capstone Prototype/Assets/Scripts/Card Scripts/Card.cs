using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;


namespace Cardlike
{
    
    public abstract class Card : ScriptableObject
    {

        public string cardName;
        public int cost;
        public string cardDescription;
        public List<ClassDeck> classDeck;

        public abstract CardType GetCardType();

        public enum CardType 
        {
            Attack, Skill, Enchantment, Glitch
        }

        public virtual void Play()
        {
            Debug.Log($"{cardName} was played, but has no defined effect.");
        }


    }

    public enum ClassDeck
    {
        Warrior,Cleric,Rouge,Paladin,Pirate,Inquisitor
    }

}
using UnityEngine;
using Cardlike;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Attack Card", menuName = "Card/Attack")]
public class AttackCard : Card
{ 
    public bool isRanged;
    public bool isAOE;
    public List<DamageInstance> damageInstances = new List<DamageInstance>();
   

    public override CardType GetCardType()
    {
        return CardType.Attack;
    }


  

    
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cardlike;

[CreateAssetMenu(fileName = "New Enchatment Card", menuName = "Card/Enchantment")]
public class EnchantmentCard : Card
{

    public int enchNum1;
    public int enchNum2;
    public int enchNum3;

    
    public override CardType GetCardType()
    {
        return CardType.Enchantment;
    }


}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cardlike;

[CreateAssetMenu(fileName = "New Glitch Card", menuName = "Card/Glitch")]
public class GlitchCard : Card
{
    public int glitchNum1;
    public int glitchNum2;
    public int glitchNum3;
    

    public override CardType GetCardType()
    {
        return CardType.Glitch;
    }

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cardlike;

[CreateAssetMenu(fileName = "New Skill Card", menuName = "Card/Skill")]
public class SkillCard : Card
{

    public int skillNum1;
    public int skillNum2;
    public int skillNum3;

    public override CardType GetCardType()
    {
        return CardType.Skill;
    }


}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cardlike;
using UnityEngine.UI;
using TMPro;
using System;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;


    public Image cardImage;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text costText;
    public TMP_Text typeText;


    private Color[] classColors =
    {
        Color.red, Color.blue,Color.green

    };



    void Start()
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        nameText.text = cardData.cardName;
        descriptionText.text = cardData.cardDescription;
        costText.text = cardData.cost.ToString();
        typeText.text = cardData.GetCardType().ToString();
        cardImage.color = classColors[(int)cardData.classDeck[0]];
    }
}

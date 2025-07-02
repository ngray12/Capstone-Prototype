using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cardlike;
using System;

public class HandManager : MonoBehaviour
{
    public DeckManager DeckManager;


    public GameObject cardPrefab;
    public Transform handTransform;

    public float handSpread = 5f;
    public float cardSpacing = 5f;
    public float verticalSpacing = 10f;

    public int maxHandSize = 12;

    public List<GameObject> cardsInHand = new List<GameObject>(); 

    
    void Start()
    {
        
    }

    public void AddCardToHand(Card cardData)
    {
        if (cardsInHand.Count < maxHandSize)
        {
            GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
            cardsInHand.Add(newCard);

            newCard.GetComponent<CardDisplay>().cardData = cardData;


            
        }

        UpdateHandVisuals();
    }

    private void Update()
    {
        //UpdateHandVisuals();
    }

    public void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count;

        if (cardCount == 1)
        {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;

        }

        for (int i = 0; i < cardCount; i++) 
        {
            float rotationAngle = (handSpread * (i-(cardCount-1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);


            float horizonatalOffest = (cardSpacing * (i - (cardCount - 1) / 2f));

            float normalizedPoisition = (2f * i / (cardCount - 1) - 1f);
            float verticalOffset =  verticalSpacing * (1 - normalizedPoisition * normalizedPoisition);


            cardsInHand[i].transform.localPosition = new Vector3(horizonatalOffest, verticalOffset, 0f);    
        }
    }
}

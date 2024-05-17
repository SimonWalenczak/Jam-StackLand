using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterObject : MonoBehaviour
{
    public bool IsOpen;

    public BoosterData Booster;
    
    private void OnMouseUp()
    {
        if (IsOpen == false)
        {
            IsOpen = true;
            
            foreach (var element in Booster.Boosters[GameManager.Instance.NbBooster].ElementsInBooster)
            {
                GameObject currentCard = Instantiate(GameManager.Instance.CardPrefab, transform.position, Quaternion.Euler(0, 180, 0));
                currentCard.GetComponent<CardVisual>().Data = element;
            }
            
            Destroy(gameObject);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CardActor : MonoBehaviour
{
    private Card _card;
    private CardVisual _cardVisual;

    private List<ElementsRequieresForCraft> Elements = new List<ElementsRequieresForCraft>();

    private void Start()
    {
        _card = GetComponent<Card>();
        _cardVisual = GetComponent<CardVisual>();

        Elements = _cardVisual.Data.ElementsRequieresForCraft;
        Elements.Sort((x, y) => x.ElementsRequieres.Count.CompareTo(y.ElementsRequieres.Count));
        Elements.Reverse();

        //Debug
        for (var index = 0; index < Elements.Count; index++)
        {
            var element = Elements[index];
            Debug.Log($"{index} : " + element.ElementToCraft);
        }
        //EndDebug
    }

    private void Update()
    {
        if (_card.Children.Count != 0)
        {
            foreach (var element in Elements)
            {
                foreach (var child in _card.Children)
                {
                    
                }
            }
        }
    }
}
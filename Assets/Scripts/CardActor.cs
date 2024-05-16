using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CardActor : MonoBehaviour
{
    private Card _card;

    [SerializeField] private ElementData Data;

    [SerializeField] private GameObject GoldValueGroup;
    [SerializeField] private GameObject DefValueGroup;
    [SerializeField] private TextMeshPro NbChildrenText;

    private void Start()
    {
        _card = GetComponent<Card>();

        GetComponent<SpriteRenderer>().sprite = Data.CardSprite;

        if (Data.Type == ElementType.Arme)
        {
            DefValueGroup.SetActive(true);
            DefValueGroup.GetComponentInChildren<TextMeshPro>().text = Data.DefValue.ToString();
        }

        GoldValueGroup.SetActive(true);
        GoldValueGroup.GetComponentInChildren<TextMeshPro>().text = Data.GoldValue.ToString();
    }

    private void Update()
    {
        NbChildrenText.text = _card.Children.Count.ToString();
    }
}
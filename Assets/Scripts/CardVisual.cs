using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CardVisual : MonoBehaviour
{
    private Card _card;

    [SerializeField] private ElementData Data;

    [SerializeField] private TextMeshPro _goldValueText;
    [SerializeField] private TextMeshPro _defValueText;
    [SerializeField] private TextMeshPro _nbChildrenText;

    [SerializeField] private GameObject Background;
    [SerializeField] private GameObject CardSubject;

    private void Start()
    {
        _card = GetComponent<Card>();

        Background.GetComponent<MeshRenderer>().material = Data.BackgroundMaterial;
        CardSubject.GetComponent<MeshRenderer>().material = Data.SubjectMaterial;

        _defValueText.text = Data.DefValue.ToString();
        _goldValueText.text = Data.GoldValue.ToString();
    }

    private void Update()
    {
        _nbChildrenText.text = _card.Children.Count.ToString();
    }
}
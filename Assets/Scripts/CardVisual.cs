using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CardVisual : MonoBehaviour
{
    private Card _card;

    public ElementData Data;

    [SerializeField] private TextMeshPro _goldValueText;
    [SerializeField] private TextMeshPro _defValueText;
    [SerializeField] private TextMeshPro _nbChildrenText;
    
    [SerializeField] private GameObject CardBody;

    private void Start()
    {
        _card = GetComponent<Card>();
        
        CardBody.GetComponentInChildren<MeshRenderer>().material = Data.CardMaterial;

        _defValueText.text = Data.DefValue.ToString();
        _goldValueText.text = Data.GoldValue.ToString();

        gameObject.name = Data.name;
    }

    private void Update()
    {
        _nbChildrenText.text = _card.Children.Count.ToString();
        
        if (_card.Children.Count != 0)
        {
            _goldValueText.gameObject.SetActive(false);
            _defValueText.gameObject.SetActive(false);

            int sortingPriority = _card.Children.Count;
            ChangeSortingPriority(sortingPriority);
        }
        else
        {
            _goldValueText.gameObject.SetActive(true);
            _defValueText.gameObject.SetActive(true);
            
            ChangeSortingPriority(0);
        }
    }
    
    [ContextMenu("ApplySortingPriority")]
    private void ChangeSortingPriority(int sortingPriority)
    {
        CardBody.GetComponentInChildren<MeshRenderer>().material.renderQueue = 3000 - sortingPriority;
        CardBody.GetComponentInChildren<MeshRenderer>().material.SetFloat("_TransparentSortPriority", sortingPriority);
    }
}
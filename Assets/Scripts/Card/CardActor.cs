using System.Collections.Generic;
using UnityEngine;

public class CardActor : MonoBehaviour
{
    private Card _card;
    private CardVisual _cardVisual;

    [HideInInspector] public List<string> childrenNames = new List<string>();

    private List<ElementsRequieresForCraft> Elements = new List<ElementsRequieresForCraft>();

    private int nbOfChildren = 0;

    [SerializeField] private float currentTimer = 0;
    [SerializeField] private float maxTimer = 0;
    [SerializeField] private bool IsCraftingCard;
    private ElementData elementToCraft;

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
            //Debug.Log($"{index} : " + element.ElementToCraft);
        }
        //EndDebug
    }

    private void Update()
    {
        if (_card.Children.Count != 0)
        {
            if (_card.Children.Count != nbOfChildren)
            {
                nbOfChildren = _card.Children.Count;
                ResetCrafting();
                StartCheckElements();
            }
        }
        else if (_card.Children.Count == 0)
        {
            nbOfChildren = 0;
        }

        if (IsCraftingCard)
        {
            if (_card.Children.Count == 0 || nbOfChildren != _card.Children.Count)
            {
                ResetCrafting();
            }

            currentTimer += Time.deltaTime;

            _cardVisual.CraftingBar.fillAmount = currentTimer / maxTimer;

            if (currentTimer >= maxTimer)
            {
                ResetCrafting();

                foreach (var child in _card.Children)
                {
                    if (child.GetComponent<CardVisual>().Data.name != "Humain")
                    {
                        Destroy(child);
                    }
                }

                _card.Children.Clear();

                if (IsCraftingCard)
                    CreatNewCard(elementToCraft);
            }
        }
    }

    private void ResetCrafting()
    {
        IsCraftingCard = false;
        currentTimer = 0;
        maxTimer = 0;
        elementToCraft = null;

        _cardVisual.CraftingBar.gameObject.SetActive(false);
    }

    private void StartCheckElements()
    {
        childrenNames.Clear();

        foreach (var child in _card.Children)
        {
            childrenNames.Add(child.name);
        }

        CheckElements();
    }

    private void CheckElements()
    {
        foreach (var element in Elements)
        {
            print($"Check Element : {element.ElementToCraft}");

            int nbElementRequiresPresent = 0;

            for (int i = 0; i < element.ElementsRequieres.Count; i++)
            {
                if (!childrenNames.Contains(element.ElementsRequieres[i].name))
                {
                    print($"{element.ElementsRequieres[i].name} ne fait pas parti des enfants de {_card.name}");
                    break;
                }

                print($"Element présent");
                nbElementRequiresPresent++;
            }

            //Check if count of elements are equal to the elements requires list
            if (nbElementRequiresPresent == element.ElementsRequieres.Count && element.ElementsRequieres.Count == _card.Children.Count && IsCraftingCard == false)
            {
                print($"Tous les élements sont prêt pour créer {element.ElementToCraft}");
                StartNewCardCreation(element);

                break;
            }
        }
    }

    private void StartNewCardCreation(ElementsRequieresForCraft element)
    {
        print($"Commencement de la création de {element.ElementToCraft.name}");

        IsCraftingCard = true;
        maxTimer = element.TimeToCraft;
        elementToCraft = element.ElementToCraft;

        StartCounterCraft(element);
    }

    private void StartCounterCraft(ElementsRequieresForCraft element)
    {
        _cardVisual.CraftingBar.gameObject.SetActive(true);
    }

    private void CreatNewCard(ElementData element)
    {
        GameObject currentCard =
            Instantiate(GameManager.Instance.CardPrefab, transform.position, Quaternion.Euler(0, 180, 0));

        CardVisual currentCardVisual = currentCard.GetComponent<CardVisual>();

        currentCardVisual.Data = element;

        print($"Element {element} créé !");
    }
}
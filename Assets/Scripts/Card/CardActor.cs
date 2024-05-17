using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

        GameManager.Instance.CardInGame.Add(gameObject);

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

        StartCoroutine(Appear());
    }

    IEnumerator Appear()
    {
        System.Random rnd = new System.Random();
        int a = rnd.Next(-3, 4);

        Vector3 positionToReach = transform.position + new Vector3(a, 0, 1);
        transform.DOJump(positionToReach, 1, 3, 1);
        yield return new WaitForSeconds(1);
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
                foreach (var child in _card.Children)
                {
                    if (child.GetComponent<CardVisual>().Data.Type != ElementType.Humain)
                    {
                        GameManager.Instance.CardInGame.Remove(child);
                        Destroy(child);
                    }
                    else
                    {
                        child.GetComponent<Card>().Children.Clear();

                        child.GetComponent<Card>().enabled = true;
                        child.GetComponent<CardVisual>().enabled = true;
                        child.GetComponent<CardActor>().enabled = true;

                        child.transform.parent = null;
                        child.GetComponent<Card>().Parent = null;
                    }
                }

                _card.Children.Clear();

                if (IsCraftingCard)
                    CreatNewCard(elementToCraft);

                ResetCrafting();
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
            if (nbElementRequiresPresent == element.ElementsRequieres.Count &&
                element.ElementsRequieres.Count == _card.Children.Count && IsCraftingCard == false)
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

        _cardVisual.CraftingBar.gameObject.SetActive(true);
    }

    private void CreatNewCard(ElementData element)
    {
        GameObject currentCard =
            Instantiate(GameManager.Instance.CardPrefab, transform.position, Quaternion.Euler(0, 180, 0));

        CardVisual currentCardVisual = currentCard.GetComponent<CardVisual>();

        if (_cardVisual.Data.Type == ElementType.Foret)
        {
            int rnd = Random.Range(0, 2);
            ElementData elementName;
            if (rnd == 0)
                elementName = GameManager.Instance.ElementCraftable.Find(x => x.name == "Bois");
            else
                elementName = GameManager.Instance.ElementCraftable.Find(x => x.name == "Fil");

            currentCardVisual.Data = elementName;
        }
        else
        {
            currentCardVisual.Data = element;
        }

        print($"Element {element} créé !");
    }
}
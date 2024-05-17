using System;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool IsStaticCard;
    public GameObject Parent;
    public List<GameObject> Children = new List<GameObject>();

    private RaycastHit[] _hits = new RaycastHit[16];

    private bool IsParentable;

    [SerializeField] private GameObject NbChildrenGroup;

    private void OnMouseOver()
    {
        if (GameManager.Instance.PlayerController.CardSelected == null && Children.Count == 0 && Parent == null)
        {
            GameManager.Instance.PlayerController.CardOvered = gameObject;
        }
    }

    private void OnMouseExit()
    {
        if (GameManager.Instance.PlayerController.CardOvered == gameObject)
        {
            GameManager.Instance.PlayerController.CardOvered = null;
        }
    }

    private void OnMouseDown()
    {
        GameManager.Instance.PlayerController.CardSelected = gameObject;

        RemoveFromParentsChildren();

        transform.parent = null;
        Parent = null;
    }

    private void OnMouseUp()
    {
        GameManager.Instance.PlayerController.CardSelected = null;

        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

        int hits = Physics.RaycastNonAlloc(r, _hits);
        for (int i = 0; i < hits; i++)
        {
            if (_hits[i].collider.TryGetComponent(out Card card))
            {
                if (_hits[i].transform.gameObject != gameObject &&
                    Children.Contains(_hits[i].transform.gameObject) == false &&
                    _hits[i].transform.gameObject.GetComponent<Card>().IsParentable && IsStaticCard == false)
                {
                    transform.parent = _hits[i].transform;
                    Parent = _hits[i].transform.gameObject;

                    // Ajoute la carte en tant qu'enfant de la carte sur laquelle elle est posée
                    _hits[i].transform.gameObject.GetComponent<Card>().AddChild(gameObject);

                    // Si la carte a des enfants, les ajoute également à la carte sur laquelle elle est posée
                    foreach (var child in Children)
                    {
                        _hits[i].transform.gameObject.GetComponent<Card>().AddChild(child);
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (Parent != null)
        {
            transform.position = new Vector3(Parent.transform.position.x, Parent.transform.position.y + 0.05f,
                Parent.transform.position.z - 0.5f);

            NbChildrenGroup.SetActive(false);
            
            if (GameManager.Instance.PlayerController.CardOvered == gameObject)
            {
                GameManager.Instance.PlayerController.CardOvered = null;
            }
        }

        if (Parent == null)
        {
            transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);

            NbChildrenGroup.SetActive(true);
        }

        IsParentable = Children.Count == 0;
    }

    public void AddChild(GameObject child)
    {
        Children.Add(child);

        // Ajoute le nouvel enfant à tous les parents de cette carte
        Card parentCard = this;
        while (parentCard.Parent != null)
        {
            parentCard = parentCard.Parent.GetComponent<Card>();
            parentCard.Children.Add(child);
        }
    }

    private void RemoveFromParentsChildren()
    {
        // Retire la carte sélectionnée de la liste d'enfants de ses parents de manière récursive
        Card parentCard = this;
        while (parentCard.Parent != null)
        {
            parentCard = parentCard.Parent.GetComponent<Card>();
            parentCard.Children.Remove(gameObject);
        }

        // Retire également tous les enfants de la carte sélectionnée de la liste d'enfants de ses parents de manière récursive
        RemoveChildrenFromParents(gameObject);
    }

    private void RemoveChildrenFromParents(GameObject child)
    {
        Card parentCard = this;
        while (parentCard.Parent != null)
        {
            parentCard = parentCard.Parent.GetComponent<Card>();

            foreach (var childrenOfChild in child.GetComponent<Card>().Children)
            {
                parentCard.Children.Remove(childrenOfChild);
            }
        }
    }

    private void AddChildrenToParent(List<GameObject> children)
    {
        List<GameObject> childrenToAdd = new List<GameObject>(children);

        if (Parent != null)
        {
            Card parentCard = Parent.GetComponent<Card>();

            if (parentCard.Children.Contains(gameObject) == false)
                parentCard.Children.Add(gameObject);

            foreach (var child in childrenToAdd)
            {
                if (parentCard.Children.Contains(child) == false)
                    parentCard.Children.Add(child);
            }

            parentCard.AddChildrenToParent(childrenToAdd);
        }
    }

    private void RemoveChildrenFromParent(List<GameObject> children)
    {
        List<GameObject> childrenToRemove = new List<GameObject>(children);

        if (Parent != null)
        {
            print("delete to parent");
            Card parentCard = Parent.GetComponent<Card>();

            if (parentCard.Children.Contains(gameObject))
                parentCard.Children.Remove(gameObject);

            foreach (var child in childrenToRemove)
            {
                if (parentCard.Children.Contains(child))
                    parentCard.Children.Remove(child);
            }

            parentCard.RemoveChildrenFromParent(childrenToRemove);
        }
    }
}
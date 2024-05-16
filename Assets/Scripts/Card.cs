using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool IsStaticCard;
    public GameObject Parent;
    public List<GameObject> Children = new List<GameObject>();

    private RaycastHit[] _hits = new RaycastHit[16];

    private bool IsParentable;

    private void OnMouseDown()
    {
        GameManager.Instance.PlayerController.CardSelected = gameObject;

        RemoveChildrenFromParent(Children);
        
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

                    AddChildrenToParent(Children);
                }
            }
        }
    }

    private void Update()
    {
        if (Parent != null)
        {
            transform.position = new Vector3(Parent.transform.position.x, Parent.transform.position.y + 0.02f,
                Parent.transform.position.z - 0.5f);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
        }

        IsParentable = Children.Count == 0;
    }

    private void AddChildrenToParent(List<GameObject> children)
    {
        if (Parent != null)
        {
            Card parentCard = Parent.GetComponent<Card>();
            
            if (parentCard.Children.Contains(gameObject) == false)
                parentCard.Children.Add(gameObject);

            foreach (var child in Children)
            {
                if (parentCard.Children.Contains(child) == false)
                    parentCard.Children.Add(child);
            }

            parentCard.AddChildrenToParent(children);
        }
    }

    private void RemoveChildrenFromParent(List<GameObject> children)
    {
        if (Parent != null)
        {
            Card parentCard = Parent.GetComponent<Card>();

            if (parentCard.Children.Contains(gameObject))
                parentCard.Children.Remove(gameObject);
            
            foreach (var child in children)
            {
                if (parentCard.Children.Contains(child))
                    parentCard.Children.Remove(child);
            }

            parentCard.RemoveChildrenFromParent(children);
        }
    }
}
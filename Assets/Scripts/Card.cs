using System;
using UnityEngine;

public class Card : MonoBehaviour
{
    public GameObject Parent;
    private RaycastHit[] _hits = new RaycastHit[16];

    private void OnMouseDown()
    {
        GameManager.Instance.PlayerController.CardSelected = gameObject;
        Parent = null;
        transform.parent = null;
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
                transform.parent = _hits[i].transform;
                Parent = _hits[i].transform.gameObject;
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
    }
}
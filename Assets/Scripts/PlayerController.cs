using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject CardSelected;

    public GameObject CardOvered;
    
    private RaycastHit[] _hits = new RaycastHit[16];

    private void Update()
    {
        if (CardSelected != null && GameManager.Instance.IsDayTime)
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

            int hits = Physics.RaycastNonAlloc(r, _hits);
            for (int i = 0; i < hits; i++)
            {
                if (_hits[i].collider.TryGetComponent(out Card card))
                {
                    continue;
                }

                Vector3 expectedPosition = new Vector3(_hits[i].point.x, _hits[i].point.y + 0.15f, _hits[i].point.z);
                CardSelected.transform.localPosition = expectedPosition;
            }
        }
    }
}
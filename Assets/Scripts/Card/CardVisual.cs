using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class CardVisual : MonoBehaviour
{
    private Card _card;

    public ElementData Data;

    [SerializeField] private TextMeshPro _goldValueText;
    [SerializeField] private TextMeshPro _defValueText;
    [SerializeField] private TextMeshPro _nbChildrenText;
    
    [SerializeField] private GameObject CardBody;

    public Image CraftingBar;
    [SerializeField] private Vector3 CraftingBarPosition;
    
    private void Start()
    {
        _card = GetComponent<Card>();
        
        CardBody.GetComponentInChildren<MeshRenderer>().material = Data.CardMaterial;

        _defValueText.text = Data.DefValue.ToString();
        _goldValueText.text = Data.GoldValue.ToString();

        gameObject.name = Data.name;
        
        SetupCraftBar(GameManager.Instance.HealthBarCanvas);
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
        
        CraftingBar.transform.position = transform.position + CraftingBarPosition;
    }

    private void SetupCraftBar(Canvas canvas)
    {
        CraftingBar.transform.SetParent(canvas.transform);
        CraftingBar.gameObject.SetActive(false);
    }
    
    [ContextMenu("ApplySortingPriority")]
    private void ChangeSortingPriority(int sortingPriority)
    {
        CardBody.GetComponentInChildren<MeshRenderer>().material.renderQueue = 3000 - sortingPriority;
        CardBody.GetComponentInChildren<MeshRenderer>().material.SetFloat("_TransparentSortPriority", sortingPriority);
    }
}
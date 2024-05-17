using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerController PlayerController;

    public Canvas HealthBarCanvas;
    public Camera Camera;

    public GameObject CardPrefab;

    public List<ElementData> ElementCraftable;

    public List<GameObject> CardInGame = new List<GameObject>();
    
    public int DefenseValue;
    public TextMeshProUGUI DefText;

    public int GoldValue;
    public TextMeshProUGUI GoldText;
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There already another GameManager in this scene !");
        }
    }

    private void Update()
    {
        DefenseValue = 0;
        
        foreach (var card in CardInGame)
        {
            DefenseValue += card.GetComponent<CardVisual>().Data.DefValue;
        }
        
        DefText.text = DefenseValue.ToString();

        GoldText.text = GoldValue.ToString();
    }
}
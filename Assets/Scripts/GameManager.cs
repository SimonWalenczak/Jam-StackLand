using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public TextMeshProUGUI AttackText;
    
    public int GoldValue;
    public TextMeshProUGUI GoldText;

    public AttackData Attacks;

    public GameObject BoosterPrefab;
    public BoosterData Boosters;
    public int NbBooster;
    public int BoosterPrice;

    public bool IsDayTime;
    public int DayTimer;
    public float CurrentDayTimer;
    public float NightTimer;

    public Image DayBar;
    public GameObject DayIcon;
    public GameObject BlackScreen;

    public TextMeshProUGUI TextNightResult;
    
    public int nbDay;
    public TextMeshProUGUI DayCountText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There already another GameManager in this scene !");
        }
    }

    private void Start()
    {
        IsDayTime = true;
        nbDay = 0;
    }

    private void Update()
    {
        DefenseValue = 0;

        foreach (var card in CardInGame)
        {
            DefenseValue += card.GetComponent<CardVisual>().Data.DefValue;
        }

        DefText.text = DefenseValue.ToString();

        AttackText.text = Attacks.Attacks[nbDay].ToString();
        
        GoldText.text = GoldValue.ToString();

        DayCountText.text = new string($"Jour {nbDay + 1}");

        DayNight();
    }

    public void BuyBooster()
    {
        if (GoldValue >= BoosterPrice)
        {
            GameObject currentBooster = Instantiate(BoosterPrefab, transform.position, Quaternion.Euler(90, 0, 0));
            currentBooster.GetComponent<BoosterObject>().Booster = Boosters;
            NbBooster++;
            GoldValue -= BoosterPrice;
        }
    }

    private void DayNight()
    {
        if (IsDayTime)
        {
            CurrentDayTimer += Time.deltaTime;

            DayBar.fillAmount = CurrentDayTimer / DayTimer;

            if (CurrentDayTimer >= DayTimer)
            {
                StartCoroutine(CheckNightAttack());
            }
        }
    }

    public IEnumerator CheckNightAttack()
    {
        IsDayTime = false;
        
        DayIcon.GetComponent<Animator>().SetBool("IsDay", false);

        BlackScreen.SetActive(true);

        string text = "";
        
        if (DefenseValue >= Attacks.Attacks[nbDay])
        {
            text = new string($"Vous avez gagné car il vous avez force défensive supérieure de {DefenseValue - Attacks.Attacks[nbDay]} !");

        }
        else
        {
            text = new string($"Vous avez perdu car il vous a manqué {Attacks.Attacks[nbDay] - DefenseValue} de force défensive !");
        }
        
        TextNightResult.text = text;

        yield return new WaitForSeconds(NightTimer);

        if (DefenseValue < Attacks.Attacks[nbDay])
        {
            SceneManager.LoadScene(0);
        }

        BlackScreen.GetComponent<Animator>().SetBool("IsDay", true);

        DayIcon.GetComponent<Animator>().SetBool("IsDay", true);

        BlackScreen.SetActive(false);
        
        CurrentDayTimer = 0;

        IsDayTime = true;

        nbDay++;
    }

    public void EndDay()
    {
        CurrentDayTimer = DayTimer - 0.5f;
    }
}
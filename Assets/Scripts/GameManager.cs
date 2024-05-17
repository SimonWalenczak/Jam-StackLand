using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    public int GoldValue;
    public TextMeshProUGUI GoldText;

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

        yield return new WaitForSeconds(NightTimer);

        BlackScreen.GetComponent<Animator>().SetBool("IsDay", true);

        DayIcon.GetComponent<Animator>().SetBool("IsDay", true);

        BlackScreen.SetActive(false);
        
        CurrentDayTimer = 0;

        IsDayTime = true;
    }
}
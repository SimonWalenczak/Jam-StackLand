using UnityEngine;
using UnityEngine.UI;

public class DestroyOnRightClick : MonoBehaviour
{
    private bool isRightClickHeld = false;
    private float rightClickHoldTime = 0f;
    public float requiredHoldTime = 2f; // Temps en secondes pour maintenir le clic droit

    public GameObject CardToDestroy;
    private Image deleteRoue;

    void Update()
    {
        // Vérifier si le clic droit est enfoncé
        if (GameManager.Instance.PlayerController.CardOvered != null &&
            GameManager.Instance.PlayerController.CardSelected != GameManager.Instance.PlayerController.CardOvered)
        {
            if (Input.GetMouseButtonDown(1))
            {
                // Vérifier si le curseur de la souris est sur l'objet
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    CardToDestroy = GameManager.Instance.PlayerController.CardOvered;
                    isRightClickHeld = true;
                    rightClickHoldTime = 0f; // Réinitialiser le temps de maintien

                    deleteRoue = CardToDestroy.GetComponent<CardVisual>().DeleteRoue;
                    deleteRoue.fillAmount = 0;
                    deleteRoue.gameObject.SetActive(true);
                }
            }
        }

        // Si le clic droit est relâché
        if (Input.GetMouseButtonUp(1))
        {
            isRightClickHeld = false;
            
            ResetCardToDestroy();
        }

        // Si le clic droit est maintenu
        if (isRightClickHeld)
        {
            rightClickHoldTime += Time.deltaTime;

            deleteRoue.fillAmount = rightClickHoldTime / requiredHoldTime;

            if (rightClickHoldTime >= requiredHoldTime)
            {
                Destroy(CardToDestroy.GetComponent<CardVisual>().DeleteRoue);

                GameManager.Instance.GoldValue += CardToDestroy.GetComponent<CardVisual>().Data.GoldValue;
                GameManager.Instance.CardInGame.Remove(CardToDestroy);

                Destroy(GameManager.Instance.PlayerController.CardOvered);
                
                CardToDestroy = null;

                deleteRoue.fillAmount = 0;
                deleteRoue.gameObject.SetActive(false);
                
                isRightClickHeld = false;
            }
        }
    }

    private void ResetCardToDestroy()
    {
        if (CardToDestroy != null)
        {
            CardToDestroy = null;

            deleteRoue.fillAmount = 0;
            deleteRoue.gameObject.SetActive(false);
        }
    }
}
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerController PlayerController;

    public Canvas HealthBarCanvas;
    public Camera Camera;

    public GameObject CardPrefab;
    
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
}

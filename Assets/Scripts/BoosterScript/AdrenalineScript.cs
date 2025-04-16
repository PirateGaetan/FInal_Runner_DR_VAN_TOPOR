using UnityEngine;

public class AdrenalineScript : MonoBehaviour
{
    private GameManager gameManager; 
    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.TriggerAdrenaline();
            Destroy(gameObject);
        }
    }
}


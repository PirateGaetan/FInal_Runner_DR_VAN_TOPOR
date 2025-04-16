using UnityEngine;

public class YellowCataScript : MonoBehaviour
{
    [SerializeField] private AudioClip hitSFX;

    private GameManager gameManager; 

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControler player = other.GetComponent<PlayerControler>();
            player.RemoveYellowSerum(gameManager.dammageYellowCatalyser);

            gameManager.TriggerSpeedReduction(gameManager.CataColisionDuration, gameManager.CataColisionSpeedReduction);
            
            AudioSource playerAudio = other.GetComponent<AudioSource>();
            playerAudio.PlayOneShot(hitSFX);
            
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class yellowPillsScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerControler>().AddYellowSerum();
            Destroy(gameObject);
        }
    }
}

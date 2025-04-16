using UnityEngine;

public class bluePillsScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerControler>().AddBlueSerum();
            Destroy(gameObject);
        }
    }
}

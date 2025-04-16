using Unity.VisualScripting;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var chunk = other.gameObject.gameObject.transform.parent.parent;
        Destroy(chunk.gameObject);
        
    }
}

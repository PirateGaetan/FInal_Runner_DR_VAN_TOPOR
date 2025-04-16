using UnityEngine;

public class Scroller : MonoBehaviour
{
    private float speed;
    void Update()
    {
        speed = GameObject.Find("GameManager").GetComponent<GameManager>().GetCurrentChunkSpeed();
        foreach(Transform child in transform)
        {
            child.position += Vector3.back * Time.deltaTime * speed;
        }
    }
}

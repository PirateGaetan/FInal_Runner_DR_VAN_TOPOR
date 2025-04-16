using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private GameManager gameManager;

    [Header("Chunks Level 1")]
    [SerializeField] private List<GameObject> chunkPrefabsLevel1;
    [SerializeField] private List<int> chunkWeightsLevel1;

    [Header("Chunks Level 2")]
    [SerializeField] private List<GameObject> chunkPrefabsLevel2;
    [SerializeField] private List<int> chunkWeightsLevel2;

    [Header("Chunks Level 3")]
    [SerializeField] private List<GameObject> chunkPrefabsLevel3;
    [SerializeField] private List<int> chunkWeightsLevel3;

    [SerializeField] private Transform container;

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("ChunkTag"))
        {
            var chunk = other.transform.parent.parent;
            var end = chunk.Find("End");

            var newChunk = Instantiate(GetRandomChunk(), end.position, end.rotation);
            newChunk.transform.SetParent(container);
        }
    }

    private GameObject GetRandomChunk()
    {
        List<GameObject> chunkPrefabs = chunkPrefabsLevel1;
        List<int> chunkWeights = chunkWeightsLevel1;

        switch (gameManager.GetCurrentLevel())
        {
            case GameManager.GameLevel.Level2:
                chunkPrefabs = chunkPrefabsLevel2;
                chunkWeights = chunkWeightsLevel2;
                break;

            case GameManager.GameLevel.Level3:
                chunkPrefabs = chunkPrefabsLevel3;
                chunkWeights = chunkWeightsLevel3;
                break;
        }

        int totalWeight = 0;
        foreach (int weight in chunkWeights)
            totalWeight += weight;

        int randomValue = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        for (int i = 0; i < chunkPrefabs.Count; i++)
        {
            cumulativeWeight += chunkWeights[i];
            if (randomValue < cumulativeWeight)
                return chunkPrefabs[i];
        }

        return chunkPrefabs[0]; // fallback
    }
}
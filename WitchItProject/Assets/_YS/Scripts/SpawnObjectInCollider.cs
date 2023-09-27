using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectInCollider : MonoBehaviour
{
    public string folderPath = "Kingdom";
    private GameObject[] prefabs;
    private BoxCollider spawnArea;

    void Start()
    {
        spawnArea = GetComponent<BoxCollider>();
        prefabs = Resources.LoadAll<GameObject>(folderPath);
        int randomCount = Random.Range(35, 51);

        for (int i = 0; i < randomCount; i++)
        {
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        Vector3 minSpawner = spawnArea.bounds.min;
        Vector3 maxSpawner = spawnArea.bounds.max;

        Vector3 randomPosition = new Vector3(
            Random.Range(minSpawner.x, maxSpawner.x),
            Random.Range(minSpawner.y, maxSpawner.y),
            Random.Range(minSpawner.z, maxSpawner.z)
        );

        GameObject randomPrefab = prefabs[Random.Range(0, prefabs.Length)];
        Instantiate(randomPrefab, randomPosition, Quaternion.identity);
    }
}

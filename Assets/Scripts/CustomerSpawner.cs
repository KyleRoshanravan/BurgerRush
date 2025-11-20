using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject[] customerPrefabs; // Assign prefabs in inspector
    public Transform[] spawnPoints;      // Points in scene where customers appear
    public float spawnInterval = 3f;     // Time between spawns

    private void Start()
    {
        InvokeRepeating("SpawnCustomer", 1f, spawnInterval);
    }

    private void SpawnCustomer()
    {
        int prefabIndex = Random.Range(0, customerPrefabs.Length);
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        Instantiate(customerPrefabs[prefabIndex], spawnPoints[spawnIndex].position, Quaternion.identity);
    }
}

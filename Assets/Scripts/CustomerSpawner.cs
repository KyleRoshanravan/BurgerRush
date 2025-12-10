using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject[] customerPrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 5f;

    public Transform counterPoint;         // Scene CounterPoint
    public TicketSpawner ticketSpawner;    // Reference to TicketSpawner
    public GameObject ticketPrefab;        // Ticket prefab
    public Sprite burgerSprite;            // Optional burger icon

    private void Start()
    {
        InvokeRepeating("SpawnCustomer", 1f, spawnInterval);
    }

    private void SpawnCustomer()
    {
        int prefabIndex = Random.Range(0, customerPrefabs.Length);
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        // Instantiate customer prefab at spawn point
        GameObject newCustomer = Instantiate(customerPrefabs[prefabIndex], spawnPoints[spawnIndex].position, Quaternion.identity);

        // Assign runtime references (scene objects) to the customer AI
        CustomerAI ai = newCustomer.GetComponent<CustomerAI>();
        ai.counterPoint = counterPoint;
        ai.ticketSpawner = ticketSpawner;
        ai.ticketPrefab = ticketPrefab;
        ai.burgerSprite = burgerSprite;
    }
}

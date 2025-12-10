using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerAI : MonoBehaviour
{
    public Transform counterPoint;
    public float waitTime = 40f;
    public TicketSpawner ticketSpawner;
    public GameObject ticketPrefab;
    public Sprite burgerSprite;

    private NavMeshAgent agent;
    private TicketUI myTicket;
    private bool orderCompleted = false;

    private static string[] firstNames = { "Alex", "Sam", "Taylor", "Jordan", "Jamie", "Casey", "Morgan", "Riley" };
    private static string[] lastNames = { "Smith", "Johnson", "Brown", "Davis", "Miller", "Wilson" };

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(counterPoint.position);

        StartCoroutine(CustomerRoutine());
    }

    IEnumerator CustomerRoutine()
    {
        while (Vector3.Distance(transform.position, counterPoint.position) > 0.5f)
            yield return null;

        // Spawn ticket above customer
        if (myTicket == null && ticketSpawner.ticketManager.ActiveTicketCount < ticketSpawner.maxTickets)
        {
            string customerName = GetRandomCustomerName();
            ticketSpawner.SpawnTicketForCustomer(customerName, transform);
            myTicket = transform.GetComponentInChildren<TicketUI>();
        }

        float timer = waitTime;
        while (timer > 0 && !orderCompleted)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        if (!orderCompleted && myTicket != null)
        {
            ticketSpawner.ticketManager.ExpireTicket(myTicket);
            Leave();
        }
    }

    public void CompleteOrder(List<string> playerIngredients)
    {
        if (myTicket != null && !orderCompleted)
        {
            ticketSpawner.ticketManager.CompleteOrder(myTicket, playerIngredients, burgerSprite);
            orderCompleted = true;
            Leave();
        }
    }

    private void Leave()
    {
        Destroy(gameObject, 1f);
    }

    private string GetRandomCustomerName()
    {
        string first = firstNames[Random.Range(0, firstNames.Length)];
        string last = lastNames[Random.Range(0, lastNames.Length)];
        return first + " " + last;
    }
}

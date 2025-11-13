using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TicketManager : MonoBehaviour
{
    [Header("Prefabs & References")]
    public GameObject ticketPrefab;      // Your Ticket UI prefab
    public Transform ticketParent;       // Container for spawned tickets

    [Header("Ingredient Prefabs")]
    public GameObject topBunPrefab;
    public GameObject bottomBunPrefab;
    public GameObject pattyPrefab;
    public List<GameObject> extraIngredients; // lettuce, tomato, cheese, etc.

    [Header("Ticket Settings")]
    public int paymentAmount = 25;
    public float ticketSpawnInterval = 5f;   // seconds between tickets
    public int minExtraIngredients = 0;
    public int maxExtraIngredients = 3;

    [Header("Customer Names")]
    public List<string> customerNames = new List<string> { "John", "Mary", "Bob", "Alice", "Tom", "Sue" };

    private void Start()
    {
        // Start spawning tickets repeatedly
        StartCoroutine(SpawnTicketsRoutine());
    }

    private IEnumerator SpawnTicketsRoutine()
    {
        while (true)
        {
            SpawnTicket();
            yield return new WaitForSeconds(ticketSpawnInterval);
        }
    }

    public void SpawnTicket()
    {
        // Pick a random customer name
        string customerName = customerNames[Random.Range(0, customerNames.Count)];

        // Build the ingredient list: mandatory + random extras
        List<GameObject> orderIngredients = new List<GameObject>
        {
            bottomBunPrefab,
            pattyPrefab,
            topBunPrefab
        };

        int extraCount = Random.Range(minExtraIngredients, maxExtraIngredients + 1);
        for (int i = 0; i < extraCount; i++)
        {
            GameObject extra = extraIngredients[Random.Range(0, extraIngredients.Count)];
            orderIngredients.Insert(1, extra); // Insert between bottom bun and patty
        }

        // Instantiate a new ticket
        GameObject ticketGO = Instantiate(ticketPrefab, ticketParent);
        TicketUI ticketUI = ticketGO.GetComponent<TicketUI>();
        ticketUI.SetupTicket(customerName, orderIngredients, 60f, paymentAmount);

        // Start tracking expiration
        StartCoroutine(HandleTicketExpiration(ticketUI));
    }

    private IEnumerator HandleTicketExpiration(TicketUI ticket)
    {
        while (ticket != null && !ticket.isExpired)
        {
            yield return null;
        }

        if (ticket != null)
        {
            MoneyManager.Instance.LoseMoney(paymentAmount);
            Destroy(ticket.gameObject);
        }
    }

    // Call this when the player serves the order correctly
    public void CompleteOrder(TicketUI ticket)
    {
        MoneyManager.Instance.AddMoney(paymentAmount);
        Destroy(ticket.gameObject);
    }
}

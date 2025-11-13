using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TicketManager : MonoBehaviour
{
    [Header("Prefabs & References")]
    public GameObject ticketPrefab;           // Ticket prefab
    public Transform ticketParent;            // UI container for tickets
    public List<GameObject> extraIngredients; // Ingredients that are optional/extras
    public GameObject topBunPrefab;
    public GameObject bottomBunPrefab;
    public GameObject pattyPrefab;

    [Header("Ticket Settings")]
    public int paymentAmount = 25;
    public float ticketSpawnInterval = 10f;  // Seconds between tickets
    public int minExtraIngredients = 0;
    public int maxExtraIngredients = 3;

    [Header("Customer Names")]
    public List<string> customerNames = new List<string>() { "John", "Mary", "Bob", "Alice", "Tom", "Sue" };

    private void Start()
    {
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
        // Random customer name
        string customerName = customerNames[Random.Range(0, customerNames.Count)];

        // Order list with required ingredients
        List<GameObject> orderIngredients = new List<GameObject>
        {
            bottomBunPrefab,
            pattyPrefab,
            topBunPrefab
        };

        // Random extra ingredients inserted between bottom bun and patty
        int extraCount = Random.Range(minExtraIngredients, maxExtraIngredients + 1);
        for (int i = 0; i < extraCount; i++)
        {
            GameObject extra = extraIngredients[Random.Range(0, extraIngredients.Count)];
            orderIngredients.Insert(1, extra);
        }

        // Spawn ticket prefab
        GameObject ticketGO = Instantiate(ticketPrefab, ticketParent);
        TicketUI ticketUI = ticketGO.GetComponent<TicketUI>();
        ticketUI.SetupTicket(customerName, orderIngredients, 60f, paymentAmount);

        // Handle expiration
        StartCoroutine(HandleTicketExpiration(ticketUI));
    }

    private IEnumerator HandleTicketExpiration(TicketUI ticket)
    {
        while (ticket != null && !ticket.isExpired)
            yield return null;

        if (ticket != null)
        {
            MoneyManager.Instance.LoseMoney(paymentAmount);
            Destroy(ticket.gameObject);
        }
    }

    public void CompleteOrder(TicketUI ticket)
    {
        MoneyManager.Instance.AddMoney(paymentAmount);
        Destroy(ticket.gameObject);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class TicketSpawner : MonoBehaviour
{
    public TicketManager ticketManager;
    public GameObject ticketPrefab;   // World-space ticket prefab
    public Sprite burgerSprite;       // Icon for ticket
    public int maxTickets = 3;

    public void SpawnTicketForCustomer(string customerName, Transform customerTransform)
    {
        if (ticketManager == null) return;
        if (ticketManager.ActiveTicketCount >= maxTickets) return;

        GameObject ticketObj = Instantiate(ticketPrefab, customerTransform.position + Vector3.up * 2f, Quaternion.identity);
        ticketObj.transform.SetParent(customerTransform); // follow customer
        TicketUI ticketUI = ticketObj.GetComponent<TicketUI>();

        // Random order
        List<string> ingredients = new List<string> { "Top Bun", "Patty", "Bottom Bun" };
        string[] extras = { "Cheese", "Lettuce", "Tomato", "Pickle", "Onion" };
        int extraCount = Random.Range(0, extras.Length + 1);
        List<string> chosenExtras = new List<string>();
        while (chosenExtras.Count < extraCount)
        {
            string pick = extras[Random.Range(0, extras.Length)];
            if (!chosenExtras.Contains(pick))
                chosenExtras.Add(pick);
        }
        ingredients.AddRange(chosenExtras);

        float timeLimit = Random.Range(20f, 40f);
        ticketUI.SetupTicket(customerName, ingredients, timeLimit, 15, burgerSprite);

        ticketManager.RegisterTicket(ticketUI);
    }
}

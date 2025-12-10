using System.Collections.Generic;
using UnityEngine;

public class TicketSpawner : MonoBehaviour
{
    public TicketManager ticketManager;
    public GameObject ticketPrefab;
    public Sprite burgerSprite;
    public int maxTickets = 3;

    // ★ Ingredient price table
    private Dictionary<string, float> ingredientPrices = new Dictionary<string, float>
    {
        { "Top Bun", 0.15f },
        { "Bottom Bun", 0.15f },
        { "Patty", 0.60f },
        { "Cheese", 0.30f },
        { "Lettuce", 0.15f },
        { "Tomato", 0.20f },
        { "Pickle", 0.10f },
        { "Onion", 0.20f }
    };

    // ★ Calculate ingredient cost
    private float CalculateBurgerCost(List<string> ingredients)
    {
        float total = 0f;

        foreach (string ing in ingredients)
        {
            if (ingredientPrices.TryGetValue(ing, out float cost))
                total += cost;
        }

        return total;
    }

    public void SpawnTicketForCustomer(string customerName, Transform customerTransform)
    {
    if (ticketManager == null) return;
    if (ticketManager.ActiveTicketCount >= maxTickets) return;

    // -------- FIXED TICKET SPAWN HERE --------
    GameObject ticketObj = Instantiate(ticketPrefab);
    ticketObj.transform.SetParent(customerTransform);
    ticketObj.transform.localPosition = new Vector3(0, 2f, 0);
    ticketObj.transform.localRotation = Quaternion.identity;
    // ------------------------------------------

    TicketUI ticketUI = ticketObj.GetComponent<TicketUI>();

    // Fixed ingredients
    List<string> ingredients = new List<string> { "Top Bun", "Patty", "Bottom Bun" };

    // Random extras
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

    float ingredientCost = CalculateBurgerCost(ingredients);
    int payout = Mathf.RoundToInt(ingredientCost * 4f);
    float timeLimit = Random.Range(20f, 40f);

    ticketUI.SetupTicket(customerName, ingredients, timeLimit, payout, burgerSprite);
    ticketManager.RegisterTicket(ticketUI);
    }

}

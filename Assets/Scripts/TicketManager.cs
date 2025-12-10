using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TicketManager : MonoBehaviour
{
    public int maxTickets = 3;
    public TextMeshProUGUI moneyText;

    private List<TicketUI> activeTickets = new List<TicketUI>();
    private int totalMoney = 0;

    // Added property to read current money
    public int TotalMoney => totalMoney;

    // Added method to deduct money
    public void DeductMoney(int amount)
    {
        totalMoney -= amount;
        moneyText.text = "$" + totalMoney;
    }

    public int ActiveTicketCount => activeTickets.Count;

    public void RegisterTicket(TicketUI ticket)
    {
        activeTickets.Add(ticket);
    }

    public void ExpireTicket(TicketUI ticket)
    {
        if (ticket != null)
        {
            totalMoney -= 5; // penalty
            moneyText.text = "$" + totalMoney;
            activeTickets.Remove(ticket);
            Destroy(ticket.gameObject);
        }
    }

    public void CompleteOrder(TicketUI ticket, List<string> playerIngredients, Sprite burgerSprite = null)
    {
        if (ticket == null) return;

        int mistakes = 0;
        foreach (var ing in ticket.ingredients)
            if (!playerIngredients.Contains(ing)) mistakes++;
        foreach (var ing in playerIngredients)
            if (!ticket.ingredients.Contains(ing)) mistakes++;

        int moneyChange = 15 - (mistakes * 5);
        totalMoney += moneyChange;
        moneyText.text = "$" + totalMoney;

        ticket.MarkCompleted();
        activeTickets.Remove(ticket);
        Destroy(ticket.gameObject, 2f); // show for 2 seconds
    }
}

using System.Collections.Generic;
using UnityEngine;

public class TicketManager : MonoBehaviour
{
    public GameObject ticketPrefab;        // Prefab of a single ticket
    public Transform ticketPanelParent;    // Parent container (TicketPanel)

    private List<TicketUI> activeTickets = new List<TicketUI>();

    public void CreateNewTicket(string customerName, List<Sprite> ingredientIcons, float timeLimit, int payment)
    {
        GameObject newTicketObj = Instantiate(ticketPrefab, ticketPanelParent);
        TicketUI ticketUI = newTicketObj.GetComponent<TicketUI>();

        if (ticketUI != null)
        {
            ticketUI.SetupTicket(customerName, ingredientIcons, timeLimit, payment);
            activeTickets.Add(ticketUI);
        }
    }

    void Update()
    {
        // Remove completed/expired tickets automatically
        for (int i = activeTickets.Count - 1; i >= 0; i--)
        {
            if (activeTickets[i].IsExpired)
            {
                Destroy(activeTickets[i].gameObject);
                activeTickets.RemoveAt(i);
            }
        }
    }
}

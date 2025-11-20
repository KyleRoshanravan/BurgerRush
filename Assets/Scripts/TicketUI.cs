using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TicketUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI customerNameText;
    public TextMeshProUGUI timerText;
    public Transform ingredientListParent;
    public GameObject ingredientIconPrefab;
    public TextMeshProUGUI paymentText;

    private float remainingTime;
    private bool timerActive = false;

    public bool IsExpired => remainingTime <= 0f;

    public void SetupTicket(string customerName, List<Sprite> ingredientIcons, float timeLimit, int payment)
    {
        customerNameText.text = customerName;
        paymentText.text = "$" + payment;
        remainingTime = timeLimit;
        timerActive = true;

        // Clear any existing icons
        foreach (Transform child in ingredientListParent)
            Destroy(child.gameObject);

        // Spawn icons bottom-to-top
        foreach (Sprite icon in ingredientIcons)
        {
            GameObject newIcon = Instantiate(ingredientIconPrefab, ingredientListParent);
            newIcon.GetComponent<Image>().sprite = icon;
        }
    }

    void Update()
    {
        if (timerActive && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            timerText.text = $"{remainingTime:F1}s";

            if (remainingTime <= 0)
            {
                timerText.text = "Time's Up!";
                timerActive = false;
            }
        }
    }
}

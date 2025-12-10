using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TicketUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI customerNameText;
    public TextMeshProUGUI orderText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI paymentText;
    public Image burgerIcon; // optional icon for burger

    [HideInInspector] public List<string> ingredients = new List<string>();
    private float remainingTime;
    private bool timerActive = false;

    public bool IsExpired => remainingTime <= 0f;

    public void SetupTicket(string customerName, List<string> ingredientList, float timeLimit, int payment, Sprite burgerSprite = null)
    {
        customerNameText.text = customerName;
        ingredients = ingredientList;
        orderText.text = "Order: " + string.Join(", ", ingredients);
        paymentText.text = "$" + payment;

        remainingTime = timeLimit;
        timerActive = true;
        UpdateTimerText();

        if (burgerIcon != null && burgerSprite != null)
        {
            burgerIcon.sprite = burgerSprite;
            burgerIcon.enabled = true;
        }
    }

    public void MarkCompleted()
    {
        if (burgerIcon != null)
            burgerIcon.color = Color.green; // simple visual feedback
    }

    void Update()
    {
        if (timerActive && remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerText();

            if (remainingTime <= 0f)
            {
                timerText.text = "Time's Up!";
                timerActive = false;
            }
        }
    }

    private void UpdateTimerText()
    {
        timerText.text = $"{remainingTime:F1}s";
    }
}

using UnityEngine;
using TMPro; // If using TextMeshPro

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;

    void OnEnable()
    {
        // Subscribe to MoneyManager event
        if (MoneyManager.Instance != null)
            MoneyManager.Instance.OnMoneyChanged += UpdateMoneyDisplay;
    }

    void OnDisable()
    {
        if (MoneyManager.Instance != null)
            MoneyManager.Instance.OnMoneyChanged -= UpdateMoneyDisplay;
    }

    void Start()
    {
        // Initialize display
        UpdateMoneyDisplay(MoneyManager.Instance.Money);
    }

    void UpdateMoneyDisplay(int currentMoney)
    {
        moneyText.text = "$" + currentMoney.ToString();
    }
}

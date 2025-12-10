using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Game/PlayerData")]
public class PlayerData : MonoBehaviour
{
    public TextMeshPro moneyText;
    public Text ratingText;

    public float money = 25;

    public bool TrySpend(float amount)
    {
        if(money < amount) return false;

        money -= amount;
        moneyText.text = "$" + money.ToString("0.00");
        return true;
    }

    public void AddMoney(float amount)
    {
        money += amount;
        moneyText.text = "$" + money.ToString("0.00");
    }
}
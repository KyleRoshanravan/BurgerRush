using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Game/PlayerData")]
public class PlayerData : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public Text ratingText;

    public float money = 25f;

    [Range(0, 5)]
    public int rating = 3; // Start with 3 stars by default

    private void Start()
    {
        UpdateRatingUI();
        UpdateMoneyUI();
    }

    // ----------------------
    // MONEY METHODS
    // ----------------------
    public bool TrySpend(float amount)
    {
        if (money < amount) return false;

        money -= amount;
        UpdateMoneyUI();
        return true;
    }

    public void AddMoney(float amount)
    {
        money += amount;
        UpdateMoneyUI();
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = "$" + money.ToString("0.00");
    }

    // ----------------------
    // RATING METHODS
    // ----------------------
    public void AddStar()
    {
        rating = Mathf.Clamp(rating + 1, 0, 5);
        UpdateRatingUI();
    }

    public void RemoveStar()
    {
        rating = Mathf.Clamp(rating - 1, 0, 5);
        UpdateRatingUI();
    }

    private void UpdateRatingUI()
    {
        if (ratingText != null)
        {
            string stars = "";
            for (int i = 0; i < 5; i++)
            {
                stars += (i < rating) ? "★" : "☆";
            }
            ratingText.text = stars;
        }
    }

    // ----------------------
    // RANDOM STAR CHANGE
    // ----------------------
    public void RandomStarChange()
    {
        if (Random.value > 0.5f)
            AddStar();
        else
            RemoveStar();
    }
}

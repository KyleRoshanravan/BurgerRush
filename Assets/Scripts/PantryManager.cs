using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PantryManager : MonoBehaviour
{
    [System.Serializable]
    public class Ingredient
    {
        public string name;
        public int price;
        public Button button; // Drag your Canvas Button here
    }

    public List<Ingredient> ingredients = new List<Ingredient>();
    public TicketManager wallet; // Drag your existing TicketManager here

    void Start()
    {
        // Add click listeners for all ingredients
        foreach (var ing in ingredients)
        {
            ing.button.onClick.AddListener(() => BuyIngredient(ing));
        }
    }

    void BuyIngredient(Ingredient ing)
    {
        if (wallet == null) return;

        if (wallet.TotalMoney >= ing.price)
        {
            wallet.DeductMoney(ing.price); // Deduct from wallet
            Debug.Log($"Bought {ing.name} for ${ing.price}. Remaining: ${wallet.TotalMoney}");
        }
        else
        {
            Debug.Log($"Not enough money to buy {ing.name}");
        }
    }
}

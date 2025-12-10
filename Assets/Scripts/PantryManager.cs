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
        
    }
}

[System.Serializable]
public class BurgerData
{
    public string ingredientName;
    public bool isTopBun;
    public bool isBottomBun;

    // Optional: patty state
    public bool isPatty;
    public string pattyState; // e.g., "Raw", "Cooked", "Overcooked"
}
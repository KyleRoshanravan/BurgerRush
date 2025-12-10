using UnityEngine;
using System.Collections.Generic;

public class PlateAssembler : MonoBehaviour
{
    [Header("Plate Settings")]
    public Transform plateCenter;        // Empty child at plate center
    public float stackSpacing = 0.01f;   // Extra space between items

    [Header("Finalization")]
    public GameObject burgerBoxPrefab;   // Prefab to spawn when burger is finalized

    private List<GameObject> stackedIngredients = new List<GameObject>();
    public List<BurgerData> finalizedBurgerData = new List<BurgerData>();

    // ----------------------------------------------------------
    // PLACE INGREDIENT
    // ----------------------------------------------------------
    public void PlaceIngredient(GameObject ingredient)
    {
        if (ingredient == null) return;

        IngredientType type = ingredient.GetComponent<IngredientType>();
        if (type == null) return;

        // Block adding ingredients if top bun exists
        if (HasTopBun() && !type.isTopBun)
            return;

        // Avoid duplicates
        if (stackedIngredients.Contains(ingredient))
        {
            RepositionStack();
            return;
        }

        // Detach from any other plate
        PlateAssembler otherPlate = ingredient.GetComponentInParent<PlateAssembler>();
        if (otherPlate != null && otherPlate != this)
            otherPlate.DetachIngredient(ingredient);

        // Freeze physics
        Rigidbody rb = ingredient.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Parent to this plate
        ingredient.transform.SetParent(transform);

        // Add to stack
        stackedIngredients.Add(ingredient);

        // Reposition the stack
        RepositionStack();
    }

    // ----------------------------------------------------------
    // DETACH INGREDIENT
    // ----------------------------------------------------------
    public void DetachIngredient(GameObject ingredient)
    {
        if (ingredient == null) return;

        stackedIngredients.Remove(ingredient);

        Rigidbody rb = ingredient.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        ingredient.transform.SetParent(null);

        RepositionStack();
    }

    // ----------------------------------------------------------
    // REPOSITION STACK
    // ----------------------------------------------------------
    private void RepositionStack()
    {
        float currentHeight = 0f;
        stackedIngredients.RemoveAll(item => item == null);

        foreach (var item in stackedIngredients)
        {
            IngredientType type = item.GetComponent<IngredientType>();
            Vector3 posOffset = Vector3.zero;

            // Use placementPoint for top or bottom bun if available
            if (type != null && type.placementPoint != null)
            {
                posOffset = item.transform.position - type.placementPoint.position;
            }

            item.transform.position = plateCenter.position + new Vector3(0f, currentHeight, 0f) + posOffset;
            item.transform.rotation = Quaternion.identity;

            float itemHeight = GetObjectHeight(item);

            // Top bun is always the final ingredient
            if (type != null && type.isTopBun)
                return;

            currentHeight += itemHeight + stackSpacing;
        }
    }

    private float GetObjectHeight(GameObject obj)
    {
        if (obj == null) return 0.1f;
        Renderer rend = obj.GetComponentInChildren<Renderer>();
        if (rend == null) return 0.1f;
        return rend.bounds.size.y;
    }

    private bool HasTopBun()
    {
        foreach (var item in stackedIngredients)
            if (item.GetComponent<IngredientType>()?.isTopBun == true)
                return true;
        return false;
    }

    // ----------------------------------------------------------
    // FINALIZE BURGER
    // ----------------------------------------------------------
    // public void FinalizeBurger()
    // {
    //     if (stackedIngredients.Count == 0) return;

    //     // Store burger data
    //     finalizedBurgerData.Clear();

    //     foreach (var item in stackedIngredients)
    //     {
    //         if (item == null) continue;

    //         IngredientType type = item.GetComponent<IngredientType>();
    //         BurgerData data = new BurgerData
    //         {
    //             ingredientName = item.name,
    //             isTopBun = type?.isTopBun ?? false,
    //             isBottomBun = type?.placementPoint != null && !type.isTopBun
    //         };

    //         // Handle patty
    //         PattyCooking patty = item.GetComponent<PattyCooking>();
    //         if (patty != null)
    //         {
    //             data.isPatty = true;
    //             data.pattyState = patty.currentState.ToString();
    //         }

    //         finalizedBurgerData.Add(data);
    //     }

    //     // Spawn box prefab
    //     if (burgerBoxPrefab != null)
    //         Instantiate(burgerBoxPrefab, plateCenter.position, Quaternion.identity);

    //     // Clean up ingredients
    //     foreach (var item in stackedIngredients)
    //         if (item != null)
    //             Destroy(item);

    //     stackedIngredients.Clear();

    //     // Disable plate to prevent further editing
    //     this.enabled = false;
    // }

    public GameObject FinalizeBurger()
{
    if (stackedIngredients.Count == 0) return null;

    // Build burger data
    finalizedBurgerData.Clear();
    foreach (var item in stackedIngredients)
    {
        if (item == null) continue;

        IngredientType type = item.GetComponent<IngredientType>();
        BurgerData data = new BurgerData
        {
            ingredientName = item.name,
            isTopBun = type?.isTopBun ?? false,
            isBottomBun = type?.placementPoint != null && !type.isTopBun
        };

        PattyCooking patty = item.GetComponent<PattyCooking>();
        if (patty != null)
        {
            data.isPatty = true;
            data.pattyState = patty.currentState.ToString();
        }

        finalizedBurgerData.Add(data);
    }

    // Spawn box
    GameObject box = null;
    if (burgerBoxPrefab != null)
    {
        box = Instantiate(burgerBoxPrefab, plateCenter.position, Quaternion.identity);
        BurgerBox boxScript = box.GetComponent<BurgerBox>();
        if (boxScript != null)
            boxScript.LoadBurger(finalizedBurgerData);
    }

    // Destroy ingredients and plate
    foreach (var item in stackedIngredients)
        if (item != null)
            Destroy(item);

    stackedIngredients.Clear();
    Destroy(gameObject);

    return box; // Return the spawned box
}



    // Optional alias
    public void RestackIngredients()
    {
        RepositionStack();
    }
}

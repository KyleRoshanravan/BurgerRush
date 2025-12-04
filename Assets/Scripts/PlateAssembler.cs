using UnityEngine;
using System.Collections.Generic;

public class PlateAssembler : MonoBehaviour
{
    [Header("Plate Settings")]
    public Transform plateCenter;        // Empty child at plate center
    public float stackSpacing = 0.01f;   // Extra space between items

    private List<GameObject> stackedIngredients = new List<GameObject>();

    // ----------------------------------------------------------
    // PLACE INGREDIENT
    // ----------------------------------------------------------
    public void PlaceIngredient(GameObject ingredient)
    {
        if (ingredient == null) return;

        // If ingredient already belongs to this stack, ensure no duplicate
        if (stackedIngredients.Contains(ingredient))
        {
            RepositionStack(); // just to be safe
            return;
        }

        // If it belonged to another plate, detach from that first (defensive)
        PlateAssembler otherPlate = ingredient.GetComponentInParent<PlateAssembler>();
        if (otherPlate != null && otherPlate != this)
            otherPlate.DetachIngredient(ingredient);

        // Freeze physics before placing
        Rigidbody rb = ingredient.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Parent to this plate
        ingredient.transform.SetParent(transform);

        // Add to list
        stackedIngredients.Add(ingredient);

        // Recalculate positions for the whole stack
        RepositionStack();
    }

    // ----------------------------------------------------------
    // DETACH / REMOVE AN INGREDIENT (called by PlayerInteractor)
    // ----------------------------------------------------------
    public void DetachIngredient(GameObject ingredient)
    {
        if (ingredient == null) return;

        // If the ingredient is in our list, remove it
        if (stackedIngredients.Contains(ingredient))
            stackedIngredients.Remove(ingredient);

        // Restore physics on the ingredient
        Rigidbody rb = ingredient.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        // Un-parent so it no longer moves with the plate
        ingredient.transform.SetParent(null);

        // Reposition remaining items so there are no gaps
        RepositionStack();
    }

    // ----------------------------------------------------------
    // REPOSITION (restack) - ensure stacked items sit flush with no gaps
    // ----------------------------------------------------------
    private void RepositionStack()
    {
        float currentHeight = 0f;

        // Clean the list of any null entries (destroyed objects)
        stackedIngredients.RemoveAll(item => item == null);

        for (int i = 0; i < stackedIngredients.Count; i++)
        {
            GameObject item = stackedIngredients[i];
            if (item == null) continue;

            float itemHeight = GetObjectHeight(item);

            item.transform.position = plateCenter.position + new Vector3(0f, currentHeight, 0f);
            item.transform.rotation = Quaternion.identity;

            currentHeight += itemHeight + stackSpacing;
        }
    }

    // Alias kept for older code that might call "Restack" or "Reposition"
    public void RestackIngredients()
    {
        RepositionStack();
    }

    // ----------------------------------------------------------
    // HELPERS
    // ----------------------------------------------------------
    private float GetObjectHeight(GameObject obj)
    {
        if (obj == null) return 0.1f;
        Renderer rend = obj.GetComponentInChildren<Renderer>();
        if (rend == null) return 0.1f; // fallback
        return rend.bounds.size.y;
    }
}

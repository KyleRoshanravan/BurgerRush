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

        IngredientType type = ingredient.GetComponent<IngredientType>();
        if (type == null) return;

        // BLOCK adding ingredients if top bun already exists
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

        // Reposition the whole stack
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

        // Clean list
        stackedIngredients.RemoveAll(item => item == null);

        foreach (var item in stackedIngredients)
        {
            IngredientType type = item.GetComponent<IngredientType>();

            if (type != null && type.isTopBun)
            {
                if (type.placementPoint != null)
                {
                    // WORLD-SPACE OFFSET from placementPoint to object origin
                    Vector3 offset = item.transform.position - type.placementPoint.position;

                    // place so placementPoint sits on current stack height
                    item.transform.position = plateCenter.position + new Vector3(0f, currentHeight, 0f) + offset;
                }
                else
                {
                    // fallback
                    float h = GetObjectHeight(item);
                    item.transform.position = plateCenter.position + new Vector3(0f, currentHeight, 0f);
                    currentHeight += h + stackSpacing;
                }

                item.transform.rotation = Quaternion.identity;

                // top bun is final ingredient
                return;
            }

            // normal ingredient
            float height = GetObjectHeight(item);
            item.transform.position = plateCenter.position + new Vector3(0f, currentHeight, 0f);
            item.transform.rotation = Quaternion.identity;

            currentHeight += height + stackSpacing;
        }
    }

    // ----------------------------------------------------------
    // HELPERS
    // ----------------------------------------------------------
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

    // Optional alias
    public void RestackIngredients()
    {
        RepositionStack();
    }
}

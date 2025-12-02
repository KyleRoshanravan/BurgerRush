using UnityEngine;
using System.Collections.Generic;

public class PlateAssembler : MonoBehaviour
{
    [Header("Plate Settings")]
    public Transform plateCenter;        // Empty child at plate center
    public float stackSpacing = 0.01f;   // Extra space between items

    private List<GameObject> stackedIngredients = new List<GameObject>();

    public void PlaceIngredient(GameObject ingredient)
    {
        if (ingredient == null) return;

        // Freeze physics before placing
        Rigidbody rb = ingredient.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Calculate total stack height so far
        float currentHeight = 0f;

        foreach (var item in stackedIngredients)
            currentHeight += GetObjectHeight(item) + stackSpacing;

        // Position ingredient
        ingredient.transform.position = plateCenter.position + new Vector3(0, currentHeight, 0);

        // Center rotation (optional)
        ingredient.transform.rotation = Quaternion.identity;

        // Add to stack
        stackedIngredients.Add(ingredient);
    }

    private float GetObjectHeight(GameObject obj)
    {
        Renderer rend = obj.GetComponentInChildren<Renderer>();
        if (rend == null) return 0.1f; // fallback
        return rend.bounds.size.y;
    }
}

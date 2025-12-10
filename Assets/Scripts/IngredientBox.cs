using UnityEngine;
using System.Collections;
using TMPro;

public class IngredientBox : MonoBehaviour
{
    [Header("Ingredient Settings")]
    public GameObject ingredientPrefab;
    public Transform spawnPoint;
    public float slideDistance = 0.3f;
    public float slideSpeed = 10f;

    [Header("Economy Settings")]
    public float cost = 0;

    [Header("Auto Label Settings")]
    public float labelHeightOffset = 0.6f;
    [Tooltip("Uniform local scale applied to the label GameObject")]
    public float labelScale = 0.5f;      // increased default scale
    [Tooltip("Font size for the TextMeshPro label")]
    public float labelFontSize = 6f;     // increased default font size

    private TextMeshPro label;
    private bool isSpawning = false;
    private Vector3 originalSpawnPos;

    void Awake()
    {
        CreateAutoLabel();
    }

    void Start()
    {
        if (spawnPoint == null)
            spawnPoint = transform;

        originalSpawnPos = spawnPoint.position;

        UpdateLabelText();
    }

    // ------------------------------------------------------------
    // AUTO-GENERATE LABEL
    // ------------------------------------------------------------
    private void CreateAutoLabel()
    {
        // If the object has no renderer (edge case), skip label creation
        Renderer boxRenderer = GetComponent<Renderer>();
        if (boxRenderer == null)
            return;

        // Create a new GameObject for the label
        GameObject textObj = new GameObject("IngredientLabel");
        textObj.transform.SetParent(transform, true);

        // Add TextMeshPro component (3D text)
        label = textObj.AddComponent<TextMeshPro>();

        // Basic TMP setup
        label.alignment = TextAlignmentOptions.Center;
        label.fontSize = labelFontSize;
        label.color = Color.white;
        label.enableWordWrapping = false;
        label.outlineWidth = 0.25f;
        label.outlineColor = Color.black;

        // Position above the box
        Bounds boxBounds = boxRenderer.bounds;
        Vector3 topCenter = boxBounds.center + new Vector3(0, boxBounds.extents.y + labelHeightOffset, 0);
        textObj.transform.position = topCenter;

        // Apply uniform local scale (tweakable via Inspector)
        textObj.transform.localScale = Vector3.one * labelScale;

        // Make label face the player at all times (billboard)
        textObj.AddComponent<Billboard>();

        // Ensure rotation correction is applied initially (in case camera exists at Start)
        // We'll also let Billboard handle continuous facing.
        textObj.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    private void UpdateLabelText()
    {
        if (label == null) return;

        if (ingredientPrefab == null)
        {
            label.text = "EMPTY";
            return;
        }

        string cleanName = ingredientPrefab.name;
        cleanName = cleanName.Replace("Prefab", "");
        cleanName = cleanName.Replace("(Clone)", "");
        cleanName = System.Text.RegularExpressions.Regex
            .Replace(cleanName, "([a-z])([A-Z])", "$1 $2");

        label.text = cleanName;
    }

    // ------------------------------------------------------------
    // Interaction
    // ------------------------------------------------------------
    public void OnInteract(PlayerInteractor interactor)
    {
        if (isSpawning)
            return;

        if (ingredientPrefab == null)
        {
            Debug.LogWarning($"{name} has no ingredient assigned!");
            return;
        }

        PlayerData playerData = interactor.GetComponent<PlayerData>();
        if(playerData == null)
        {
            Debug.LogWarning("Player has no Player Data.");
            return;
        }

        // Check money
        if (!playerData.TrySpend(cost))
        {
            Debug.Log("Player can't afford this ingredient!");
            return;
        }

        StartCoroutine(SpawnAndGive(interactor));
    }

    private IEnumerator SpawnAndGive(PlayerInteractor interactor)
    {
        isSpawning = true;

        // Slide forward
        Vector3 targetPos = originalSpawnPos + spawnPoint.forward * slideDistance;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * slideSpeed;
            spawnPoint.position = Vector3.Lerp(originalSpawnPos, targetPos, t);
            yield return null;
        }

        // Spawn ingredient
        Vector3 spawnPos = targetPos + Vector3.up * 0.1f;
        GameObject newObj = Instantiate(ingredientPrefab, spawnPos, Quaternion.identity);

        interactor.AutoPickUp(newObj);

        // Slide back
        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * slideSpeed;
            spawnPoint.position = Vector3.Lerp(targetPos, originalSpawnPos, t);
            yield return null;
        }

        isSpawning = false;
    }
}

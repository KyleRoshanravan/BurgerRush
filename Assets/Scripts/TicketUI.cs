using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class TicketUI : MonoBehaviour
{
    public TextMeshProUGUI customerNameText;
    public TextMeshProUGUI timerText;
    public Transform ingredientListParent;
    public TextMeshProUGUI paymentText;

    public Vector2 iconSize = new Vector2(50, 50);

    public float timeRemaining;
    public bool isExpired { get; private set; } = false;

    public void SetupTicket(string customerName, List<GameObject> ingredientPrefabs, float timeLimit, int payment)
    {
        customerNameText.text = customerName;
        paymentText.text = "$" + payment;
        timeRemaining = timeLimit;
        isExpired = false;

        // Clear previous ingredient icons
        foreach (Transform t in ingredientListParent)
            Destroy(t.gameObject);

        // Spawn ingredient icons
        foreach (var prefab in ingredientPrefabs)
        {
            Sprite sprite = null;
            var iconComp = prefab.GetComponent<IngredientIcon>();
            if (iconComp != null) sprite = iconComp.icon;

            if (sprite == null)
            {
                Debug.LogWarning(prefab.name + " has no icon assigned!");
                continue;
            }

            GameObject iconGO = new GameObject(prefab.name + "_Icon", typeof(RectTransform), typeof(Image));
            iconGO.transform.SetParent(ingredientListParent, false);
            Image iconImage = iconGO.GetComponent<Image>();
            iconImage.sprite = sprite;
            iconImage.preserveAspect = true;

            RectTransform rt = iconGO.GetComponent<RectTransform>();
            rt.sizeDelta = iconSize;
        }
    }

    private void Update()
    {
        if (isExpired) return;

        timeRemaining -= Time.deltaTime;
        timerText.text = Mathf.CeilToInt(timeRemaining) + "s";

        if (timeRemaining <= 0f)
        {
            isExpired = true;
            timerText.text = "0s";
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactRange = 3f;
    public string interactTag = "Pickable"; // Tag for pickable objects

    [Header("Hold Settings")]
    public Transform holdPoint; // Where held objects go
    public float moveSpeed = 10f;

    [Header("Crosshair Settings")]
    public Image crosshairImage; // Assign in Inspector
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Color boxHoverColor = Color.green;

    private GameObject hoveredObject;
    private GameObject heldObject;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (crosshairImage != null)
            crosshairImage.color = normalColor;
    }

    void Update()
    {
        HandleHover();
        HandleClick();
        HandleHeldObject();
    }

    // ------------------------------------------------------------
    // Hover detection
    // ------------------------------------------------------------
    void HandleHover()
    {
        if (heldObject != null)
        {
            if (hoveredObject != null)
            {
                OnHoverExit(hoveredObject);
                hoveredObject = null;
            }
            UpdateCrosshair(false, false);
            return;
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            GameObject obj = hit.collider.gameObject;
            bool isBox = obj.GetComponent<IngredientBox>() != null;
            bool isPickable = obj.CompareTag(interactTag);

            if (isBox || isPickable)
            {
                if (hoveredObject != obj)
                {
                    if (hoveredObject != null) OnHoverExit(hoveredObject);
                    hoveredObject = obj;
                    OnHoverEnter(hoveredObject, isBox);
                }

                UpdateCrosshair(true, isBox);
                return;
            }
        }

        // No hit
        if (hoveredObject != null)
        {
            OnHoverExit(hoveredObject);
            hoveredObject = null;
        }
        UpdateCrosshair(false, false);
    }

    void OnHoverEnter(GameObject obj, bool isBox)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null)
            rend.material.color = isBox ? boxHoverColor : hoverColor;

        Debug.Log($"Hovering over {(isBox ? "Ingredient Box" : "Pickable Object")}: {obj.name}");
    }

    void OnHoverExit(GameObject obj)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null)
            rend.material.color = Color.white;
    }

    // ------------------------------------------------------------
    // Pick up and drop logic
    // ------------------------------------------------------------
    void HandleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // If already holding, drop the object
            if (heldObject != null)
            {
                DropObject();
                return;
            }

            // Check if hitting an IngredientBox
            if (hoveredObject != null)
            {
                IngredientBox box = hoveredObject.GetComponent<IngredientBox>();
                if (box != null)
                {
                    box.OnInteract(this);
                    return;
                }

                // Otherwise, try to pick it up normally
                PickUpObject(hoveredObject);
            }
        }
    }

    public void AutoPickUp(GameObject obj)
    {
        if (obj == null) return;

        heldObject = obj;
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        hoveredObject = null;

        Debug.Log($"Auto-picked up new object: {obj.name}");
    }

    void PickUpObject(GameObject obj)
    {
        if (obj == null || obj == heldObject) return;

        heldObject = obj;
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        OnHoverExit(obj);
        hoveredObject = null;

        Debug.Log($"Picked up object: {obj.name}");
    }

    void DropObject()
    {
        if (heldObject == null) return;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;

            // optional gentle push forward
            rb.AddForce(Camera.main.transform.forward * 2f, ForceMode.Impulse);
        }

        Debug.Log($"Dropped object: {heldObject.name}");
        heldObject = null;
    }

    // ------------------------------------------------------------
    // Object movement while held
    // ------------------------------------------------------------
    void HandleHeldObject()
    {
        if (heldObject == null || holdPoint == null) return;

        heldObject.transform.position = Vector3.Lerp(
            heldObject.transform.position,
            holdPoint.position,
            moveSpeed * Time.deltaTime
        );

        heldObject.transform.rotation = Quaternion.Lerp(
            heldObject.transform.rotation,
            holdPoint.rotation,
            moveSpeed * Time.deltaTime
        );
    }

    // ------------------------------------------------------------
    // Crosshair update
    // ------------------------------------------------------------
    void UpdateCrosshair(bool isHovering, bool isBox)
    {
        if (crosshairImage == null) return;
        if (!isHovering)
            crosshairImage.color = normalColor;
        else
            crosshairImage.color = isBox ? boxHoverColor : hoverColor;
    }
}

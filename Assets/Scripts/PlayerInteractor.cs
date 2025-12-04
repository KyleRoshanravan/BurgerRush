using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactRange = 3f;

    [Header("Hold Settings")]
    public Transform holdPoint;
    public float moveSpeed = 10f;

    [Header("Crosshair Settings")]
    public Image crosshairImage;
    public Color normalColor = Color.white;
    public Color hoverPickableColor = Color.yellow;
    public Color hoverPlateColor = Color.cyan;

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
    // HOVER DETECTION
    // ------------------------------------------------------------
    void HandleHover()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            GameObject obj = hit.collider.gameObject;
            if (obj != hoveredObject)
            {
                OnHoverExit();
                hoveredObject = obj;
                OnHoverEnter(obj);
            }
            return;
        }

        OnHoverExit();
        hoveredObject = null;
    }

    void OnHoverEnter(GameObject obj)
    {
        if (heldObject != null)
            return;

        PlateAssembler plate = obj.GetComponent<PlateAssembler>();
        IngredientBox box = obj.GetComponent<IngredientBox>();

        // Crosshair only
        if (plate != null)
            crosshairImage.color = hoverPlateColor;
        else if (obj.CompareTag("Pickable"))
            crosshairImage.color = hoverPickableColor;
        else
            crosshairImage.color = normalColor;
    }

    void OnHoverExit()
    {
        crosshairImage.color = normalColor;
    }

    // ------------------------------------------------------------
    // CLICK HANDLING
    // ------------------------------------------------------------
    void HandleClick()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        // If holding an ingredient
        if (heldObject != null)
        {
            if (hoveredObject != null)
            {
                PlateAssembler plate = hoveredObject.GetComponent<PlateAssembler>();
                if (plate != null)
                {
                    plate.PlaceIngredient(heldObject);
                    heldObject = null;
                    return;
                }
            }

            DropObject();
            return;
        }

        // If not holding — try IngredientBox
        if (hoveredObject != null)
        {
            IngredientBox box = hoveredObject.GetComponent<IngredientBox>();
            if (box != null)
            {
                box.OnInteract(this);
                return;
            }

            // Otherwise pick up a pickable
            if (hoveredObject.CompareTag("Pickable"))
            {
                PickUpObject(hoveredObject);
            }
        }
    }

    // ------------------------------------------------------------
    // PICKUP / DROP
    // ------------------------------------------------------------
    public void AutoPickUp(GameObject obj)
    {
        // Detach from plate if needed
        obj.transform.SetParent(null);

        heldObject = obj;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    void PickUpObject(GameObject obj)
    {
        // Detach from plate if needed
        obj.transform.SetParent(null);

        heldObject = obj;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    void DropObject()
    {
        if (heldObject == null) return;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        heldObject = null;
    }

    // ------------------------------------------------------------
    // HELD OBJECT MOVEMENT
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
}

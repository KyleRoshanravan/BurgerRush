using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public float interactRange = 3f;
    private Pickable hoveredObject;
    private Pickable heldObject;

    void Update()
    {
        HandleHover();
        HandleClick();
    }

    void HandleHover()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            Pickable pickable = hit.collider.GetComponent<Pickable>();
            if (pickable != null)
            {
                if (hoveredObject != pickable)
                {
                    hoveredObject?.OnHoverExit();
                    hoveredObject = pickable;
                    hoveredObject.OnHoverEnter();
                }
                return;
            }
        }

        // If not hitting any pickable
        if (hoveredObject != null)
        {
            hoveredObject.OnHoverExit();
            hoveredObject = null;
        }
    }

    void HandleClick()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            if (hoveredObject != null)
            {
                if (heldObject == hoveredObject)
                {
                    hoveredObject.TogglePickUp(transform);
                    heldObject = null;
                }
                else
                {
                    hoveredObject.TogglePickUp(transform);
                    heldObject = hoveredObject;
                }
            }
        }
    }
}

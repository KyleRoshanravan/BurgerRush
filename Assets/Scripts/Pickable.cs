using UnityEngine;

/// <summary>
/// Abstract base class for any clickable, pickable, highlightable object.
/// Inherit this to create your own pickup logic and highlight visuals.
/// </summary>
[RequireComponent(typeof(Collider))]
public abstract class Pickable : MonoBehaviour
{
    protected bool isPickedUp = false;
    protected bool isHovered = false;

    /// <summary>
    /// Called when the player clicks this object to pick it up.
    /// </summary>
    public abstract void OnPickUp(Transform player);

    /// <summary>
    /// Called when the player drops this object.
    /// </summary>
    public abstract void OnDrop();

    /// <summary>
    /// Called every frame while the object is held.
    /// </summary>
    public virtual void OnHold(Transform player) { }

    /// <summary>
    /// Called when the player's cursor starts hovering over this object.
    /// </summary>
    public virtual void OnHoverEnter() 
    {
        isHovered = true;
    }

    /// <summary>
    /// Called when the player's cursor stops hovering over this object.
    /// </summary>
    public virtual void OnHoverExit() 
    {
        isHovered = false;
    }

    /// <summary>
    /// Called when the player clicks the object (toggle pickup/drop).
    /// </summary>
    public void TogglePickUp(Transform player)
    {
        if (isPickedUp)
        {
            OnDrop();
            isPickedUp = false;
        }
        else
        {
            OnPickUp(player);
            isPickedUp = true;
        }
    }
}

using UnityEngine;

public class TicketLookAt : MonoBehaviour
{
    public Transform lookTarget; // Assign the object to face (e.g., order counter)

    void LateUpdate()
    {
        if (lookTarget != null)
        {
            // Keep ticket upright and face target
            Vector3 targetPosition = lookTarget.position;
            targetPosition.y = transform.position.y; // maintain height
            transform.LookAt(targetPosition);
        }
    }
}

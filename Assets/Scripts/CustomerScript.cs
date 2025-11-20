using UnityEngine;

public class Customer : MonoBehaviour
{
    public float waitTime = 5f; // How long the customer waits for order
    private bool isWaiting = true;

    void Start()
    {
        // Start the customer behavior
        StartCoroutine(CustomerRoutine());
    }

    private System.Collections.IEnumerator CustomerRoutine()
    {
        // Customer arrives (could play animation here)
        yield return new WaitForSeconds(waitTime);

        if (isWaiting)
        {
            Leave(); // Customer leaves if order isn't served
        }
    }

    public void Serve()
    {
        // Called when order is served
        isWaiting = false;
        // Play happy animation or effect
        Destroy(gameObject, 1f); // Remove customer after short delay
    }

    private void Leave()
    {
        // Play leaving animation/effect
        Destroy(gameObject, 1f); // Remove customer
    }
}

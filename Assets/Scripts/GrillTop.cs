using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GrillTop : MonoBehaviour
{
    [Header("Optional Effects")]
    public AudioSource sizzleSound; // optional sizzling sound
    public ParticleSystem smokeEffect; // optional smoke particle effect

    private void Start()
    {
        // Make sure collider is a trigger
        Collider col = GetComponent<Collider>();
        if (!col.isTrigger)
        {
            col.isTrigger = true;
            Debug.LogWarning("GrillTop collider was not a trigger — set automatically!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering is a patty
        PattyCooking patty = other.GetComponent<PattyCooking>();
        if (patty != null)
        {
            // Start cooking
            patty.StartCooking();

            // Optional effects
            if (sizzleSound != null && !sizzleSound.isPlaying)
                sizzleSound.Play();

            if (smokeEffect != null && !smokeEffect.isPlaying)
                smokeEffect.Play();

            Debug.Log($"Patty {other.name} started cooking on grill!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object leaving is a patty
        PattyCooking patty = other.GetComponent<PattyCooking>();
        if (patty != null)
        {
            patty.StopCooking();

            // Optional: stop sizzle when no patties left
            if (sizzleSound != null && sizzleSound.isPlaying)
                sizzleSound.Stop();

            if (smokeEffect != null && smokeEffect.isPlaying)
                smokeEffect.Stop();

            Debug.Log($"Patty {other.name} removed from grill.");
        }
    }
}

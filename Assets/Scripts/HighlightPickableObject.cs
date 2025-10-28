using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class HighlightPickableObject : Pickable
{
    public float holdDistance = 2f;
    public float followSpeed = 10f;
    public Color highlightColor = Color.yellow;

    private Transform player;
    private Rigidbody rb;
    private Renderer rend;
    private Color originalColor;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    void Update()
    {
        if (isPickedUp && player != null)
            OnHold(player);

        // Smooth highlight color transition
        if (rend != null)
        {
            Color target = isHovered ? highlightColor : originalColor;
            rend.material.color = Color.Lerp(rend.material.color, target, Time.deltaTime * 10f);
        }
    }

    public override void OnPickUp(Transform player)
    {
        this.player = player;
        rb.useGravity = false;
        rb.linearDamping = 10f;
    }

    public override void OnDrop()
    {
        rb.useGravity = true;
        rb.linearDamping = 0f;
        player = null;
    }

    public override void OnHold(Transform player)
    {
        Vector3 targetPos = player.position + player.forward * holdDistance;
        Vector3 move = targetPos - transform.position;
        rb.linearVelocity = move * followSpeed;
    }
}

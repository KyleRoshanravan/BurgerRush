using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 15f;
    public float jumpForce = 25f;
    [SerializeField] private float friction = 10f;
    [SerializeField] private float gravity = 3f;
    [SerializeField] private float groundOffset = 1f;

    private Rigidbody rb;
    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // prevent spinning
    }

    void Update()
    {
        // Movement input
        float horzMove = Input.GetAxisRaw("Horizontal");
        float vertMove = Input.GetAxisRaw("Vertical");
        moveDirection = (transform.forward * vertMove + transform.right * horzMove).normalized;

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Apply movement & physics
        rb.AddForce(moveDirection * moveSpeed / 10, ForceMode.Impulse);
        rb.linearVelocity = new Vector3(
            rb.linearVelocity.x * (100 - friction) / 100,
            rb.linearVelocity.y - gravity / 10,
            rb.linearVelocity.z * (100 - friction) / 100
        );
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundOffset + 0.15f);
    }
}

using UnityEngine;

public class PlayerMouseLook : MonoBehaviour
{
    [Header("Settings")]
    public float mouseSensitivity = 100f;
    public Camera playerCamera; // assign the child camera

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Read mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Look up/down (camera only)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the player body left/right
        transform.Rotate(Vector3.up * mouseX);
    }
}

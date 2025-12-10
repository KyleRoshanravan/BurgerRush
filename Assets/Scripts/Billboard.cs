using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main == null) return;

        // Make the label face the camera
        transform.LookAt(Camera.main.transform);

        // Rotate 180 degrees on Y so text isn't backwards
        transform.Rotate(0f, 180f, 0f);
    }
}
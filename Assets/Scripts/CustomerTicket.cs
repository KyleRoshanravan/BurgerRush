using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CustomerTicket : MonoBehaviour
{
    public TextMeshProUGUI orderText;
    public Transform customerHead;

    void LateUpdate()
    {
        // Make UI always face camera
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);

        // Follow customer
        if (customerHead != null)
        {
            transform.position = customerHead.position + Vector3.up * 2;
        }
    }

    public void SetOrder(string text)
    {
        orderText.text = text;
    }
}

using UnityEngine;
public class CustomerOrderPoint : MonoBehaviour
{
    public static CustomerOrderPoint Instance;
    private void Awake() { Instance = this; }
}

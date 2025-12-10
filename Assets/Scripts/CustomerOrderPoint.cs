using UnityEngine;
using System.Collections.Generic;

public class CustomerOrderPoint : MonoBehaviour
{
    public static CustomerOrderPoint Instance;

    public List<Transform> orderPoints = new List<Transform>();

    private void Awake()
    {
        Instance = this;

        // Automatically collect all children as order points
        orderPoints.Clear();
        foreach (Transform child in transform)
        {
            orderPoints.Add(child);
        }
    }

    public Transform GetRandomPoint()
    {
        return orderPoints[Random.Range(0, orderPoints.Count)];
    }
}

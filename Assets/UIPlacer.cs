using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlacer : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private void Update()
    {        // Example so we can test the Health Bar functionality
        if (Camera.main != null)
            transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
    }

}

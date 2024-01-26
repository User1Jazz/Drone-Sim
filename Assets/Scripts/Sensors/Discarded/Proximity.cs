using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proximity : MonoBehaviour
{
    public float value = 0f;
    public Transform origin;
    public Vector3 rayDirection = Vector3.forward;
    public bool debugMode = true;

    Vector3 hitPoint = Vector3.zero;
    bool hitDetected = false;

    void FixedUpdate()
    {
        RaycastHit hit;
        Ray downRay = new Ray(origin.position, rayDirection);


        hitDetected = Physics.Raycast(downRay, out hit);
        if (hitDetected)
        {
            value = hit.distance;
            hitPoint = hit.point;
        }
    }

    void OnDrawGizmos()
    {
        if (debugMode && hitDetected)
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin.position, hitPoint);
        }
    }
}

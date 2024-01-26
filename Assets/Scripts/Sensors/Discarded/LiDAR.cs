using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * This script is using the 2D vision cone to detect the objects
 * https://www.youtube.com/watch?v=luLrhoTZYD8&t=181s modified by User1Jazz
 */

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class LiDAR : Vision
{
    // Visualisation vars
    public Material VisionConeMaterial;         // Reference to the material of the vision cone
    Mesh VisionConeMesh;                        // Reference to the vision cone mesh
    MeshFilter MeshFilter_;                     // Reference to the mesh filter

    // Calculation vars
    private Vector3[] Vertices;                 // Array of vertices at the end of the vision cone
    private float angleRad = 0f;                // Variable that contains the vision angle in radians

    // Vision parameters
    [Range(0f, 360f)]
    public float VisionAngle;                   // Vision Angle
    public LayerMask VisionObstructingLayer;    // Layer with objects that obstruct the enemy view, like walls, for example
    [Range(6, 150)]
    public int VisionConeResolution = 120;      // The vision cone will be made up of triangles, the higher this value is the pretier the vision cone will be

    // Output vars
    public List<Transform> collidedObjects;     // List of objects that the vision cone detected; NOTE: This list will always appear empty but in fact contains the data for a brief moment before it is discarded. Unity Inspector will not detect and show that data



    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material = VisionConeMaterial;
        MeshFilter_ = gameObject.GetComponent<MeshFilter>();
        VisionConeMesh = new Mesh();
        angleRad = VisionAngle * Mathf.Deg2Rad;
    }

    void Update()
    {
        angleRad = VisionAngle * Mathf.Deg2Rad;
        DrawVisionCone();      // Calling the vision cone function every frame just so the cone is updated every frame
    }

    void FixedUpdate()
    {
        CalculateVisionCone();
    }

    void CalculateVisionCone()
    {
        Vertices = new Vector3[VisionConeResolution + 1];
        Vertices[0] = Vector3.zero;
        float Currentangle = -angleRad / 2;
        float angleIcrement = angleRad / (VisionConeResolution - 1);
        float Sine;
        float Cosine;

        // Get distance
        for (int i = 0; i < VisionConeResolution; i++)
        {
            // Calculate sine and cosine
            Sine = Mathf.Sin(Currentangle);
            Cosine = Mathf.Cos(Currentangle);

            // Calculate direction and vertex point
            Vector3 RaycastDirection = (transform.forward * Cosine) + (transform.right * Sine);
            Vector3 VertForward = (Vector3.forward * Cosine) + (Vector3.right * Sine);

            // Check for collisions and update vertices array
            if (Physics.Raycast(transform.position, RaycastDirection, out RaycastHit hit, VisionRange, VisionObstructingLayer))
            {
                Vertices[i + 1] = VertForward * hit.distance;

                // Add transform to collided objects list
                if (!collidedObjects.Contains(hit.collider.transform))
                {
                    collidedObjects.Add(hit.collider.transform);
                }
            }
            else
            {
                Vertices[i + 1] = VertForward * VisionRange;
            }


            Currentangle += angleIcrement;  // Update current angle
        }

        UpdateTargetList();         // Update the target list
        collidedObjects.Clear();    // Clear the list collided objects (prepare for the incomming new data)
    }

    // Function to create the vision cone mesh
    void DrawVisionCone()
    {
        int[] triangles = new int[(VisionConeResolution - 1) * 3];

        VisionConeMesh.Clear();

        // Render the vision cone
        if (renderVision)
        {
            for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
            {
                triangles[i] = 0;
                triangles[i + 1] = j + 1;
                triangles[i + 2] = j + 2;
            }
            VisionConeMesh.vertices = Vertices;
            VisionConeMesh.triangles = triangles;
            MeshFilter_.mesh = VisionConeMesh;
        }
    }

    void UpdateTargetList()
    {
        // Add objects to the target list
        foreach (Transform transform in collidedObjects)
        {/* ((targetMask.layer & (1<<transform.gameObject.layer)) != 0) */
            if (((targetMask & (1 << transform.gameObject.layer)) != 0) && !targetObjects.Contains(transform))
            {
                targetObjects.Add(transform);
            }
        }
        int itemCount = collidedObjects.Count;

        // Try remove objects from target list that are not in the sight any more (placed in the try block to prevent the issue with going through an empty list on runtime)
        try
        {
            foreach (Transform transform in targetObjects)
            {
                if (!collidedObjects.Contains(transform))
                {
                    targetObjects.Remove(transform);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    // Draw the distance line
    void OnDrawGizmos()
    {
        float Currentangle = -(VisionAngle * Mathf.Deg2Rad) / 2;

        float Sine = Mathf.Sin(Currentangle);
        float Cosine = Mathf.Cos(Currentangle);

        Vector3 RaycastDirection = (transform.forward * Cosine);
        Vector3 VertForward = (Vector3.forward * Cosine) + (Vector3.right * Sine);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + RaycastDirection * VisionRange * 3.815f);
    }
}

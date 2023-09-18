using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Main class for vision sensors (cone vision, camera, etc.)
 * This class is not the parent for simple vision scripts such as simple proximity
 */

public class Vision : MonoBehaviour
{
    // Vision Parameters
    public float VisionRange;                   // Range of object detection
    public LayerMask targetMask;                // Target objects' layer
    public List<Transform> targetObjects;       // List of target objects
    public bool renderVision = true;            // Boolean to determine whether to render the vision or not (only some scripts that inherit from this class use this variable)

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

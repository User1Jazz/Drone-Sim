using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class WorldObject
{
    // Editable in Unity editor only
    public string objectName = "World Object";  // Name of the object for identification (in world editor)
    public Mesh mesh;                           // Mesh of the object (for visualisation and colision)
    public Material material;                   // Material of the object
    public Texture icon;                        // Icon of the object (for world editor visualisation)
    //public MonoScript script;                   // Script for the object (optional; Used if the object needs to perform some action)

    // Editable in world editor only
    [HideInInspector] public Vector3 position;  // Position of the object in the world space
    [HideInInspector] public Vector3 size;      // Size of the object
    [HideInInspector] public Vector3 rotation;  // Rotation of the object

    // Not editable; Assigned once added in the sim world
    [HideInInspector] public int worldID;  // Identifier in the world (not editable)
}

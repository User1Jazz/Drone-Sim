using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectButton : MonoBehaviour
{
    WorldEditor worldEditorScript;      // Reference to the world editor script
    public WorldObject assignedObject;  // Variable that holds the world object that has been assigned to the button

    // Start is called before the first frame update
    void Start()
    {
        worldEditorScript = FindObjectOfType<WorldEditor>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddObject()
    {
        worldEditorScript.AddObject(assignedObject);
    }
}

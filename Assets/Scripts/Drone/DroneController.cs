using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(DroneRB))]
public class DroneController : MonoBehaviour
{
    public Vector2 cyclic;  // Pitch and roll
    public float pedals;    // Yaw
    public float throttle;  // Throttle (vertical force)
    public float maxPitch = 30f;
    public float maxRoll = 30f;
    public float yawPower = 15;

    void Update()
    {
        ManualControls();
    }

    void ManualControls()
    {
        cyclic.x = Input.GetAxis("Vertical");
        cyclic.y = -Input.GetAxis("Horizontal");
        
        if (Input.GetKey("q"))
        {
            pedals = -1f;
        }
        if (Input.GetKey("e"))
        {
            pedals = 1f;
        }
        if(Input.GetKeyUp("q") || Input.GetKeyUp("e"))
        {
            pedals = 0f;
        }

        if (Input.GetKey("r"))
        {
            throttle = 1f;
        }
        if (Input.GetKey("f"))
        {
            throttle = -1f;
        }
        if (Input.GetKeyUp("r") || Input.GetKeyUp("f"))
        {
            throttle = 0f;
        }
    }
}

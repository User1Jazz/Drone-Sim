using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlType{
    AssistON, AssistOFF
};

//[RequireComponent(typeof(DroneRB))]
public class DroneController : MonoBehaviour
{
    public Vector2 cyclic;                                          // Pitch and roll
    public float pedals;                                            // Yaw
    public float throttle;                                          // Throttle (vertical force)
    [Tooltip("Degrees per second")] public float yawSpeed = 15f;    // Yaw power
    [Tooltip("Degrees per second")] public float pitchSpeed = 15f;  // Pitch speed (deg/sec)
    [Tooltip("Degrees per second")] public float rollSpeed = 15f;   // Max roll (deg/sec)
    public ControlType controlType = ControlType.AssistON;

    [Header("Assist ON Parameters")]
    [Tooltip("Degrees")] public float maxPitch = 30f;               // Max pitch angle (deg)
    [Tooltip("Degrees")] public float maxRoll = 30f;                // Max roll angle (deg)

    //[Header("Assist OFF Parameters")]

    void Update()
    {
        ManualControls();
    }

    void ManualControls()
    {
        //cyclic.x = Input.GetAxis("Vertical");
        //cyclic.y = -Input.GetAxis("Horizontal");

        if (Input.GetKey("w"))
        {
            cyclic.x = 1f;
        }
        if (Input.GetKey("s"))
        {
            cyclic.x = -1f;
        }
        if (Input.GetKeyUp("w") || Input.GetKeyUp("s"))
        {
            cyclic.x = 0f;
        }

        if (Input.GetKey("a"))
        {
            cyclic.y = 1f;
        }
        if (Input.GetKey("d"))
        {
            cyclic.y = -1f;
        }
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        {
            cyclic.y = 0f;
        }

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

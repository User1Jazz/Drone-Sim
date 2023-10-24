using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    public Vector2 cyclic;                                         // Pitch and roll
    public float pedals;                                           // Yaw
    
    [Tooltip("Yaw Speed (Deg/s)")] public float yawSpeed = 15f;    // Yaw power
    [Tooltip("Pitch Speed (m/s)")] public float pitchSpeed = 15f;  // Pitch speed (deg/sec)
    [Tooltip("Roll Speed (m/s)")] public float rollSpeed = 15f;    // Max roll (deg/sec)

    Rigidbody rb;
    public float motorPower = 5f;
    public float throttle;                                         // Throttle (vertical force)

    //[Header("Assist OFF Parameters")]

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Hover();

        Throttle(throttle * motorPower);
        Nick(pitchSpeed * cyclic.x);
        Roll(rollSpeed * cyclic.y);
        Yaw(yawSpeed * pedals);
    }

    void Update()
    {
        HandleManualInput();
    }

    void  HandleManualInput()
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

    void Hover()
    {
        rb.AddRelativeForce(Vector3.up * rb.mass * Physics.gravity.magnitude);
    }

    void Throttle(float force)
    {
        rb.AddRelativeForce(Vector3.up * rb.mass * Physics.gravity.magnitude * force);
    }

    void Nick(float force)
    {
        rb.AddRelativeForce(Vector3.forward * force);
    }

    void Roll(float force)
    {
        rb.AddRelativeForce(-Vector3.right * force);
    }

    void Yaw(float force)
    {
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0f, force, 0f) * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    
    
}

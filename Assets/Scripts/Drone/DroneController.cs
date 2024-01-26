using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    public Vector3 linear;                                          // X => Forward/Backward,   Y =>Left/Right,     Z => Up/Down
    public Vector3 angular;                                         // X => Roll,               Y => Pitch,         Z => Yaw (turn left/right)

    [Tooltip("Yaw Speed (Deg/s)")] public float yawSpeed = 15f;    // Yaw power
    [Tooltip("Pitch Speed (m/s)")] public float pitchSpeed = 15f;  // Pitch speed (deg/sec)
    [Tooltip("Roll Speed (m/s)")] public float rollSpeed = 15f;    // Max roll (deg/sec)

    Rigidbody rb;
    public float motorPower = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Hover();

        Throttle(linear.z * motorPower);
        Nick(pitchSpeed * linear.x);
        Roll(rollSpeed * linear.y);
        Yaw(yawSpeed * angular.z);
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

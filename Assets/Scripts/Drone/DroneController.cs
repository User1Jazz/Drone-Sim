using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    public Vector3 linear;                                          // X => Forward/Backward,   Y =>Left/Right,     Z => Up/Down
    public Vector3 angular;                                         // X => Roll,               Y => Pitch,         Z => Yaw (turn left/right)

    [Tooltip("Max Yaw Speed (Deg/s)")] public float maxYawSpeed = 15f;    		// Max Yaw power (deg/s)
    [Tooltip("Max Throttle Speed (m/s)")] public float maxThrottleSpeed = 15f; 	// Max Throttle speed (m/sec)
    [Tooltip("Max Roll Speed (m/s)")] public float maxRollSpeed = 15f;    		// Max roll (m/sec)
	
	[Tooltip("Yaw Speed (Deg/s)")] public float yawSpeed = 10f;    			// Yaw power (deg/s)
    [Tooltip("Throttle Speed (m/s)")] public float throttleSpeed = 10f;  	// Throttle speed (m/sec)
    [Tooltip("Roll Speed (m/s)")] public float rollSpeed = 10f;    			// Roll (m/sec)

    Rigidbody rb;
	public float speedMultiplier = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Hover();
		
		rollSpeed *= speedMultiplier;
		throttleSpeed *= speedMultiplier;
		yawSpeed *= speedMultiplier;
		if(throttleSpeed > maxThrottleSpeed)
		{
			throttleSpeed = maxThrottleSpeed * speedMultiplier;
		}
        Throttle(linear.z * throttleSpeed);
		if(rollSpeed > maxRollSpeed)
		{
			rollSpeed = maxRollSpeed * speedMultiplier;
		}
		if(yawSpeed > maxYawSpeed)
		{
			yawSpeed = maxYawSpeed * speedMultiplier;
		}
        Nick(rollSpeed * linear.x);
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

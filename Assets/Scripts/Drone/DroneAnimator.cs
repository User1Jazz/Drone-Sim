using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAnimator : MonoBehaviour
{
    [SerializeField] DroneController droneControllerScript;
    [SerializeField] Transform droneMainTransform;

    // Rotation animation vars
	public float maxRollAngle = 20f;
    float tiltAmountForward = 0f;
    float tiltVelocityForward = 0f;

    float tiltAmountRight = 0f;
    float tiltVelocityRight = 0f;

    float yAxis = 0;


    // Propeller animation vars
    [SerializeField] List<Transform> clockwisePropellers;
    [SerializeField] List<Transform> counterClockwisePropellers;
    public float maxPropellerRPM = 20000f;
    float rotationRate = 1f;

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleRotation();
        HandleMotors();
    }

    
    void HandleRotation()
    {
		float angleAmount = maxRollAngle * ((droneControllerScript.rollSpeed / droneControllerScript.speedMultiplier) / droneControllerScript.maxRollSpeed);
        tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, droneMainTransform.eulerAngles.x + angleAmount * droneControllerScript.linear.x, ref tiltVelocityForward, 0.1f);     // Calculate forward tilt
        tiltAmountRight = Mathf.SmoothDamp(tiltAmountRight, droneMainTransform.eulerAngles.z + angleAmount * droneControllerScript.linear.y, ref tiltVelocityRight, 0.1f);           // Calculate side tilt

        yAxis = droneMainTransform.eulerAngles.y;																															// Get yaw

        Vector3 rotation = new Vector3(tiltAmountForward, yAxis, tiltAmountRight);                                                                                          // Create a rotation vector

        transform.rotation = Quaternion.Euler(rotation);                                                                                                                    // Apply rotation
    }

    void HandleMotors()
    {
        foreach (Transform propeller in clockwisePropellers)
        {
            propeller.Rotate(Vector3.forward, maxPropellerRPM * rotationRate);
        }
        foreach (Transform propeller in counterClockwisePropellers)
        {
            propeller.Rotate(Vector3.forward, maxPropellerRPM * -rotationRate);
        }
    }
}

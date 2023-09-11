using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DroneMotor : MonoBehaviour
{
    public float maxPower = 0.075f;     // Power(Thrust?) to weight ratio
    public Transform propeller;
    public float maxPropellerRPM = 20000f;
    public bool counterClockwise = false;

    public void UpdateMotor(Rigidbody rb, float throttle, int totalNumberOfMotors)
    {
        /* THIS SHOULD IMPLEMENT AUTO HOVERING FEATURE WHEN FLYING AROUND (BUT THERE IS SOME PROBLEM...)
        Vector3 upVec = transform.forward;
        upVec.x = 0f;
        upVec.y = 0f;
        float diff = Physics.gravity.magnitude * (1 - upVec.magnitude);
        Line 22 should be like this then: motorForce = transform.forward * ((rb.mass * Physics.gravity.magnitude + diff) + (throttle * maxPower)) / totalNumberOfMotors;*/

        Vector3 motorForce = Vector3.zero;
        motorForce = transform.forward * ((rb.mass * Physics.gravity.magnitude) + (throttle * maxPower)) / totalNumberOfMotors;
        rb.AddForce(motorForce, ForceMode.Force);

        HandlePropeller(1f);
    }

    public void HandlePropeller(float rotationRate)
    {
        if (!propeller)
        {
            Debug.LogError(gameObject.name + ": propeller not assigned to DroneMotor script");
            return;
        }
        if (!counterClockwise)
        {
            propeller.Rotate(Vector3.forward, maxPropellerRPM * rotationRate);
        }
        else
        {
            propeller.Rotate(Vector3.forward, -maxPropellerRPM * rotationRate);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(DroneController))]
public class DroneRB : MonoBehaviour
{
    public float weight = 1f;
    public bool weightInPounds = false;
    private Rigidbody rb;
    private DroneController controller;
    private float startDrag;
    private float startAngularDrag;
    public List<DroneMotor> motors;

    public float pitch = 0f;
    public float roll = 0f;
    public float yaw = 0f;

    private float finalPitch = 0f;
    private float finalRoll = 0f;
    private float finalYaw = 0f;
    public float lerpSpeed = 1f;

    const float lbsToKg = 0.454f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<DroneController>();

        startDrag = rb.drag;
        startAngularDrag = rb.angularDrag;

        if (weightInPounds)
        {
            weight = weight * lbsToKg;
        }

        rb.mass = weight;
    }

    void FixedUpdate()
    {
        if (!rb)
        {
            Debug.LogError(gameObject.name + ": Rigidbody not attached for DroneRB script");
            return;
        }
        HandlePhysics();
    }

    void HandlePhysics()
    {
        HandleMotors();
        HandleControls();
    }

    void HandleMotors()
    {
        //rb.AddForce(Vector3.up * (rb.mass * Physics.gravity.magnitude));
        foreach (DroneMotor motor in motors)
        {
            motor.UpdateMotor(rb, controller.throttle, motors.Count);
        }
    }

    void HandleControls()
    {
        pitch = controller.cyclic.x * controller.maxPitch;
        roll = controller.cyclic.y * controller.maxRoll;
        yaw += controller.pedals * controller.yawPower * Time.deltaTime;

        finalPitch = Mathf.Lerp(finalPitch, pitch, Time.deltaTime * lerpSpeed);
        finalRoll = Mathf.Lerp(finalRoll, roll, Time.deltaTime * lerpSpeed);
        finalYaw = Mathf.Lerp(finalYaw, yaw, Time.deltaTime * lerpSpeed);

        Quaternion rot = Quaternion.Euler(finalPitch, finalYaw, finalRoll);
        rb.MoveRotation(rot);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMU : MonoBehaviour
{
    public Rigidbody rb;

    public Vector3 angularVelocity;
    public Vector3 acceleration;
    public Quaternion orientation;
    public float[] angular_velocity_covariance;
    public float[] linear_acceleration_covariance;
    public float[] orientation_covariance;

    /*public IMU(Vector3 _angularVelocity, Vector3 _acceleration, Quaternion _orientation, float[] _angular_velocity_covariance, float[] _linear_acceleration_covariance, float[] _orientation_covariance)
    {
        angularVelocity = _angularVelocity;
        acceleration = _acceleration;
        orientation = _orientation;
        angular_velocity_covariance = _angular_velocity_covariance;
        linear_acceleration_covariance = _linear_acceleration_covariance;
        orientation_covariance = _orientation_covariance;
    }*/

    void Update()
    {
        angularVelocity = rb.angularVelocity; // Angular velocity in radians per second
        acceleration = rb.velocity; // Linear acceleration in units per second squared

        // Calculate orientation quaternion based on the Rigidbody's rotation
        Quaternion orientation = transform.rotation;

        // Set covariance matrices (adjust these based on your simulation)
        angular_velocity_covariance = new float[9];
        linear_acceleration_covariance = new float[9];
        orientation_covariance = new float[9];
    }

    /*public IMU GetIMUReadings()
    {
        angularVelocity = rb.angularVelocity; // Angular velocity in radians per second
        acceleration = rb.velocity; // Linear acceleration in units per second squared

        // Calculate orientation quaternion based on the Rigidbody's rotation
        orientation = transform.rotation;

        // Set covariance matrices (adjust these based on your simulation)
        angular_velocity_covariance = new float[9];
        linear_acceleration_covariance = new float[9];
        orientation_covariance = new float[9];

        IMU imu = new IMU(angularVelocity, acceleration, orientation, angular_velocity_covariance, linear_acceleration_covariance, orientation_covariance);

        return imu;
    }*/
}

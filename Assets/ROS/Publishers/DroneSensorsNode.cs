using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;
using RosMessageTypes.DroneSimMessages;
using RosMessageTypes.Geometry;

public class DroneSensorsNode : MonoBehaviour
{
    // ROS Variables
    ROSConnection ros;                                  // ROS Connection
    public string topicName = "/D#/data";               // Topic name
    public float publishMessageFrequency = 0.05f;       // Publish the message every N seconds
    private float timeElapsed;                          // Used to determine how much time has elapsed since the last message was published

    public IMU imu;                                     // Reference to the IMU Script
    public IR ir;                                       // Reference to the IR Script
    public VirtualCamera droneCam;                      // Reference to the camera script

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        // Create a new texture to read the pixels into
        if (timeElapsed > publishMessageFrequency)
        {
            PublishData();
            timeElapsed = 0;
        }
    }

    public void Init()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<Drone_SensorsMsg>(topicName);
    }

    void PublishData()
    {
        Drone_SensorsMsg message = new Drone_SensorsMsg(
            new HeaderMsg(),
            droneCam.GetImage(),
            ir.value,
            new QuaternionMsg(imu.orientation.x, imu.orientation.y, imu.orientation.z, imu.orientation.w),
            new Vector3Msg(imu.angularVelocity.x, imu.angularVelocity.y, imu.angularVelocity.z),
            new Vector3Msg(imu.acceleration.x, imu.acceleration.y, imu.acceleration.z));

        ros.Publish(topicName, message);
    }
}

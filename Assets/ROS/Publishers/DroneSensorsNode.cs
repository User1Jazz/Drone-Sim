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
	
	[SerializeField] bool initializeOnStart = false;

    // Start is called before the first frame update
    void Start()
    {
		if(initializeOnStart)
		{
			Init();
		}
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
            PublishData();
            timeElapsed = 0;
        }
    }

    public void Init()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<DroneSensorsMsg>(topicName);
    }

    void PublishData()
    {
		 //Note: Y and Z axis switched due to Unity's coordinate system
        DroneSensorsMsg message = new DroneSensorsMsg(
            ir.value,
            new QuaternionMsg(imu.orientation.x, imu.orientation.z, imu.orientation.y, imu.orientation.w),
            new Vector3Msg(imu.angularVelocity.x, imu.angularVelocity.z, imu.angularVelocity.y),
            new Vector3Msg(imu.acceleration.x, imu.acceleration.z, imu.acceleration.y),
			new Vector3Msg(imu.world_position.x, imu.world_position.z, imu.world_position.y),
			new Vector3Msg(imu.local_position.x, imu.local_position.z, imu.local_position.y),
			droneCam.GetImage());

        ros.Publish(topicName, message);
    }
}

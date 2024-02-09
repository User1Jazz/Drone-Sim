using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;
using RosMessageTypes.Geometry;

public class DroneTargetPublisher : MonoBehaviour
{
	// ROS Variables
    ROSConnection ros;                                  // ROS Connection
    public string topicName = "/D#/target";             // Topic name
    public float publishMessageFrequency = 0.05f;       // Publish the message every N seconds
    private float timeElapsed;                          // Used to determine how much time has elapsed since the last message was published

    public Vector3 targetPosition;                       // Reference to the IMU Script
	
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
        ros.RegisterPublisher<Vector3Msg>(topicName);
    }

    void PublishData()
    {
        Vector3Msg message = new Vector3Msg((double)targetPosition.x, (double)targetPosition.z, (double)targetPosition.y); // Y and Z axis switched due to Unity's coordinate system

        ros.Publish(topicName, message);
    }
}

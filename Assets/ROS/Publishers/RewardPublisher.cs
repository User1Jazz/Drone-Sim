using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;
using RosMessageTypes.DroneSimMessages;
using RosMessageTypes.Geometry;

public class RewardPublisher : MonoBehaviour
{
	// ROS Variables
    ROSConnection ros;                                  // ROS Connection
    public string topicName = "/D#/reward";             // Topic name
    public float publishMessageFrequency = 0.05f;       // Publish the message every N seconds
    private float timeElapsed;                          // Used to determine how much time has elapsed since the last message was published
	
	[SerializeField] bool initializeOnStart = false;
	
	public float reward;
	[SerializeField] RewardCalculator rewardCalc;
	
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
		GetReward();

        // Create a new texture to read the pixels into
        if (timeElapsed > publishMessageFrequency)
        {
            PublishData();
            timeElapsed = 0;
        }
    }
	
	void GetReward()
	{
		reward = rewardCalc.CalculateReward();
	}
	
	public void Init()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<Float32Msg>(topicName);
    }

    void PublishData()
    {
        Float32Msg message = new Float32Msg(reward);

        ros.Publish(topicName, message);
    }
}

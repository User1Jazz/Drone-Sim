using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;
using RosMessageTypes.DroneSimMessages;

public class SessionInfoPublisher : MonoBehaviour
{
	// ROS Variables
    ROSConnection ros;                                  // ROS Connection
    public string topicName = "/session/data";          // Topic name
    public float publishMessageFrequency = 0.05f;       // Publish the message every N seconds
    private float timeElapsed;                          // Used to determine how much time has elapsed since the last message was published
	[Tooltip("Drone status topic name (the topic name will look like this: /<drone_id>/<Drone Status Topic Name>")]
	public string droneStatusTopicName = "status";		// Drone status topic name (the total topic name will look like this: /<drone_id>/<droneStatusTopicName>
	
	[SerializeField] Session sessionScript;
	
	[SerializeField] bool initializeOnStart = false;
	bool initialised = false;
	
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
			// Publish session info
            PublishSessionData();
			// Publish each drone status
			for(int i = 0; i < sessionScript.drones.Count; i++)
			{
				PublishDroneStatus(i);
			}
            timeElapsed = 0;
        }
    }
	
	public void Init()
    {
		if(!initialised)
		{
			ros = ROSConnection.GetOrCreateInstance();
			ros.RegisterPublisher<SessionInfoMsg>(topicName);
			foreach(GameObject drone in sessionScript.drones)
			{
				ros.RegisterPublisher<BoolMsg>("/" + drone.name + "/" + droneStatusTopicName);
			}
			initialised = true;
		}
    }
	
	// Function to publish general information about the session
	void PublishSessionData()
    {
		SessionInfoMsg message = new SessionInfoMsg(sessionScript.hasEnded, sessionScript.currentStage, sessionScript.runCount);

        ros.Publish(topicName, message);
    }
	
	// Function to publish specific drone status (whether its gameobject is active or not)
	void PublishDroneStatus(int droneIndex)
	{
		string topName = "/" + sessionScript.drones[droneIndex].name + "/status";
		
		BoolMsg message = new BoolMsg(sessionScript.drones[droneIndex].activeSelf);
		
		ros.Publish(topName, message);
	}
}

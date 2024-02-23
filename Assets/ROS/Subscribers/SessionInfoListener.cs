using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;
using RosMessageTypes.DroneSimMessages;

public class SessionInfoListener : MonoBehaviour
{
    public string topicName = "/session/data";
	public Dictionary<string, bool> readyDrones = new Dictionary<string, bool>();
	
	[SerializeField] bool initializeOnStart = false;
	
	public SessionInfoPublisher sessionInfoPublisher;
	public Session sessionScript;
	
	public bool waitForDroneReport = true;
	
    // Start is called before the first frame update
    void Start()
    {
        if(initializeOnStart)
		{
			Init();
		}
    }
	
	public void Init()
    {
		foreach(KeyValuePair<string, bool> drone in readyDrones)
		{
			string topName = "/" + drone.Key + "/" + sessionInfoPublisher.droneStatusTopicName;
			ROSConnection.GetOrCreateInstance().Subscribe<DroneStatusMsg>(topName, ListenerFunction);
		}
    }
	
	void ListenerFunction(DroneStatusMsg message)
    {
		readyDrones[message.id] = message.active;
		
		// Check if drones are ready for the episode
		foreach(KeyValuePair<string, bool> drone in readyDrones)
		{
			if(drone.Value == false && waitForDroneReport)
			{
				return;
			}
		}
		Debug.Log("Starting next episode");
		sessionScript.StartSession(true);
    }
}

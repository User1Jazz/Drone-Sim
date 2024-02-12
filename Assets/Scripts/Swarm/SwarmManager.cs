using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmManager : MonoBehaviour
{
    public List<GameObject> drones;
    public List<Transform> startPlatforms;
    public int swarmSize = 5;
    public GameObject dronePrefab;
    public bool deploySwarmOnStart = true;
    public float deployOffset = 0.1f;
	public List<Transform> targets;
	public bool randomlyGetTargets = true;

    public bool manualControl = false;
	
	public Session session;
	public SessionInfoPublisher sessionInfoPublisher;
	public SessionInfoListener sessionInfoListener;
	
	public bool startSessionOnStart = true;

    // Start is called before the first frame update
    void Start()
    {
        if (deploySwarmOnStart)
        {
            if (startPlatforms.Count <= 0)
                Debug.LogError("Number of start platforms is lower than expected. Make sure that the number of platforms is above 0. Current number of platforms: " + startPlatforms.Count);

            if (swarmSize <= 0)
                Debug.LogError("Swarm size is lower than 1. Increase the number of swarm members to deploy swarm. Current number of platforms: " + startPlatforms.Count);

            if (swarmSize > startPlatforms.Count)
                Debug.LogWarning("Swarm size exceeds the number of platforms. Expected number of platforms: " + swarmSize + ", current number of platforms: " + startPlatforms.Count);

            if (swarmSize < startPlatforms.Count)
                Debug.LogWarning("Swarm size is lower than the number of platforms. Increase the swarm size to use all platforms. Current swarm size: " + swarmSize + ", current number of platforms: " + startPlatforms.Count);

            DeploySwarm();
			if(startSessionOnStart)
			{
				StartEpisode(false);
			}
			sessionInfoPublisher.Init();
			sessionInfoListener.Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DeploySwarm()
    {
        for (int i = 0; i < swarmSize; i++)
        {
            if (i < startPlatforms.Count)
            {
                drones.Add(InitiateDrone("D" + i, startPlatforms[i]));
				session.drones.Add(drones[i]);
				session.previousDroneStatus.Add(true);
            }
        }
        Debug.Log("Swarm of size " + drones.Count + " deployed");
    }

    GameObject InitiateDrone(string droneID, Transform startPlatform)
    {
        Vector3 deployPosition = new Vector3(startPlatform.position.x, startPlatform.position.y + startPlatform.localScale.y + deployOffset, startPlatform.position.z);
        GameObject drone = Instantiate(dronePrefab, deployPosition, startPlatform.rotation);

        drone.name = droneID;

        drone.GetComponent<DroneSensorsNode>().topicName = "/" + droneID + "/data";
        drone.GetComponent<DroneControlNode>().topicName = "/" + droneID + "/cmd";
		drone.GetComponent<RewardPublisher>().topicName = "/" + droneID + "/reward";
		drone.GetComponent<DroneTargetPublisher>().topicName = "/" + droneID + "/target";
        drone.GetComponent<ManualControlNode>().topicName = "/" + droneID + "/cmd";
        drone.GetComponent<ManualControlNode>().manualMode = manualControl;
		drone.GetComponent<RewardCalculator>().session = session;
		drone.GetComponent<RewardCalculator>().randomlyGenerateTargets = randomlyGetTargets;
		
		if(targets.Count > 0)
		{
			Transform tgt = RequestNextTarget();
			drone.GetComponent<DroneTargetPublisher>().targetPosition = tgt.position;
			drone.GetComponent<RewardCalculator>().swarmManager = this;
			drone.GetComponent<RewardCalculator>().generateTargetOnStart = false;
			drone.GetComponent<RewardCalculator>().targetPosition = tgt.position;
		}
		
		drone.GetComponent<RewardCalculator>().ResetTimer();
        drone.GetComponent<DroneSensorsNode>().Init();
        drone.GetComponent<DroneControlNode>().Init();
		drone.GetComponent<RewardPublisher>().Init();
		drone.GetComponent<DroneTargetPublisher>().Init();
        drone.GetComponent<ManualControlNode>().Init();
		
		sessionInfoListener.readyDrones.Add(droneID, true);

        return drone;
    }
	
	public Transform RequestNextTarget()
	{
		return targets[Random.Range(0,targets.Count-1)];
	}
	
	public void RedeploySwarm()
	{
		for(int i = 0; i < drones.Count; i++)
		{
			drones[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
			drones[i].transform.position = new Vector3(startPlatforms[i].position.x, startPlatforms[i].position.y + startPlatforms[i].localScale.y + deployOffset, startPlatforms[i].position.z);
			drones[i].transform.rotation = startPlatforms[i].rotation;
			drones[i].SetActive(true);
			drones[i].GetComponent<RewardCalculator>().ResetTimer();
		}
        Debug.Log("Swarm of size " + drones.Count + " re-deployed");
	}
	
	public void StartEpisode(bool redeployDrones)
	{
		session.StartSession(redeployDrones);
	}
	
	void OnDrawGizmos()
    {
		if(targets.Count > 0)
		{
			foreach(Transform target in targets)
			{
				if(target != null)
				{
					// Draw a yellow sphere at the transform's position
					Gizmos.color = Color.yellow;
					Gizmos.DrawSphere(target.position, 0.1f);
				}
			}			
		}
    }
}

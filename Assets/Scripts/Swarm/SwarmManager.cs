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
	public GameObject targetObject;
	public bool spawnTargetObject = false;
	public Transform targetObjectsParent;
	bool targetsDespawned = true;
	[Range(0f,1f)]
	public float targetMarkerSize = 0.2f;
	public bool randomlyGetTargets = true;

    public bool manualControl = false;
	
	public Session session;
	public SessionInfoPublisher sessionInfoPublisher;
	public SessionInfoListener sessionInfoListener;
	
	public bool startSessionOnStart = true;
	
	public bool endOnCollision = true;

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
                drones.Add(InitiateDrone(i, "D" + i, startPlatforms[i]));
				session.drones.Add(drones[i]);
				session.previousDroneStatus.Add(true);
            }
        }
        Debug.Log("Swarm of size " + drones.Count + " deployed");
    }

    GameObject InitiateDrone(int droneNum, string droneID, Transform startPlatform)
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
		drone.GetComponent<RewardCalculator>().endOnCollision = endOnCollision;
		drone.GetComponent<RewardCalculator>().session = session;
		drone.GetComponent<RewardCalculator>().randomlyGenerateTargets = randomlyGetTargets;
		
		if(targets.Count > 0)
		{
			// Handle target
			Transform tgt = RequestNextTarget(droneNum);
			if(spawnTargetObject)
			{
				GameObject target = Instantiate(targetObject, tgt.position, Quaternion.identity, targetObjectsParent);
				target.transform.localScale = new Vector3(targetMarkerSize+0.2f, targetMarkerSize+0.2f, targetMarkerSize+0.2f);
			}
			drone.GetComponent<DroneTargetPublisher>().targetPosition = tgt.position;
			drone.GetComponent<RewardCalculator>().droneID = droneNum;
			drone.GetComponent<RewardCalculator>().swarmManager = this;
			drone.GetComponent<RewardCalculator>().generateTargetOnStart = false;
			drone.GetComponent<RewardCalculator>().targetPosition = tgt.position;
		}
		
		drone.GetComponent<RewardCalculator>().Reset();
        drone.GetComponent<DroneSensorsNode>().Init();
        drone.GetComponent<DroneControlNode>().Init();
		drone.GetComponent<RewardPublisher>().Init();
		drone.GetComponent<DroneTargetPublisher>().Init();
        drone.GetComponent<ManualControlNode>().Init();
		
		sessionInfoListener.readyDrones.Add(droneID, true);

        return drone;
    }
	
	public Transform RequestNextTarget(int droneID)
	{
		if(randomlyGetTargets)
		{
			return targets[Random.Range(0,targets.Count-1)];
		}else
		{
			return targets[droneID];
		}
		
	}
	
	public void RedeploySwarm()
	{
		for(int i = 0; i < drones.Count; i++)
		{
			drones[i].transform.position = new Vector3(startPlatforms[i].position.x, startPlatforms[i].position.y + startPlatforms[i].localScale.y + deployOffset, startPlatforms[i].position.z);
			drones[i].transform.rotation = startPlatforms[i].rotation;
			drones[i].GetComponent<DroneController>().linear = Vector3.zero;
			drones[i].GetComponent<DroneController>().angular = Vector3.zero;
			drones[i].SetActive(true);
			drones[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
			drones[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			drones[i].GetComponent<RewardCalculator>().endOnCollision = endOnCollision;
			drones[i].GetComponent<RewardCalculator>().Reset();
			
			if(spawnTargetObject)
			{
				GameObject target = Instantiate(targetObject, drones[i].GetComponent<DroneTargetPublisher>().targetPosition, Quaternion.identity, targetObjectsParent);
				target.transform.localScale = new Vector3(targetMarkerSize+0.2f, targetMarkerSize+0.2f, targetMarkerSize+0.2f);
				Debug.Log("Target spawned");
			}
		}
        Debug.Log("Swarm of size " + drones.Count + " re-deployed");
		targetsDespawned = false;
	}
	
	public void StartEpisode(bool redeployDrones)
	{
		session.StartSession(redeployDrones);
	}
	
	public void DeleteTargetMesh()
	{
		// Destroy previous targets
		if(targetObjectsParent.childCount > 0)
		{
			Debug.Log("Deleting targets...");
			for(int i = targetObjectsParent.childCount - 1; i >= 0; i--)
			{
				Destroy(targetObjectsParent.GetChild(i).gameObject);
			}
		}
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
					Gizmos.DrawSphere(target.position, targetMarkerSize);
				}
			}			
		}
    }
}

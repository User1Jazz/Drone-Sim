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

    public bool manualControl = false;

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
        drone.GetComponent<ManualControlNode>().topicName = "/" + droneID + "/cmd";
        drone.GetComponent<ManualControlNode>().manualMode = manualControl;

        drone.GetComponent<DroneSensorsNode>().Init();
        drone.GetComponent<DroneControlNode>().Init();
		drone.GetComponent<RewardPublisher>().Init();
        drone.GetComponent<ManualControlNode>().Init();


        return drone;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Session : MonoBehaviour
{
	public bool changeStageOnEnd = true;	// Toggle to change the stage at the end of the session
	public bool loopStages = true;			// Go back to stage 1 after the last stage has been passed
	public List<GameObject> stages;			// List of stages
	public int currentStage = 0;			// Current active stage
	public SwarmManager swarmManager;		// Reference to the swarm manager
	public bool hasEnded = false;			// session has-ended flag
	public bool goToTheNextStage = false;	// Toggle set by the drones to signal their completion
	public List<GameObject> drones;			// List of drones at runtime (for session management)
	public float secondsPerLevel = 60;		// Time given for every level
	public float levelTime = 0f;			// Stopwatch
	public int runCount = 0;
	
    // Start is called before the first frame update
    void Start()
    {
        ActivateStage();					// Activate the first stage
		hasEnded = false;					// Set the end flag to false (default)
		goToTheNextStage = false;			// Set the next stage flag to false (default)
		runCount = 0;						// Set runcount to 0 (default)
    }

    // Update is called once per frame
    void Update()
    {
		levelTime += Time.deltaTime;
		
		// Check session status and start next session if ended
        hasEnded = CheckSessionStatus();
		if(hasEnded)
		{
			StartSession();
			runCount++;
		}
    }
	
	// Function to start the session
	void StartSession()
	{
		levelTime = 0f;
		ActivateStage();
		swarmManager.RedeploySwarm();
		hasEnded = false;
		goToTheNextStage = false;
	}
	
	// Function to activate the current stage (and disable other stages)
	void ActivateStage()
	{
		for(int i = 0; i < stages.Count; i++)
		{
			if(i == currentStage)
			{
				stages[i].SetActive(true);
			}else
			{
				stages[i].SetActive(false);
			}
		}
	}
	
	// Function to check the session status
	bool CheckSessionStatus()
	{
		// If at least one drone is still active, the session is ongoing
		foreach(GameObject drone in drones)
		{
			if(drone.activeSelf == true)
			{
				return false;
			}
		}
		// Otherwise it is time to reset the stage (e.g. due to timeout) or go to the next stage (e.g. due to success)
		if(goToTheNextStage && changeStageOnEnd)
		{
			if(loopStages && currentStage >= stages.Count-1)
			{
				currentStage = 0;
			}
			else if(currentStage < stages.Count-1)
			{
				currentStage++;
			}
		}
		return true;
	}
}

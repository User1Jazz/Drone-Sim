using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceReward : RewardCalculator
{
	// Target position hyperparameters
	public Vector3 targetPosMin = new Vector3(-100f, 5f, -100f);
	public Vector3 targetPosMax = new Vector3(100f, 100f, 100f);
	public bool generateTargetOnStart = true;
	public bool randomlyGenerateTargets = false;
	public float successRadius = 2f;
	public float successReward = -100f;
	public float failureReward = 100f;
	public float episodeLifetime = 60f;
	public float gainTimeOnSuccess = 1f;
	
	
	// Runtime parameters
	public Vector3 targetPosition;
	Vector3 startPos;
	public float timeRemaining;
	public bool episodeOver = false;
	
	// Called on first frame update
	void Start()
	{
		timeRemaining = episodeLifetime;
		startPos = transform.position;
		if(generateTargetOnStart)
		{
			SetTargetPosition();
		}
	}
	
	// Called every frame update
	void Update()
	{
		if(timeRemaining > 0f)
		timeRemaining -= Time.deltaTime;
		
		if(Vector3.Distance(transform.position, targetPosition) < successRadius && randomlyGenerateTargets)
		{
			timeRemaining += gainTimeOnSuccess;
			SetTargetPosition();
		}
	}
	
	// Function to set the target position
	public void SetTargetPosition()
	{
		targetPosition = new Vector3(startPos.x + Random.Range(targetPosMin.x, targetPosMax.x), startPos.y + Random.Range(targetPosMin.y, targetPosMax.y), startPos.z + Random.Range(targetPosMin.z, targetPosMax.z));
	}
	
	// Function to calculate the reward
	public override float CalculateReward()
	{
		if(Vector3.Distance(transform.position, targetPosition) < successRadius && timeRemaining > 0f)
		{
			return successReward;
		}else if(timeRemaining > 0f)
		{
			return Vector3.Distance(transform.position, targetPosition) / Vector3.Distance(startPos, targetPosition) * 100f;
		}else
		{
			return Vector3.Distance(transform.position, targetPosition) / Vector3.Distance(startPos, targetPosition) * 100f * failureReward;
		}
	}
	
	void OnDrawGizmos()
    {
		if(targetPosition != null)
		{
			// Draw a yellow sphere at the transform's position
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(targetPosition, successRadius);			
		}
    }
}

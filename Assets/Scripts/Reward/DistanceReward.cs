using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceReward : RewardCalculator
{
	public float successRadius = 2f;
	private Vector3 previousPosition = Vector3.zero;
	private Vector3 pokePosition = Vector3.zero;
	public float idleTimeout = 1f;
	float positionTimeElapsed = 0f;
	float pokeTimeElapsed = 0f;
	
	
	// Called every frame update
	void Update()
	{
		if(timeRemaining > 0f)
		{
			if(collided)
			{
				collided = false;
				gameObject.SetActive(false);
			}
			timeRemaining -= Time.deltaTime;
		}
		else
		{
			gameObject.SetActive(false);
		}
		
		positionTimeElapsed += Time.deltaTime;
		if(positionTimeElapsed > ((idleTimeout+0.5f)/idleTimeout))
		{
			previousPosition = transform.position;
			positionTimeElapsed = 0f;
		}
		pokeTimeElapsed += Time.deltaTime;
		if(pokeTimeElapsed > idleTimeout)
		{
			pokePosition = previousPosition;
			pokeTimeElapsed = 0f;
		}
		
		if(Vector3.Distance(transform.position, targetPosition) < successRadius)
		{
			session.successfulRuns++;
			if(randomlyGenerateTargets)
			{
				timeRemaining += gainTimeOnSuccess;
				SetTargetPosition();
			}else
			{
				gameObject.SetActive(false);
			}
			
		}
	}
	
	// Function to calculate the reward
	public override float CalculateReward()
	{
		// Get the success reward on reaching the target
		if(Vector3.Distance(transform.position, targetPosition) < successRadius && timeRemaining > 0f)
		{
			session.goToTheNextStage = true;
			return successReward * rewardScale;
		}
		// Get runtime reward
		else if(timeRemaining > 0f)
		{
			// If collision reward enabled (see RewardCalculator script) and if collided, give punishment
			if(collided)
			{
				return failureReward * rewardScale;
			}
			// If within the radius of a circle where target is in the center and the start position is at the edge
			if(Vector3.Distance(targetPosition, transform.position) <= Vector3.Distance(targetPosition, startPos))
			{
				// To encourage the agent to move around (the 'poke' reward [i.e. punishment])
				if(Vector3.Distance(transform.position, pokePosition) <= 0.1f)
				{
					return (1f - Vector3.Distance(targetPosition, transform.position) / Vector3.Distance(targetPosition, startPos)) * failureReward * rewardScale;
				}
				// currentDist / startDist * successReward
				return (1f - Vector3.Distance(targetPosition, transform.position) / Vector3.Distance(targetPosition, startPos)) * successReward * rewardScale;
			}
			// If outside the circle, receive punishment reward
			else
			{
				// currentDist / startDist * failureReward
				return (1f - Vector3.Distance(transform.position, targetPosition) / Vector3.Distance(startPos, targetPosition)) * -failureReward * rewardScale;
			}
		}
		// Get punishment on timeout
		else
		{
			return failureReward * rewardScale;
		}
	}
	
	public override void OnDrawGizmos()
    {
		if(targetPosition != null)
		{
			// Draw a yellow sphere at the transform's position
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(targetPosition, successRadius);			
		}
		Gizmos.DrawWireSphere(pokePosition, 0.1f);
    }
}

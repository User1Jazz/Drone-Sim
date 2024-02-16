using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceReward : RewardCalculator
{
	public float successRadius = 2f;
	
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
		
		if(Vector3.Distance(transform.position, targetPosition) < successRadius)
		{
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
			return successReward;
		}
		// Get runtime reward
		else if(timeRemaining > 0f)
		{
			if(collided)
			{
				return Vector3.Distance(transform.position, targetPosition) * failureReward;
			}
			// If within the radius of a circle where target is in the center and the start position is at the edge
			if(Vector3.Distance(targetPosition, transform.position) <= Vector3.Distance(targetPosition, startPos))
			{
				// currentDist / startDist * successReward
				return (1f - Vector3.Distance(targetPosition, transform.position) / Vector3.Distance(targetPosition, startPos)) * successReward;
			}
			// If outside the circle, receive punishment reward
			else
			{
				// currentDist / startDist * failureReward
				return (1f - Vector3.Distance(transform.position, targetPosition) / Vector3.Distance(startPos, targetPosition)) * -failureReward;
			}
		}
		// Get the worst punishment on timeout
		else
		{
			return Vector3.Distance(transform.position, targetPosition) * failureReward;
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
    }
}

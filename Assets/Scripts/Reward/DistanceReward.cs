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
		if(Vector3.Distance(transform.position, targetPosition) < successRadius && timeRemaining > 0f)
		{
			session.goToTheNextStage = true;
			return successReward;
		}else if(timeRemaining > 0f)
		{
			return Vector3.Distance(transform.position, targetPosition) / Vector3.Distance(startPos, targetPosition) * 100f;
		}else
		{
			return Vector3.Distance(transform.position, targetPosition) / Vector3.Distance(startPos, targetPosition) * 100f * failureReward;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DynamicDistanceReward : RewardCalculator
{
	public float successRadius = 2f;
	private Vector3 previousPosition = Vector3.zero;
	private Vector3 pokePosition = Vector3.zero;
	public float idleTimeout = 1f;
	float positionTimeElapsed = 0f;
	float pokeTimeElapsed = 0f;
	public bool punishOnTimeout = false;
	public enum Axis {X, Y, Z};
	public Axis axis = Axis.X;
	
	
	// Called every frame update
	void Update()
	{
		if(timeRemaining > 0f)
		{
			if(collided || reachedFinish)
			{
				CalculateReward();
				if(endOnCollision)
				{
					gameObject.SetActive(false);
				}
			}
			timeRemaining -= Time.deltaTime;
		}
		else
		{
			gameObject.SetActive(false);
		}
		
		positionTimeElapsed += Time.deltaTime;
		if(positionTimeElapsed > idleTimeout)
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
	
	public override void Reset()
	{
		ResetTimer();
		collided = false;
		reachedFinish = false;
		pokePosition = transform.position;
		previousPosition = transform.position;
		rewardPublisherScript.GetReward();
	}
	
	// Function to calculate the reward
	public override float CalculateReward()
	{
		if(reachedFinish)
		{
			session.goToTheNextStage = true;
			return successReward * rewardScale;
		}
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
			return GetAxisReward(axis);
		}
		// Get punishment on timeout
		else if (punishOnTimeout)
		{
			return failureReward * rewardScale;
		}
		else
		{
			// If collision reward enabled (see RewardCalculator script) and if collided, give punishment
			if(collided)
			{
				return failureReward * rewardScale;
			}
			return GetAxisReward(axis);
		}
	}
	
	public float GetAxisReward(Axis _axis)
	{
		float target = 0f;
		float drone = 0f;
		float reference = 0f;
		if(_axis == Axis.X)
		{
			target = targetPosition.x;
			drone = transform.position.x;
			reference = pokePosition.x;
		}
		else if(_axis == Axis.Y)
		{
			target = targetPosition.y;
			drone = transform.position.y;
			reference = pokePosition.y;
		}
		else
		{
			target = targetPosition.z;
			drone = transform.position.z;
			reference = pokePosition.z;
		}
		if(reference > target)
		{
			if(drone < reference)
			{
				return Math.Abs((1f - Vector3.Distance(targetPosition, transform.position) / Vector3.Distance(targetPosition, startPos)) * successReward * rewardScale);
			}
			else if(drone > reference)
			{
				return -Math.Abs((1f - Vector3.Distance(pokePosition, transform.position))*0.5f * successReward * rewardScale); //-Math.Abs((1f - Vector3.Distance(targetPosition, transform.position) / Vector3.Distance(targetPosition, startPos)) * successReward * rewardScale);
			}
			else
			{
				return 0.0f;
			}
		}
		else
		{
			if(drone > reference)
			{
				return Math.Abs((1f - Vector3.Distance(targetPosition, transform.position) / Vector3.Distance(targetPosition, startPos)) * successReward * rewardScale);
			}
			else if(drone < reference)
			{
				return -Math.Abs((1f - Vector3.Distance(pokePosition, transform.position))*0.5f * successReward * rewardScale); //-Math.Abs((1f - Vector3.Distance(targetPosition, transform.position) / Vector3.Distance(targetPosition, startPos)) * successReward * rewardScale);
			}
			else
			{
				return 0.0f;
			}
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

using UnityEngine;

public class RewardCalculator : MonoBehaviour
{
	// Target position hyperparameters
	public Vector3 targetPosMin = new Vector3(-100f, 5f, -100f);
	public Vector3 targetPosMax = new Vector3(100f, 100f, 100f);
	public bool generateTargetOnStart = true;
	public bool randomlyGenerateTargets = false;
	public float successReward = 1f;
	public float failureReward = -1f;
	public float rewardScale = 1f;
	public float episodeLifetime = 60f;
	public float gainTimeOnSuccess = 1f;
	public DroneTargetPublisher targetPublisher;
	
	public SwarmManager swarmManager;
	public Session session;
	
	public bool endOnCollision = true;
	protected bool collided = false;
	
	// Runtime parameters
	public Vector3 targetPosition;
	protected Vector3 startPos;
	public float timeRemaining;
	public bool episodeOver = false;
	
	// Called on first frame update
	void Start()
	{
		ResetTimer();
		startPos = transform.position;
		if(generateTargetOnStart)
		{
			SetTargetPosition();
		}
	}
	
	void Update()
	{
		if(timeRemaining > 0f)
			timeRemaining -= episodeLifetime - session.levelTime;
	}
	
	// Function to set the target position
	public virtual void SetTargetPosition()
	{
		if(swarmManager == null)
		{
			targetPosition = new Vector3(startPos.x + Random.Range(targetPosMin.x, targetPosMax.x), startPos.y + Random.Range(targetPosMin.y, targetPosMax.y), startPos.z + Random.Range(targetPosMin.z, targetPosMax.z));
			targetPublisher.targetPosition = targetPosition;
		}
		else
		{
			Vector3 tgt = swarmManager.RequestNextTarget().position;
			targetPosition = tgt;
			targetPublisher.targetPosition = targetPosition;
		}
		
	}
	
	public void Reset()
	{
		ResetTimer();
		collided = false;
	}
	
	public void ResetTimer()
	{
		episodeLifetime = session.secondsPerLevel;
		timeRemaining = session.secondsPerLevel;
	}
	
    public virtual float CalculateReward()
	{
		return 0f;
	}
	
	protected void OnCollisionEnter(Collision collision)
    {
		collided = true;
    }
	
	protected void OnCollisionExit(Collision collision)
    {
		collided = false;
    }
	
	public virtual void OnDrawGizmos()
    {
		if(targetPosition != null)
		{
			// Draw a yellow sphere at the transform's position
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(targetPosition, 0.25f);			
		}
    }
}

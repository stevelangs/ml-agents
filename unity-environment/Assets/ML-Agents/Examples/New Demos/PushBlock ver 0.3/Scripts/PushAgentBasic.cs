//Put this script on your blue cube.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAgentBasic : Agent
{
	public GameObject ground; //ground game object. we will use the area bounds to spawn the blocks
	[HideInInspector]
	public Bounds areaBounds; //the bounds of the pushblock area
	PushBlockAcademy academy;
	public GameObject goal; //goal to push the block to
    public GameObject block; //the orange block we are going to be pushing
    public GameObject obstacle; //don't let the block touch the obstacle
	[HideInInspector]
	public GoalDetect goalDetect; //this script detects when the block touches the goal
	Rigidbody blockRB;  //cached on initialization
	Rigidbody agentRB;  //cached on initialization
	Material groundMaterial; //cached on Awake()
	Renderer groundRenderer; //we will be changing the ground material based on success/failue

	void Awake()
	{
		brain = FindObjectOfType<Brain>(); //only one brain in the scene so this should find our brain. BRAAAINS.
		academy = FindObjectOfType<PushBlockAcademy>(); //cache the academy
	}

    public override void InitializeAgent()
    {
		base.InitializeAgent();
		goalDetect = block.GetComponent<GoalDetect>();
		goalDetect.agent = this; 
		agentRB	= GetComponent<Rigidbody>(); //cache the agent rigidbody
		blockRB	= block.GetComponent<Rigidbody>(); //cache the block rigidbody
		areaBounds = ground.GetComponent<Collider>().bounds; //get the ground's bounds
		groundRenderer = ground.GetComponent<Renderer>(); //get the ground renderer so we can change the material when a goal is scored
		groundMaterial = groundRenderer.material; //starting material
    }

	public override List<float> CollectState()
	{

		Vector3 agentPosRelToGoal = agentRB.position - goal.transform.position; //vector to agent from goal
		Vector3 blockPosRelToGoal = blockRB.position - goal.transform.position; //vector to blockRB from goal
		Vector3 blockPosRelToObstacle = blockRB.position - obstacle.transform.position; //vector to blockRB from goal
		Vector3 blockPosRelToAgent = blockRB.position - agentRB.position; //vector to blockRB from agent
		Vector3 obstaclePosRelToAgent = obstacle.transform.position - agentRB.position; //vector to blockRB from agent
		
		Vector3 agentPos = agentRB.position - ground.transform.position; //vector to blockRB from agent
		Vector3 goalPos = goal.transform.position - ground.transform.position;  //pos of goal rel to ground
		Vector3 blockPos = blockRB.transform.position - ground.transform.position;  //pos of goal rel to ground
		Vector3 obstaclePos = obstacle.transform.position - ground.transform.position;  //pos of goal rel to ground

		//COLLECTING STATES
		MLAgentsHelpers.CollectVector3State(state, agentPos);  //pos of agent rel to ground
		MLAgentsHelpers.CollectVector3State(state, goalPos);  //pos of goal rel to ground
		MLAgentsHelpers.CollectVector3State(state, blockPos);  //pos of block rel to ground
		MLAgentsHelpers.CollectVector3State(state, obstaclePos);  //pos of obstacl rel to ground

		MLAgentsHelpers.CollectVector3State(state, agentPosRelToGoal);  //vector to agent from goal
		MLAgentsHelpers.CollectVector3State(state, blockPosRelToGoal); //vector to blockRB from goal
		MLAgentsHelpers.CollectVector3State(state, blockPosRelToObstacle); //vector to blockRB from ostacle
		MLAgentsHelpers.CollectVector3State(state, blockPosRelToAgent);  //vector to blockRB from agent
		MLAgentsHelpers.CollectVector3State(state, obstaclePosRelToAgent);  //vector to obstacle from agent

		MLAgentsHelpers.CollectVector3State(state, blockRB.velocity); //block's vel
		MLAgentsHelpers.CollectVector3State(state, agentRB.velocity); //agent's vel
		// MLAgentsHelpers.CollectRotationState(state, agentRB.transform); //agent's rotation


		return state;
	}

	//use the ground's bounds to pick a random spawn pos
    public Vector3 GetRandomSpawnPos(float spawnHeight)
    {
        Vector3 randomSpawnPos = Vector3.zero;
        float randomPosX = Random.Range(-areaBounds.extents.x * academy.spawnAreaMarginMultiplier, areaBounds.extents.x * academy.spawnAreaMarginMultiplier);
        float randomPosZ = Random.Range(-areaBounds.extents.z * academy.spawnAreaMarginMultiplier, areaBounds.extents.z * academy.spawnAreaMarginMultiplier);
        randomSpawnPos = ground.transform.position + new Vector3(randomPosX, spawnHeight, randomPosZ );
        return randomSpawnPos;
    }

	//woot
	public void IScoredAGoal()
	{
		reward += 5; //you get a point
		done = true; //if we mark an agent as done it will be reset automatically. AgentReset() will be called.
		StartCoroutine(GoalScoredSwapGroundMaterial(academy.goalScoredMaterial, 1)); //swap ground material for a bit to indicate we scored.

	}

	//orange block touched the red obstacle
	public void BlockTouchedObstacle()
	{
		reward -= 5; //you get a point
		done = true; //if we mark an agent as done it will be reset automatically. AgentReset() will be called.
		StartCoroutine(GoalScoredSwapGroundMaterial(academy.failMaterial, 1)); //swap ground material for a bit to indicate we scored.

	}

	//swap ground material, wait time seconds, then swap back to the regular ground material.
	IEnumerator GoalScoredSwapGroundMaterial(Material mat, float time)
	{
		groundRenderer.material = mat;
		yield return new WaitForSeconds(time); //wait for 2 sec
		groundRenderer.material = groundMaterial;
	}


	public void MoveAgent(float[] act) 
	{
	
		//AGENT ACTIONS
		// this is where we define the actions our agent can use...stuff like "go left", "go forward", "turn" ...etc.

        if (brain.brainParameters.actionSpaceType == StateType.continuous)
        {
			//Continuous control means we are letting the neural network set the direction on a sliding scale. 
			//We will define the number of "slots" we want to use here. In this example we need 2 "slots" to define:
			//right/left movement (act[0])
			//forward/back movement (act[1])
				
			//Example: Right/Left Movement. It is defined in this line:
			//Vector3 directionX = Vector3.right * Mathf.Clamp(act[0], -1f, 1f);

			//The neural network is setting the act[0] value using a float in between -1 & 1. 
			//If it chooses 1 then the agent will go right. 
			//If it chooses -1 the agent will go left. 
			//If it chooses .42 then it will go a little bit right
			//If it chooses -.8 then it will go left (well...80% left)
			
			// //Energy Conservation Penalties
			// //Give penalties based on how fast the agent chooses to go. The agent should only exert as much energy as necessary.
			// //This is how animals work as well. i.e. You're probably not running in place at all times
			if(act[0] != 0) //left/right movement
			{
				reward -= Mathf.Abs(act[0])/10000; //the larger the movement, the more penalty given. I chose to divide it by 1000 based on trial & error.
			}
			if(act[1] != 0) //forward/back movement
			{
				reward -=  Mathf.Abs(act[1])/10000;//the larger the movement, the more penalty given. I chose to divide it by 1000 based on trial & error.
			}
			 
			Vector3 directionX = Vector3.right * Mathf.Clamp(act[0], -1f, 1f);  //go left or right in world space
            Vector3 directionZ = Vector3.forward * Mathf.Clamp(act[1], -1f, 1f); //go forward or back in world space
			// Vector3 directionX = Vector3.right * act[0];  //go left or right in world space
            // Vector3 directionZ = Vector3.forward * act[1]; //go forward or back in world space
        	Vector3 dirToGo = directionX + directionZ; //add them together. this is the dir we want to go
			agentRB.AddForce(dirToGo * academy.agentRunSpeed, ForceMode.VelocityChange); //GO


			if(dirToGo != Vector3.zero)
			{
				agentRB.rotation = Quaternion.Lerp(agentRB.rotation, Quaternion.LookRotation(dirToGo), Time.deltaTime * academy.agentRotationSpeed);
			}

        }
    }

	public override void AgentStep(float[] act)
	{

        MoveAgent(act); //perform agent actions
		reward -= .002f; // penalty given each step so agent doesn't just stand around doing nothing
		// reward -= .0005f; // penalty given each step so agent doesn't just stand around doing nothing

		bool fail = false;  // did the agent or block get pushed off the edge?

		if (!Physics.Raycast(agentRB.position, Vector3.down, 3)) //if the agent has gone over the edge, we done.
		{
			fail = true; //fell off bro
			reward -= 1f; // BAD AGENT
			transform.position =  GetRandomSpawnPos(1.5f); //if we fell off we need to reset our position.
			done = true; //if we mark an agent as done it will be reset automatically. AgentReset() will be called.
		}

		if (!Physics.Raycast(blockRB.position, Vector3.down, 3)) //if the block has gone over the edge, we done.
		{
			fail = true; //fell off bro
			reward -= 1f; // BAD AGENT
			ResetBlock(); //reset block pos
			done = true; //if we mark an agent as done it will be reset automatically. AgentReset() will be called.
		}

		if (fail)
		{
			StartCoroutine(GoalScoredSwapGroundMaterial(academy.failMaterial, 1f)); //swap ground material to indicate fail
		}
	}
	
	void ResetBlock()
	{
		block.transform.position = GetRandomSpawnPos(1.5f); //get a random pos
        blockRB.velocity = Vector3.zero; //reset vel back to zero
        blockRB.angularVelocity = Vector3.zero; //reset angVel back to zero
	}


	//In the editor, if "Reset On Done" is checked then AgentReset() will be called automatically anytime we mark done = true in an agent script.
	public override void AgentReset()
	{
		ResetBlock();
		goal.transform.position = GetRandomSpawnPos(.35f); //pick random spawn pos with a y height of .35f. Height was chosen based on trial/error.
		obstacle.transform.position = GetRandomSpawnPos(.5f); //pick random spawn pos with a y height of .5f. Height was chosen based on trial/error.
	}
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSoccer : Agent
{

	public enum Team
    {
        red, blue
    }
	public enum AgentRole
    {
        striker, defender, goalie
    }
	// ReadRewardData readRewardData;
	public Team team;
	public AgentRole agentRole;
	// public float teamFloat;
	// public float playerID;
	public int playerIndex;
	public SoccerFieldArea area;
	[HideInInspector]
	public Rigidbody agentRB;
	public bool showRaycastRays;
	// [HideInInspector]
	// public Vector3 startingPos;

	public List<float> myState = new List<float>(); //list for state data. to be updated every FixedUpdate in this script
	SoccerAcademy academy;
	Renderer renderer;
	public Vector3 playerDirToTargetGoal;
	public Vector3 playerDirToDefendGoal;

	public float agentEnergy = 100;
	public bool tired = false;
    string[] detectableObjects  = {"ball", "wall", "redAgent", "blueAgent"};
    // string[] detectableObjects  = {"ball", "wall", "agent"};

	public void ChooseRandomTeam()
	{
		team = (Team)Random.Range(0,2);
		renderer.material = team == Team.red? academy.redMaterial: academy.blueMaterial;
		// area.playerStates[playerIndex].defendGoal = team == Team.red? area.redGoal: area.blueGoal;
	}

	public void JoinRedTeam(AgentRole role)
	{
		agentRole = role;
		team = Team.red;
		// area.playerStates[playerIndex].a
		renderer.material = academy.redMaterial;
	}

	public void JoinBlueTeam(AgentRole role)
	{
		agentRole = role;
		team = Team.blue;
		// area.playerStates[playerIndex].a
		renderer.material = academy.blueMaterial;
	}

    void Awake()
    {
		agentEnergy = 100; 
		renderer = GetComponent<Renderer>();
		academy = FindObjectOfType<SoccerAcademy>(); //get the academy
		// readRewardData = FindObjectOfType<ReadRewardData>(); //get reward data script


		// brain = agentRole == AgentRole.striker? academy.redBrainStriker: agentRole == AgentRole.defender? academy.redBrainDefender: academy.redBrainGoalie;
		brain = agentRole == AgentRole.striker? academy.brainStriker: agentRole == AgentRole.defender? academy.redBrainDefender: academy.brainGoalie;
		PlayerState playerState = new PlayerState();
		// playerState.teamFloat = 0;
		// teamFloat = 0;
		// playerState.agentRoleFloat = agentRole == AgentRole.striker? 0: agentRole == AgentRole.defender? 1: 2;
		// playerState.currentTeamFloat = (float)Random.Range(0,2); //return either a 0 or 1 * max is exclusive ex: Random.Range(0,10) will pick a int between 0-9
		// playerState.agentScript.team = (Team)Random.Range(0,2);

		// ChooseRandomTeam();
		// SetPlayerColor();
		maxStep = academy.maxAgentSteps;
		// playerState.playerID = area.redPlayers.Count; //float id used to id individual
		// playerID = playerState.playerID;
		playerState.agentRB = GetComponent<Rigidbody>(); //cache the RB
		agentRB = GetComponent<Rigidbody>(); //cache the RB
		agentRB.maxAngularVelocity = 50;
		playerState.startingPos = transform.position;
		playerState.agentScript = this;
		// playerState.targetGoal = area.blueGoal;
		// playerState.defendGoal = area.redGoal;
		// area.redPlayers.Add(playerState);
		area.playerStates.Add(playerState);
		playerIndex = area.playerStates.IndexOf(playerState);
		playerState.playerIndex = playerIndex;

		// //we need to set up each player. most of this is unused right now but will be useful when we start collecting other player's states
        // if(team == Team.red)
        // {
		// 	// brain = agentRole == AgentRole.striker? academy.redBrainStriker: agentRole == AgentRole.defender? academy.redBrainDefender: academy.redBrainGoalie;
		// 	// brain = agentRole == AgentRole.striker? academy.brainStriker: agentRole == AgentRole.defender? academy.redBrainDefender: academy.brainGoalie;
		// 	// PlayerState playerState = new PlayerState();
		// 	playerState.currentTeamFloat = 0;
		// 	// teamFloat = 0;
		// 	// playerState.agentRoleFloat = agentRole == AgentRole.striker? 0: agentRole == AgentRole.defender? 1: 2;
		// 	// maxStep = academy.maxAgentSteps;
		// 	// playerState.playerID = area.redPlayers.Count; //float id used to id individual
		// 	// playerID = playerState.playerID;
		// 	// playerState.agentRB = GetComponent<Rigidbody>(); //cache the RB
		// 	// agentRB = GetComponent<Rigidbody>(); //cache the RB
		// 	// playerState.startingPos = transform.position;
		// 	// playerState.agentScript = this;
		// 	// playerState.targetGoal = area.blueGoal;
		// 	// playerState.defendGoal = area.redGoal;
        //     // area.redPlayers.Add(playerState);
        //     area.playerStates.Add(playerState);
		// 	playerIndex = area.playerStates.IndexOf(playerState);
		// 	playerState.playerIndex = playerIndex;

        // }
        // else if(team == Team.blue)
        // {
		// 	// brain = agentRole == AgentRole.striker? academy.blueBrainStriker: agentRole == AgentRole.defender? academy.blueBrainDefender: academy.blueBrainGoalie;
		// 	brain = agentRole == AgentRole.striker? academy.brainStriker: agentRole == AgentRole.defender? academy.blueBrainDefender: academy.brainGoalie;
		// 	PlayerState playerState = new PlayerState();
		// 	playerState.currentTeamFloat = 1;
		// 	teamFloat = 1;
		// 	playerState.agentRoleFloat = agentRole == AgentRole.striker? 0: agentRole == AgentRole.defender? 1: 2;
		// 	maxStep = academy.maxAgentSteps;
		// 	playerState.playerID = area.bluePlayers.Count; //float id used to id individual
		// 	playerID = playerState.playerID;
		// 	playerState.agentRB = GetComponent<Rigidbody>(); //cache the RB
		// 	agentRB = GetComponent<Rigidbody>(); //cache the RB
		// 	playerState.startingPos = transform.position;
		// 	playerState.agentScript = this;
		// 	// playerState.targetGoal = area.redGoal;
		// 	// playerState.defendGoal = area.blueGoal;
        //     // area.bluePlayers.Add(playerState);
        //     area.playerStates.Add(playerState);
		// 	playerIndex = area.playerStates.IndexOf(playerState);
		// 	playerState.playerIndex = playerIndex;
        // }


		// //we need to set up each player. most of this is unused right now but will be useful when we start collecting other player's states
        // if(team == Team.red)
        // {
		// 	// brain = agentRole == AgentRole.striker? academy.redBrainStriker: agentRole == AgentRole.defender? academy.redBrainDefender: academy.redBrainGoalie;
		// 	brain = agentRole == AgentRole.striker? academy.brainStriker: agentRole == AgentRole.defender? academy.redBrainDefender: academy.brainGoalie;
		// 	PlayerState playerState = new PlayerState();
		// 	playerState.teamFloat = 0;
		// 	teamFloat = 0;
		// 	playerState.agentRoleFloat = agentRole == AgentRole.striker? 0: agentRole == AgentRole.defender? 1: 2;
		// 	maxStep = academy.maxAgentSteps;
		// 	playerState.playerID = area.redPlayers.Count; //float id used to id individual
		// 	playerID = playerState.playerID;
		// 	playerState.agentRB = GetComponent<Rigidbody>(); //cache the RB
		// 	agentRB = GetComponent<Rigidbody>(); //cache the RB
		// 	playerState.startingPos = transform.position;
		// 	playerState.agentScript = this;
		// 	playerState.targetGoal = area.blueGoal;
		// 	playerState.defendGoal = area.redGoal;
        //     area.redPlayers.Add(playerState);
        //     area.playerStates.Add(playerState);
		// 	playerIndex = area.playerStates.IndexOf(playerState);
		// 	playerState.playerIndex = playerIndex;

        // }
        // else if(team == Team.blue)
        // {
		// 	// brain = agentRole == AgentRole.striker? academy.blueBrainStriker: agentRole == AgentRole.defender? academy.blueBrainDefender: academy.blueBrainGoalie;
		// 	brain = agentRole == AgentRole.striker? academy.brainStriker: agentRole == AgentRole.defender? academy.blueBrainDefender: academy.brainGoalie;
		// 	PlayerState playerState = new PlayerState();
		// 	playerState.teamFloat = 1;
		// 	teamFloat = 1;
		// 	playerState.agentRoleFloat = agentRole == AgentRole.striker? 0: agentRole == AgentRole.defender? 1: 2;
		// 	maxStep = academy.maxAgentSteps;
		// 	playerState.playerID = area.bluePlayers.Count; //float id used to id individual
		// 	playerID = playerState.playerID;
		// 	playerState.agentRB = GetComponent<Rigidbody>(); //cache the RB
		// 	agentRB = GetComponent<Rigidbody>(); //cache the RB
		// 	playerState.startingPos = transform.position;
		// 	playerState.agentScript = this;
		// 	playerState.targetGoal = area.redGoal;
		// 	playerState.defendGoal = area.blueGoal;
        //     area.bluePlayers.Add(playerState);
        //     area.playerStates.Add(playerState);
		// 	playerIndex = area.playerStates.IndexOf(playerState);
		// 	playerState.playerIndex = playerIndex;
        // }

        // startingPos = transform.position; //cache the starting pos in case we want to spawn players back at their startingpos
    }

    public override void InitializeAgent()
    {
		base.InitializeAgent();
    }

  	public override List<float> CollectState()
    {
		// myState = area.playerStates[playerIndex].state; //states for all players are collected in the SoccerFieldArea script. we can pull this player's state by index. This will be useful when we are tracking other players
		// state.AddRange(myState);
		// myState = area.playerStates[playerIndex].state; //states for all players are collected in the SoccerFieldArea script. we can pull this player's state by index. This will be useful when we are tracking other players
		// area.CollectPlayerState(area.playerStates[playerIndex]);

   playerDirToTargetGoal = Vector3.zero; //set the target goal based on which team this player is currently on
         playerDirToDefendGoal = Vector3.zero;//set the defend goal based on which team this player is currently on
        // Vector3 playerDirToTargetGoal = Vector3.zero; //set the target goal based on which team this player is currently on
        // var playerTeam = ps.tea
        // if(ps.currentTeamFloat == 0)//I'm on the red team
        // blueBallPosReward = (15 - (ballRB.position - redGoal.position).magnitude)/15;
        // redBallPosReward = (15 - (ballRB.position - blueGoal.position).magnitude)/15;
        // // print ("blueBallPosReward: " + blueBallPosReward);
        // print ("redBallPosReward: " + redBallPosReward);
        // float ballDistFromRedGoal = (ballRB.position - ground.transform.position).magnitude;
        // float ballDistFromBlueGoal = (redGoal.position - ground.transform.position).magnitude;
        Vector3 ballDirToTargetGoal = Vector3.zero;
        Vector3 ballDirToDefendGoal = Vector3.zero;
        if(team == AgentSoccer.Team.red)//I'm on the red team
        {
            playerDirToTargetGoal = area.blueGoal.position - agentRB.position;
           playerDirToDefendGoal = area.redGoal.position - agentRB.position;
            ballDirToTargetGoal = area.blueGoal.position - area.ballRB.position;
            ballDirToDefendGoal =  area.redGoal.position - area.ballRB.position;
            // ps.ballPosReward = redBallPosReward;


        }
        if(team == AgentSoccer.Team.blue)//I'm on the blue team
        {
            playerDirToTargetGoal = area.redGoal.position - agentRB.position;
            playerDirToDefendGoal = area.blueGoal.position - agentRB.position;
            ballDirToTargetGoal = area.redGoal.position - area.ballRB.position;
            ballDirToDefendGoal =  area.blueGoal.position - area.ballRB.position;
            // ps.ballPosReward = blueBallPosReward;
        }

        // // Vector3 playerDirToDefendGoal = Vector3.zero;//set the defend goal based on which team this player is currently on
        // if(ps.agentScript.team == AgentSoccer.Team.red)//I'm on the red team
        // {
        // }
        // if(ps.agentScript.team == AgentSoccer.Team.blue)//I'm on the blue team
        // {
        // }
        // Vector3 playerDirToDefendGoal = ps.defendGoal.transform.position - ps.agentRB.position;
        Vector3 playerPos = agentRB.position - area.ground.transform.position;
        Vector3 ballPos = area.ballRB.position - area.ground.transform.position;
        Vector3 playerDirToBall = area.ballRB.position - agentRB.position;
        // Vector3 playerDirToRedGoal = redGoal.transform.position - ps.agentRB.position;
        // Vector3 playerDirToBlueGoal = blueGoal.transform.position - ps.agentRB.position;
        // Vector3 playerPos = ps.agentRB.position - ground.transform.position;
        // Vector3 playerDirToBall = ballRB.position - ps.agentRB.position;

        // float playerDistToBall = playerDirToBall.sqrMagnitude;
        // ps.state.Add(playerDistToBall);

        // Vector3 redGoalPosition = redGoal.transform.position - ground.transform.position;
        // Vector3 blueGoalPosition = blueGoal.transform.position - ground.transform.position;
        // Vector3 ballDirToRedGoal = redGoal.transform.position - ballRB.position;
        // Vector3 ballDirToBlueGoal = blueGoal.transform.position - ballRB.position;
        // Vector3 ballDirToTargetGoal = ps.targetGoal.transform.position - ballRB.position;
        // Vector3 ballDirToDefendGoal =  ps.defendGoal.transform.position - ballRB.position;

        // ps.state.Clear(); //instead of creating a new list each tick we will reuse this one
        // ps.state.Add(ps.playerID); //whoami 
        // ps.state.Add(ps.currentTeamFloat); //which team 
        // ps.state.Add(ps.agentRoleFloat);
        //capture the agent's current role
        // float currentRole = ps.agentScript.agentRole == AgentSoccer.AgentRole.striker? 1: ps.agentScript.agentRole == AgentSoccer.AgentRole.defender? 2: ps.agentScript.agentRole == AgentSoccer.AgentRole.goalie? 3: 0;
        
        //capture the agent's current team
        // float currentTeam = team == AgentSoccer.Team.red? 1: team == AgentSoccer.Team.blue? 2: 0;
        // ps.state.Add(currentRole);
        // ps.state.Add(currentTeam);
        // ps.state.Add(ps.ballPosReward);
        // MLAgentsHelpers.ConvertBoolToFloat
        MLAgentsHelpers.CollectVector3State(state, agentRB.velocity); //agent's vel
         MLAgentsHelpers.CollectRotationState(state, agentRB.transform); //agent's rotation
         MLAgentsHelpers.CollectVector3State(state, playerPos); //player abs position rel to field
        // CollectVector3State(ps.state, ballPos); //dir from player to red goal
         MLAgentsHelpers.CollectVector3State(state, playerDirToBall); //dir from agent to ball
        // CollectVector3State(ps.state, redGoalPosition);  //red goal abs position
        // CollectVector3State(ps.state, blueGoalPosition); //blue goal abs position
        MLAgentsHelpers.CollectVector3State(state,  playerDirToTargetGoal); //dir from player to red goal
        MLAgentsHelpers.CollectVector3State(state,  playerDirToDefendGoal); //dir from player to blue goal
        MLAgentsHelpers.CollectVector3State(state, ballDirToTargetGoal); //dir from ball to target goal
        MLAgentsHelpers.CollectVector3State(state, ballDirToDefendGoal); //dir from ball to defend goal


        // CollectVector3State(ps.state, ballDirToRedGoal); //dir from ball to target goal
        // CollectVector3State(ps.state, ballDirToBlueGoal); //dir from ball to defend goal
        // CollectVector3State(ps.state, playerDirToRedGoal); //dir from player to red goal
        // CollectVector3State(ps.state, playerDirToBlueGoal); //dir from player to blue goal
        // CollectVector3State(ps.state, ballDirToRedGoal); //dir from ball to target goal
        // CollectVector3State(ps.state, ballDirToBlueGoal); //dir from ball to defend goal


        // CollectVector3State(ps.state, ps.targetGoal.transform.position - ps.agentRB.position); //dir from player to target goal
        // CollectVector3State(ps.state, ps.defendGoal.transform.position - ps.agentRB.position); //dir from player to defend goal
        // CollectVector3State(ps.state, ps.targetGoal.transform.position - ballRB.position); //dir from ball to target goal
        // CollectVector3State(ps.state, ps.defendGoal.transform.position - ballRB.position); //dir from ball to defend goal
         MLAgentsHelpers.CollectVector3State(state, area.ballRB.velocity);


		// state.AddRange(area.playerStates[playerIndex].state);
		RaycastAndAddState(agentRB.transform.position, transform.forward); //forward
		RaycastAndAddState(agentRB.transform.position, transform.forward + transform.right); //right forward
		RaycastAndAddState(agentRB.transform.position, transform.right); //right
		RaycastAndAddState(agentRB.transform.position, transform.forward - transform.right); //left forward
		RaycastAndAddState(agentRB.transform.position, -transform.right); //left

		state.Add(agentEnergy/100);
		return state;
	}





  	// public override List<float> CollectState()
    // {
	// 	// myState = area.playerStates[playerIndex].state; //states for all players are collected in the SoccerFieldArea script. we can pull this player's state by index. This will be useful when we are tracking other players
	// 	// state.AddRange(myState);
	// 	// myState = area.playerStates[playerIndex].state; //states for all players are collected in the SoccerFieldArea script. we can pull this player's state by index. This will be useful when we are tracking other players
	// 	area.CollectPlayerState(area.playerStates[playerIndex]);
	// 	state.AddRange(area.playerStates[playerIndex].state);
	// 	RaycastAndAddState(agentRB.transform.position, transform.forward); //forward

	// 	return state;
	// }

	public void RaycastAndAddState(Vector3 pos, Vector3 dir)
	{
		RaycastHit hit;
		// float hitDist = 5; //how far away was it. if nothing was hit then this will return our max raycast dist (which is 10 right now)
		// float hitObjHeight = 0;
		// float raycastDist;
		if(showRaycastRays)
		{
			Debug.DrawRay(pos, dir * 30, Color.green, .1f, true);
			// print("drawing debug rays");
		}

		// float[] subList = new float[detectableObjects.Length + 5];
		float[] subList = new float[detectableObjects.Length + 2];
		//bit array looks like this
		// [walkableSurface, avoidObstacle, nothing hit, distance] if true 1, else 0
		// [0] walkableSurface
		// [1] walkableSurface
		// [2] no hit
		// [3] hit distance
		var noHitIndex = detectableObjects.Length; //if we didn't hit anything this will be 1
		var hitDistIndex = detectableObjects.Length + 1; //if we hit something the distance will be stored here.
		// var hitNormalX = detectableObjects.Length + 2; //if we hit something the distance will be stored here.
		// var hitNormalY = detectableObjects.Length + 3; //if we hit something the distance will be stored here.
		// var hitNormalZ = detectableObjects.Length + 4; //if we hit something the distance will be stored here.

		// string[] detectableObjects  = { "banana", "agent", "wall", "badBanana", "frozenAgent" };

		// if (Physics.SphereCast(transform.position, 1.0f, position, out hit, rayDistance))
		if (Physics.Raycast(pos, dir, out hit, 30)) // raycast forward to look for walls
		// if (Physics.SphereCast(transform.position, 1.0f, position, out hit, rayDistance))
		{
			for (int i = 0; i < detectableObjects.Length; i++)
			{
				if (hit.collider.gameObject.CompareTag(detectableObjects[i]))
				{
					subList[i] = 1;  //tag hit
					// print("raycast hit: " + detectableObjects[i]);
					subList[hitDistIndex] = hit.distance / 30; //hit distance is stored in second to last pos
					
					// subList[hitNormalX] = hit.normal.x;
					// subList[hitNormalY] = hit.normal.y;
					// subList[hitNormalZ] = hit.normal.z;

					// if(team == Team.red && hit.collider.gameObject.CompareTag("redAgent"))
					// {
					// 	if(hit.distance < 5)
					// 	{
					// 		reward -= .001f;
					// 	}
					// }
					// if(team == Team.blue && hit.collider.gameObject.CompareTag("blueAgent"))
					// {
					// 	if(hit.distance < 5)
					// 	{
					// 		reward -= .001f;
					// 	}
					// }
					
					break;
				}
			}
		}
		else
		{
			subList[noHitIndex] = 1f; //nothing hit
		}
		// stateArray = subList; //for debug
		// print(stateArray);
		state.AddRange(new List<float>(subList));  //adding n = detectableObjects.Length + 2 items to the state



		// if (Physics.Raycast(pos, dir, out hit, 5)) // raycast forward to look for walls
		// {
		// 	if(hit.collider.CompareTag("walkableSurface"))
		// 	{
		// 		hitDist = hit.distance;
		// 		hitObjHeight = hit.transform.localScale.y;
		// 		// print(hit.collider.name + hit.distance);
		// 	}
		// 	if(hit.collider.CompareTag("avoidObstacle"))
		// 	{
		// 		hitDist = hit.distance;
		// 		hitObjHeight = hit.transform.localScale.y;
		// 		// print(hit.collider.name + hit.distance);
		// 	}
		// }
		// else //nothing hit
		// {

		// }
		// // state.Add(didWeHitSomething);
		// state.Add(hitDist);
		// state.Add(hitObjHeight);
	}
	public void MoveAgent(float[] act) {

        if (brain.brainParameters.actionSpaceType == StateType.continuous)
        {

				reward -= Mathf.Abs(act[0])/10000; //conserve energy
				reward -= Mathf.Abs(act[1])/10000; //conserve energy
				reward -= Mathf.Abs(act[2])/10000; //conserve energy
			// if(act[0] != 0)
			// {
			// 	float energyConservationPentalty = Mathf.Abs(act[0])/10000;
			// 	// print("act[0] = " + act[0]);
			// 	reward -= energyConservationPentalty;
			// 	// reward -= .0001f;
			// }
			// if(act[1] != 0)
			// {
			// 	float energyConservationPentalty = Mathf.Abs(act[1])/10000;
			// 	// print("act[1] = " + act[1]);
			// 	reward -= energyConservationPentalty;
			// }
			// if(act[0] != 0)
			// {
			// if(!tired && agentEnergy > 0)
			// reward -= 1 - agentEnergy/100;
			if(agentEnergy <= 100f)
			{
				if(act[0] < .25f && act[1]< .25f)
				{
					agentEnergy += 2f; //recharge
				}
				if(act[0] > .25f || act[1] > .25f)
				{
					agentEnergy -= Mathf.Abs(act[0]/4);
					agentEnergy -= Mathf.Abs(act[1]/4);
				}
			}
			else{
				agentEnergy = 100;
			}


			if(agentEnergy <= 0)
			{
				// if(act[0] > .25f || act[1] > .25f)
				// {
				// 	agentEnergy -= Mathf.Abs(act[0]/4);
				// 	agentEnergy -= Mathf.Abs(act[1]/4);
				// }
				agentEnergy = 100;
				reward -= .1f;
			// }
			// else{
			}


			// if(agentEnergy > 0)
			// {
				// agentEnergy -= Mathf.Abs(act[0]/2);
				// agentEnergy -= Mathf.Abs(act[1]/2);
				// agentEnergy += .001f; //regen a little bit of energy each step
				// agentEnergy -= Mathf.Abs(act[1])/100;


				Vector3 directionX = Vector3.right * Mathf.Clamp(act[0], -2, 2);  //go left or right in world space
				// float speedX
				// agentRB.AddForce(directionX * (agentEnergy/100) * Random.Range(1, 10), ForceMode.VelocityChange); //added random.range so not everyone moves the same
				agentRB.AddForce(directionX * (agentEnergy/100) * academy.agentRunSpeed, ForceMode.VelocityChange); //added random.range so not everyone moves the same
				// agentRB.AddForce(directionX * (academy.agentRunSpeed * (agentEnergy/100) * Random.Range(1, 5)), ForceMode.VelocityChange); //added random.range so not everyone moves the same
				// agentRB.AddForce(directionX * academy.agentRunSpeed, ForceMode.VelocityChange); //GO
				// Vector3 directionZ = Vector3.right * Mathf.Clamp(act[1], Random.Range(-1, 0), Random.Range(0,1));  //go left or right in world space
				Vector3 directionZ = Vector3.forward * Mathf.Clamp(act[1], -2, 2); //go forward or back in world space
				// agentRB.AddForce(directionZ * Random.Range(.3f, 1) * academy.agentRunSpeed, ForceMode.VelocityChange); //GO
				// agentRB.AddForce(directionZ * (agentEnergy/100) * Random.Range(1, 10), ForceMode.VelocityChange); //GO
				agentRB.AddForce(directionZ * (agentEnergy/100) * academy.agentRunSpeed, ForceMode.VelocityChange); //GO
				// agentRB.AddForce(directionZ * (academy.agentRunSpeed * (agentEnergy/100) * Random.Range(1, 5)), ForceMode.VelocityChange); //GO
				// agentRB.AddForce(directionZ * Random.Range(0, 1) * (academy.agentRunSpeed * Random.Range(0, 2)), ForceMode.VelocityChange); //GO
				// Vector3 dirToGo = (directionX * Random.Range(.3f, 1)) + (directionZ * Random.Range(.3f, 1)); //the dir we want to go
				Vector3 dirToGo = directionX + directionZ; //the dir we want to go
				// agentRB.AddForce(dirToGo * academy.agentRunSpeed, ForceMode.VelocityChange); //GO
				// if(dirToGo != Vector3.zero)
				// {
					// agentRB.MoveRotation(Quaternion.Lerp(agentRB.rotation, Quaternion.LookRotation(dirToGo.normalized), Time.deltaTime * academy.agentRotationSpeed));
					agentRB.AddTorque(transform.up * Mathf.Clamp(act[2], -1f, 1f) * academy.agentRotationSpeed, ForceMode.VelocityChange); //turn right or left

					// agentRB.rotation = Quaternion.Lerp(agentRB.rotation, Quaternion.LookRotation(dirToGo), Time.deltaTime * academy.agentRotationSpeed);
					// agentRB.rotation = Quaternion.LookRotation(dirToGo);
				// }
			// }
			// else
			// {
			// 	// agentEnergy += 50;
			// 	agentEnergy = 100;
			// 	// reward -= .01f; //you ran of energy. don't do that
			// 	// StartCoroutine(NeedToRest());
			// }


			// 	// print("act[0] = " + act[0]);
			// 	// reward -= energyConservationPentalty;
			// 	// reward -= .0001f;
			// }
			// if(act[1] != 0)
			// {
			// 	float energyConservationPentalty = Mathf.Abs(act[1])/10000;
			// 	// print("act[1] = " + act[1]);
			// 	// reward -= energyConservationPentalty;
			// }


			// if(act[2] != 0)
			// {
			// 	float energyConservationPentalty = Mathf.Abs(act[2])/1000;
			// 	// print("act[2] = " + act[2]);
			// 	reward -= energyConservationPentalty;
			// }
			// Vector3 directionX = Vector3.right * Mathf.Clamp(act[0], -1f, 1f);  //go left or right in world space
            // Vector3 directionZ = Vector3.forward * Mathf.Clamp(act[1], -1f, 1f); //go forward or back in world space
        	// Vector3 dirToGo = directionX + directionZ; //the dir we want to go
			// agentRB.AddForce(dirToGo * academy.agentRunSpeed, ForceMode.VelocityChange); //GO

			// agentRB.AddTorque(transform.up * Mathf.Clamp(act[2], -1f, 1f) * academy.agentRotationSpeed, ForceMode.VelocityChange); //turn right or left
			
			
			// Vector3 directionX = Vector3.right * act[0];  //go left or right in world space
            // Vector3 directionZ = Vector3.forward * act[1]; //go forward or back in world space
			// Vector3 directionX = Vector3.right * Mathf.Clamp(act[0], Random.Range(-1, 0), Random.Range(0,1));  //go left or right in world space

			// agentRB.transform.LookAt(dirToGo);

			// agentRB.AddTorque(transform.up * act[2] * ascademy.agentRotationSpeed, ForceMode.VelocityChange); //turn right or left

        }
    }


	// detect when we touch the goal
	// void OnCollisionEnter(Collision col)
	// {
	// 	// if(col.gameObject.CompareTag("wall")) //touched goal
	// 	// {
	// 	// 	reward -= .1f; //]
	// 	// 	// print("collided with avoidObstacle");
	// 	// 	// done = true; //if we mark an agent as done it will be reset automatically. AgentReset() will be called.
	// 	// 	// StartCoroutine(GoalScoredSwapGroundMaterial(academy.goalScoredMaterial, 2)); //swap ground material for a bit to indicate we scored.
	// 	// }
	// 	// if(col.gameObject.CompareTag("agent")) //touched goal
	// 	if(col.gameObject.CompareTag("redAgent") || col.gameObject.CompareTag("blueAgent")) //try to stay away from others
	// 	{
	// 		reward -= .0001f; //]
	// 		// print("collided with avoidObstacle");
	// 		// done = true; //if we mark an agent as done it will be reset automatically. AgentReset() will be called.
	// 		// StartCoroutine(GoalScoredSwapGroundMaterial(academy.goalScoredMaterial, 2)); //swap ground material for a bit to indicate we scored.
	// 	}
	// 	// if(col.gameObject.CompareTag("ball")) //touched goal
	// 	// {
	// 	// 	reward += .05f; //]
	// 	// 	// print("collided with avoidObstacle");
	// 	// 	// done = true; //if we mark an agent as done it will be reset automatically. AgentReset() will be called.
	// 	// 	// StartCoroutine(GoalScoredSwapGroundMaterial(academy.goalScoredMaterial, 2)); //swap ground material for a bit to indicate we scored.
	// 	// }
	// 	// if(col.gameObject.CompareTag("goal")) //touched goal
	// 	// {
	// 	// 	reward += 1; //you get a point
	// 	// 	done = true; //if we mark an agent as done it will be reset automatically. AgentReset() will be called.
	// 	// 	// StartCoroutine(GoalScoredSwapGroundMaterial(academy.goalScoredMaterial, 2)); //swap ground material for a bit to indicate we scored.
	// 	// }
	// }
	
	// IEnumerator NeedToRest()
	// {
	// 	tired = true;
	// 	yield return new WaitForSeconds(academy.coolDownTime);
	// 	tired = false;
	// 	agentEnergy = 100; //restore agent energy;
	// }

	public override void AgentStep(float[] act)
	{
		// print(readRewardData.currentMeanReward);
		// reward += .0001f; //mainly for goalies. not sure hssow this will affect offense. idea is to stay alive longer
		reward -= .001f; //hurry up
		// float 
		// if(agentRole == AgentRole.goalie)
		// {
		// 	reward += .0001f; //mainly for goalies. not sure how this will affect offense. idea is to stay alive longer
		// }
		// print()
        MoveAgent(act); //perform agent actions
		// if(agentRole == AgentRole.goalie)
		// {
		// 	if(playerDirToDefendGoal.sqrMagnitude < 4)
		// 	{
		// 		reward += .001f;  //COACH SAYS: good job
		// 	}
		// 	else
		// 	{
		// 		reward -= .001f; //COACH SAYS: stay by the goal idiot
		// 	}
		// }
		// float sqrMagnitudeFromAgentToBall = (area.ballRB.position - agentRB.position).sqrMagnitude;

		// if(sqrMagnitudeFromAgentToBall < 4)
		// {

		// reward += sqrMagnitudeFromAgentToBall/10000;
		// }
		// print()
		// reward += area.playerStates[playerIndex].ballPosReward/100;

		// if (!Physics.Raycast(agentRB.position, Vector3.down, 20)) //if the block has gone over the edge, we done.
		// {
		// 	// fail = true; //fell off bro
		// 	reward -= 1f; // BAD AGENT
		// 	// ResetBlock(shortBlockRB); //reset block pos
		// 	done = true; //if we mark an agent as done it will be reset automatically. AgentReset() will be called.
		// }
		// print(brain.name);
		// if(readRewardData.rewardDataDict.ContainsKey(brain.name) && readRewardData.rewardDataDict[brain.name].currentMeanReward > -.8)
		// {

		// 	// print("reward > .8");
		// }
	}

	public override void AgentReset()
	{
		transform.position =  area.GetRandomSpawnPos();
		agentRB.velocity = Vector3.zero; //we want the agent's vel to return to zero on reset
		if(academy.randomizePlayersTeamForTraining)
		{
			ChooseRandomTeam();
		}
		agentEnergy = 100;
	}


	public override void AgentOnDone()
	{

	}
}

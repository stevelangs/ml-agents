using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidAgent : Agent {
  	// [HideInInspector]
    public bool fell;
 	// public float strength;


	Vector3 past_velocity;
	// Brain agentBrain;
	public HumanoidAcademy academy;
	public Rigidbody[] bodyParts;
	// public List<Rigidbody> bodyParts = new List<Rigidbody>();
	public Rigidbody hips;
	public Rigidbody chest;
	public float targetHeightHips;
	public float targetHeightChest;
	public int bodyPartsCount;

	void Awake()
	{
		academy = FindObjectOfType<HumanoidAcademy>();
		brain = academy.brain;
		targetHeightChest = chest.position.y;
		targetHeightHips = hips.position.y;
		// Rigidbody[] bodyPartsTemp = GetComponentsInChildren<Rigidbody>(true);
		bodyParts = GetComponentsInChildren<Rigidbody>(true);
		// foreach(Rigidbody rb in bodyPartsTemp)
		// {
		// 	if(rb != hips)
		// 	{
		// 		bodyParts.Add(rb);
		// 	}
		// }
		foreach(Rigidbody rb in bodyParts)
		{
			rb.maxAngularVelocity = 8;
		}
		// agentBrain = FindObjectOfType<Brain>();
		bodyPartsCount = bodyParts.Length;
		// bodyPartsCount = bodyParts.Count;
	}
    public override void InitializeAgent()
    {
		base.InitializeAgent();
	}
	public override void AgentReset()
    {
		// print("Done. spawn a new player");
		academy.SpawnPlayerAndDestroyThisOne(this.gameObject);
    }

    public override List<float> CollectState()
    {
			// MLAgentsHelpers.CollectVector3State(state, hips.transform.position);
			// MLAgentsHelpers.CollectVector3State(state, hips.velocity);
			// MLAgentsHelpers.CollectVector3State(state, hips.angularVelocity);
			// MLAgentsHelpers.CollectRotationState(state, hips.transform);
		foreach(Rigidbody rb in bodyParts)
		{
			// MLAgentsHelpers.CollectVector3State(state, rb.transform.position);
			MLAgentsHelpers.CollectVector3State(state, rb.transform.localPosition);
			MLAgentsHelpers.CollectVector3State(state, rb.velocity);
			MLAgentsHelpers.CollectVector3State(state, rb.angularVelocity);
			MLAgentsHelpers.CollectLocalRotationState(state, rb.transform);
			// MLAgentsHelpers.CollectRotationState(state, rb.transform);
		}
			// MLAgentsHelpers.CollectVector3State(state, hips.transform.localPosition);
			// MLAgentsHelpers.CollectVector3State(state, hips.velocity);
			// MLAgentsHelpers.CollectVector3State(state, hips.angularVelocity);
			// MLAgentsHelpers.CollectRotationState(state, hips.transform);


		return state;

	}

	void MoveAgent(float[] act)
	{
		int actionLength = act.Length;
		// int bodyPartsArrayLength = bodyParts.Length;
		// print(bodyPartsArrayLength + " ");
		// int currentActionIndex = 0;
		for(int x = 0; x < bodyPartsCount - 1; x++)
		{
			var rb = bodyParts[x];
			rb.AddTorque(rb.transform.right * academy.strength * act[x * 3]);
			rb.AddTorque(rb.transform.up * academy.strength * act[(x * 3) + 1]);
			rb.AddTorque(rb.transform.forward * academy.strength * act[(x * 3) + 2]);
			// reward -= (Mathf.Abs(act[x * 3] + act[(x * 3) + 1] +  act[(x * 3) + 2]))/1000;
		}

		// hips.AddTorque(hips.transform.right * academy.strength * act[0], ForceMode.VelocityChange);
		// hips.AddTorque(hips.transform.up * academy.strength * act[1], ForceMode.VelocityChange);
		// hips.AddTorque(hips.transform.forward * academy.strength * act[2], ForceMode.VelocityChange);
		// foreach(Rigidbody rb in bodyParts)
		// {
		// 	rb.AddTorque(rb.transform.right * academy.strength * act)
		// }
	}
    public override void AgentStep(float[] act)
    {
		// print("agent step");
		MoveAgent(act);
		// reward -= chest.velocity.sqrMagnitude/10000;
		// if (!done)
        // {
			reward += chest.position.y/100;
			reward += hips.position.y/100;
		if(chest.position.y < targetHeightChest - .5f)
		{
			done = true;
			reward = -1;
		}

			// reward += targetHeightChest - chest.position.y;
			// reward += targetHeightHips - hips.position.y;
			// if(targetHeightChest - chest.position.y < .1f)
			// {
			// 	reward += .1f;
			// }
			// if(targetHeightHips - hips.position.y < .1f)
			// {
			// 	reward += .1f;
			// }

            // reward = (0
            // // - 0.01f * torque_penalty
            // + 1.0f * hips.GetComponent<Rigidbody>().velocity.x
            // // - 0.05f * Mathf.Abs(body.transform.position.z - body.transform.parent.transform.position.z)
            // // - 0.05f * Mathf.Abs(body.GetComponent<Rigidbody>().velocity.y)
            // );
        // }
		if (fell)
		{
			print("fell");
			done = true;
			reward = -1;
			fell = false;
		}

	}

	// void ResetPosition()



}

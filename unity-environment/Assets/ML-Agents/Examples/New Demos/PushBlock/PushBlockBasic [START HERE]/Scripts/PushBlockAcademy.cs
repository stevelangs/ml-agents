//Every scene needs an academy script. 
//Create an empty gameObject and attach this script.
//The brain needs to be a child of the Academy gameObject.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlockAcademy : Academy {

	public float agentRunSpeed; 
	public float agentRotationSpeed;
	public float spawnAreaMarginMultiplier; //ex: .9 means 90% of spawn area will be used.... .1 margin will be left (so players don't spawn off of the edge). the higher this value, the longer training time required
    public Material goalScoredMaterial; //when a goal is scored the ground will use this material for a few seconds.
    public Material failMaterial; //when fail, the ground will use this material for a few seconds. 

	public float gravityMultiplier; //use ~3 to make things less floaty


	void State()
	{
		Physics.gravity *= gravityMultiplier; //so things are less floaty.

	}
	public override void AcademyReset()
	{
	}

	public override void AcademyStep()
	{
	}

}

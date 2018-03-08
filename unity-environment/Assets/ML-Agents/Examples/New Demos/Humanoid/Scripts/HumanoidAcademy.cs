using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidAcademy : Academy {

	public GameObject humanoidPrefab;
	public float gravityMultiplier;
	public float strength;
	public Brain brain;
	// Use this for initialization
	void Start () {
		// SpawnPlayer();

		
	}
	
	public void SpawnPlayer()
	{

		print("spawn player");
		GameObject player = Instantiate(humanoidPrefab, new Vector3(0f,2.043f, 0f), Quaternion.identity);
		// player.GetComponent<HumanoidAgent>().brain = brain;

	}
	public void SpawnPlayerAndDestroyThisOne(GameObject oldPlayer)
	{
		// DestroyImmediate(oldPlayer);
		// Destroy(oldPlayer);
		// oldPlayer.SetActive(false);
		print("instantiate new player");
		GameObject player = Instantiate(humanoidPrefab, new Vector3(0f,2.043f, 0f), Quaternion.identity);
		// done = true;

		// player.GetComponent<HumanoidAgent>().brain = brain;

	}

	public override void InitializeAcademy()
	{
		SpawnPlayer();
		base.InitializeAcademy();
	}
	public override void AcademyReset()
	{

	}
	// Update is called once per frame
	void Update () {
		
	}





}

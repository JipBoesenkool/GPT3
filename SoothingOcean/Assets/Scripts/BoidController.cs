using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// these define the flock's behavior
/// </summary>
public class BoidController : MonoBehaviour
{
	public float minVelocity = 5;
	public float maxVelocity = 20;
	public float randomness = 1;
	public BoidFlocking prefab;
	public Transform target;

	internal Vector3 flockCenter;
	internal Vector3 flockVelocity;

	List<GameObject> school = new List<GameObject>();

	void Start()
	{
		
	}

	void Update()
	{
		Vector3 center = Vector3.zero;
		Vector3 velocity = Vector3.zero;
		foreach (GameObject boid in school)
		{
			center += boid.transform.localPosition;
			velocity += boid.GetComponent<Rigidbody>().velocity;
		}
		flockCenter = center / school.Count;
		flockVelocity = velocity / school.Count;
	}



	private void DebugSpawn(int flockSize){
		for (int i = 0; i < flockSize; i++)
		{
			BoidFlocking boid = Instantiate(prefab, transform.position, transform.rotation) as BoidFlocking;
			boid.transform.parent = transform;
			boid.transform.localPosition = new Vector3(
				Random.value * GetComponent<Collider>().bounds.size.x,
				Random.value * GetComponent<Collider>().bounds.size.y,
				Random.value * GetComponent<Collider>().bounds.size.z) - GetComponent<Collider>().bounds.extents;
			boid.controller = this;
			school.Add(boid.gameObject);
		}
	}

	public void AddFish( GameObject fish ){
		//set parent gameobject
		fish.transform.parent = transform;
		fish.GetComponent<Collider>().enabled = false;

		//destroy flock AI
		Flock fishFlock = fish.GetComponent<Flock>();
		fishFlock.RemoveFishFromManager ();
		Destroy (
			fishFlock
		);
			
		fish.AddComponent<BoidFlocking> ();

		//add to school
		school.Add(fish);
	}
}
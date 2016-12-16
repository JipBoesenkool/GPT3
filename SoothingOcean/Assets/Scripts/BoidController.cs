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
	public GameObject prefab;

	public GameObject player;
	public SphereCollider boundingBox;
	public Transform target;

	public GameObject debugFlockCenter;

	internal Vector3 flockCenter;
	internal Vector3 flockVelocity;
	internal Vector3 flockDir;

	public float turnSpeed;
	public float movementSpeed;

	List<GameObject> school = new List<GameObject>();

	void Start()
	{
		school.Add (player);
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

		if(debugFlockCenter != null){
			debugFlockCenter.transform.position = flockCenter;
		}

		InputTemp ();
	}
		
	private void InputTemp(){
		var v = -Input.GetAxis("Vertical"); // use the same axis that move back/forth
		var h = -Input.GetAxis("Horizontal"); // use the same axis that turns left/right

		Vector3 rot = debugFlockCenter.transform.rotation.eulerAngles;
		rot.x += v * turnSpeed;
		rot.y += -h * turnSpeed;

		float angle = rot.x;
		if (angle > 270f)
		{
			angle -= 360f;
		}

		//Debug.Log("angle: " + angle.ToString());
		if (angle < -89f || angle > 89f)
		{
			rot = debugFlockCenter.transform.rotation.eulerAngles;
		}

		debugFlockCenter.transform.rotation = Quaternion.Euler(rot);
		flockDir = debugFlockCenter.transform.forward * movementSpeed;
	}

	private void DebugSpawn(int flockSize){
		for (int i = 0; i < flockSize; i++)
		{
			GameObject boid = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
			boid.transform.parent = transform;
			boid.transform.localPosition = new Vector3(
				Random.value * GetComponent<Collider>().bounds.size.x,
				Random.value * GetComponent<Collider>().bounds.size.y,
				Random.value * GetComponent<Collider>().bounds.size.z) - GetComponent<Collider>().bounds.extents;
			boid.GetComponent<BoidFlocking>().controller = this;
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
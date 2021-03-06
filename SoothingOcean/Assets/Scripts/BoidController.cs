using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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

	public List<GameObject> school = new List<GameObject>();

	public FishstickCountScript guiScript;

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

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Menu");
        }

		InputController ();
	}
		
	private void InputController(){
		var v = -Input.GetAxis("Vertical"); // use the same axis that move back/forth
		var h = -Input.GetAxis("Horizontal"); // use the same axis that turns left/right

        //Voor touch movement
        if (Input.touchCount > 0)
        {
            h = -Input.touches[0].deltaPosition.x * 0.10f;
            v = -Input.touches[0].deltaPosition.y * 0.10f;
        }

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
		if(fishFlock == null){
			return;
		}
		fishFlock.RemoveFishFromManager ();
		Destroy (
			fishFlock
		);
			
		fish.AddComponent<BoidFlocking> ();

		//add to school
		school.Add(fish);


		//update points in gui
		guiScript.UpdatePoints ( CountPoints() );
	}

	public int CountPoints(){
		int total = 0;
		foreach(GameObject gobj in school){
			PointScript ps = gobj.GetComponent<PointScript> ();
			if(ps != null){
				total += gobj.GetComponent<PointScript> ().fishstickValue;
			}
			else{
				Debug.Log ("Missing pointscript");
			}

		}
		return total;
	}
}
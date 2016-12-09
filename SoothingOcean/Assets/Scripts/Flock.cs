using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {

	public GameObject parent;
	public FlockManager manager;

	public bool turning = false;

	public float rotationSpeed = 4f;

	public float speed;
	public float minSpeed;
	public float maxSpeed;

	public float neighbourDistance = 2f;
	public float avoidDistance	   = 1f;

	public Vector3 newGoalPos;

	// Use this for initialization
	void Start () {
		//random speed
		speed = Random.Range (minSpeed, maxSpeed);

		//get parent script
		parent = transform.parent.gameObject;
		manager = parent.GetComponent<FlockManager> ();

	}

	// Update is called once per frame
	void Update () {
		if (turning) {
			Vector3 direction = newGoalPos - transform.position;
			transform.rotation = Quaternion.Slerp (
				transform.rotation,
				Quaternion.LookRotation (direction),
				rotationSpeed * Time.deltaTime
			);
			speed = Random.Range (minSpeed, maxSpeed);
		} else {
			if(Random.Range(0,10) < 1){
				ApplyRules ();
			}
		}

		transform.Translate (0,0, Time.deltaTime * speed);
	}
		
	void OnTriggerEnter(Collider other){
		if(other.tag == "FishTank"){
			turning = false;
		}
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "FishTank"){
			newGoalPos = other.gameObject.transform.position;

			turning = true;
		}
	}

	private void ApplyRules(){
		GameObject[] gos;
		gos = manager.fishes;

		Vector3 vCentre = Vector3.zero;
		Vector3 vAvoid 	= Vector3.zero;
		float gSpeed = 0.1f;

		Vector3 goalPos = manager.goalPos;
		float dist;

		int groupSize = 0;
		foreach(GameObject go in gos){
			if(go != this.gameObject){
				dist = Vector3.Distance (
					go.transform.position,
					this.transform.position
				);

				if(dist <= neighbourDistance){
					vCentre += go.transform.position;
					groupSize++;

					if(dist < avoidDistance){
						vAvoid = vAvoid + (this.transform.position - go.transform.position);
					}

					Flock otherFlock = go.GetComponent<Flock> ();
					gSpeed += otherFlock.speed;
				}
			}
		}

		if(groupSize > 0){
			vCentre = vCentre/groupSize + (goalPos - this.transform.position);
			speed = gSpeed / groupSize;

			Vector3 direction = (vCentre + vAvoid) - transform.position;
			if(direction != Vector3.zero){
				transform.rotation = Quaternion.Slerp (
					transform.rotation,
					Quaternion.LookRotation(direction),
					rotationSpeed * Time.deltaTime
				);
			}
		}

	}
}

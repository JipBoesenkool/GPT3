using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {
	private Color32 _original;
	private Color32 _red 		= new Color32(160,22,22, 128);

	public GameObject parent;
	public FlockManager manager;

	public bool turning = false;
	public bool fleeing = false;

	public float rotationSpeed = 4f;

	public float speed;
	public float minSpeed;
	public float maxSpeed;

	public float neighbourDistance = 2f;
	public float avoidDistance	   = 1f;

	public Vector3 newGoalPos;

	public float fleeSpeedMod;
	public GameObject objToAvoid;
	public float avoidFleeDist;

	// Use this for initialization
	void Start () {
		_original = this.GetComponentInChildren<Renderer> ().material.color;

		//random speed
		speed = Random.Range (minSpeed, maxSpeed);

		//get parent script
		parent = transform.parent.gameObject;
		manager = parent.GetComponent<FlockManager> ();

	}

	// Update is called once per frame
	void Update () {
		float swimSpeed = speed;
		Vector3 direction = Vector3.zero;

		if (turning) {
			speed = Random.Range (minSpeed, maxSpeed);
			swimSpeed = speed;
			direction = newGoalPos - transform.position;
			Turn ( direction );
		} else if( fleeing ){
			//check if we still have to avoid the player
			float dist = Vector3.Distance(transform.position, objToAvoid.transform.position);
			if (dist > avoidFleeDist) {
				this.GetComponentInChildren<Renderer> ().material.color = _original; 
				fleeing = false;
			}

			//fleeDir
			direction = transform.position - objToAvoid.transform.position;
			swimSpeed *= fleeSpeedMod;
			Turn ( direction );
		} else {
			if(Random.Range(0,10) < 1){
				ApplyRules ();
			}
		}

		transform.Translate (0,0, Time.deltaTime * swimSpeed);
	}

	private void Turn( Vector3 dir ){
		transform.rotation = Quaternion.Slerp (
			transform.rotation,
			Quaternion.LookRotation (dir),
			rotationSpeed * Time.deltaTime
		);
	}
		
	void OnTriggerEnter(Collider other){
		if(other.tag == "FishTank"){
			turning = false;
		}

		if(other.tag == "Player"){
			Flee (other.gameObject);
		}
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "FishTank"){
			newGoalPos = other.gameObject.transform.position;

			turning = true;
		}
	}

	public void Flee( GameObject objToAvoid ){
		this.objToAvoid = objToAvoid;
		this.GetComponentInChildren<Renderer> ().material.color = _red;
		fleeing = true;
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

using UnityEngine;
using System.Collections;

public class FishAI : MonoBehaviour
{
	
	bool isMoving = false;

	public float speed = 10.0f;

	Transform newTarget;

	void Start() {
		
	}

	void Update () {
		if (isMoving == false) {
			newTarget = GameObject.Find ("Player").transform;
			isMoving = true;
		}

		transform.position = Vector3.MoveTowards(transform.position, newTarget.position, speed * Time.deltaTime);

		if (transform.position == newTarget.position) {
			isMoving = false;
		}
	}
}
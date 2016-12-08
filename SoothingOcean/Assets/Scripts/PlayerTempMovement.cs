using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTempMovement : MonoBehaviour {

	public float speed;
	public float rotateSpeed;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var v = Input.GetAxis("Vertical"); // use the same axis that move back/forth
		var h = Input.GetAxis("Horizontal"); // use the same axis that turns left/right

		Vector3 rot = transform.rotation.eulerAngles;
		rot.x += v * rotateSpeed;
		rot.y += -h * rotateSpeed;

		float angle = rot.x;
		if( angle > 270f ){
			angle -= 360f;
		}

		Debug.Log ("angle: " + angle.ToString());
		if( angle < -89f || angle > 89f ){
			rot = transform.rotation.eulerAngles;
		}

		transform.rotation = Quaternion.Euler (rot);

		transform.position += transform.forward * speed * Time.deltaTime;
	}
}

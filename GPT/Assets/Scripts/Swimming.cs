using UnityEngine;
using System.Collections;

public class Swimming : MonoBehaviour {
	float amplitudeX = 1.0f;
	float amplitudeY = 1f;
	float omegaX = 5.0f;
	float omegaY = 5.0f;
	float bobSpeed = 2f; //deltatime get devided by this higher is slower
	Vector3 startPos;
	float index;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
	}

	// Update is called once per frame
	void Update(){
		index += Time.deltaTime / bobSpeed;

		Vector3 currPos = transform.position;

		//set Y bob
		float x = amplitudeX*Mathf.Sin (omegaX*index);
		float y = amplitudeY*Mathf.Sin (omegaY*index);
		Vector3 newPos = new Vector3 (0,y,0);
		transform.position += (newPos * Time.deltaTime);

		newPos.x = x;

		//calculate look pos
		//look towards new position
		Vector3 lookPos = startPos - newPos;
		float angle = Mathf.Atan2(lookPos.y, lookPos.x);
		Debug.Log (angle);
	}
}

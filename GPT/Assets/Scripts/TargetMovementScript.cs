using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovementScript : MonoBehaviour {

    public bool inSchool;
    public GameObject leader;
    public float speed;

	// Use this for initialization
	void Start () {
        inSchool = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (inSchool == true)
        {
            this.transform.position = Vector2.MoveTowards((leader.transform.position - new Vector3(4, 0, 0)), this.transform.position, speed * Time.deltaTime);
        }
    }
}

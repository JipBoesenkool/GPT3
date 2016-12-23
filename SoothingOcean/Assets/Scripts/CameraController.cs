using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;
    public float smoothing = 5f;
    public int flocksize = 1;
    public int newFlock = 1;
    public int initialDistance = 20;

    Vector3 offset;
    Vector3 Distance;
    Vector3 increment;
	// Use this for initialization
	void Start () {
        Distance = new Vector3(0, 0, initialDistance);

        offset = transform.position - (transform.parent.position - (Distance));
	}
	
	// Update is called once per frame
	void Update () {

        float currentAngle = transform.eulerAngles.y;
        float desiredAngle = transform.parent.eulerAngles.y;
        float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * smoothing);

        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        if (flocksize !=  newFlock)
        {
            Distance = Distance +  new Vector3(0, 0, 5);
            offset = transform.position - (transform.parent.position - (Distance));
            flocksize = newFlock;
        }
        Debug.Log(offset);
        transform.position = transform.parent.position - (rotation * offset);

       // transform.LookAt(target.transform);

    }
}

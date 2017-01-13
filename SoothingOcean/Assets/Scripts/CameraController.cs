using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;
    public float rotationSmoothing;
    public float zoomSmoothing;
    public float zoomPerFish;

    public int initialDistance;
    Vector3 distanceVector;

    public BoidController bc;

    private float currentX;
    private float currentY; 

	// Use this for initialization
	void Start () {
        //Debug.Log(distance);
    }
	
	// Update is called once per frame
	void LateUpdate () {
        currentY = RotateAroundY();
        currentX = RotateAroundX();
        Quaternion rotation = Quaternion.Euler(target.eulerAngles.x, currentY, 0);

        //get current view distance
        distanceVector = new Vector3(0, 0, CalculateViewDistance());

        //from angle, find a position at distance BEHIND(that why -) target, look at target
        transform.position = target.position + (rotation * -distanceVector);
        transform.LookAt(target.transform);
    }

    float RotateAroundY()
    {
        float wantedY = target.eulerAngles.y;

        //Damp the rotation
        float angle = Mathf.LerpAngle(currentY, wantedY, Time.deltaTime * rotationSmoothing);
        //Debug.Log(angle);
        return angle;
    }

    float RotateAroundX()
    {
        float wantedX = target.eulerAngles.x;

        // Damp the height rotation
        float angle = Mathf.Lerp(currentX, wantedX, rotationSmoothing * Time.deltaTime);
        //Debug.Log(angle);
        return angle;
    }

    float CalculateViewDistance()
    {
        //Calculate distance wanted
        float distance;
        distance = initialDistance + zoomPerFish * bc.school.Count;

        //Debug.Log(distance);

        //Damped the transition if distance is changed
        distance =  Mathf.Lerp(distanceVector.z, distance, zoomSmoothing * Time.deltaTime);

        //Debug.Log(distance);

        return distance;
    }
}

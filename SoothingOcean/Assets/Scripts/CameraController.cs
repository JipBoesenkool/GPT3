using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;
    public float rotationSmoothing;
    public float heightSmoothing;
    public int flocksize = 1;
    public int newFlock = 1;
    public int initialDistance = 20;

    private float currentX;
    private float currentY;


    Vector3 distance;
    Vector3 increment;
	// Use this for initialization
	void Start () {
        distance = new Vector3(0, 0, initialDistance);
        Debug.Log(distance);
    }
	
	// Update is called once per frame
	void LateUpdate () {
        currentY = RotateAroundY();
        currentX = RotateAroundX();

        Quaternion rotation  = Quaternion.Euler(target.eulerAngles.x, currentY, 0);

        if (flocksize !=  newFlock)
        {
            distance = distance + new Vector3(0, 0, 3);
            flocksize = newFlock;
        }

        //Debug.Log(rotation);

        transform.position = target.position + (rotation * -distance);

       transform.LookAt(target.transform);
    }

    float RotateAroundY()
    {
        float desiredAngle = target.eulerAngles.y;

        //Damp the rotation
        float angle = Mathf.LerpAngle(currentY, desiredAngle, Time.deltaTime * rotationSmoothing);
        //Debug.Log(angle);
        return angle;
    }

    float RotateAroundX()
    {
        float wantedHeight = target.eulerAngles.x;

        // Damp the height rotation
        float angle = Mathf.Lerp(currentX, wantedHeight, heightSmoothing * Time.deltaTime);
        //Debug.Log(angle);
        return angle;
    }
}

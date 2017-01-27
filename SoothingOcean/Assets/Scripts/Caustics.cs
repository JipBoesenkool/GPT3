using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caustics : MonoBehaviour {

    public float maxDeviationX = 500;
    public float maxDeviationZ = 500;

    public float speedX = 10;
    public float speedZ = 20;

    private float startX;
    private float startZ;

    private bool goingUpX = false;
    private bool goingUpZ = false;


    void Start()
    {
        startX = transform.position.x;
        startZ = transform.position.z;
    }

    void Update()
    {
        if (goingUpX)
        {
            transform.Translate(Time.deltaTime * speedX, 0f, 0f);
            if (transform.position.x > startX + maxDeviationX) goingUpX = false;
        }
        else
        {
            transform.Translate(Time.deltaTime * -speedX, 0f, 0f);
            if (transform.position.x < startX - maxDeviationX) goingUpX = true;
        }
        if (goingUpZ)
        {
            transform.Translate(0f, Time.deltaTime * speedZ , 0f);
            if (transform.position.z > startZ + maxDeviationZ) goingUpZ = false;
        }
        else
        {
            transform.Translate(0f, Time.deltaTime * -speedZ, 0f);
            if (transform.position.z < startZ - maxDeviationZ) goingUpZ = true;
        }
    }
}

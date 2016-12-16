using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CausticsMobile : MonoBehaviour {

    public float maxDeviationX;
    public float maxDeviationZ;

    public float speedX;
    public float speedZ;

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

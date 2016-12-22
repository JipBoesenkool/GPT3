using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRandomRotator : MonoBehaviour {

    public string tagToRotate;

    private GameObject[] objectsToRotate;

    // Use this for initialization
    void Start()
    {
<<<<<<< HEAD
        objectsToRotate = GameObject.FindGameObjectsWithTag(tagToRotate);//Get all objects to rotate
=======
        objectsToRotate = GameObject.FindGameObjectsWithTag(tagToRotate);
>>>>>>> origin/master

        foreach (GameObject objectToRotate in objectsToRotate)
        {
            objectToRotate.transform.Rotate(0f, Random.Range(0, 360), 0f);//give random rotation between 0 and 360 degrees on the y axis
        }
    }
}

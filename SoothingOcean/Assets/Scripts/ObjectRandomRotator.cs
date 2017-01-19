using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRandomRotator : MonoBehaviour {

    public string tagToRotate;

    private GameObject[] objectsToRotate;

    public bool tipOver;
    public int deviation;

    // Use this for initialization
    void Start()
    {
        objectsToRotate = GameObject.FindGameObjectsWithTag(tagToRotate);//Get all objects to rotate

        foreach (GameObject objectToRotate in objectsToRotate)
        {
            if (tipOver)
            {
                objectToRotate.transform.Rotate(Random.Range(-deviation, deviation+1), 0f, Random.Range(-deviation, deviation+1));
            }
            objectToRotate.transform.Rotate(0f, Random.Range(0, 361), 0f);//give random rotation between 0 and 360 degrees on the y axis
        }
    }
}

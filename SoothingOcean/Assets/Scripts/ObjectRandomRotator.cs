using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRandomRotator : MonoBehaviour {

    public string tagToRotate;

    private GameObject[] objectsToRotate;

    // Use this for initialization
    void Start()
    {
        objectsToRotate = GameObject.FindGameObjectsWithTag(tagToRotate);

        foreach (GameObject objectToRotate in objectsToRotate)
        {
            objectToRotate.transform.Rotate(0f, Random.Range(0, 360), 0f);
        }
    }
}

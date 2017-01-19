using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObjectOnTerrain : MonoBehaviour {

    public float offset;

	void Start () {

        // raycast to find the y-position of the masked collider at the transforms x/z
        RaycastHit hit;
        // note that the ray starts at 100 units
        Ray ray = new Ray(transform.position + Vector3.up * 10, Vector3.down);

        if (Physics.Raycast(ray, out hit)) {        
             if (hit.collider != null) {
                 // this is where the gameobject is actually put on the ground
                 transform.position = new Vector3 (transform.position.x, hit.point.y + offset, transform.position.z);
             }
         }

	}
}

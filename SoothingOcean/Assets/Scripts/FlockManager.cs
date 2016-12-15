using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour {
	private Color32 _original;
	private Color32 _red 		= new Color32(160,22,22, 128);

	public GameObject debugPoint;
	public Vector3 tankSize;

	public GameObject fishPrefab;
	public int numFish = 10;
	public GameObject[] fishes;

	public Vector3 goalPos = Vector3.zero;

	// Use this for initialization
	void Start () {
		//init fields
		fishes  = new GameObject[numFish];

		//get tanksize (bounding box)
		tankSize = this.GetComponent<BoxCollider> ().size;
		tankSize /= 2;

		//spawn fishes
		for(int i = 0; i < numFish; i++){
			Vector3 pos = randomPos ();
			fishes [i] = (GameObject)Instantiate (
				fishPrefab,
				pos,
				Quaternion.identity
			);
			fishes[i].transform.parent = this.transform;
		}

		//goalPos
		goalPos = randomPos ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Random.Range(0,10000) < 50){
			goalPos = randomPos ();

			if(debugPoint != null){
				debugPoint.transform.position = goalPos;
			}
		}
	}

	private Vector3 randomPos(){
		return new Vector3 (
			Random.Range(-tankSize.x, tankSize.x) + transform.position.x,
			Random.Range(-tankSize.y, tankSize.y) + transform.position.y,
			Random.Range(-tankSize.z, tankSize.z) + transform.position.z
		);
	}
}

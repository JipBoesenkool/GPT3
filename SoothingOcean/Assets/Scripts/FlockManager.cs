using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour {
	private Color32 _original;
	private Color32 _red 		= new Color32(160,22,22, 128);

	public FishSpawner fSpawner;
	//static demo
	public SpawnData demoSpawnData;
	public bool 	 demo;

	public GameObject debugPoint;
	public Vector3 tankSize;

	public List<GameObject> fishes;

	public Vector3 goalPos = Vector3.zero;
	public bool 	isActive = false;

	// Use this for initialization
	void Start () {
		//init fields
		fishes = new List<GameObject>();

		//get tanksize (bounding box)
		tankSize = this.GetComponent<BoxCollider> ().size;
		tankSize /= 2;

		//goalPos
		goalPos = randomPos ();

		if(demo){
			Spawn (demoSpawnData, transform.position);
		}
	}

	public void Spawn( SpawnData sd, Vector3 spawnerPos ){
		//init variables
		this.transform.position = spawnerPos;

		//spawn fishes
		for(int i = 0; i < sd.fishAmount; i++){
			Vector3 fishPos = randomPos ();
			GameObject fish = (GameObject)Instantiate (
				sd.fishPrefab,
				fishPos,
				Quaternion.identity
			);

			fish.transform.parent = this.transform;

			//set scale
			fish.transform.localScale *= sd.fishSize;

			//set points of the fish
			PointScript ps = fish.GetComponent<PointScript>();
			if (ps == null) {
				Debug.Log ("Fish prefab does not have a point script");
				return;
			} else {
				ps.fishstickValue = sd.pointsPerFish;
			}

			//add to list
			fishes.Add (fish);
		}

		isActive = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(isActive){
			//check if it should be active
			if(!demo && Vector3.Distance(fSpawner.player.transform.position, transform.position) > fSpawner.GetMaxRange()){
				Deactivate ();
				return;
			}

			//change goal position 
			if(Random.Range(0,10000) < 50){
				goalPos = randomPos ();

				if(debugPoint != null){
					debugPoint.transform.position = goalPos;
				}
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

	private void Deactivate(){
		//Clean up old fishes
		isActive = false;
		for(int i = 0; i < fishes.Count; i++){
			Destroy(fishes[i]);
		}
		fishes.Clear();

		fSpawner.Spawn (this);
	}

	public void RemoveFish( GameObject fish ){
		fishes.Remove (fish);
	}
}

using UnityEngine;

/// <summary>
/// 1. Maak een empty game object en noem het SpawnManager.
/// 2. Voeg het player object toe aan dit script.
/// 3. Voeg de spawnable fish prefabs voor iedere layer toe aan het prefabs array.
/// </summary>

public class FishSpawner : MonoBehaviour
{
	public GameObject player;			// Player object om de spawn posities te bepalen.
	public int spawnDataIndex;

	//flockmanager object pool
	public GameObject flockManPrefab;
	public SpawnData[] spawnData;		// spawn data voor iedere spawn layer.
	public int flockManagersAmount;

	public float sensorMin = float.MaxValue;
	public float sensorMax = float.MinValue;
	public float spawnMultiplier = 1.0f;

	void Start ()
	{
		sensorMin = float.MaxValue;
		sensorMax = float.MinValue;
		spawnMultiplier = 1.0f;

		//init the fish tanks
		for(int i = 0; i < flockManagersAmount; i++){
			FlockManager fm = Instantiate (
				flockManPrefab,
				Vector3.zero,
				Quaternion.identity
			).GetComponent<FlockManager>();
			fm.transform.SetParent(this.transform);

			fm.fSpawner = this;

			Spawn (fm);
		}
	}

	// Spawned de betreffende objecten.
	public void Spawn (FlockManager fm)
	{
		// Positie van de speler binnen halen..
		Vector3 playerPos = player.transform.position;

		//check in welke layer de speler zwemt.
		if(playerPos.y > 430){			//flying fish!!!
			return;
		}else if(playerPos.y < 160){	//layer 3
			spawnDataIndex = 3;
		}else if(playerPos.y < 250){	//layer 2
			spawnDataIndex = 2;
		}else if(playerPos.y < 340){	//layer 1
			spawnDataIndex = 1;
		}else if(playerPos.y < 430){	//layer 0
			spawnDataIndex = 0;
		}

		// Get random position
		Vector3 randomPos = GetRandomPositionInRange(
			playerPos, 
			spawnData[spawnDataIndex].minRange, 
			spawnData[spawnDataIndex].maxRange
		);

		//get Y height
		RaycastHit hit;
		Ray ray = new Ray(new Vector3(randomPos.x,410f,randomPos.z), Vector3.down);
		if (Physics.Raycast(ray, out hit, 420f)) {
			randomPos.y = hit.point.y + 20f;
		}

		// set fish tank.
		fm.Spawn(
			spawnData[spawnDataIndex],
			randomPos
		);
	}

	Vector3 GetRandomPositionInRange(Vector3 playerPos ,float minRange, float maxRange){
		float magnitude = maxRange - minRange;
		Vector3 minDistance = new Vector3 (minRange,minRange,minRange);
		Vector3 randomPos = (Random.insideUnitSphere * magnitude) + minDistance;
		return (randomPos + playerPos);
	}

	public int GetMaxRange(){
		return spawnData [spawnDataIndex].maxRange;
	}

	public void SetSensorAverage(float s){
		if(s > sensorMax){
			sensorMax = s;
		}
		else if(s < sensorMin){
			sensorMin = s;
		}
			
		float avg = (s - sensorMin) / (sensorMax - sensorMin);

		spawnMultiplier = avg + 1;
	}
}
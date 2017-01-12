using UnityEngine;

/// <summary>
/// 1. Maak een empty game object en noem het SpawnManager.
/// 2. Voeg het player object toe aan dit script.
/// 3. Voeg de spawnable fish prefabs voor iedere layer toe aan het prefabs array.
/// </summary>

public class FishSpawner : MonoBehaviour
{
	public float timer = 5;			// Tijd tussen iedere spawn.
	public GameObject player;		// Player object om de spawn posities te bepalen.
	public GameObject[] prefabs;	// prefab voor iedere spawn layer.
	public GameObject fishPrefab;

	//flockmanager object pool
	public GameObject flockManPrefab;
	public int flockManagersAmount;

	private int layer = 1;				// Checkt in welke layer de speler zwemt.
	public float maxRange = 120;		// Range tussen de speler en de maximale spawn afstand.

	void Start ()
	{
		//init the fish tanks
		for(int i = 0; i < flockManagersAmount; i++){
			FlockManager fm = Instantiate (
				flockManPrefab,
				Vector3.zero,
				Quaternion.identity
			).GetComponent<FlockManager>();
			fm.fSpawner = this;

			Spawn (fm);
		}

	}

	// Spawned de betreffende objecten.
	public void Spawn (FlockManager fm)
	{
		// Positie van de speler binnen halen..
		Vector3 playerPos = player.transform.position;

		int amount;
		int points;
		float size;

		//check in welke layer de speler zwemt.
		if(playerPos.y > 399){
			layer = 1;

			amount 	= 10;
			points 	= 1;
			size 	= 1f;

		}else if(playerPos.y > 299){
			layer = 2;

			amount 	= 10;
			points 	= 2;
			size 	= 1.5f;

		}else if(playerPos.y > 199){
			layer = 3;

			amount 	= 5;
			points 	= 3;
			size 	= 2f;

		}else if(playerPos.y > 99){
			layer = 4;

			amount 	= 5;
			points 	= 5;
			size 	= 2f;
		}

		// loopt totdat er een positie is gevonden dat niet achter de map zit.
		Vector3 randomPos = getRandomPosition (playerPos, maxRange);
		RaycastHit hit;
		Ray ray = new Ray(new Vector3(randomPos.x,499f,randomPos.z), Vector3.down);
		if (Physics.Raycast(ray, out hit, 500f)) {
			randomPos.y = hit.point.y + 20f;
		}

		// Enemy spawnen.
		fm.Spawn(fishPrefab, amount, size, points, randomPos);
	}

	Vector3 getRandomPosition(Vector3 pos, float maxRange){
		float randomX = Random.Range (pos.x - maxRange, pos.x + maxRange);
		float randomY = Random.Range (pos.y - maxRange, pos.y + maxRange);
		float randomZ = Random.Range (pos.z - maxRange, pos.z + maxRange);

		Vector3 randomPos = new Vector3 ( randomX, randomY, randomZ );
		return randomPos;
	}
}
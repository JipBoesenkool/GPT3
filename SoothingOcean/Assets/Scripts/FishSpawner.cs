using UnityEngine;

/// <summary>
/// 1. Maak een empty game object en noem het SpawnManager.
/// 2. Voeg het player object toe aan dit script.
/// 3. Voeg de spawnable fish prefabs voor iedere layer toe aan het prefabs array.
/// </summary>

public class FishSpawner : MonoBehaviour
{
	public float timer = 10;			// Tijd tussen iedere spawn.
	public GameObject player;			// Player object om de spawn posities te bepalen.
	public GameObject[] prefabs;		// prefab voor iedere spawn layer.

	private int layer = 1;				// Checkt in welke layer de speler zwemt.
	private float maxRange = 30;		// Range tussen de speler en de maximale spawn afstand.

	void Start ()
	{
		InvokeRepeating ("Spawn", timer, timer);
	}

	// Spawned de betreffende objecten.
	void Spawn ()
	{
		// Positie van de speler binnen halen..
		float posX = player.transform.position.x;
		float posY = player.transform.position.y;
		float posZ = player.transform.position.z;

		//check in welke layer de speler zwemt.
		if(posY > 399){
			layer = 1;
			maxRange = 30;
		}else if(posY > 299){
			layer = 2;
			maxRange = 60;
		}else if(posY > 199){
			layer = 3;
			maxRange = 100;
		}else if(posY > 99){
			layer = 4;
			maxRange = 125;
		}

		// Random enemy soort uitkiezen.
		int spawnPointIndex = Random.Range (0, layer -1);

		// while loop variablen
		int loop = 0;
		Vector3 randomPos = new Vector3 (0,0,0);

		// loopt totdat er een positie is gevonden dat niet achter de map zit.
		while (loop < 15) {
			randomPos = getRandomPosition (posX, posY, posZ,maxRange);
			RaycastHit hit;
			if (Physics.Raycast (player.transform.position, randomPos)) {
				print ("recalculating");
				loop++;
			} else {
				loop = 15;
			}
		}

		// Enemy spawnen.
		Instantiate (prefabs[spawnPointIndex], randomPos, new Quaternion(0,0,0,0));
	}

	Vector3 getRandomPosition(float posX, float posY, float posZ, float maxRange){
		float randomX = Random.Range (posX - maxRange, posX + maxRange);
		float randomY = Random.Range (posY - maxRange, posY + maxRange);
		float randomZ = Random.Range (posZ - maxRange, posZ + maxRange);
		Vector3 randomPos = new Vector3 (randomX,randomY,randomZ);

		return randomPos;
	}
}
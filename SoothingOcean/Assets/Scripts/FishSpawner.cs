using UnityEngine;

/// <summary>
/// 1. Maak een empty game object en noem het SpawnManager.
/// 2. Voeg het player object toe aan dit script.
/// 3. Voeg iedere soort enemy toe die aan het begin van de game zichtbaar moet zijn.
/// </summary>

public class FishSpawner : MonoBehaviour
{
	
	public float timer = 10;			// Tijd tussen iedere spawn.
	public float maxRange = 75;			// Range tussen de speler en de maximale spawn afstand.
	public GameObject player;			// Player object om de spawn posities te bepalen.
	public GameObject[] prefabs;		// prefab voor iedere spawn layer.

	private int layer = 1;				// Checkt in welke layer de speler zwemt.

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
		// Positie bepalen.
		float randomX = Random.Range (posX - maxRange, posX + maxRange);
		float randomY = Random.Range (posY - maxRange, posY + maxRange);
		float randomZ = Random.Range (posZ - maxRange, posZ + maxRange);
		if (randomY > 360) {
			randomY = 360;
		}
		// Enemy spawnen.
		Instantiate (prefabs[spawnPointIndex], new Vector3(randomX,randomY,randomZ), new Quaternion(0,0,0,0));
		



	}
}
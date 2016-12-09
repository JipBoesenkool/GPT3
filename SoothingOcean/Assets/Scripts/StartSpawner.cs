using UnityEngine;

/// <summary>
/// 1. Maak een empty game object en noem het SpawnManager.
/// 2. Voeg het player object toe aan dit script.
/// 3. Voeg iedere soort enemy toe die aan het begin van de game zichtbaar moet zijn.
/// </summary>

public class StartSpawner : MonoBehaviour
{
	public float amountOfEnemies = 20;			// Aantal enemies die aan het begin van de game gespawnd worden.
	public float maxRange = 100;				// Range tussen de speler en de maximale spawn afstand.
	public GameObject player;					// Player object om de spawn posities te bepalen.
	public GameObject[] enemies;              	// Enemy prefabs die gespawnd kunnen worden.

	void Start ()
	{
		// Start positie krijgen.
		float startX = player.transform.position.x;
		float startY = player.transform.position.y;
		float startZ = player.transform.position.z;

		// Random enemies worden gespawnd op random plaatsen.
		for(int i = 0; i < amountOfEnemies; i++){
			// Random enemy soort uitkiezen.
			int spawnPointIndex = Random.Range (0, enemies.Length);
			// Positie bepalen.
			float randomX = Random.Range (startX - maxRange, startX + maxRange);
			float randomY = Random.Range (startY - maxRange, startY + maxRange);
			float randomZ = Random.Range (startZ - maxRange, startZ + maxRange);
			// Enemy spawnen.

			var temp = Instantiate (enemies[spawnPointIndex], new Vector3(randomX,randomY,randomZ), new Quaternion(0,0,0,0));
            temp.transform.SetParent(transform);
		}
	}
}
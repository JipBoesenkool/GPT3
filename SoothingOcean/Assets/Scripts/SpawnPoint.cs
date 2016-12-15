using UnityEngine;

/// <summary>
/// 1. Maak een empty game object en noem het SpawnManager.
/// 2. Maak een of meerdere empty game objects, noem ze SpawnPoint en plaats ze onder de SpawnManager.
/// 3. Voeg alle moedels toe die door dit spawnpunt gespawnd kunnen worden.
/// </summary>

public class SpawnPoint : MonoBehaviour
{
	public float spawnTime = 3f;        	// Interval tussen spawns.
	public bool randomSpawnTime = false;	// False gebruikt een vaste spawn tijd (zoals hierboven aangegeven), true gebruikt een random spawntijd tussen 0 en de aangegeven spawntijd.
	public float maxScale = 1;				// Maakt de objecten die gespawned worden groter.
	public GameObject[] models;          	// Array van models die gespawnd kunnen worden.
	public Transform[] spawnPoints;        // Array van spawnpunten (Empty game objects) waar de models gespawnd worden.

	void Start ()
	{
		// Roept Spawn functie aan na (spawnTime) seconden, en herhaald dit iedere (spawnTime) seconden.
		if (!randomSpawnTime) { // Spawned met een fixed delay.
			InvokeRepeating ("Spawn", spawnTime, spawnTime);
		} 
		else { // Spawned met een random delay.
			float random = Random.Range (1, spawnTime);
			Invoke ("RandomSpawn", random);
		}
	}
	
	// Roept de spawn methode aan met een random interval.
	void RandomSpawn()
	{
		Spawn ();
		float randomTime = Random.Range (1, spawnTime);
		Invoke("RandomSpawn", randomTime);
	}

	// Spawned de betreffende objecten.
	void Spawn ()
	{
		// random nummer voor het kiezen welk spawnpunt gebruikt word.
		int modelIndex = Random.Range (0, models.Length);
		int spawnPointIndex = Random.Range (0,spawnPoints.Length);
		// Maakt een instantie van de model prefab en plaatst het op een random spawnpoint.
		GameObject spawnedobject = Instantiate (models[modelIndex], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
		spawnedobject.transform.localScale = Vector3.one * Random.Range(1, maxScale);
	}
}
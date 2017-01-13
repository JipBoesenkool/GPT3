using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnData {
	[Range(1,4)]
	public int layer;
	[Range(0,20)]
	public int fishAmount;
	public int pointsPerFish;
	[Range(1f,2.5f)]
	public float fishSize;
	public GameObject fishPrefab;

	[Range(0,240)]
	public int minRange;
	[Range(0,240)]
	public int maxRange;
}

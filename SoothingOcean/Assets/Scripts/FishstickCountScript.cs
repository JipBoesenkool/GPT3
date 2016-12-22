using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishstickCountScript : MonoBehaviour {

	public Text points_Text;
	public int fishstickPoints;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdatePoints( int points ){
		fishstickPoints = points;
		points_Text.text = fishstickPoints.ToString ();
	}
}

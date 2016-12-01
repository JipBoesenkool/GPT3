using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private Dictionary<string, Color> variables;

	// Use this for initialization
	void Start () {
        variables = new Dictionary<string, Color>();
        variables.Add("green", new Color(27, 219, 131));
        variables.Add("orange", new Color(255, 185, 0));
        variables.Add("red", new Color(255, 0, 79));	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

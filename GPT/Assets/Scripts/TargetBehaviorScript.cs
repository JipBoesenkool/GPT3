using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehaviorScript : MonoBehaviour {

    public enum Shape { Square, Circle, Triangle, Pentagon};

    public Color color;
    public Shape shape;

	// Use this for initialization
	void Start () {      
        this.GetComponent<SpriteRenderer>().color = color;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

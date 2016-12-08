using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour {

    public float deltaHeight;
    public float time;

	// Use this for initialization
	void Start () {
        iTween.MoveBy(this.gameObject, iTween.Hash("y", deltaHeight, "time", time, "looptype", "pingpong", "easetype", iTween.EaseType.easeInOutSine));
    }
}

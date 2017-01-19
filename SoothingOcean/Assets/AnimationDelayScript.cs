using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDelayScript : MonoBehaviour {

    public Animation anim;

	// Use this for initialization
	void Start () {
        StartCoroutine(waiter());
    }
	
	// Update is called once per frame
	void Update () {

    }

    IEnumerator waiter()
    {
        while (true)
        {
            int wait_time = Random.Range(0, 4);
            anim.Play("Basic");
            yield return new WaitForSeconds(wait_time);
        }
    }
}

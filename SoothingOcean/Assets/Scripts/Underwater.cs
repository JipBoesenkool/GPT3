using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Underwater : MonoBehaviour {

    public float waterLevel;

    private bool isUnderwater;

    public Color normalColor;
    public Color underwaterColor;

    public float fogDensity;


	// Use this for initialization
	void Start () {
    }

    // Update is called once per frame
    void Update() {
        if ((transform.position.y < waterLevel) != isUnderwater)
        {
            isUnderwater = transform.position.y < waterLevel;
        }
        if (isUnderwater) SetUnderWater();
        if (!isUnderwater) SetAboveWater();
    }

    void SetAboveWater()
    {
        RenderSettings.fogColor = normalColor;
    }

    void SetUnderWater()
    {
        RenderSettings.fogColor = underwaterColor;
        RenderSettings.fogDensity = fogDensity;
    }
}

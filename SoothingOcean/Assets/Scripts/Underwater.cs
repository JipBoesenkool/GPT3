using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Underwater : MonoBehaviour {

    public float waterLevel;
    public float deepestWaterLevel;

    private bool isUnderwater;

    public Color normalFogColor;
    public Color underwaterFogColor;

    public float fogDensityUnderwater;
    public float fogDensityAbovewater;
    private float fogDifference;

    private Light sunlight;
    private GameObject sunlightObject;


    // Use this for initialization
    void Start () {
        sunlightObject = GameObject.Find("Sunlight");
        sunlight = sunlightObject.GetComponent<Light>();
        fogDifference = Mathf.Abs(fogDensityUnderwater - 3*fogDensityAbovewater); //difference between 3* fogdensity above water and fogdensity in deepest water
    }

    // Update is called once per frame
    void Update() {
        
        UpdateFog();

        float lightpercentage = (transform.position.y - deepestWaterLevel) / (waterLevel - deepestWaterLevel); // between 0 and 1 how deep in the see the player is.

        UpdateLightLevel(lightpercentage);

        RenderSettings.fogDensity = (3*fogDensityAbovewater + (fogDifference - (fogDifference * lightpercentage))); //Update fogdensity depending on depth of player, starting at 3* density of above water
    }

    /// <summary>
    /// Sets light intensity depending on depth of player
    /// </summary>
    void UpdateLightLevel(float lightpercentage)
    {
        if (isUnderwater)
        {
            
            sunlight.intensity = 0.5f + (0.5f * lightpercentage); // light intensity between 0.3 and 1 depending on depth
        }
    }

    /// <summary>
    /// Check to see if the fish is below water level and update fog if it changes
    /// </summary>
    void UpdateFog()
    {
        //If there is a change in above/below water level, update fog to match
        if ((transform.position.y < waterLevel) != isUnderwater)
        {
            isUnderwater = transform.position.y < waterLevel;
            if (isUnderwater) SetUnderWater();
            if (!isUnderwater) SetAboveWater();
        }
    }

    /// <summary>
    /// Set fog for above water level
    /// </summary>
    void SetAboveWater()
    {
        RenderSettings.fogColor = normalFogColor;
        RenderSettings.fogDensity = fogDensityAbovewater;
    }

    /// <summary>
    /// Set fog for below water level
    /// </summary>
    void SetUnderWater()
    {
        RenderSettings.fogColor = underwaterFogColor;
        RenderSettings.fogDensity = fogDensityUnderwater;
    }
}

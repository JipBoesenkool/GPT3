using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : MonoBehaviour
{
    public enum FishType
    {
        Trout,
        Tuna,
        Bristlemouth,
        Forelle
    };

    public enum FishColor
    {
        Color1,
        Color2,
        Color3
    }

    public FishColor fishColor;
    public Color color;
    public FishType type;
    public float size;
    public bool inSchool;

    // Use this for initialization
    void Start()
    {
        if (size == 0)
        {
            size = 1;
        }

        switch (fishColor)
        {
            case FishColor.Color1:
                gameObject.GetComponent<Renderer>().material.color = Color.black;
                break;

            case FishColor.Color2:
                gameObject.GetComponent<Renderer>().materials[0].color = Color.red;
                break;

            case FishColor.Color3:
                gameObject.GetComponent<Renderer>().materials[0].color = Color.blue;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

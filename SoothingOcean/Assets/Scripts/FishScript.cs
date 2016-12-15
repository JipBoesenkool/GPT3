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

    public Color color;
    public FishType type;
    public float size;

    // Use this for initialization
    void Start()
    {
        if (size == 0)
        {
            size = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

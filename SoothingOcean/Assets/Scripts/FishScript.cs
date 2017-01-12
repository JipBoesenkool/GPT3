using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : MonoBehaviour
{
    //public enum FishType
    //{
    //    Trout,
    //    Tuna,
    //    Bristlemouth,
    //    Forelle
    //};

    //public Color color;
    //public FishType type;
    public float size;
    public float requiredPoints;

    // Use this for initialization
    void Start()
    {

    }
     
    public void OnSpawn(float size, float requiredPoints)
    {
        this.size = size;
        this.transform.localScale = Vector3.one * Random.Range(1, size);
        this.requiredPoints = requiredPoints;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caustics : MonoBehaviour {

    private Projector projector;
    public MovieTexture mt;

    void Start()
    {
        projector = GetComponent<Projector>();
        projector.material.SetTexture("_ShadowTex", mt);
        mt.loop = true;
        mt.Play();
    }
}

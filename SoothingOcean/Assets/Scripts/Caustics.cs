using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caustics : MonoBehaviour {

#if UNITY_ANDROID
#elif UNITY_IOS
#else
    private Projector projector;
    public MovieTexture mt;

#endif
    void Start()
    {
#if UNITY_ANDROID
#elif UNITY_IOS
#else
        projector = GetComponent<Projector>();
        projector.material.SetTexture("_ShadowTex", mt);
        mt.loop = true;
        mt.Play();
#endif
    }
}

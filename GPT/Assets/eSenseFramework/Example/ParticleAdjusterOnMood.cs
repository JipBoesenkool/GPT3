using UnityEngine;
using eSense;
using UnityEngine.UI;
using System.Collections.Generic;
using eSense.graph;
using System;

/// <summary>
/// An example class that shows the data from eSense in a more visual way. It also has some features that a final application would have like gracefull pause on disconnect.
/// </summary>
public class ParticleAdjusterOnMood : MonoBehaviour {
    public eSenseGraph graph;

    public Color particleColorCalm;
    public Color ParticleColorTense;

    public Light ballLight;
    public AnimationCurve curve = new AnimationCurve();
    private ParticleSystem partSystem;

    public Dropdown dropdown;
    public Image img;
    public Text textField;

    // Use this for initialization
    void Start () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 30;
        partSystem = this.GetComponent<ParticleSystem>();
        eSenseFramework.OnuMhoChanged += UpdateSiemens;
        eSenseFramework.OnPossibleDisconnectDetected += DisconnectStopper;
        //do the UI
        List<string> options = new List<string>();
        options.AddRange(Microphone.devices);
        dropdown.AddOptions(options);
    }

    private void DisconnectStopper()
    {
        StopMeasure();
    }

    void Update()
    {
        if (eSenseFramework.IsMeasuring())
        {
            img.color = Color.green;
        }
        else
        {
            img.color = Color.red;
        }
    }

    public void StartMeasure()
    {
        if (dropdown.value >= dropdown.options.Count)
        {
            eSenseFramework.StartMeasurement();
        }
        else
        {
            eSenseFramework.StartMeasurement(dropdown.options[dropdown.value].text);
        }
    }

    public void StopMeasure()
    {
        eSenseFramework.StopMeasurement();
    }

    public void ClearGraph()
    {
        graph.ClearDataSamples();
    }

    private float lastTime = 0f;
    void UpdateSiemens(double s)
    {
        textField.text = s.ToString("0.000") + " μMho";
        if (Time.time - lastTime >= 0.2f)
        {
            graph.AddDataSample(new eSenseGraph.DataSample(Time.time, (float)s));
            lastTime = Time.time;
        }
        UpdateParticleTween((float)s, graph.CurrentMinimumValue, graph.CurrentMaximumValue);
    }

    void OnDestroy()
    {
        eSenseFramework.OnuMhoChanged -= UpdateSiemens;
        eSenseFramework.OnPossibleDisconnectDetected -= DisconnectStopper;
    }

    void UpdateParticleTween(float value, float min, float max)
    {
        float lerpValue = Mathf.InverseLerp(min, max, value);
        float lerpLog = LogLerp(0f, 1f, lerpValue);
        Color lerpColor = Color.Lerp(particleColorCalm, ParticleColorTense, lerpLog);
        ballLight.color = lerpColor;
        partSystem.startColor = lerpColor;
        ParticleSystem.ShapeModule shape = partSystem.shape;
        shape.radius = Mathf.Lerp(0.4f, 0.2f, lerpLog);
        partSystem.startSpeed = Mathf.Lerp(0.25f, 1.75f, lerpLog);
        partSystem.startLifetime = Mathf.Lerp(1f, 0.25f, lerpLog);
        ParticleSystem.EmissionModule emission = partSystem.emission;
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3
        float rate = Mathf.Lerp(250, 1500, lerpLog);
        ParticleSystem.MinMaxCurve curve = emission.rate;
        curve.mode = ParticleSystemCurveMode.Constant;
        curve.constantMin = rate;
        curve.constantMax = rate;
        emission.rate = curve;
#else
        emission.rate = Mathf.Lerp(250, 1500, lerpLog);
#endif
    }

    float LogLerp(float from, float to, float val)
    {
        return from + curve.Evaluate(val) * (to - from);
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using eSense;
using System.Collections.Generic;
using eSense.graph;
using System;

/// <summary>
/// An example class that shows you how the SDK can be used to extract nearly all it's data.
/// </summary>
public class TestScript : MonoBehaviour {

    public Text hertz;
    public Text resistance;
    public Text siemens;
    public Text celcius;

    public Text measuring;

    public Dropdown dropdown;
    public eSenseGraph graphHz;
    public eSenseGraph graphBufferFrames;
    public eSenseGraph graphBufferLoc;
    
    void Start () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 30;
        eSenseFramework.OnHertzChanged += UpdateHertz;
        eSenseFramework.OnOhmChanged += UpdateResistance;
        eSenseFramework.OnuMhoChanged += UpdateSiemens;
        eSenseFramework.OnTemperatureChanged += UpdateCelcius;
        eSenseFramework.OnPossibleDisconnectDetected += DisconnectStopper;
        List<string> options = new List<string>();
        options.AddRange(Microphone.devices);
        dropdown.AddOptions(options);
    }

    void OnDestroy()
    {
        eSenseFramework.OnHertzChanged -= UpdateHertz;
        eSenseFramework.OnOhmChanged -= UpdateResistance;
        eSenseFramework.OnuMhoChanged -= UpdateSiemens;
        eSenseFramework.OnTemperatureChanged -= UpdateCelcius;
        eSenseFramework.OnPossibleDisconnectDetected -= DisconnectStopper;
    }

    void Update()
    {
        if (eSenseFramework.IsMeasuring())
        {
            measuring.text = "Measurements in progress.";
        }
        else
        {
            measuring.text = "Measurements stopped.";
        }
    }

    public void StartMeasure()
    {
        if(dropdown.value >= dropdown.options.Count)
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

    public void UpdateHertz(double hz)
    {
        hertz.text = hz.ToString("0.00") + " Hz";
    }

    void UpdateResistance(double r)
    {
        resistance.text = (r/1000d).ToString("0.00") + " kΩ";
    }
    private double[] siemensAvg = new double[5];
    void UpdateSiemens(double s)
    {
        graphHz.AddDataSample(new eSenseGraph.DataSample(Time.time, (float)s));
        for (int i = 1; i < siemensAvg.Length; i++)
        {
            siemensAvg[i - 1] = siemensAvg[i];
        }
        siemensAvg[siemensAvg.Length-1] = s;
        double avg = 0;
        for (int i = 0; i < siemensAvg.Length; i++)
        {
            avg += siemensAvg[i];
        }
        avg = avg / siemensAvg.Length;
        siemens.text = avg.ToString("0.000") + " μMho";
    }

    void UpdateCelcius(double t)
    {
        celcius.text = t.ToString("0.0") + " °C";
    }

    private void DisconnectStopper()
    {
        StopMeasure();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using eSense;
using System;
using UnityEngine.UI;

public class SensorScript : MonoBehaviour
{
	public Text text;
	public int bufferLength = 60;
	double[] averageSiemens;
	public int index;

	public FishSpawner fs;

	void Start()
	{
		averageSiemens = new double[bufferLength];
		index = -59;

		eSenseFramework.StartMeasurement("", true);
		eSenseFramework.OnuMhoChanged += UpdateSiemens;
		//eSenseFramework.OnPossibleDisconnectDetected += Stop;

	}

	void UpdateSiemens(double s)
	{
		//first fill the array
		if (index < 0) {
			averageSiemens [index + 59] = s;
			index++;
			return;
		}

		//check if array should loop
		if (index >= 60) {
			index = 0;
		}

		//add to buffer
		averageSiemens[index] = s;
		index++;

		//get avarage of buffer
		double avg = GetAvarage ();

        //debug 
		text.text = "Siem: " + s.ToString().Substring(0,s.ToString().IndexOf(".") + 2) + " - Avg: " + avg.ToString().Substring(0,avg.ToString().IndexOf(".") + 2);

		//pass value to spawnmanager
		fs.SetSensorAverage((float)avg);
	}

	void Stop()
	{
		eSenseFramework.StopMeasurement();
		averageSiemens = new double[60];
		index = -59;
	}

	private double GetAvarage(){
		double avg = 0;

		for (int i = 0; i < bufferLength; i++)
		{
			avg += averageSiemens[i];
		}
		avg /= bufferLength;

		return avg;
	}
}

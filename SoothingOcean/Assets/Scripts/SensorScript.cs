using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using eSense;
using System;
using UnityEngine.UI;

public class SensorScript : MonoBehaviour
{
	public Text text;
	public int bufferLength = 20;
	double[] averageSiemens;
	public int index;
	public bool sensorActivated;

	public FishSpawner fs;

	void Start()
	{
		if(!sensorActivated){
			return;
		}

		text.enabled = true;

		averageSiemens = new double[bufferLength];
		index = -59;

		eSenseFramework.StartMeasurement("", true);
		eSenseFramework.OnuMhoChanged += UpdateSiemens;

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
		float avg = GetAvarage ();

        //debug 
		text.text = "Avg: " + avg.ToString().Substring(0,avg.ToString().IndexOf(".") + 2);

		//pass value to spawnmanager
		fs.SetSensorAverage(avg);
	}

	void Stop()
	{
		eSenseFramework.StopMeasurement();
		averageSiemens = new double[60];
		index = -59;
	}

	private float GetAvarage(){
		float avg = 0;

		for (int i = 0; i < bufferLength; i++)
		{
			avg += (float)averageSiemens [i];
		}
		avg /= bufferLength;

		return avg;
	}
}

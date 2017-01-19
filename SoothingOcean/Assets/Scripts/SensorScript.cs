using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using eSense;
using System;
using UnityEngine.UI;

public class SensorScript : MonoBehaviour
{
	public Text text;
	public int averageLength = 60;

	double startValue = 0.0;
	double[] averageSiemens;
	double avg;
	double lastAvg = 0;
	int count;

	public FishSpawner fs;

	void Start()
	{
		averageSiemens = new double[averageLength];
		count = -averageLength;

		eSenseFramework.StartMeasurement("", true);
		eSenseFramework.OnuMhoChanged += UpdateSiemens;
		//eSenseFramework.OnPossibleDisconnectDetected += Stop;

	}

	void UpdateSiemens(double s)
	{
		if (startValue == 0.0)
		{
			startValue = s;
		}

		if (count >= 0)
		{
			averageSiemens[count] = s;
			avg = 0;

			for (int i = 0; i < averageSiemens.Length; i++)
			{
				avg += averageSiemens[i];
			}
			text.text = "Siemens: " + s + " - Average: " + avg;
			avg /= averageLength;
		}

		if (count == averageLength - 1)
		{
			count = 0;

			fs.spawnMultiplier = (float)avg;

			lastAvg = avg;
		}
		else
		{
			count++;
		}
	}

	void Stop()
	{
		eSenseFramework.StopMeasurement();
		startValue = 0.0;
		count = 0;
		averageSiemens = new double[60];
	}
}

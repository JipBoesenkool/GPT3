The only class that should be used by anyone implementing the eSense is eSenseFramework.cs in the eSense namespace. 
The class should be initiated with StartMeasurement by assigning it a microphone. The microphone is a name created in Unity when requestiong all microphones. Look at Examples for more details. It can optionally filter 1 frame spikes by ommiting them from results.

StopMeasurement can be used to stop measurements and output.

You can use all static events in this class to gain information about Hertz, Ohm, Micro Siemens, and Tempterature changes. (Please note that the SDK can not detect which device is connected. Therefor any device but the eSense Temperature will return bogus data for temperature. And anything but the eSense Skin Resistance will return bogus data for Micro Siemens. Communicate this with your users!)

You can also recieve an event when a spike happens. Even when spike filtering is OFF. This allows for custom spike filtering or doing something based on when spikes happen.

Furthermore you can automatically get a possible disconnect of the eSense device through OnPossibleDisconnectDetected(). This method effectively looks for a lot of spikes and reports when this is the case. Usually it goes off when it detects any sort of noise for half a second. This works even when spike filtering is OFF.

The example scenes show a Test scene which contains bare minimum reading of variables outputted by the SDK.
And a Visualization scene which demonstrates how this data can be used to visualize stress or calm through the SDK. It can also be used to read out trends and changes on the data useful for analisys.

For experimentation with this SDK you can use any sort of tone modulator (Including smartphone apps) to deliver a sine wave to the microphone input. This way you can easily test device compatibility or tuning without the actual eSense device. However you should always test on a real eSense device before release.

Here are some testing values to try:
Frequency sine input:
606.666,	1328,		2102,		5372,		7845.83
Resistance in Ohm:
9456.26,	48374.85,	99069.28,	470129.78,	999969.82
Micro Siemens in µS (uMho):
105.75,		20.67,		10.09,		2.13,		1.00
Degrees in °C:
87.34,		41.76,		25.20,		-5.39,		-18.06


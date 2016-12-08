using UnityEngine;
using System.Collections;
using eSense.Internal;
using System;
using System.Collections.Generic;

namespace eSense
{
    /// <summary>
    /// The class that enables you to listen to the eSense device.
    /// </summary>
    public static class eSenseFramework
    {
        //Hertz value changed (raw)
        public delegate void HertzChanged(double frequency);
        /// <summary>
        /// Throws an event upon the eSense updating.
        /// </summary>
        public static event HertzChanged OnHertzChanged;
        //Ohm resistance value changed
        public delegate void OhmChanged(double ohms);
        /// <summary>
        /// Throws an event upon the eSense updating.
        /// </summary>
        public static event OhmChanged OnOhmChanged;
        //uMho resistance value changed
        public delegate void uMhoChanged(double uMho);
        /// <summary>
        /// Throws an event upon the eSense updating.
        /// </summary>
        public static event uMhoChanged OnuMhoChanged;
        //temperature value changed
        public delegate void TempChanged(double celcius);
        /// <summary>
        /// Throws an event upon the eSense updating. Result is in Celcius
        /// </summary>
        public static event TempChanged OnTemperatureChanged;//celsius
        //spike detected
        public delegate void SpikeDetected();
        /// <summary>
        /// Throws an event upon the eSense detecting a spike. (Will also be called when not filtering spikes.)
        /// </summary>
        public static event SpikeDetected OnSpikeDetected;
        //Probable Disconnect
        public delegate void ProbableDisconnect();
        /// <summary>
        /// Throws an event upon the eSense detecting over 8 spikes within 1 second. Most likely it is detecting noise and thus disconnect.
        /// </summary>
        public static event ProbableDisconnect OnPossibleDisconnectDetected;
        //state
        private static bool isMeasuring;
        //list of spikes
        private static List<float> spikeTimes = new List<float>();

        /// <summary>
        /// Used to check if the eSense is currently actively measuring or not.
        /// </summary>
        /// <returns>True if the device is currently measuring</returns>
        public static bool IsMeasuring()
        {
            return isMeasuring;
        }
        /// <summary>
        /// Starts the eSense measuring.
        /// </summary>
        /// <param name="targetMicrophone">The microphone used to record. (Unity microphone devices)</param>
        /// <param name="filterSpikes">Should the SDK filter spikes before output? Filters 1/2 frame spikes.</param>
        /// <returns>True if succesful.</returns>
        public static bool StartMeasurement(string targetMicrophone = null, bool filterSpikes = true)
        {
            eSenseAnalysis.OnHertzChanged += PassHertz;
            eSenseAnalysis.OnOhmChanged += PassOhm;
            eSenseAnalysis.OnuMhoChanged += PassuMho;
            eSenseAnalysis.OnTemperatureChanged += PassTemperature;
            eSenseAnalysis.OnSpikeDetected += PassSpikeDetected;
            isMeasuring = eSenseAnalysis.StartMeasurement(targetMicrophone, filterSpikes);
            return isMeasuring;
        }
        /// <summary>
        /// Stops the eSense measuring.
        /// </summary>
        public static void StopMeasurement()
        {
            isMeasuring = false;
            eSenseAnalysis.OnHertzChanged -= PassHertz;
            eSenseAnalysis.OnOhmChanged -= PassOhm;
            eSenseAnalysis.OnuMhoChanged -= PassuMho;
            eSenseAnalysis.OnTemperatureChanged -= PassTemperature;
            eSenseAnalysis.OnSpikeDetected -= PassSpikeDetected;
            eSenseAnalysis.StopMeasurement();
        }

        private static void PassHertz(double hertz)
        {
            if (OnHertzChanged != null)
            {
                OnHertzChanged(hertz);
            }
        }

        private static void PassOhm(double ohms)
        {
            if (OnOhmChanged != null)
            {
                OnOhmChanged(ohms);
            }
        }

        private static void PassuMho(double uMho)
        {
            if (OnuMhoChanged != null)
            {
                OnuMhoChanged(uMho);
            }
        }

        private static void PassTemperature(double celcius)
        {
            if (OnTemperatureChanged != null)
            {
                OnTemperatureChanged(celcius);
            }
        }


        private static void PassSpikeDetected()
        {
            if (OnSpikeDetected != null)
            {
                OnSpikeDetected();
            }
            //if someone listens to this, we keep a list of spikes and call this event if we see something that resembles disconnect.
            if(OnPossibleDisconnectDetected != null)
            {
                float t = Time.time - 1f;
                for (int i = 0; i < spikeTimes.Count; i++)
                {
                    if(spikeTimes[i] < t)
                    {
                        spikeTimes.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        break;
                    }
                }
                spikeTimes.Add(t + 1f);
                if(spikeTimes.Count >= 8)
                {
                    spikeTimes.Clear();
                    OnPossibleDisconnectDetected();
                }
            }
        }
    }
}

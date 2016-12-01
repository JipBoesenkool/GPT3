using UnityEngine;
using System.Collections;
using System;

namespace eSense.Internal
{
    /// <summary>
    /// An internal class to actually measure and calculate the output from the eSense device.
    /// </summary>
    public class eSenseAnalysis : MonoBehaviour
    {
        //mic info
        private static string deviceName;
        private static AudioClip micClip;
        private const int bufferLengthSeconds = 5;
        //buffer position
        private static int lastPosition = 0;
        private static int position = 0;
        //calculation variables
        private static double frequency;
        private static double lastFrequency;
        private static bool valid_data = false;
        private static bool filterSpikesFromOutput = true;
        //variables for calcFrequency
        private static int sampleCounter = 0;
        private static int lastSample = 0;
        private static int transitionCounter = 0;
        private static bool stop = false;
        private static bool firstCount = true;
        private static double t_Sample = 1d / 44100d;
        private static double t1 = 0.0;
        private static double t2 = 0.0;

        //DELEGATES
        //Hertz value changed (raw)
        public delegate void HertzChanged(double frequency);
        /// <summary>
        /// This is an internal event, please use the class eSenseFramework.
        /// </summary>
        public static event HertzChanged OnHertzChanged;
        //Ohm resistance value changed
        public delegate void OhmChanged(double ohms);
        /// <summary>
        /// This is an internal event, please use the class eSenseFramework.
        /// </summary>
        public static event OhmChanged OnOhmChanged;
        //uMho resistance value changed
        public delegate void uMhoChanged(double uMho);
        /// <summary>
        /// This is an internal event, please use the class eSenseFramework.
        /// </summary>
        public static event uMhoChanged OnuMhoChanged;
        //temperature value changed
        public delegate void TempChanged(double celcius);
        /// <summary>
        /// This is an internal event, please use the class eSenseFramework.
        /// </summary>
        public static event TempChanged OnTemperatureChanged;//celsius
        //Spike Detection
        public delegate void SpikeDetected();
        /// <summary>
        /// This is an internal event, please use the class eSenseFramework.
        /// </summary>
        public static event SpikeDetected OnSpikeDetected;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            GameObject gameObject = new GameObject("[eSense InputAnalysis]");
            DontDestroyOnLoad(gameObject);
            gameObject.AddComponent<eSenseAnalysis>();
        }

        /// <summary>
        /// This is an internal method, please use the class eSenseFramework.
        /// </summary>
        /// <param name="targetMicrophone"></param>
        /// <param name="filterSpikes"></param>
        /// <returns></returns>
        public static bool StartMeasurement(string targetMicrophone, bool filterSpikes = true)
        {
            filterSpikesFromOutput = filterSpikes;
            //find device and connect/read it to an audio clip
            if (Microphone.devices.Length > 0)
            {
                if (!string.IsNullOrEmpty(targetMicrophone))
                {
                    deviceName = targetMicrophone;
                }
                else
                {
                    deviceName = Microphone.devices[0];
                }
                Debug.Log("Device picked: " + deviceName);
                int minimal;
                int maximal;
                Microphone.GetDeviceCaps(deviceName, out minimal, out maximal);
                //Debug.Log("Capmin: " + minimal + " Capmax: " + maximal);
                if (minimal > 44100 || maximal < 44100)
                {
                    Debug.LogError("Can't get 44100 frequency! Started at frequency " + maximal);
                    micClip = Microphone.Start(deviceName, true, bufferLengthSeconds, maximal);//get best possible frequency
                }
                else
                {
                    micClip = Microphone.Start(deviceName, true, bufferLengthSeconds, 44100);//get it at 44100
                }
            }
            else
            {
                Debug.LogError("No microphone found.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// This is an internal method, please use the class eSenseFramework.
        /// </summary>
        public static void StopMeasurement()
        {
            Microphone.End(deviceName);
        }

        private static int[] GetMicDataThisFrame()
        {
            //set last position for this loop
            lastPosition = position;
            //update position
            position = Microphone.GetPosition(deviceName);
            //read micdata for this frame
            float[] micData = new float[0];
            if (lastPosition != position)//no microphone changes
            {
                if (lastPosition < position)
                {
                    micData = new float[position - lastPosition];
                }
                else //wrapped around
                {
                    //TODO Unity sample wrapping is bugged, so for now we return an empty array back here by doing nothing at all. This skips 1 sample.
                    //micData = new float[(micClip.samples - lastPosition) + position];
                }
                if (micData.Length != 0)
                    micClip.GetData(micData, lastPosition);
            }
            //from float to byte array
            int[] micDataInt32 = new int[micData.Length];
            for (int i = 0; i < micData.Length; i++)
            {
                micDataInt32[i] = FloatToInt32(micData[i]);
            }
            //return the micDataArray
            return micDataInt32;
        }

        // Update is called once per frame
        void Update()
        {
            if (!String.IsNullOrEmpty(deviceName) && Microphone.IsRecording(deviceName))
            {
                int[] micDataInt32 = GetMicDataThisFrame();
                if (micDataInt32.Length == 0)
                    return;
                bool valid;
                for (int i = 0; i < micDataInt32.Length; i++)
                {
                    // give each sample value into calcFrequency:
                    valid = calcFrequency(micDataInt32[i]);
                    // check the result:
                    if (valid)
                    {
                        //ALWAYS do spike detection first.
                        if (OnSpikeDetected != null && isFrequencySpike())
                        {
                            OnSpikeDetected();
                        }
                        if (!filterSpikesFromOutput || !isFrequencySpike())//if we have no spike or no que to filter spike, pass data. Does not pass data if spike && has to filter spike.
                        {
                            //print found result
                            if (OnHertzChanged != null)
                            {
                                OnHertzChanged(frequency);
                            }
                            if (OnOhmChanged != null)
                            {
                                OnOhmChanged(calc_Ohms(frequency));
                            }
                            if (OnuMhoChanged != null)
                            {
                                OnuMhoChanged(calc_uMho(frequency));
                            }
                            if (OnTemperatureChanged != null)
                            {
                                OnTemperatureChanged(calc_Celcius(calc_Ohms(frequency)));
                            }
                        }
                        //Debug.Log("calculated frequency: " + frequency + " Hz");
                        //Debug.Log("Resistance: " + calc_Ohms(frequency));
                        //Debug.Log("Siemens: " + calc_uMho(frequency));
                        //Debug.Log("Celcius: " + calc_Celcius(calc_Ohms(frequency)));
                        requestCalcNextFrequency();
                    }
                }
            }
        }

        private static int FloatToInt32(float value)
        {
            return (int)(value * (1 << 31));
        }

        private static bool calcFrequency(int adc_value)
        {
            if (!valid_data)
            {
                sampleCounter++;
                if (sampleCounter == 4410) { stop = true; }
                // positiver oder negativer ubergang
                if (((lastSample > 0) && (adc_value <= 0)) || ((lastSample < 0) && (adc_value >= 0)))
                {
                    transitionCounter++;
                    if (firstCount)
                    {
                        sampleCounter = 0;
                        transitionCounter = 0;
                        firstCount = false;
                        if (adc_value == 0) { t1 = 0; } // Startmesswert ist gleich 0
                        else
                        {
                            t1 = (t_Sample - (-lastSample) * t_Sample / (adc_value - lastSample)); // Berechnung von T1
                        }
                    }
                    // �bergang von - auf + und SampleCounter >= 4410
                    else if (stop && (lastSample < 0) && (adc_value >= 0))
                    {
                        if (adc_value == 0) { t2 = 0; } // letzter Messwert ist gleich 0
                        else
                        {
                            t2 = ((-lastSample) * t_Sample / (adc_value - lastSample)); // Berechnung von T2
                        }
                        stop = false;
                        lastFrequency = frequency;
                        frequency = (transitionCounter) / ((t1 + (sampleCounter - 1) * t_Sample + t2) * 2);
                        firstCount = true;
                        valid_data = true;
                    }
                }
                lastSample = adc_value;
            }

            return valid_data;  // returns "true" if a new frequency is calculated
        }

        private static double calc_uMho(double frequency)
        {
            return 1e6 / calc_Ohms(frequency);
        }

        private static double calc_Ohms(double frequency)
        {
            double y, x;
            double c4 = 1.5172992798552474e-10;
            double c3 = -1.520552019660566e-6;
            double c2 = 0.0158949895906025;
            double c1 = 24.38291170054427;
            double c0 = -10323.37783968269;

            x = frequency; // in Hz
            y = x * (x * (x * (x * c4 + c3) + c2) + c1) + c0;
            // Debug.Log(x+"Hz ,"+ (y / 1000.0) + "kOhms, "+ (1e6 / y) +"uMho\n");
            return y;
        }

        private static double calc_Celcius(double resistance)
        {
            double R25 = 100e3f;
            double B25_50 = 4066f;
            double T0C = 273.15f;

            double celsius = B25_50 * (25 + T0C) / (B25_50 + Math.Log(resistance / R25) * (25 + T0C)) - T0C;

            //Debug.Log(frequency + "Hz ," + celsius + " °C");
            return celsius;
        }

        private static void requestCalcNextFrequency()
        {
            valid_data = false;
        }

        private static bool isFrequencySpike()
        {
            //more then 10% difference? Spike!
            if(Math.Abs(lastFrequency - frequency) > (lastFrequency*0.1f))
            {
                return true;
            }
            return false;
        }
    }
}
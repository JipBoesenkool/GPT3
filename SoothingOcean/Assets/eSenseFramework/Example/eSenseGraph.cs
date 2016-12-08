using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace eSense.graph
{
    /// <summary>
    /// This graph is more for testing then actual SDK usage. It could be left out of not needed/used.
    /// </summary>
    public class eSenseGraph : MonoBehaviour
    {
        [System.Serializable]
        public struct DataSample
        {
            public float time;
            public float value;

            public DataSample(float time, float value)
            {
                this.time = time;
                this.value = value;
            }

            public static DataSample operator +(DataSample s1, DataSample s2)
            {
                return new DataSample(s1.time + s2.time, s1.value + s2.value);
            }

            public static DataSample operator -(DataSample s1, DataSample s2)
            {
                return new DataSample(s1.time - s2.time, s1.value - s2.value);
            }

            public static DataSample operator *(DataSample s, float f)
            {
                return new DataSample(s.time * f, s.value * f);
            }

            public static DataSample operator /(DataSample s, float f)
            {
                return new DataSample(s.time / f, s.value / f);
            }
        }

        public delegate void GraphDelegate(eSenseGraph graph);
        /// <summary>
        /// Fires an event if the sample data changes.
        /// </summary>
        public event GraphDelegate SamplesChanged;

        [Header ("References")]
        [SerializeField] private eSenseGraphLine linePrefab;
        [SerializeField] private Transform xAxisContainer;
        [SerializeField] private Transform yAxisContainer;
        [SerializeField] private Transform bottomLeftCorner;
        [SerializeField] private Transform topRightCorner;

        [Header ("Settings")]
        public float historyTime = 2f;
        public int amountOfPoints = 100;

        [Space (8)]
        public float defaultMinValue = 0f;
        public float defaultMaxValue = 0f;

        [Space(8)]
        public bool resizeMinMaxValues;
        public float absoluteMinValue = float.MinValue;
        public float absoluteMaxValue = float.MaxValue;

        private Text[] xAxisLabels;
        private Text[] yAxisLabels;
        private eSenseGraphLine[] graphLines;

        private List<DataSample> samples;
        public List<DataSample> Samples
        {
            get { return this.samples; }
        }

        private float currMinValue;
        public float CurrentMinimumValue
        {
            get { return currMinValue; }
        }
        public float LifetTimeMinimumValue { get; private set; }

        private float currMaxValue;
        public float CurrentMaximumValue
        {
            get { return currMaxValue; }
        }
        public float LifeTimeMaximumValue { get; private set; }

        public float CurrentAverage { get; private set; }

        private int lifeTimeAverageCount = 0;
        public float LifeTimeAverage { get; private set; }

        private bool isDirty = true;

        private void Awake ()
        {
            this.samples = new List<DataSample>();

            if (this.xAxisContainer)
            {
                this.xAxisLabels = this.xAxisContainer.GetComponentsInChildren<Text>(true);
                for (int i = 0; i < this.xAxisLabels.Length; i++)
                    this.xAxisLabels[i].text = "-";
            }
            if (this.yAxisContainer)
            {
                this.yAxisLabels = this.yAxisContainer.GetComponentsInChildren<Text>(true);
                for (int i = 0; i < this.yAxisLabels.Length; i++)
                    this.yAxisLabels[i].text = "-";
            }

            this.graphLines = new eSenseGraphLine[this.amountOfPoints];
            for (int i = 0; i < this.amountOfPoints; i++)
            {
                eSenseGraphLine newLine = Instantiate(this.linePrefab);
                newLine.gameObject.SetActive(false);
                newLine.transform.SetParent(this.transform, false);
                this.graphLines[i] = newLine;
            }
            this.linePrefab.gameObject.SetActive(false);
        }

        protected virtual void OnEnable ()
        {
            if (this.isDirty && this.gameObject.activeInHierarchy && this.samples.Count > 0)
            {
                this.isDirty = false;
                this.UpdateGraph();
            }
        }

        public bool clear = false;

        protected virtual void Update ()
        {
            if (this.clear)
            {
                this.clear = false;
                this.ClearDataSamples();
            }

            if (this.isDirty)
            {
                this.isDirty = false;
                this.UpdateGraph();
            }
        }
        /// <summary>
        /// Adds a data sample to the graph.
        /// </summary>
        /// <param name="sample">DataSample containing a timestamp with data.</param>
        public virtual void AddDataSample (DataSample sample)
        {
            this.LifetTimeMinimumValue = Mathf.Min(this.LifetTimeMinimumValue, sample.value);
            this.LifeTimeMaximumValue = Mathf.Max(this.LifeTimeMaximumValue, sample.value);

            float cumm = this.LifeTimeAverage * this.lifeTimeAverageCount;
            this.lifeTimeAverageCount++;
            this.LifeTimeAverage = (cumm > 0f) ? cumm / this.lifeTimeAverageCount : 0f;

            this.samples.Add(sample);
            for (int i = 0; i < this.samples.Count; i++)
            {
                if (this.samples[i].time < sample.time - this.historyTime)
                {
                    this.samples.RemoveAt(i);
                    i--;
                }
            }

            this.isDirty = true;
            if (this.SamplesChanged != null)
                this.SamplesChanged(this);
        }
        
        /// <summary>
        /// Removes datasamples from the graph.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        public virtual void RemoveDataSamples (int startIndex, int count)
        {
            if (startIndex < this.samples.Count && count > 0)
            {
                this.samples.RemoveRange(startIndex, count);

                this.isDirty = true;
                if (this.SamplesChanged != null)
                    this.SamplesChanged(this);
            }
        }

        /// <summary>
        /// Clears all of this graphs data.
        /// </summary>
        /// <param name="clearLifeTimeAverage"></param>
        public virtual void ClearDataSamples (bool clearLifeTimeAverage = false)
        {
            if (clearLifeTimeAverage)
            {
                this.lifeTimeAverageCount = 0;
                this.LifeTimeAverage = 0f;
            }

            this.samples.Clear();

            this.currMinValue = Mathf.Max(0f, this.absoluteMinValue);
            this.currMaxValue = Mathf.Max(this.currMinValue, Mathf.Min(0f, this.absoluteMaxValue));

            this.isDirty = true;
            if (this.SamplesChanged != null)
                this.SamplesChanged(this);
        }

        /// <summary>
        /// Updates the graph. Usually called automatically upon change.
        /// </summary>
        public virtual void UpdateGraph ()
        {
            if (!this.gameObject.activeInHierarchy)
            {
                this.isDirty = true;
                return;
            }

            DataSample curr;
            float minTime = 0f;
            float maxTime = this.historyTime;

            if (this.samples != null && this.samples.Count > 0)
            {
                minTime = this.samples[0].time;
                maxTime = Mathf.Max(this.samples[this.samples.Count - 1].time, minTime + this.historyTime);
            }

            if (this.resizeMinMaxValues)
            {
                this.currMinValue = float.MaxValue;
                this.currMaxValue = float.MinValue;
                for (int i = 0; i < this.samples.Count; i++)
                {
                    curr = this.samples[i];
                    this.currMinValue = Mathf.Min((float)curr.value, this.currMinValue);
                    this.currMaxValue = Mathf.Max((float)curr.value, this.currMaxValue);
                }
                this.currMinValue = Mathf.Max(this.currMinValue, this.absoluteMinValue);
                this.currMaxValue = Mathf.Max(this.currMinValue, Mathf.Min(this.currMaxValue, this.absoluteMaxValue));
            }

            float minValue = Mathf.Min(this.CurrentMinimumValue, this.defaultMinValue);
            float maxValue = Mathf.Max(this.currMaxValue, this.defaultMaxValue);

            float timeDelta = (maxTime - minTime);
            if (this.xAxisLabels != null)
            {
                for (int i = 0; i < this.xAxisLabels.Length; i++)
                {
                    float time = minTime + ((timeDelta / (this.xAxisLabels.Length - 1)) * i);
                    this.xAxisLabels[i].text = string.Format("{0:0}", time);
                }
            }

            float valueDelta = Mathf.Max ((maxValue - minValue), 0f);
            if (this.yAxisLabels != null)
            {
                for (int i = 0; i < this.yAxisLabels.Length; i++)
                {
                    float val = minValue + ((valueDelta / (this.yAxisLabels.Length - 1)) * i);
                    this.yAxisLabels[this.yAxisLabels.Length - (i + 1)].text = string.Format("{0:0.0}", val);
                }
            }

            Vector2 bottomLeftCornerPosition = this.bottomLeftCorner.localPosition;
            Vector2 topRightCornerPosition = this.topRightCorner.localPosition;

            eSenseGraphLine line;
            Vector2 previousPosition = Vector2.zero;
            float buffersPerPoint = Mathf.Max (this.samples.Count / (float)this.amountOfPoints, 1f);
            float buffersIndexCummulative = 0f;
            int previousBuffersIndex = 0;

            this.CurrentAverage = 0f;

            int l = Mathf.Min(this.amountOfPoints, this.samples.Count);
            for (int i = 0; i < this.graphLines.Length; i++)
            {
                line = this.graphLines[i];
                
                if (i >= l)
                {
                    this.graphLines[i].gameObject.SetActive(false);
                    continue;
                }
                
                buffersIndexCummulative += buffersPerPoint;
                int newBuffersIndex = Mathf.FloorToInt(buffersIndexCummulative);
                int count = newBuffersIndex - previousBuffersIndex;

                if (count > 0)
                {
                    curr = new DataSample();
                    for (int n = 0; n < count; n++)
                        curr += this.samples[previousBuffersIndex + n];
                    this.CurrentAverage += curr.value;
                    curr /= count;
                }
                else
                {
                    curr = this.samples[newBuffersIndex];
                    this.CurrentAverage += curr.value;
                }
                previousBuffersIndex = newBuffersIndex;

                line = this.graphLines[i];
                float xp = 0f;
                if (i > 0)
                    xp = Mathf.InverseLerp(minTime, maxTime, curr.time);
                float yp = Mathf.InverseLerp(minValue, maxValue, curr.value);
                this.graphLines[i].gameObject.SetActive((i > 0));
                Vector2 pos = new Vector2(
                    Mathf.Lerp(bottomLeftCornerPosition.x, topRightCornerPosition.x, xp),
                    Mathf.Lerp(bottomLeftCornerPosition.y, topRightCornerPosition.y, yp));
                line.Position(pos);
                if (i == 0)
                    previousPosition = pos;
                line.DrawTowards(previousPosition);
                previousPosition = pos;
            }

            if (l > 0)
                this.CurrentAverage /= l;
        }
    }
}

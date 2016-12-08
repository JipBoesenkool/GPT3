using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace eSense.graph
{
    public class eSenseGraphLine : MonoBehaviour
    {
        public RectTransform line;
        public Vector3 rotationOffset;

        private Vector2 previousPosition;

        public void Position (Vector2 position)
        {
            this.transform.localPosition = position;
            this.previousPosition = position;
        }

        public void DrawTowards (Vector2 to)
        {
            Vector2 from = this.previousPosition;
            Vector2 sizeDelta = this.line.sizeDelta;
            sizeDelta.x = Vector2.Distance(from, to);
            this.line.sizeDelta = sizeDelta;

            if (from != to)
            {
                this.line.rotation = Quaternion.LookRotation((from - to).normalized, Vector3.forward);
                this.line.Rotate(rotationOffset, Space.Self);
            }  
        }
    }
}

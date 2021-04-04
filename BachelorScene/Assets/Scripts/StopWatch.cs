using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using System;

namespace Behaviour
{

    public class StopWatch : MonoBehaviour
    {
        public List<GameObject> Segments;
        public Material SegmentOnMaterial;
        public Material SegmentOffMaterial;

        private bool active;
        private static Timer timer;
        private float timeAcc = 0;

        // Start is called before the first frame update
        void Start()
        {
            foreach (GameObject gameObject in Segments)
            {
                Material[] segmentMaterial = gameObject.GetComponent<Renderer>().materials;
                for (int i = 0; i < segmentMaterial.Length; i++)
                {
                    segmentMaterial[i] = SegmentOffMaterial;
                }

                gameObject.GetComponent<Renderer>().materials = segmentMaterial;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (active)
            {
                timeAcc += Time.deltaTime;

                UpdateSegmentDisplay(Mathf.FloorToInt(timeAcc));
            }

            // Toggle Timer
            if (Input.GetKeyDown(KeyCode.B))
            {
                active = !active;
            }

            // Reset Timer
            if (Input.GetKeyDown(KeyCode.R))
            {
                active = false;
                timeAcc = 0f;

                UpdateSegmentDisplay(0);
            }
        }

        private void UpdateSegmentDisplay(int seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);

            int onesSeconds = time.Seconds % 10;
            int tensSeconds = Mathf.FloorToInt(time.Seconds / 10);
            int onesMinutes = time.Minutes % 10;
            int tensMinutes = Mathf.FloorToInt(time.Minutes / 10);

            UpdateSegment(Segments[0], tensMinutes);
            UpdateSegment(Segments[1], onesMinutes);
            UpdateSegment(Segments[2], tensSeconds);
            UpdateSegment(Segments[3], onesSeconds);
        }
        private void UpdateSegment(GameObject segment, int digit)
        {
            bool[] states = SegmentDisplay.getStatesForDigit(digit);
            Material[] segmentMaterial = segment.GetComponent<Renderer>().materials;

            for (int i = 0; i < segmentMaterial.Length; i++)
            {
                segmentMaterial[i] = states[i] ? SegmentOnMaterial : SegmentOffMaterial;
            }

            segment.GetComponent<Renderer>().materials = segmentMaterial;
        }
    }

    public static class SegmentDisplay
    {
        //This table references the segmentdisplay object created and imported, that is why order is unnormal.
        public static bool[][] decoderTable = new bool[10][]
        {
        new bool[7] { true, true, true, false, true, true, true },      //0
        new bool[7] { false, false, false, false, false, true, true },  //1
        new bool[7] { true, false, true, true, true, false, true },     //2
        new bool[7] { false, false, true, true, true, true, true },     //3
        new bool[7] { false, true, false, true, false, true, true },    //4
        new bool[7] { false, true, true, true, true, true, false },     //5
        new bool[7] { true, true, true, true, true, true, false },      //6
        new bool[7] { false, false, false, false, true, true, true },   //7
        new bool[7] { true, true, true, true, true, true, true },       //8
        new bool[7] { false, true, false, true, true, true, true }      //9
        };

        public static bool[] getStatesForDigit(int n)
        {
            return SegmentDisplay.decoderTable[n];
        }
    }
}
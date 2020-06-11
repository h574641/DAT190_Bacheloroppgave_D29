using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SevenSegmentHelper
{
    // Helper methods for rendering seven segment displays using meshes and materials

    private static Dictionary<GameObject, int> SegmentPreviousDigit = new Dictionary<GameObject, int>();

    public static void InitializeSegment(GameObject segment, Material onMaterial, Material offMaterial)
    {
        Material[] segmentMaterials = segment.GetComponent<Renderer>().materials;
        for (int i = 0; i < segmentMaterials.Length; i++)
        {
            segmentMaterials[i] = offMaterial;
        }

        segment.GetComponent<Renderer>().materials = segmentMaterials;
    }

    public static void UpdateSegment(GameObject segment, int digit, Material onMatieral, Material offMaterial)
    {
        int previousDigit = SegmentPreviousDigit.GetOrDefault(segment, -1);

        if (digit != previousDigit || previousDigit == -1)
        {
            bool[] currentStates = previousDigit == -1 ? null : SegmentDisplay.getStatesForDigit(previousDigit);
            bool[] digitStates = SegmentDisplay.getStatesForDigit(digit);

            Material[] segmentMaterial = segment.GetComponent<Renderer>().materials;
            for (int i = 0; i < segmentMaterial.Length; i++)
            {
                if (currentStates == null || digitStates[i] != currentStates[i])
                {
                    segmentMaterial[i] = digitStates[i] ? onMatieral : offMaterial;
                }
            }

            segment.GetComponent<Renderer>().materials = segmentMaterial;
        }

        SegmentPreviousDigit[segment] = digit;
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

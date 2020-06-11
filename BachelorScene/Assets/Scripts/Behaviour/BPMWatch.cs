using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPMWatch : MonoBehaviour
{
    public List<GameObject> Segments;
    public Material SegmentOnMaterial;
    public Material SegmentOffMaterial;

    public BabyController BabyController;
    public AttachPulseOxymeter attachPulseOxymeter;

    // Start is called before the first frame update
    void Start()
    {
        // Set up initial materials
        foreach (GameObject segment in Segments)
        {
            SevenSegmentHelper.InitializeSegment(segment, SegmentOnMaterial, SegmentOffMaterial);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int bpm = attachPulseOxymeter.IsOximeterAttached ? (int)BabyController.BPMController.BPM : 0;
        UpdateSegmentDisplay(bpm);
    }

    private void UpdateSegmentDisplay(int time)
    {
        List<int> digits = MathHelper.GetDigitsForNumber(time, Segments.Count);

        for (int i = 0; i < Segments.Count; i++)
        {
            SevenSegmentHelper.UpdateSegment(Segments[i], digits[i], SegmentOnMaterial, SegmentOffMaterial);
        }
    }
}

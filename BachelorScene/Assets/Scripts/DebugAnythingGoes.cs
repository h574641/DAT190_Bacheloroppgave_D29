using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAnythingGoes : MonoBehaviour
{
    // Generic dumping place for any debug

    static long TimeMethod(Action methodToTime)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

        stopwatch.Start();
        methodToTime();
        stopwatch.Stop();

        return stopwatch.ElapsedMilliseconds;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

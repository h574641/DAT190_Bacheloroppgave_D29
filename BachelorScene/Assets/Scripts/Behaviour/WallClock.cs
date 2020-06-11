using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WallClock : MonoBehaviour
{
    public GameObject HourHand;
    public GameObject MinuteHand;
    private int previousAngleHour = 0;
    private int previousAngleMinute = 0;

    private static float nextUpdateIn = 0f;


    // Update clock hand visuals
    // Only do this roughly ever minute when it is needed
    public void UpdateVisuals()
    {
        var Time = DateTime.Now;
        int hour = Time.Hour;
        int minute = Time.Minute;
        int second = Time.Second;

        var angleHour = Mathf.FloorToInt((hour + minute / 60f) * 30);
        var angleMinute = minute * 6;

        if (angleHour > previousAngleHour)
        {
            previousAngleHour = angleHour;
            HourHand.transform.eulerAngles = new Vector3(0, 90, angleHour);
        }

        if (angleMinute > previousAngleMinute)
        {
            previousAngleMinute = angleMinute;
            MinuteHand.transform.eulerAngles = new Vector3(0, 90, angleMinute);
        }

        nextUpdateIn = 60f - second + 2f;
    }

    void Start()
    {
        UpdateVisuals();
    }

    void Update()
    {
        if (nextUpdateIn <= 0f)
        {
            UpdateVisuals();
        }

        nextUpdateIn -= Time.unscaledDeltaTime;
    }

}

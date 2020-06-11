using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyController : MonoBehaviour
{
    private static float minimumTint = 0.2f;

    public bool IsBreathing;

    //Average resting respiratory rates by age are:
    //birth to 6 weeks: 30–40 breaths per minute
    public float RespirationRatePM;

    public GameObject BreathBone;

    public Material BabyMaterial;
    public GameObject BabyMesh;

    [Range(0f, 0.2f)]
    public float TintPercentage;
    public Color TintColor; //#88BDFF

    private Renderer babyRenderer;

    [HideInInspector]
    public BabyBPMController BPMController;

    private float tintTimeAcc;
    private float tintTimeTotal;
    private float tintStart;
    private float tintEnd;
    private bool shouldLerpTint;

    public void UpdateBreathing()
    {
        float duration = 60 / RespirationRatePM;

        if (IsBreathing && RespirationRatePM > 0)
        {
            float lerp = Mathf.PingPong(Time.time, duration) / duration;
            BreathBone.transform.localScale = new Vector3(Mathf.Lerp(0.9f, 1.2f, lerp), 1, 1);
        }

        Color finalTintColor = Color.Lerp(Color.white, TintColor, TintPercentage);
        babyRenderer.material.SetColor(Shader.PropertyToID("_Color"), finalTintColor);
    }

    public void UpdateTintLerp()
    {
        if (shouldLerpTint)
        {
            TintPercentage = Mathf.Min(Mathf.Lerp(tintStart, tintEnd, Mathf.Clamp(tintTimeAcc / tintTimeTotal, 0.0f, 1.0f)), minimumTint);

            if (tintTimeAcc > tintTimeTotal)
            {
                shouldLerpTint = false;
            }

            tintTimeAcc += Time.deltaTime;
        }
    }

    public void SetTintPercentage(float percent)
    {
        TintPercentage = Mathf.Min(percent, minimumTint);
    }

    public void SetTintPercentage(float percent, float duration)
    {
        shouldLerpTint = true;
        tintTimeAcc = 0f;
        tintTimeTotal = duration;
        tintStart = TintPercentage;
        tintEnd = percent;
    }

    public void DecreaseTintPercentage(float percent)
    {
        SetTintPercentage(TintPercentage - percent);
    }

    public void DecreaseTintPercentage(float percent, float duration)
    {
        SetTintPercentage(TintPercentage - percent, duration);
    }

    void Start()
    {
        babyRenderer = BabyMesh.GetComponent<Renderer>();
        BPMController = GetComponent<BabyBPMController>();
    }

    void Update()
    {
        UpdateBreathing();
        UpdateTintLerp();
    }
}

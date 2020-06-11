using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeatVibrations : MonoBehaviour
{
    public BabyController BabyController;
    public HashSet<OVRInput.Controller> TargetControllers { get; protected set; } = new HashSet<OVRInput.Controller>();

    public bool Vibrating { get; protected set; } = false;

    [System.NonSerialized]
    public float VibrationBPM;

    public float VibrationOnMultiplier = 0.2f;

    public float VibrationMaxStrength = 1.0f;
    public float VibrationMinStrength = 0.0f;

    // TODO - Implement
    public bool DistanceBasedVibrationStrength = true;

    public bool LogVibrationOnOff = false;

    private float nextVibration;
    private float vibrationOffIn;

    public void VibrateController(float strength, OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(1, strength, controller);
    }

    private void vibrationsOn()
    {
        if (LogVibrationOnOff && TargetControllers.Count > 0)
        {
            Debug.Log($"Vibrations On - {TargetControllers.Count}");
        }

        foreach (OVRInput.Controller controller in TargetControllers)
        {
            VibrateController(VibrationMaxStrength, controller);
        }
    }

    private void vibrationsOff()
    {
        if (LogVibrationOnOff && TargetControllers.Count > 0)
        {
            Debug.Log($"Vibrations Off - {TargetControllers.Count}");
        }

        foreach (OVRInput.Controller controller in TargetControllers)
        {
            VibrateController(VibrationMinStrength, controller);
        }
    }

    public void UpdateAndVibrateControllers()
    {
        if (!Vibrating && nextVibration <= 0f)
        {
            vibrationsOn();

            Vibrating = true;
            nextVibration = 60 / VibrationBPM;
            vibrationOffIn = nextVibration * VibrationOnMultiplier;
        }
        else
        {
            if (Vibrating && vibrationOffIn <= 0f)
            {
                vibrationsOff();

                Vibrating = false;
            }
        }

        nextVibration -= Time.deltaTime;
        vibrationOffIn -= Time.deltaTime;
    }

    public void UpdateToBabyBPM()
    {
        VibrationBPM = BabyController.BPMController.BPM;
    }

    void Update()
    {
        UpdateToBabyBPM();
        UpdateAndVibrateControllers();
    }

    void Start()
    {

    }
}

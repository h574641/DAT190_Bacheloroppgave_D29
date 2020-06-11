using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario1ObjectCollection : MonoBehaviour
{
    public UIStopWatch StopWatch;
    public HeatLamp HeatLamp;
    public BabyController BabyController;
    public BabyContraction BabyContraction;
    public CPRController CprController;
    public JawGrip JawGrip;
    public VentilatorController Ventilator;
    public AttachPulseOxymeter PulseOxymeter;
    public OxygenTurnKnob OxygenTurnKnob;
    public EndEvaluationController EndEvaluationController;
    public AudioManager SceneAmbientAudioManager;
    public RubbingController RubbingControll;
    public bool UseSuccessSound;
    public bool ShowStepsDuringScenario;

    private void Start()
    {
        if (this.StopWatch == null)
        {
            Debug.LogError("Stopwatch object or component is null. Errors may occur after this.");
        }

        if (this.BabyController == null)
        {
            Debug.LogError("BabyControler object or component is null. Errors may occur after this.");
        }

        if (this.BabyContraction == null)
        {
            Debug.LogError("BabyContraciton object or component is null. Errors may occur after this.");
        }

        if (this.CprController == null)
        {
            Debug.LogError("CprController object or component is null. Errors may occur after this.");
        }

        if (this.JawGrip == null)
        {
            Debug.LogError("Jawgrip object or component is null. Errors may occur after this.");
        }

        if (this.Ventilator == null)
        {
            Debug.LogError("VentilatorController object or component is null. Errors may occur after this.");
        }

        if (this.PulseOxymeter == null)
        {
            Debug.LogError("pulseOxymeter object or component is null. Errors may occur after this.");
        }

        if (this.HeatLamp == null)
        {
            Debug.LogError("heatlamp object or component is null. Errors may occur after this.");
        }

        if (this.OxygenTurnKnob == null)
        {
            Debug.LogError("OxygenTurnKnob object or component is null. Errors may occur after this.");
        }

        if (this.EndEvaluationController == null)
        {
            Debug.LogError("EndEvaluationController object or component is null. Errors may occur after this.");
        }

        if (this.SceneAmbientAudioManager == null)
        {
            Debug.LogError("SceneAmbientAudioManager object or component is null. Errors may occur after this.");
        }
    }

    private void Update()
    {
        EndEvaluationController.showEvaluation = ShowStepsDuringScenario;
    }
}

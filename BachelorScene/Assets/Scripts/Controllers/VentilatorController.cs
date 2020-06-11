using System;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class VentilatorController : MonoBehaviour
{
    private static int RequiredAmoutOfDepressions = 30;
    public int RequiredTimeToVentilate = 30;
    public int LowestDepressionRate = 20;
    public int HighestDepressionRate = 80;
    public int TooLongPauseInVentilation = 20;
    public bool SuccessfulVentilation = false;

    public GameObject LeftBone;
    public GameObject RightBone;
    public float m_minimumBoneScale = 0.5f;
    public float m_maximumBoneScale = 1.0f;

    [HideInInspector]
    public double VentilationRate;
    [HideInInspector]
    public float VentilationDuration;
    public bool StartedVentilating { get; private set; } = false;

    private float m_keyboardLerpStep = 5.0f;
    public float lerp = 1f;
    private float previousLerp = 1f;
    private float previousBoneLerp = -1f;

    private float lastVentilationTime;
    private bool depressed;

    private PeekAQueue<float> DepressionsQueue = new PeekAQueue<float>(RequiredAmoutOfDepressions);
    private bool depressionSoundPlayed = false;
    private bool releaseSoundPlayed = false;

    private AudioManager audioManager;

    public GameObject BabyBreastBone;
    private float m_minimumBreastBoneScale = 0.99f;
    private float m_maximumBreastBoneScale = 1.2f;

    private VentilatorAttacher ventilatorAttacher;

    private bool simulatedPress;

    private OVRGrabbable grabbable;
    private bool setTooltipForSqueeze;
    private OVRInput.Controller grabbedBy;
    private string previousTooltip;

    private void Start()
    {
        audioManager = GetComponent<AudioManager>();
        ventilatorAttacher = GetComponentInChildren<VentilatorAttacher>();
        grabbable = GetComponent<OVRGrabbable>();
    }

    private void Update()
    {
        updateLerpAcordingToInput();
        updateBones();
        updateDepressedState();

        playSoundAndUpdateStateForSound();

        updateTooltips();

        if (!SuccessfulVentilation && ventilatorAttacher.Attached && StartedVentilating && checkVentilatingOverDuration())
        {
            SuccessfulVentilation = true;
        }
    }

    private void updateTooltips()
    {
        OVRInput.Controller controller = grabbable?.grabbedBy?.Controller ?? OVRInput.Controller.None;

        if (grabbable.isGrabbed && controller != OVRInput.Controller.None)
        {
            string buttonName = VRButtonHelper.HumanizeButton(VRButtonHelper.GetIndexTrigger(controller));
            string tooltip = $"{buttonName} - Squeeze Ventilator";

            grabbedBy = controller;
            setTooltipForSqueeze = true;

            if (!HandVRTooltipController.HasTooltipText(controller))
            {
                HandVRTooltipController.ShowTooltip(controller, tooltip);
                previousTooltip = tooltip;
            }
        }

        if (setTooltipForSqueeze && controller == OVRInput.Controller.None)
        {
            if (HandVRTooltipController.GetTooltipText(grabbedBy).Equals(previousTooltip))
            {
                HandVRTooltipController.HideTooltip(grabbedBy);
            }

            setTooltipForSqueeze = false;
            grabbedBy = OVRInput.Controller.None;
        }
    }

    private void playSoundAndUpdateStateForSound()
    {
        depressionSoundPlayed = lerp < 0.1 && depressionSoundPlayed ? false : true;
        releaseSoundPlayed = lerp > 0.9 && releaseSoundPlayed ? false : true;

        bool ventilatorIsBeingDepressed = lerp - previousLerp < 0 ? true : false;
        bool ventilatorIsBeingReleased = lerp - previousLerp > 0 ? true : false;

        if (ventilatorIsBeingDepressed && !depressionSoundPlayed)
        {
            audioManager?.Play("VentilatorPress");
            depressionSoundPlayed = true;
        }

        if (ventilatorIsBeingReleased && !releaseSoundPlayed)
        {
            audioManager?.Play("VentilatorRelease");
            releaseSoundPlayed = true;
        }
    }

    private void registrerDepression()
    {
        if (depressed && ventilatorAttacher.Attached)
        {
            DepressionsQueue.Enqueue(Time.time);
            calculateVentilationRate();
        }
    }

    //Checks if the user is using KBM or VR controllers, updates lerp, and StartedVentilating
    private void updateLerpAcordingToInput()
    {
        previousLerp = lerp;

        if (VRKBMInputState.UsingVR)
        {
            OVRGrabbable grabbable = GetComponent<OVRGrabbable>();
            OVRInput.Controller controller = grabbable?.grabbedBy?.Controller ?? OVRInput.Controller.None;
            if (grabbable.isGrabbed)
            {
                lerp = 1 - OVRInput.Get(VRButtonHelper.GetIndexTriggerAnalogue(controller));
                StartedVentilating = true;
            }
            else
            {
                // The user is no longer holding the object, reset to unsqueezed
                lerp = 1;
            }
        }
        else if (VRKBMInputState.UsingKBM)
        {
            if (Input.GetKey(KeyCode.P) || simulatedPress)
            {
                lerp = Mathf.Clamp(lerp - m_keyboardLerpStep * Time.deltaTime, 0.0f, 1.0f);
                StartedVentilating = true;
            }
            else
            {
                lerp = Mathf.Clamp(lerp + m_keyboardLerpStep * Time.deltaTime, 0.0f, 1.0f);
            }
        }
    }

    private void updateBones()
    { 
        // Prevents small jumps to cause updates every single frame
        if (Mathf.Abs(lerp - previousBoneLerp) > 0.025)
        {
            LeftBone.transform.localScale = new Vector3(Mathf.Lerp(m_minimumBoneScale, m_maximumBoneScale, lerp), 1, 1);
            RightBone.transform.localScale = new Vector3(Mathf.Lerp(m_minimumBoneScale, m_maximumBoneScale, lerp), 1, 1);

            if (ventilatorAttacher.Attached)
            {
                BabyBreastBone.transform.localScale = new Vector3(Mathf.Lerp(m_maximumBreastBoneScale, m_minimumBreastBoneScale, lerp), 1, 1);
            }

            previousBoneLerp = lerp;
        }
    }

    private void updateDepressedState()
    {
        if (depressed)
        {
            if (lerp >= 0.5 && previousLerp < 0.5)
            {
                depressed = false;
            }
        }
        else
        {
            if (lerp < 0.5)
            {
                depressed = true;
                registrerDepression();
            }
        }
    }

    //Attempts to calculate the rate the user is pressing the ventilator per minute
    private void calculateVentilationRate()
    {
        int queueCount = DepressionsQueue.Count;

        if (queueCount >= 3)
        {
            float sum = 0;

            for (int i = 0; i < queueCount - 1; i++)
            {
                sum += DepressionsQueue.Peek(i) - DepressionsQueue.Peek(i + 1);
            }

            VentilationRate = 60 / (sum / (queueCount - 1));
            Debug.Log("Vent rate: " + VentilationRate);
        }
    }

    private bool checkVentilatingOverDuration()
    {
        if (DepressionsQueue.Count == RequiredAmoutOfDepressions)
        {
            float firstComp = DepressionsQueue.Peek(RequiredAmoutOfDepressions - 1);
            float lastComp = DepressionsQueue.Peek();
            bool enoughDepressions = lastComp - firstComp < RequiredTimeToVentilate; //check if there is enough depressions in the a given time
            VentilationDuration = lastComp - firstComp;

            return enoughDepressions && !checkForLongPauses() && checkVentilationRate();
        }

        return false;
        
    }

    private bool checkVentilationRate()
    {
        calculateVentilationRate();

        return LowestDepressionRate <= VentilationRate && VentilationRate <= HighestDepressionRate;
    }

    private bool checkForLongPauses()
    {
        int queCount = DepressionsQueue.Count;

        if (queCount > 3)
        {
            for (int i = 0; i < queCount - 2; i++)
            {
                if (DepressionsQueue.Peek(i) - DepressionsQueue.Peek(i + 1) > TooLongPauseInVentilation)
                {
                    return true;
                }
            }
        }

        return false;
    }
    
    public void SimulateKeyPress(bool squeeze)
    {
        simulatedPress = squeeze;
    }
}
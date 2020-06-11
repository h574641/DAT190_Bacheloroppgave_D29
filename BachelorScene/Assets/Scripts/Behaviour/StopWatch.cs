using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using System;

public class StopWatch : MonoBehaviour
{
    public List<GameObject> Segments;
    public Material SegmentOnMaterial;
    public Material SegmentOffMaterial;

    private bool active;
    private static Timer timer;
    private float timeAcc = 0;

    private Dictionary<OVRInput.Controller, string> displayedTooltips = new Dictionary<OVRInput.Controller, string>();

    // Start is called before the first frame update
    void Start()
    {
        // Set up initial materials
        foreach (GameObject segment in Segments)
        {
            SevenSegmentHelper.InitializeSegment(segment, SegmentOnMaterial, SegmentOnMaterial);
        }

        // Set up events to the VR interactive button
        var buttonScript = gameObject.GetComponent<BaseButton>();

        buttonScript.OnActivatedEvent += ButtonScript_OnActivatedEvent;
        buttonScript.OnDectivatedEvent += ButtonScript_OnDectivatedEvent;
        buttonScript.OnInteractEvent += ButtonScript_OnInteractEvent;
        buttonScript.OnUnInteractEvent += ButtonScript_OnUnInteractEvent;

        active = buttonScript.m_active;

        // Update display so it starts at 00:00
        UpdateSegmentDisplay(0);
    }

    private void ButtonScript_OnUnInteractEvent(object button, object from)
    {
        hideInteractionTooltip(getControllerFromInteraction(from));
    }

    private OVRInput.Controller getControllerFromInteraction(object from)
    {
        return (from as ButtonInteracter)?.Controller ?? OVRInput.Controller.None;
    }

    private void ButtonScript_OnInteractEvent(object button, object from, bool active)
    {
        showInteractionTooltip(getControllerFromInteraction(from));
    }

    private void ButtonScript_OnDectivatedEvent(object button, object from)
    {
        ResetTimer();
    }

    private void ButtonScript_OnActivatedEvent(object button, object from)
    {
        StartTimer();
    }

    private void showInteractionTooltip(OVRInput.Controller controller)
    {
        string buttonName = VRButtonHelper.HumanizeButton(VRButtonHelper.GetHandTrigger(controller));
        string newStatus = active ? "Reset Stopwatch" : "Stop Stopwatch";
        string tooltip = $"{buttonName} - {newStatus}";

        HandVRTooltipController.ShowTooltip(controller, tooltip);

    }

    public void hideInteractionTooltip(OVRInput.Controller controller)
    {
         HandVRTooltipController.HideTooltip(controller);
    }

    public void StartTimer()
    {
        PlayCorrectStateSound();

        active = true;
    }

    public void ToggleTimer()
    {
        PlayCorrectStateSound();

        active = !active;
    }

    public void ResetTimer()
    {
        PlayCorrectStateSound();

        active = false;
        timeAcc = 0f;

        UpdateSegmentDisplay(0);
    }
    public bool isStopWatchStarted()
    {
        return active;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            timeAcc += Time.deltaTime;

            UpdateSegmentDisplay(Mathf.FloorToInt(timeAcc));
        }

        // Debug hotkeys for keyboard 
        // Toggle Timer
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleTimer();
        }

        // Reset Timer
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetTimer();
        }
    }

    private void PlayCorrectStateSound()
    {
        AudioManager audioManager = gameObject.GetComponent<AudioManager>();

        audioManager.Play(active ? "Boop" : "Beep");
    }

    private void UpdateSegmentDisplay(int seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);

        int onesSeconds = time.Seconds % 10;
        int tensSeconds = Mathf.FloorToInt(time.Seconds / 10);
        int onesMinutes = time.Minutes % 10;
        int tensMinutes = Mathf.FloorToInt(time.Minutes / 10);

        SevenSegmentHelper.UpdateSegment(Segments[0], tensMinutes, SegmentOnMaterial, SegmentOffMaterial);
        SevenSegmentHelper.UpdateSegment(Segments[1], onesMinutes, SegmentOnMaterial, SegmentOffMaterial);
        SevenSegmentHelper.UpdateSegment(Segments[2], tensSeconds, SegmentOnMaterial, SegmentOffMaterial);
        SevenSegmentHelper.UpdateSegment(Segments[3], onesSeconds, SegmentOnMaterial, SegmentOffMaterial);
    }
}
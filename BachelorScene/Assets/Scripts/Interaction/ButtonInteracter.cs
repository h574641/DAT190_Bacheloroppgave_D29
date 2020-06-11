using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteracter : MonoBehaviour
{
    public Collider[] m_interativityColliders;

    public OVRInput.Controller m_controller;

    public GameObject Hand;
    public OVRInput.Controller Controller
    {
        get
        {
            return GetComponentInParent<OVRGrabber>()?.Controller ?? OVRInput.Controller.None;
        }
    }

    public enum ButtonChoice
    {
        HandTrigger,
        IndexTrigger,
        ActionButton
    }


    private HashSet<BaseButton> buttons = new HashSet<BaseButton>();

    private float interactionThreshold = 0.4f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Check if activation button is active
    private bool checkInput(BaseButton button)
    {
        switch (button.m_button)
        {
            case ButtonChoice.HandTrigger:
                return OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, m_controller) > interactionThreshold;

            case ButtonChoice.IndexTrigger:
                return OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, m_controller) > interactionThreshold;

            case ButtonChoice.ActionButton:
                return OVRInput.Get(VRButtonHelper.GetTopFaceButton(m_controller));

            default:
                return false;
        }
    }

    // Send events to target buttons
    void Update()
    {
        foreach (BaseButton button in buttons)
        {
            bool inputDown = checkInput(button);

            button?.OnInteract(this, inputDown);
        }
    }

    // Add potential target button
    void OnTriggerEnter(Collider otherCollider)
    {
        BaseButton button = otherCollider.GetComponentInParent<BaseButton>();

        if (button != null)
        {
            buttons.Add(button);
        }
    }

    // Remove potential target button
    void OnTriggerExit(Collider otherCollider)
    {
        BaseButton button = otherCollider.GetComponentInParent<BaseButton>();

        if (button != null && buttons.Contains(button))
        {
            buttons.Remove(button);
            button.OnUninteract(this);
        }
    }
}

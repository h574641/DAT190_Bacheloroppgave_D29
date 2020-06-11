using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenTurnKnob : MonoBehaviour
{
    public float Percent = 0f;
    public float MinimumPercent = 0f;
    public float MaximumPercent = 0.999f;

    public float RotationOffset = 0f;
    public float RotationRange = 360f; // Negative values turn the other way

    private Vector3 initialHandRotation;
    private Vector3 previousHandRotation;
    private GameObject hand;

    private bool rotating;
    private float previousRotation;
    private float percentAtGrabStart;

    private GameObject getHandFromInteraction(object from)
    {
        return (from as ButtonInteracter).Hand;
    }

    private OVRInput.Controller getControllerFromInteraction(object from)
    {
        return (from as ButtonInteracter)?.Controller ?? OVRInput.Controller.None;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            Percent = 0.95f;
        }
    }
    void Start()
    {
        BaseButton buttonScript = gameObject.GetComponent<BaseButton>();

        buttonScript.OnInteractEvent += ButtonScript_OnInteractEvent;
        buttonScript.OnUnInteractEvent += ButtonScript_OnUnInteractEvent;
    }

    private void ButtonScript_OnUnInteractEvent(object button, object from)
    {
        hideInteractionTooltip(getControllerFromInteraction(from));
        updateRotationPercent();

        rotating = false;
    }

    private void ButtonScript_OnInteractEvent(object button, object from, bool active)
    {
        OVRInput.Controller controller = getControllerFromInteraction(from);

        showInteractionTooltip(controller);

        // Turn logic
        if (active && !rotating)
        {
            hand = getHandFromInteraction(from);

            rotating = true;
            initialHandRotation = getLocalSpaceRotation(hand).eulerAngles;
            previousHandRotation = initialHandRotation;
            percentAtGrabStart = Percent;
        }

        if (!active && rotating)
        {
            rotating = false;
        }
    }

    private void updateRotationIfNeeded(float newRotation)
    {
        if (Mathf.Abs(newRotation - previousRotation) > 0.001)
        {
            Vector3 knobAngles = transform.localEulerAngles;

            transform.localEulerAngles = new Vector3(knobAngles.x, knobAngles.y, newRotation);
            previousRotation = newRotation;
        }
    }

    // TODO - Find better way to determine rotation
    private Quaternion getCorrectRotation(Quaternion rotation)
    {
        // This does not work as expected
        //return Quaternion.Inverse(transform.rotation) * rotation;

        return rotation;
    }

    private Quaternion getLocalSpaceRotation(GameObject obj)
    {
        return getCorrectRotation(obj.transform.rotation);
    }

    private float getCorrectDelta(float n, float m)
    {
        float delta = n - m;

        if (delta < -180)
        {
            delta += 360;
        }

        if (Mathf.Abs(delta) > 180)
        {
            delta -= 360 * Mathf.Sign(delta);
        }

        return delta;
    }

    private void updateRotationPercent()
    {
        if (rotating)
        {
            Vector3 handRotation = getLocalSpaceRotation(hand).eulerAngles;
            float deltaPreviousRotation = getCorrectDelta(handRotation.z, previousHandRotation.z);

            Percent = Mathf.Clamp(Percent - deltaPreviousRotation / Mathf.Abs(RotationRange), MinimumPercent, MaximumPercent);

            previousHandRotation = handRotation;
        }
    }

    private void showInteractionTooltip(OVRInput.Controller controller)
    {
        string tooltip;
        string buttonName = VRButtonHelper.HumanizeButton(VRButtonHelper.GetHandTrigger(controller));

        if (rotating)
        {
            tooltip = $"{buttonName} (Hold) - Turn Knob";
        } 
        else
        {
            tooltip = $"{buttonName} (Hold) - Grab Knob";
        }

        HandVRTooltipController.ShowTooltip(controller, tooltip);
    }

    public void hideInteractionTooltip(OVRInput.Controller controller)
    {
        HandVRTooltipController.HideTooltip(controller);
    }

    void FixedUpdate()
    {
        updateRotationPercent();
        updateRotationIfNeeded(RotationOffset + Percent * RotationRange);
    }
}

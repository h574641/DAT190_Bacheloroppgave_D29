using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRButtonHelper
{
    // Helper methods to grab buttons by generic name on controllers
    // Allows using "Top Face Button" over figuring out if it is Y or B manually

    public static Dictionary<OVRInput.Button, string> buttonNames = new Dictionary<OVRInput.Button, string>
    {
        { OVRInput.Button.One, "A" },
        { OVRInput.Button.Two, "B" },
        { OVRInput.Button.Three, "X" },
        { OVRInput.Button.Four, "Y" },

        { OVRInput.Button.PrimaryThumbstick, "Left Stick" },
        { OVRInput.Button.SecondaryThumbstick, "Right Stick" },

        { OVRInput.Button.PrimaryIndexTrigger, "Index Trigger" },
        { OVRInput.Button.PrimaryHandTrigger, "Hand Trigger" },
        { OVRInput.Button.SecondaryIndexTrigger, "Index Trigger" },
        { OVRInput.Button.SecondaryHandTrigger, "Hand Trigger" },
    };

    public static OVRInput.Button GetTopFaceButton(OVRInput.Controller controller)
    {
        switch (controller)
        {
            case OVRInput.Controller.LTouch:
                return OVRInput.Button.Four;

            case OVRInput.Controller.RTouch:
                return OVRInput.Button.Two;

            default:
                return OVRInput.Button.None;
        }
    }

    public static OVRInput.Button GetBottomFaceButton(OVRInput.Controller controller)
    {
        switch (controller)
        {
            case OVRInput.Controller.LTouch:
                return OVRInput.Button.Three;

            case OVRInput.Controller.RTouch:
                return OVRInput.Button.One;

            default:
                return OVRInput.Button.None;
        }
    }

    public static OVRInput.Button GetIndexTrigger(OVRInput.Controller controller)
    {
        switch (controller)
        {
            case OVRInput.Controller.LTouch:
                return OVRInput.Button.PrimaryIndexTrigger;

            case OVRInput.Controller.RTouch:
                return OVRInput.Button.SecondaryIndexTrigger;

            default:
                return OVRInput.Button.None;
        }
    }

    public static OVRInput.Axis1D GetIndexTriggerAnalogue(OVRInput.Controller controller)
    {
        switch (controller)
        {
            case OVRInput.Controller.LTouch:
                return OVRInput.Axis1D.PrimaryIndexTrigger;

            case OVRInput.Controller.RTouch:
                return OVRInput.Axis1D.SecondaryIndexTrigger;

            default:
                return OVRInput.Axis1D.None;
        }
    }

    public static OVRInput.Button GetHandTrigger(OVRInput.Controller controller)
    {
        switch (controller)
        {
            case OVRInput.Controller.LTouch:
                return OVRInput.Button.PrimaryHandTrigger;

            case OVRInput.Controller.RTouch:
                return OVRInput.Button.SecondaryHandTrigger;

            default:
                return OVRInput.Button.None;
        }
    }

    public static OVRInput.Axis1D GetHandTriggerAnalogue(OVRInput.Controller controller)
    {
        switch (controller)
        {
            case OVRInput.Controller.LTouch:
                return OVRInput.Axis1D.PrimaryHandTrigger;

            case OVRInput.Controller.RTouch:
                return OVRInput.Axis1D.SecondaryHandTrigger;

            default:
                return OVRInput.Axis1D.None;
        }
    }

    public static string HumanizeButton(OVRInput.Button button)
    {
        return buttonNames.GetOrDefault(button, "Unknown");
    }
}

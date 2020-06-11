using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbingTooltips : MonoBehaviour
{
    public GameObject LeftHand;
    public GameObject RightHand;

    private OVRGrabber leftHandGrabber;
    private OVRGrabber rightHandGrabber;

    // Start is called before the first frame update
    void Start()
    {
        leftHandGrabber = LeftHand.GetComponent<OVRGrabber>();
        rightHandGrabber = RightHand.GetComponent<OVRGrabber>();
    }

    void updateControllerTooltip(GameObject hand, OVRGrabber grabber)
    {
        // Grabbing tooltips should have lowest priority

        OVRInput.Controller controller = grabber.Controller;
        string buttonName = VRButtonHelper.HumanizeButton(VRButtonHelper.GetHandTrigger(controller));
        string grabTooltip = $"{buttonName} (Hold) - Grab";

        if (grabber.grabbedObject == null && grabber.grabbingCandidates.Count > 0)
        {
            if (!HandVRTooltipController.HasTooltipText(controller))
            {
                HandVRTooltipController.ShowTooltip(controller, grabTooltip);
            }
        }
        else
        {
            if (HandVRTooltipController.GetTooltipText(controller).Equals(grabTooltip))
            {
                HandVRTooltipController.HideTooltip(controller);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateControllerTooltip(LeftHand, leftHandGrabber);
        updateControllerTooltip(RightHand, rightHandGrabber);
    }
}

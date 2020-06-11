using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPRInteraction : MonoBehaviour
{
    public CPRController CPR;
    public bool StopCompressionWhenNoHands = false;
    public int MinimumControllersPerCompression = 2;

    private bool previousState = false;
    private int previousControllersPressing = 0;

    private HashSet<OVRInput.Controller> controllersInCPRCollider = new HashSet<OVRInput.Controller>();

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        setTooltips();
        checkUserInput();
    }

    private void setTooltips()
    {
        foreach (OVRInput.Controller controller in controllersInCPRCollider)
        {
            string buttonName = VRButtonHelper.HumanizeButton(VRButtonHelper.GetTopFaceButton(controller));
            string tooltip = $"{buttonName} - Compress";

            HandVRTooltipController.ShowTooltip(controller, tooltip);
        }
    }

    // Check if the user is performing the inputs for a compression
    // This means using one/both controllers, and then letting go off all buttons to reset
    // Queues up a CPR compression if inputs change
    private void checkUserInput()
    {
        int activeControllers = 0;
        bool newState = false;

        foreach (OVRInput.Controller controller in controllersInCPRCollider)
        {
            if (controller != OVRInput.Controller.None) {
                activeControllers += OVRInput.Get(VRButtonHelper.GetTopFaceButton(controller)) ? 1 : 0;
            }
        }

        if (previousState)
        {
            newState = activeControllers > 0;
        }
        else
        {
            newState = activeControllers >= MinimumControllersPerCompression;
        }

        CPR.UpdateCompressionState(newState && !previousState);

        previousState = newState;
        previousControllersPressing = activeControllers;
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        ButtonInteracter interacter = otherCollider.GetComponentInParent<ButtonInteracter>();

        if (interacter != null)
        {
            OVRInput.Controller controller = interacter.Controller;

            controllersInCPRCollider.Add(controller);
            CPR.StartCompressions();
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        ButtonInteracter interacter = otherCollider.GetComponentInParent<ButtonInteracter>();

        if (interacter != null)
        {
            OVRInput.Controller controller = interacter.Controller;

            HandVRTooltipController.HideTooltip(controller);
            controllersInCPRCollider.Remove(controller);

            if (controllersInCPRCollider.Count == 0)
            {
                // TODO - Check if removing hands from area stopping CPR is intuitive or not
                if (StopCompressionWhenNoHands)
                {
                    CPR.StopCompressions();
                }
            }
        }
    }
}

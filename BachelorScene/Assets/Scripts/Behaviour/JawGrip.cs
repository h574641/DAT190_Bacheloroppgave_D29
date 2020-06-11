using System;
using System.Collections.Generic;
using UnityEngine;

public class JawGrip : MonoBehaviour
{
    private static Vector3 jawBoneNormalTransformLocalRotation = new Vector3(0, 0, -110f);
    private static Vector3 jawBoneGripTransformLocalRotation = new Vector3(0, 0, -125f);

    private static float jawGripNormalRotation = -15f;
    private static float jawGripGripRotation = 15f;

    [HideInInspector]
    public bool HoldingGrip = false;
    [HideInInspector]
    public bool PreviousGrip = false;

    private bool userInput;
    private bool previousUserInput;

    public GameObject JawBone, HeadBone;
    public bool ForceHandInCollider = false;
    public bool AllowTogglingJawGrip = false;

    public float LerpTimeMultiplier = 1f;

    private float animationLerp = 0f;
    private float animationLerpPrevious = 0f;

    private Rigidbody headBoneRigidbody;

    // Enums would be saner, but this works
    private string previousTransform;

    private HashSet<OVRInput.Controller> controllersInCollider = new HashSet<OVRInput.Controller>();
    private HashSet<OVRInput.Controller> relevantInputControllers = new HashSet<OVRInput.Controller>();

    void Start()
    {
        headBoneRigidbody = HeadBone.GetComponent<Rigidbody>();
    }

    void Update()
    {
        updateRelevantControllers();
        updateUserInput();
        updateGripStatus();
        setTooltips();

        updateAnimationLerp();
        updateTransformsAndRotations();
    }

    private void updateAnimationLerp()
    {
        float direction = HoldingGrip ? 1 : -1;

        animationLerpPrevious = animationLerp;
        animationLerp = Mathf.Clamp(animationLerp + Time.deltaTime * LerpTimeMultiplier * direction, 0.0f, 1.0f);
    }

    // Update baby transformations and rotations with lerping
    private void updateTransformsAndRotations()
    {
        if (Mathf.Abs(animationLerp - animationLerpPrevious) >= 0.005) {
            Vector3 angles = HeadBone.transform.eulerAngles;
            float rotationLerp = Mathf.Lerp(jawGripNormalRotation, jawGripGripRotation, animationLerp);

            HeadBone.transform.eulerAngles = new Vector3(angles.x, angles.y, Mathf.Clamp(rotationLerp, jawGripNormalRotation, jawGripGripRotation));
            //HeadBone.transform.Rotate(Vector3.forward, Mathf.Lerp(jawGripNormalRotation, jawGripGripRotation, animationLerp));
            JawBone.transform.localEulerAngles = Vector3.Lerp(jawBoneNormalTransformLocalRotation, jawBoneGripTransformLocalRotation, animationLerp);
        }
    }

    private void updateGripStatus()
    {
        if (AllowTogglingJawGrip)
        {
            if (userInput && !previousUserInput)
            {
                HoldingGrip = !HoldingGrip;
            }
        }
        else
        {
            HoldingGrip = userInput;
        }
    }

    // Keep controllers that are relevant to the jawgrip
    // This includes controllers that initally held the button within the collider if not using toggle mode
    private void updateRelevantControllers()
    {
        HashSet<OVRInput.Controller> newRelevantControllers = new HashSet<OVRInput.Controller>();

        foreach (OVRInput.Controller controller in relevantInputControllers)
        {
            if (!ForceHandInCollider && gripButtonHeld(controller)) 
            {
                newRelevantControllers.Add(controller);
            }
            else
            {
                HandVRTooltipController.HideTooltip(controller);
            }
        }

        foreach (OVRInput.Controller controller in controllersInCollider)
        {
            newRelevantControllers.Add(controller);
        }

        relevantInputControllers.Clear();
        relevantInputControllers.UnionWith(newRelevantControllers);
    }

    private bool gripButtonHeld(OVRInput.Controller controller)
    {
        if (controller != OVRInput.Controller.None)
        {
            if (OVRInput.Get(VRButtonHelper.GetTopFaceButton(controller)))
            {
                return true;
            }
        }

        return false;
    }

    private void updateUserInput()
    {
        if (Input.GetKey(KeyCode.K))
        {
            previousUserInput = userInput;
            userInput = true;

            return;
        }

        foreach (OVRInput.Controller controller in relevantInputControllers)
        {
            if (gripButtonHeld(controller))
            {
                previousUserInput = userInput;
                userInput = true;

                return;
            }
        }

        previousUserInput = userInput;
        userInput = false;
    }

    // These transform updates are deprecated
    // updateTransformsAndRotations() does the same but smoother
    private void updateTransform()
    {
        transformBonesBackToNormal();
        transformBonesToJawGrip();
    }

    private void transformBonesBackToNormal()
    {
        if (!HoldingGrip && previousTransform != "backToNormal")
        {
            HeadBone.transform.Rotate(Vector3.forward, jawGripNormalRotation);
            JawBone.transform.localEulerAngles = jawBoneNormalTransformLocalRotation;

            previousTransform = "backToNormal";
        }
    }

    private void transformBonesToJawGrip()
    {
        if (HoldingGrip && previousTransform != "toJawGrip")
        {
            HeadBone.transform.Rotate(Vector3.forward, jawGripGripRotation);
            JawBone.transform.localEulerAngles = jawBoneGripTransformLocalRotation;

            previousTransform = "toJawGrip";
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        ButtonInteracter interacter = otherCollider.GetComponentInParent<ButtonInteracter>();
        OVRGrabber grabber = otherCollider.GetComponentInParent<OVRGrabber>();

        if (interacter != null && grabber?.grabbedObject == null)
        {
            OVRInput.Controller controller = interacter.Controller;

            controllersInCollider.Add(controller);
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        ButtonInteracter interacter = otherCollider.GetComponentInParent<ButtonInteracter>();

        if (interacter != null)
        {
            OVRInput.Controller controller = interacter.Controller;

            HandVRTooltipController.HideTooltip(controller);
            controllersInCollider.Remove(controller);
        }
    }

    private void setTooltips()
    {
        foreach (OVRInput.Controller controller in relevantInputControllers)
        {
            string tooltip = "";
            string buttonName = VRButtonHelper.HumanizeButton(VRButtonHelper.GetTopFaceButton(controller));

            if (gripButtonHeld(controller))
            {
                if (!AllowTogglingJawGrip)
                {
                    tooltip = $"{buttonName} (Hold) - Holding Jaw Grip";
                }
            }
            else
            {
                if (!AllowTogglingJawGrip)
                {
                    tooltip = $"{buttonName} (Hold) - Hold Jaw Grip";
                }
                else
                {
                    string info = HoldingGrip ? "Release Jaw Grip" : "Start Jaw Grip";

                    tooltip = $"{buttonName} - {info}";
                }
            }

            if (!String.IsNullOrEmpty(tooltip))
            {
                HandVRTooltipController.ShowTooltip(controller, tooltip);
            }
        }
    }
}
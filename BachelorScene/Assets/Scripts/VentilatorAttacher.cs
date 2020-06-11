using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO
// Implement detaching the ventilator
// Check that both tooltips work
// Make attaching ungrab the object, requires magic in OVR land

public class VentilatorAttacher : MonoBehaviour
{
    [HideInInspector]
    public bool Attached;

    [HideInInspector]
    public OVRInput.Controller ControllerInCollider;

    public GameObject Ventilator;

    public Rigidbody AttachA;
    public GameObject AttachB;
    public GameObject MaskBone;
    public int BreakForce = 200;

    private VentilatorAttachPoint targetAttachPoint;
    private FixedJoint joint;
    private Rigidbody attachBodyRigidBody;
    private bool maskStatusChanged = false;
    private OVRInput.Controller controllerThatAttached;

    void Start()
    {
        attachBodyRigidBody = MaskBone.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        checkIfJointBroke();
        updateMaskPosition();

        checkForVentilatorDetach();
        checkForVentilatorAttach();

        maskStatusChanged = false;
    }

    private void checkIfJointBroke()
    {
        if (Attached && joint == null)
        {
            DetachMask();
        }
    }

    private void updateMaskPosition()
    {
        if (Attached)
        {
            // Doesn't work as expected, causes problem with OVR Grabbing
            //attachBodyRigidBody.transform.position = targetAttachPoint.AttachTo.transform.position;
            //attachBodyRigidBody.transform.rotation = targetAttachPoint.AttachTo.transform.rotation;
            //attachBodyRigidBody.velocity = Vector3.zero;
        }
    }

    private void checkForVentilatorAttach()
    {
        if (!maskStatusChanged && !Attached && ControllerInCollider != OVRInput.Controller.None)
        {
            if (OVRInput.GetDown(VRButtonHelper.GetTopFaceButton(ControllerInCollider)))
            {
                controllerThatAttached = ControllerInCollider;

                AttachMask();
                HideTooltip();

                maskStatusChanged = true;
            }
        }
    }

    private void checkForVentilatorDetach()
    {
        if (!maskStatusChanged && Attached)
        {
            if (OVRInput.GetDown(VRButtonHelper.GetTopFaceButton(ControllerInCollider)) || OVRInput.GetDown(VRButtonHelper.GetTopFaceButton(controllerThatAttached)))
            {
                DetachMask();
                HideTooltip();

                controllerThatAttached = OVRInput.Controller.None;

                maskStatusChanged = true;
            }
        }
    }

    private void ShowAttachTooltip()
    {
        string buttonName = VRButtonHelper.HumanizeButton(VRButtonHelper.GetTopFaceButton(ControllerInCollider));
        string tooltip = $"{buttonName} - Attach Ventilator";

        HandVRTooltipController.ShowTooltip(ControllerInCollider, tooltip);
    }

    private void ShowDetachTooltip()
    {
        string buttonName = VRButtonHelper.HumanizeButton(VRButtonHelper.GetTopFaceButton(ControllerInCollider));
        string tooltip = $"{buttonName} - Detach Ventilator";

        HandVRTooltipController.ShowTooltip(ControllerInCollider, tooltip);
    }

    private void HideTooltip()
    {
        HandVRTooltipController.HideTooltip(ControllerInCollider);
    }

    private void setMaskCollidersIsTrigger(bool state)
    {
        foreach (Collider collider in MaskBone.GetComponents<Collider>())
        {
            collider.isTrigger = state;
        }
    }

    private void AttachMask()
    {
        if (!Attached)
        {
            joint = AttachB.AddComponent<FixedJoint>() as FixedJoint;

            joint.breakForce = BreakForce;
            joint.connectedBody = AttachA;

            setMaskCollidersIsTrigger(true);

            Attached = true;
        }
    }

    private void DetachMask()
    {
        if (Attached)
        {
            if (joint != null)
            {
                Destroy(joint);
            }

            setMaskCollidersIsTrigger(false);

            Attached = false;
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        VentilatorAttachPoint attachPoint = otherCollider.GetComponentInChildren<VentilatorAttachPoint>();
        OVRInput.Controller controller = Ventilator.GetComponentInChildren<OVRGrabbable>()?.grabbedBy?.Controller ?? OVRInput.Controller.None;

        if (attachPoint != null && controller != OVRInput.Controller.None && !Attached)
        {
            ControllerInCollider = controller;
            targetAttachPoint = attachPoint;
            ShowAttachTooltip();
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        VentilatorAttachPoint attachPoint = otherCollider.GetComponentInParent<VentilatorAttachPoint>();

        if (attachPoint != null)
        {
            HideTooltip();
            ControllerInCollider = OVRInput.Controller.None;
        }
    }
}

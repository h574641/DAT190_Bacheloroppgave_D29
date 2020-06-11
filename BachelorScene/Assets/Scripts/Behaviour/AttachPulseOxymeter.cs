using UnityEngine;

public class AttachPulseOxymeter : MonoBehaviour
{
    public BoxCollider PulseOximeterCollider;
    public GameObject PulseOximeter, RightFootToeBone, LeftFootToeBone;
    public bool IsOximeterAttached { get; protected set; } = false;

    private static Vector3 rightFootPositionTransform = new Vector3(-0.0032f, -0.0116f, 0.0115f);
    private static Vector3 leftFootPositionTransform = new Vector3(0.0048f, 0.01f, 0.0151f);

    private static Vector3 rightFootRotationTransform = new Vector3(-71.22f, -46.8f, -45.171f);
    private static Vector3 leftFootRotationTransform = new Vector3(96.31699f, -137.463f, -51.35797f);

    private string displayedTooltip;
    private OVRInput.Controller controllerInCollider;
    private GameObject targetBone;

    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other?.gameObject;

        if (!IsOximeterAttached && (gameObject == RightFootToeBone || gameObject == LeftFootToeBone))
        {
            controllerInCollider = PulseOximeter.GetComponentInChildren<OVRGrabbable>()?.grabbedBy?.Controller ?? OVRInput.Controller.None;
            targetBone = gameObject;

            ShowAttachTooltip();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject gameObject = other?.gameObject;

        if (!IsOximeterAttached && (gameObject == RightFootToeBone || gameObject == LeftFootToeBone))
        {
            HideTooltip();

            targetBone = null;
            controllerInCollider = OVRInput.Controller.None;
        }
    }

    void FixedUpdate()
    {
        CheckForVentilatorAttach();
    }

    private void DisablePhysicsCollider(GameObject obj)
    {
        foreach (BoxCollider collider in obj.GetComponents<BoxCollider>())
        {
            if (!collider.isTrigger)
            {
                collider.enabled = false;
            }
        }
    }

    private void CheckForVentilatorAttach()
    {
        if (!IsOximeterAttached && controllerInCollider != OVRInput.Controller.None)
        {
            if (OVRInput.Get(VRButtonHelper.GetTopFaceButton(controllerInCollider)))
            {
                OVRGrabbable grabber = PulseOximeter.GetComponentInChildren<OVRGrabbable>();

                grabber.ReleaseObject();
                grabber.enabled = false;

                attachOxymeterToFoot(targetBone);
                DisablePhysicsCollider(PulseOximeter);
                IsOximeterAttached = true;

                HideTooltip();
            }
        }
    }

    private void attachOxymeterToFoot(GameObject foot)
    {
        bool attachedToRightFoot = foot == RightFootToeBone;

        PulseOximeter.GetComponent<Rigidbody>().isKinematic = true;
        PulseOximeter.transform.parent = foot.transform;
        PulseOximeter.transform.position = foot.transform.position;
        PulseOximeter.transform.localPosition = attachedToRightFoot ? rightFootPositionTransform : leftFootPositionTransform;
        PulseOximeter.transform.localEulerAngles = attachedToRightFoot ? rightFootRotationTransform : leftFootRotationTransform;
    }

    private void ShowAttachTooltip()
    {
        string buttonName = VRButtonHelper.HumanizeButton(VRButtonHelper.GetTopFaceButton(controllerInCollider));
        string tooltip = $"{buttonName} - Attach To Foot";

        HandVRTooltipController.ShowTooltip(controllerInCollider, tooltip);
    }

    private void HideTooltip()
    {
        HandVRTooltipController.HideTooltip(controllerInCollider);
    }
}

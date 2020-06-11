using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbingInteracter : MonoBehaviour
{
    public GameObject Baby;
    public string LocationName;

    private BabyController babyController;
    private RubbingController babyRubController;

    private HashSet<OVRInput.Controller> controllersInCollider;
    private Dictionary<OVRInput.Controller, GameObject> controllerToGameObject;
    private Dictionary<OVRInput.Controller, Vector3> interactionStartPosition;
    private Dictionary<OVRInput.Controller, float> interactionStartTime;

    // Start is called before the first frame update
    void Start()
    {
        babyController = Baby.GetComponentInChildren<BabyController>();
        babyRubController = Baby.GetComponentInChildren<RubbingController>();

        controllersInCollider = new HashSet<OVRInput.Controller>();
        controllerToGameObject = new Dictionary<OVRInput.Controller, GameObject>();
        interactionStartPosition = new Dictionary<OVRInput.Controller, Vector3>();
        interactionStartTime = new Dictionary<OVRInput.Controller, float>();
}

    // Update is called once per frame
    void Update()
    {
        checkControllers();
        setTooltips();
    }

    private bool checkTopFaceButton(OVRInput.Controller controller)
    {
        return OVRInput.Get(VRButtonHelper.GetTopFaceButton(controller));
    }

    private void checkControllers()
    {
        foreach (OVRInput.Controller controller in controllersInCollider)
        {
            if (checkTopFaceButton(controller))
            {
                GameObject hand = controllerToGameObject[controller];

                interactionStartPosition[controller] = interactionStartPosition.GetOrDefault(controller, hand.transform.position);
                interactionStartTime[controller] = interactionStartTime.GetOrDefault(controller, Time.time);
            }
            else
            {
                interactionDone(controller);
            }
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
            controllerToGameObject[controller] = otherCollider.gameObject;
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        ButtonInteracter interacter = otherCollider.GetComponentInParent<ButtonInteracter>();

        if (interacter != null)
        {
            OVRInput.Controller controller = interacter.Controller;

            interactionDone(controller);

            controllersInCollider.Remove(controller);
            controllerToGameObject.Remove(controller);
        }
    }

    private void interactionDone(OVRInput.Controller controller)
    {
        HandVRTooltipController.HideTooltip(controller);

        GameObject hand = controllerToGameObject.GetOrDefault(controller, null);
        Vector3 startPosition = interactionStartPosition.GetOrDefault(controller, Vector3.zero);
        Vector3 handPosition = hand?.transform?.position ?? Vector3.zero;

        float startTime = interactionStartTime.GetOrDefault(controller, -1);

        if (startPosition != Vector3.zero && handPosition != Vector3.zero && startTime != -1)
        {
            float distance = Vector3.Distance(startPosition, handPosition);
            float duration = Time.time - startTime;

            Rub rub = new Rub(distance, duration, startTime, LocationName);

            babyRubController.Rubs.Enqueue(rub);
        }

        interactionStartPosition.Remove(controller);
        interactionStartTime.Remove(controller);
    }

    private void setTooltips()
    {
        foreach (OVRInput.Controller controller in controllersInCollider)
        {
            string tooltip;
            string buttonName = VRButtonHelper.HumanizeButton(VRButtonHelper.GetTopFaceButton(controller));

            if (checkTopFaceButton(controller))
            {
                tooltip = $"{buttonName} (Hold) - Move Controller to Stimulate";
            }
            else
            {
                tooltip = $"{buttonName} (Hold) - Stimulate Baby";
            }
            

            HandVRTooltipController.ShowTooltip(controller, tooltip);
        }
    }
}

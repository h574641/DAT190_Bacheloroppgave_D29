using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HeatLamp : MonoBehaviour
{
    public GameObject HeatLampLight;
    public GameObject HeatLampMesh;
    public Material HeatLampOnMaterial;
    public Material HeatLampOffMaterial;
    public GameObject SwitchMesh;

    private BaseButton buttonScript;
    private bool _lightState;

    public bool LightState {
        get
        {
            return _lightState;
        }

        set
        {
            _lightState = value;
            buttonScript.m_active = value;

            UpdateLights();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonScript = GetComponent<BaseButton>();
        LightState = buttonScript.m_active;

        buttonScript.OnActivatedEvent += ButtonScript_OnActivatedEvent;
        buttonScript.OnDectivatedEvent += ButtonScript_OnDectivatedEvent;
        buttonScript.OnInteractEvent += ButtonScript_OnInteractEvent;
        buttonScript.OnUnInteractEvent += ButtonScript_OnUnInteractEvent;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            LightState = !LightState;
        }
    }

    public void ToggleHeatLampMaterial()
    {
        HeatLampMesh.GetComponent<Renderer>().material = LightState ? HeatLampOnMaterial : HeatLampOffMaterial;
    }

    public void UpdateSwitchRotation()
    {
        float angle = LightState ? -10f : 10f;
        Vector3 localAngles = SwitchMesh.transform.localEulerAngles;

        if (localAngles.z != angle)
        {
            SwitchMesh.transform.localEulerAngles = new Vector3(angle, localAngles.y, localAngles.z);
        }
    }

    public void UpdateLights()
    {
        HeatLampLight.GetComponent<Light>().enabled = LightState;
        UpdateSwitchRotation();
        ToggleHeatLampMaterial();
    }

    private OVRInput.Controller getControllerFromInteraction(object from)
    {
        return (from as ButtonInteracter)?.Controller ?? OVRInput.Controller.None;
    }

    private void ButtonScript_OnUnInteractEvent(object button, object from)
    {
        hideInteractionTooltip(getControllerFromInteraction(from));
    }

    private void ButtonScript_OnInteractEvent(object button, object from, bool active)
    {
        showInteractionTooltip(getControllerFromInteraction(from));
    }

    private void ButtonScript_OnDectivatedEvent(object button, object from)
    {
        LightState = false;
    }

    private void ButtonScript_OnActivatedEvent(object button, object from)
    {
        LightState = true;
    }

    private void showInteractionTooltip(OVRInput.Controller controller)
    {
        string buttonName = VRButtonHelper.HumanizeButton(VRButtonHelper.GetTopFaceButton(controller));
        string newStatus = LightState ? "Turn Off Heatlamp" : "Turn On Heatlamp";
        string tooltip = $"{buttonName} - {newStatus}";

        HandVRTooltipController.ShowTooltip(controller, tooltip);
    }

    public void hideInteractionTooltip(OVRInput.Controller controller)
    {
        HandVRTooltipController.HideTooltip(controller);
    }
}

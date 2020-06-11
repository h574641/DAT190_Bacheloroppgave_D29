using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandVRTooltipController : MonoBehaviour
{
    public Text RightHandText;
    public Text LeftHandText;

    public GameObject RightHandTextPanel;
    public GameObject LeftHandTextPanel;

    [HideInInspector]
    public bool LeftHandTooltipShown = false;
    [HideInInspector]
    public bool RightHandTooltipShown = false;

    public static HandVRTooltipController Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        LeftHandTextPanel.SetActive(false);
        RightHandTextPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /* TODO: Improve tooltips?
        if (LeftHandTooltipShown || RightHandTooltipShown)
        {
            RightHandTextPanel.transform.rotation = Quaternion.Euler(0f, RightHandTextPanel.transform.eulerAngles.y, 0f);
            LeftHandTextPanel.transform.rotation = Quaternion.Euler(0f, LeftHandTextPanel.transform.eulerAngles.y, 0f);
            
            RightHandTextPanel.transform.rotation = Quaternion.Euler(RightHandTextPanel.transform.eulerAngles.x, RightHandTextPanel.transform.eulerAngles.y, 0f);
            LeftHandTextPanel.transform.rotation = Quaternion.Euler(LeftHandTextPanel.transform.eulerAngles.x, LeftHandTextPanel.transform.eulerAngles.y, 0f);
            
        }
    */
    }

    public static void ShowLeftHandTooltip(string tooltipText)
    {
        RectTransform rect = Instance.LeftHandTextPanel.GetComponent<RectTransform>();

        Instance.LeftHandTextPanel.SetActive(true);

        Instance.LeftHandText.text = tooltipText;
        Instance.LeftHandTooltipShown = true;

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Instance.LeftHandText.preferredWidth + 80);
    }

    public static void ShowRightHandTooltip(string tooltipText)
    {
        RectTransform rect = Instance.RightHandTextPanel.GetComponent<RectTransform>();

        Instance.RightHandTextPanel.SetActive(true);

        Instance.RightHandText.text = tooltipText;
        Instance.RightHandTooltipShown = true;

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Instance.RightHandText.preferredWidth + 80);
    }

    public static void ShowTooltip(OVRInput.Controller controller, string tooltipText)
    {
        switch (controller)
        {
            case OVRInput.Controller.LTouch:
                ShowLeftHandTooltip(tooltipText);
                break;

            case OVRInput.Controller.RTouch:
                ShowRightHandTooltip(tooltipText);
                break;
        }
    }

    public static void HideTooltip(OVRInput.Controller controller)
    {
        switch (controller)
        {
            case OVRInput.Controller.LTouch:
                HideLeftHandTooltip();
                break;

            case OVRInput.Controller.RTouch:
                HideRightHandTooltip();
                break;
        }
    }

    public static void HideLeftHandTooltip()
    {
        Instance.LeftHandText.text = "";
        Instance.LeftHandTextPanel.SetActive(false);

        Instance.LeftHandTooltipShown = false;
    }

    public static void HideRightHandTooltip()
    {
        Instance.RightHandText.text = "";
        Instance.RightHandTextPanel.SetActive(false);

        Instance.RightHandTooltipShown = false;
    }

    public static string GetTooltipText(OVRInput.Controller controller)
    {
        switch (controller)
        {
            case OVRInput.Controller.LTouch:
                return GetLeftHandTooltipText();

            case OVRInput.Controller.RTouch:
                return GetRightHandTooltipText();

            default:
                return null;
        }
    }

    public static string GetLeftHandTooltipText()
    {
        return Instance.LeftHandText.text;
    }

    public static string GetRightHandTooltipText()
    {
        return Instance.RightHandText.text;
    }

    public static bool HasTooltipText(OVRInput.Controller controller)
    {
        switch (controller)
        {
            case OVRInput.Controller.LTouch:
                return HasLeftHandTooltipText();

            case OVRInput.Controller.RTouch:
                return HasRightHandTooltipText();

            default:
                return false;
        }
    }

    public static bool HasLeftHandTooltipText()
    {
        return !string.IsNullOrEmpty(Instance.LeftHandText.text);
    }

    public static bool HasRightHandTooltipText()
    {
        return !string.IsNullOrEmpty(Instance.RightHandText.text);
    }
}

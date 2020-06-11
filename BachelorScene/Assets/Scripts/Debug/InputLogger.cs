using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputLogger : MonoBehaviour
{
    public Text TextField;
    public bool Enabled = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Enabled)
        {
            return;
        }

        List<string> status = new List<string>();
        var controllers = OVRInput.GetConnectedControllers();

        foreach (OVRInput.Button input in Enum.GetValues(typeof(OVRInput.Button)))
        {
            if (OVRInput.GetDown(input, controllers) || OVRInput.GetUp(input, controllers))
            {
                status.Add(input.ToString() + " (" + (OVRInput.GetDown(input, controllers) ? "Down" : "Up") + ")");
            }
        }

        foreach (OVRInput.RawButton input in Enum.GetValues(typeof(OVRInput.RawButton)))
        {
            if (OVRInput.GetDown(input, controllers) || OVRInput.GetUp(input, controllers))
            {
                status.Add(input.ToString() + " (" + (OVRInput.GetDown(input, controllers) ? "down" : "up") + ")");
            }
        }

        foreach (OVRInput.Button input in Enum.GetValues(typeof(OVRInput.Button)))
        {
            if (OVRInput.Get(input, controllers))
            {
                status.Add(input.ToString() + " (" + OVRInput.Get(input, controllers).ToString() + ")");
            }
        }

        foreach (OVRInput.RawButton input in Enum.GetValues(typeof(OVRInput.RawButton)))
        {
            if (OVRInput.Get(input, controllers))
            {
                status.Add(input.ToString() + " (" + OVRInput.Get(input, controllers).ToString() + ")");
            }
        }

        foreach (OVRInput.Axis1D input in Enum.GetValues(typeof(OVRInput.Axis1D)))
        {
            if (OVRInput.Get(input, controllers) > 0.2)
            {
                status.Add(input.ToString() + " (" + OVRInput.Get(input, controllers).ToString() + ")");
            }
        }

        if (status.Count > 0)
        {
            Debug.Log(string.Join(", ", status));
            
            if (TextField != null)
            {
                TextField.text = string.Join(", ", status);
            }
        }
    }
}

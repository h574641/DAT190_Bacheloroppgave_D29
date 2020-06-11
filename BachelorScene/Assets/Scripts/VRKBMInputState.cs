using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VRKBMInputState : MonoBehaviour
{
    public static bool State = true;

    public static bool UsingVR = true;
    public static bool UsingKBM = false;

    public static bool HMDPresent = false;

    // Check if user is pressing either of the top face buttons
    public static bool HasVRInput()
    {
        return OVRInput.Get(OVRInput.Button.Two) || OVRInput.Get(OVRInput.Button.Four);
    }

    // Check if user is clicking in the scene
    public static bool HasKBMInput()
    {
        return Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2);
    }

    void Start()
    {

    }


    // Update VR/KBM status depending on user input
    void CheckUserInput()
    {
        if (!UsingVR && HasVRInput())
        {
            UsingVR = true;
            UsingKBM = false;

            Debug.Log("Using VR");
        }

        if (!UsingKBM && HasKBMInput())
        {
            UsingVR = false;
            UsingKBM = true;

            Debug.Log("Using KBM");
        }
    }

    void Update()
    {
        // Don't do anything if this isn't running in editor
        if (!Application.isEditor)
        {
            return;
        }

        CheckUserInput();
    }
}

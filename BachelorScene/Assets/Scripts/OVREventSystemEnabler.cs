using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OVREventSystemEnabler : MonoBehaviour
{
    public List<GameObject> RequiresEventSystem;
    public GameObject EventSystem;

    private bool currentState;

    public bool EventSystemNeeded()
    {
        return RequiresEventSystem.Any(go => go.activeSelf);
    }

    void Start()
    {
        bool newState = EventSystemNeeded();

        EventSystem.SetActive(newState);
        currentState = newState;

    }

    void Update()
    {
        bool newState = EventSystemNeeded();

        if (newState != currentState)
        {
            EventSystem.SetActive(newState);
            currentState = newState;
        }
    }
}

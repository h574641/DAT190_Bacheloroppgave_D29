using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartProximityDetector : MonoBehaviour
{
    public HeartBeatVibrations HeartBeatVibrations;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        ButtonInteracter interacter =  otherCollider.GetComponentInParent<ButtonInteracter>();

        if (interacter != null)
        {
            HeartBeatVibrations.TargetControllers.Add(interacter.Controller);
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        ButtonInteracter interacter = otherCollider.GetComponentInParent<ButtonInteracter>();

        if (interacter != null)
        {
            HeartBeatVibrations.TargetControllers.Remove(interacter.Controller);
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyDepenetrationSetter : MonoBehaviour
{
    public float depenetrationValue;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Rigidbody rb in gameObject.GetComponentsInChildren<Rigidbody>())
        {
            rb.maxDepenetrationVelocity = depenetrationValue;
        }

        foreach (CharacterJoint cj in gameObject.GetComponentsInChildren<CharacterJoint>())
        {
            cj.enableProjection = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

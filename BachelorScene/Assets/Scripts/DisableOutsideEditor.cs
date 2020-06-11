using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOutsideEditor : MonoBehaviour
{
    void Start()
    {
        if (!Application.isEditor)
        {
            gameObject.SetActive(false);
        }
    }
}

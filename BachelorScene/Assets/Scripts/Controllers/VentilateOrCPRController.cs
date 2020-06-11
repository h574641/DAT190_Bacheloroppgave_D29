using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentilateOrCPRController : MonoBehaviour
{
    public GameObject Baby;

    public bool PerformingCPR = false;
    private CPRController cprController;
    private float cprSpeed = 1f;
    private bool startedCPR = false;
    private bool cprPause = false;

    public GameObject Ventilator;
    public bool IsVentilating = false;
    private VentilatorController ventialtorController;
    private float ventilatingSpeed = 1.1f;
    
    private  float fluxuationNumber = 1;

    // Start is called before the first frame update
    void Start()
    {
        ventialtorController = Ventilator.GetComponent<VentilatorController>();
        cprController = Baby.GetComponent<CPRController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsVentilating)
        {
            if ((int)fluxuationNumber % 2 == 0)
            {
                ventialtorController.SimulateKeyPress(true);
                fluxuationNumber += ventilatingSpeed * Time.deltaTime;
            }
            else
            {
                ventialtorController.SimulateKeyPress(false);
                fluxuationNumber += ventilatingSpeed * Time.deltaTime;
            }
        }

        if (PerformingCPR)
        {
            if (!startedCPR)
            {
                cprController.StartCompressions();
                startedCPR = true;
            }

            if ((int)fluxuationNumber % 2 == 0 && !cprPause)
            {
                cprController.SimulateCPR();
                cprPause = true;
            }
            else if ((int)fluxuationNumber % 2 != 0 && cprPause)
            {
                cprController.SimulateCPR();
                cprPause = false;
            }

            fluxuationNumber += cprSpeed * Time.deltaTime;
        }
    }
}

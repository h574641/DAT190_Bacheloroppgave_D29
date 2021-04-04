using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Behaviour

{

    public class LightSwitch : MonoBehaviour
    {
        public List<GameObject> LightList;
        public GameObject Switch;
        bool LightState = true;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ToggleLight();
                ToggleSwitchRotation();

                /*
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(ray, out hit, 100.0f))
                {
                    ToggleLight();
                    ToggleSwitchRotation();
                }
                */
            }
        }

        private void ToggleSwitchRotation()
        {
            float angle = LightState ? -10f : 10f;
            Switch.transform.Rotate(angle, 0f, 0f, Space.Self);
        }

        private void ToggleLight()
        {
            LightState = !LightState;
            foreach (GameObject gameObject in LightList)
            {
                gameObject.GetComponent<Light>().enabled = LightState;
            }

        }
    }
}

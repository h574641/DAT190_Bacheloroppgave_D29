using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Behaviour
{
    public class HeatLamp : MonoBehaviour
    {
        public GameObject HeatLampLight;
        public GameObject HeatLampMesh;
        public Material HeatLampOnMaterial;
        public Material HeatLampOffMaterial;
        public GameObject SwitchMesh;
        bool LightState = true;

        // Start is called before the first frame update
        void Start()
        {
            //EditorUtility.SetDirty(this);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                ToggleLight();
                ToggleSwitchRotation();
                ToggleHeatLampMaterial();
            }
        }

        private void ToggleHeatLampMaterial()
        {
            HeatLampMesh.GetComponent<Renderer>().material = LightState ? HeatLampOnMaterial : HeatLampOffMaterial;
        }
        private void ToggleSwitchRotation()
        {
            float angle = LightState ? -20f : 20f;
            SwitchMesh.transform.Rotate(angle, 0f, 0f, Space.Self);
        }

        private void ToggleLight()
        {
            LightState = !LightState;

            HeatLampLight.GetComponent<Light>().enabled = LightState;


        }
    }
}
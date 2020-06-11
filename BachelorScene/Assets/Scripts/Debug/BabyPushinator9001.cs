using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyPushinator9001 : MonoBehaviour
{
    public GameObject m_Baby;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("Yeeting baby");

            m_Baby.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(0f, 1f)) * 10000);
        }
    }
}

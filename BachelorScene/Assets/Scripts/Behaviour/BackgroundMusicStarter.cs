using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioManager>().Play("BackgroundAudio");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

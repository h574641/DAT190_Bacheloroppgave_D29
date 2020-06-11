using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compression
{
    private bool down = true;
    private float speed = 0.9f;
    private float breathboneLength = 1f; //length of breathbone
    private float topComp = 1f;
    private float bottomComp = 0.8905f;
    public bool SuccessfulCompression { get; set; } //boolean to check if a compression was successful

    public float Compress()
    {
        if (down)
        {
            SuccessfulCompression = false;
            float newLength = breathboneLength - speed * Time.deltaTime;

            if (newLength < bottomComp) //when breathbone is compressed enough, switch to increasing the length again
            {
                down = false;
            }

            breathboneLength = newLength;
        }
        else
        {
            breathboneLength += speed * Time.deltaTime;

            if (breathboneLength >= topComp) //when breathbone is back to full length, stop compressing and reset
            {
                down = true;
                SuccessfulCompression = true;
            }
        }

        return breathboneLength;
    }
}

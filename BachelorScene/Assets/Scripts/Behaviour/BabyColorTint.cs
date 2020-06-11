using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyColorTint : MonoBehaviour
{
    public Material babyMaterial;
    public Color tintColor;
    public GameObject babyMesh;

    [Range(0f,0.2f)]
    public float tintPercentage;

    private Renderer rend;

    void Start()
    {
        rend = babyMesh.GetComponent<Renderer>();
    }

    void Update()
    {
        Color finalTintColor = Color.Lerp(Color.white, tintColor, tintPercentage);
        rend.material.SetColor(Shader.PropertyToID("_Color"), finalTintColor);
    }
    public void setTintPercentage(float percent)
    {
        if(percent > 0.2)
        {
            this.tintPercentage = 0.2f;
        } else
        {
            this.tintPercentage = percent;
        }
    }
}

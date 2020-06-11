using UnityEngine;

public class OxygenDisplay : MonoBehaviour
{
    public OxygenTurnKnob OxygenKnob;
    public GameObject DisplayHand;

    [HideInInspector]
    public float OxygenPercentage;


    private float previousAngle;

    public void UpdateVisuals()
    {
        OxygenPercentage = OxygenKnob.Percent;

        var angle = Mathf.Lerp(-150f, 90f, OxygenPercentage);

        if (angle != previousAngle)
        {
            DisplayHand.transform.eulerAngles = new Vector3(0, 90, angle);
            previousAngle = angle;
        }
    }

    void Start()
    {
        UpdateVisuals();
    }

    void Update()
    {
        UpdateVisuals();
    }

}

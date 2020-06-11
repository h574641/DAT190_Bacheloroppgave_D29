using UnityEngine;
using UnityEngine.UI;

public class UIBpmMonitor : MonoBehaviour
{   
    public BabyController BabyController;
    public AttachPulseOxymeter attachPulseOxymeter;
    public Text DisplayText;

    void Update()
    {
        int bpm = attachPulseOxymeter.IsOximeterAttached ? (int)BabyController.BPMController.BPM : 0;
        UpdateSegmentDisplay(bpm);
    }

    private void UpdateSegmentDisplay(int number)
    {
        DisplayText.text = $"{number:D3}";
    }
}
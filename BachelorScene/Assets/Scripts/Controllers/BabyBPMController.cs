using System.Collections;
using UnityEngine;

public class BabyBPMController : MonoBehaviour
{
    [HideInInspector]
    public float BPM = -1f;

    private int targetBpm = 0;

    private int fluctuateMinimum = 0;
    private int fluctuateMaximum = 0;

    private bool startedFluctuateRoutine = false;

    public void SetBpmRange(int lower, int top)
    {
        if (BPM <= 0)
        {
            BPM = lower;
        }

        Debug.Log($"SetBPMRange {lower} {top}");

        targetBpm = top;

        fluctuateMinimum = lower;
        fluctuateMaximum = top;

        if (!startedFluctuateRoutine)
        {
            StartCoroutine(fluctuateBetween());
        }
    }

    //method trying to simulate pulse between two values
    private IEnumerator fluctuateBetween()
    {
        System.Random rnd = new System.Random();

        startedFluctuateRoutine = true;

        while (true)
        {
            // Randomly fluctuate if we are inside the range
            // Otherwise slowly make our way into the range
            if (fluctuateMinimum <= BPM && BPM <= fluctuateMaximum)
            {
                bool upOrDown = rnd.Next(0, 101) > 49;

                BPM = MathHelper.Approach(BPM, targetBpm, Time.deltaTime * rnd.Next(1, rnd.Next(2, 5)));
                BPM = Mathf.Clamp(BPM, fluctuateMinimum, fluctuateMaximum);

                // try to make target BPM trend towards being inside the range
                if (fluctuateMinimum <= targetBpm && targetBpm <= fluctuateMaximum)
                {
                    targetBpm += upOrDown ? 1 : -1;
                }
                else
                {
                    if (targetBpm < fluctuateMinimum)
                    {
                        targetBpm += rnd.Next(0, 3) > 0 ? 1 : -1;
                    }
                    else
                    {
                        targetBpm += rnd.Next(0, 3) > 0 ? -1 : 1;
                    }
                }
            }
            else
            {
                int maxStep = Mathf.Max(5 * Mathf.FloorToInt(Mathf.Log(BPM < fluctuateMinimum ? fluctuateMinimum - BPM : BPM - fluctuateMaximum)), 3);

                BPM = MathHelper.Approach(BPM, BPM <= fluctuateMinimum ? fluctuateMinimum : fluctuateMaximum, Time.deltaTime * rnd.Next(1, rnd.Next(2, maxStep * (BPM < fluctuateMinimum ? 1 : -1))));
            }

            yield return 0;
        }
    }

    void Start()
    {
        SetBpmRange(40, 60);
    }
}

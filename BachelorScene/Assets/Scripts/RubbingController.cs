using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbingController : MonoBehaviour
{
    public PeekAQueue<Rub> Rubs { get; protected set; }
    public bool SuccessfulRubbing = false;
    public int MaxTimeBetweenThreeRubs = 60;
    public int RubCount;

    void Start()
    {
        Rubs = new PeekAQueue<Rub>(32);
    }

    public void Update()
    {
        SuccessfulRubbing = approveRubbing();
        RubCount = Rubs.Count;
    }

    private bool approveRubbing()
    {
        for (int i = 0; i < Rubs.Count - 2; i++)
        {
            if (Rubs.Peek(i).OccuredAt - Rubs.Peek(i + 2).OccuredAt < MaxTimeBetweenThreeRubs)
            {
                return true;
            }
        }        

        return false;
    }
}

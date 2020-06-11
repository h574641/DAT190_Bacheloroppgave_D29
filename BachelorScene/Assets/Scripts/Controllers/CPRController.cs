using UnityEngine;
using UnityEngine.PlayerLoop;

public class CPRController : MonoBehaviour
{
    public int RequiredAmoutOfCompressions = 30;
    public int TooLongPause = 10;
    public int LowestCompressionRate = 50;
    public int HighestCompressionRate = 140;
    public int RequiredTimeToCompress = 30;

    public GameObject BreathBone;

    public bool FinishCompression = false; //let other parts of game know if player performed enough compression over RequiredTimeToCompress duration
    public bool StartedCPR = false;

    private bool isCounting = false;
    private bool isCompressing = false;

    [HideInInspector]
    public double CompressionRate;
    [HideInInspector]
    public double CompressionDuration;

    private Compression comp = new Compression();

    private PeekAQueue<float> compressionQueue;

    public CPRController()
    {
        compressionQueue = new PeekAQueue<float>(RequiredAmoutOfCompressions);
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCounting && checkCompressionsOverDuration()) 
        {
            FinishCompression = true;
        }

        if (VRKBMInputState.UsingKBM)
        {
            UpdateCompressionState(Input.GetKeyDown(KeyCode.X));

            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartCompressions();
            }
        }

        updateCompressionDuration();
    }

    //Method that checks que if if there is a longer pause than 5 sec between two compressions
    private bool checkForLongPauses()
    {
        int queCount = compressionQueue.Count;

        if (queCount > 3)
        {
            for (int i = 0; i < queCount - 2; i++)
            {
                if (compressionQueue.Peek(i) - compressionQueue.Peek(i + 1) > TooLongPause)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void calculateCompressionRate()
    {
        int queueCount = compressionQueue.Count;

        if (queueCount >= 3)
        {
            float sum = 0;

            for (int i = 0; i < queueCount - 1; i++)
            {
                sum += compressionQueue.Peek(i) - compressionQueue.Peek(i + 1);
            }

            CompressionRate = 60 / (sum / (queueCount - 1));
            Debug.Log("Comp rate: " + CompressionRate);
        }
    }

    private bool checkCompressionRate()
    {
        return LowestCompressionRate <= CompressionRate && CompressionRate <= HighestCompressionRate;
    }

    private void updateCompressionDuration()
    {
        if (compressionQueue.Count > 10)
        {
            float firstComp = compressionQueue.Peek(compressionQueue.Count -1);
            float lastComp = compressionQueue.Peek();
            CompressionDuration = lastComp - firstComp;
        }
    }
    private bool checkCompressionsOverDuration()
    {
        if (compressionQueue.Count == RequiredAmoutOfCompressions)
        {
            float firstComp = compressionQueue.Peek(RequiredAmoutOfCompressions - 1);
            float lastComp = compressionQueue.Peek();
            bool enoughCompressions = lastComp - firstComp < RequiredTimeToCompress; //check if there is enough compressions in the a given time

            return enoughCompressions && !checkForLongPauses() && checkCompressionRate();
        }

        return false;
    }

    //method handling what to do when player presses the "perform compression button"
    public void UpdateCompressionState(bool state)
    {
        if (isCompressing)
        {
            if (comp.SuccessfulCompression)
            {
                isCompressing = false;

                compressionQueue.Enqueue(Time.time); //add timestamp of end of compression to queue
                calculateCompressionRate();
            }

            BreathBone.transform.localScale = new Vector3(comp.Compress(), 1, 1); //update breathbone length with new value
        }

        if (!isCompressing && state && isCounting)
        {
            isCompressing = true; //start single compression
        }
    }

    //a method other parts of the system can use to start counting compressions
    public void StartCompressions()
    {
        StartedCPR = true;
        isCounting = true;
    }

    //a method other parts of the system can use to stop counting compressions
    public void StopCompressions()
    {
        isCounting = false;
        isCompressing = false;
    }

    public void SimulateCPR()
    {
        UpdateCompressionState(true);
    }
}


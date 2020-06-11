using UnityEngine;

public class BabyContraction : MonoBehaviour
{
    public GameObject BabyMesh, LeftLegBone, RightLegBone, RightElbowBone, LeftElbowBone, ChestBone, HeadBone, JawBone;
    public bool BabyHasTonus = false;
    public bool BabyIsCrying = false;

    private HingeJoint leftLegHinge, rightLegHinge, rightElbowHinge, leftElbowHinge, chestBoneHinge, headBoneHinge;
    private JointSpring leftLegBoneSpring, rightLegBoneSpring, rightElbowBoneSpring, leftElbowBoneSpring, chestBoneSpring, headBoneSpring;
    private float springTargetPosistionLerp;


    //Setup hinges and springs
    void Start()
    {
        leftLegHinge = LeftLegBone.GetComponent<HingeJoint>();
        leftLegBoneSpring = leftLegHinge.spring;

        rightLegHinge = RightLegBone.GetComponent<HingeJoint>();
        rightLegBoneSpring = rightLegHinge.spring;

        rightElbowHinge = RightElbowBone.GetComponent<HingeJoint>();
        rightElbowBoneSpring = rightElbowHinge.spring;

        leftElbowHinge = LeftElbowBone.GetComponent<HingeJoint>();
        leftElbowBoneSpring = leftElbowHinge.spring;

        chestBoneHinge = ChestBone.GetComponent<HingeJoint>();
        chestBoneSpring = chestBoneHinge.spring;

        headBoneHinge = HeadBone.GetComponent<HingeJoint>();
        headBoneSpring = headBoneHinge.spring;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            BabyHasTonus = true;
            BabyIsCrying = true;
            Debug.Log("Legs and arms contract");
        }

        if (BabyHasTonus)
        {
            updateSpringsTargetPositions();
        }
        
        if (BabyIsCrying)
        {
            updateJawPosition();
            playBabyCrySound();
        }
    }
    private void updateSpringsTargetPositions()
    {
        springTargetPosistionLerp = Mathf.PingPong(Time.time, 1);

        leftLegBoneSpring.targetPosition = Mathf.Lerp(0,100,springTargetPosistionLerp);
        leftLegHinge.spring = leftLegBoneSpring;

        rightLegBoneSpring.targetPosition = -Mathf.Lerp(0, 100, springTargetPosistionLerp);
        rightLegHinge.spring = rightLegBoneSpring;

        rightElbowBoneSpring.targetPosition = Mathf.Lerp(0, 100, springTargetPosistionLerp);
        rightElbowHinge.spring = rightElbowBoneSpring;

        leftElbowBoneSpring.targetPosition = Mathf.Lerp(0, 100, springTargetPosistionLerp);
        leftElbowHinge.spring = leftElbowBoneSpring;

        chestBoneSpring.targetPosition = -20;
        chestBoneHinge.spring = chestBoneSpring;

        headBoneSpring.targetPosition = -10;
        headBoneHinge.spring = headBoneSpring;
    }

    private void updateJawPosition()
    {
        JawBone.transform.localEulerAngles = new Vector3(0, 0, -120f);
    }

    private void playBabyCrySound()
    {
        BabyIsCrying = false;
        BabyMesh.GetComponent<AudioManager>().Play("CryingLoop");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    [SerializeField] GameObject indexSphere;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] OVRHand MYRightHand;
    [SerializeField] OVRSkeleton MYRightSkeleton;

    private bool isDataValid = true;
    private bool isDataHighConfidence = true;
    private float ThumbPinchStrength;

    private GameObject ball;
    private Rigidbody rb;
    private Vector3 ballScale;

    // Update is called once per frame
    void Update()
    {
        ThumbPinchStrength = MYRightHand.GetFingerPinchStrength(OVRHand.HandFinger.Thumb);
        Vector3 indexTipPos = MYRightSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position;
        Quaternion indexTipRotate = MYRightSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.rotation;

        indexSphere.transform.position = indexTipPos;
        indexSphere.transform.rotation = indexTipRotate;

        isDataValid = MYRightHand.IsDataValid;
        isDataHighConfidence = MYRightHand.IsDataHighConfidence;
     
        if (ThumbPinchStrength > 0.9)
        {
            if (ballPrefab == null) {
                return;
            }
            if (ball == null)
            {
                ball = Instantiate(ballPrefab);
                ball.transform.parent = indexSphere.transform;
                ball.transform.localPosition = Vector3.zero;
                rb = ball.GetComponent<Rigidbody>();
                rb.isKinematic = true;


                Invoke("ThrowBall", 2f);
            }
        }

        if (ball != null && ball.transform.parent != null)
        {
            ballScale = ball.transform.localScale;
            ballScale = ballScale * 1.01f;
            ball.transform.localScale = ballScale;
        }
    }

    void ThrowBall()
    {
        rb.isKinematic = false;
        ball.transform.parent = null;
        rb.AddForce(MYRightHand.PointerPose.forward * 10f, ForceMode.Impulse);
    }

}

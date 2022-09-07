using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grab : MonoBehaviour
{

    [SerializeField] OVRHand MYRightHand;
    [SerializeField] OVRSkeleton MYRightSkeleton;
    [SerializeField] GameObject IndexSphere;
    private bool isIndexPinching;
    private float ThumbPinchStrength;
    private bool isDataValid = true;
    private bool isDataHighConfidence = true;

    private string debugText;

    void Update()
    {
        isIndexPinching = MYRightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        ThumbPinchStrength = MYRightHand.GetFingerPinchStrength(OVRHand.HandFinger.Thumb);

        Vector3 indexTipPos = MYRightSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position;
        Quaternion indexTipRotate = MYRightSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.rotation;

        IndexSphere.transform.position = indexTipPos;
        IndexSphere.transform.rotation = indexTipRotate;

        isDataValid = MYRightHand.IsDataValid;
        isDataHighConfidence = MYRightHand.IsDataHighConfidence;

        debugText = "";
        debugText += $"isDataValid: {isDataValid}\n";
        debugText += $"isHandConfidence: {isDataHighConfidence}\n";
        DebugUIBuilder.instance.AddLabel(debugText, DebugUIBuilder.DEBUG_PANE_RIGHT);
        DebugUIBuilder.instance.Show();

    }
    void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay");
        if (ThumbPinchStrength > 0.9)
        {
            other.gameObject.transform.parent = IndexSphere.transform;
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.transform.localPosition = Vector3.zero;

            bool isLost = !(isDataValid && isDataHighConfidence);
            if(isLost){
                DebugUIBuilder.instance.AddLabel("It is lost!!!!", DebugUIBuilder.DEBUG_PANE_RIGHT);
                DebugUIBuilder.instance.Show();
                Destroy(other.gameObject);
            }
        }
        else
        {
            other.GetComponent<Rigidbody>().isKinematic = false;
            other.transform.parent = null;
        }
    }
}
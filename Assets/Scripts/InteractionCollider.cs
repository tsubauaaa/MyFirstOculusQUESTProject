using System;
using OculusSampleFramework;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InteractionCollider : MonoBehaviour
{
    [SerializeField] private TextMesh _debugText;
    [SerializeField] private ButtonController _resetButton;
    private Rigidbody  _rigidBody;
    private Vector3    _initPosition;
    private Quaternion _initRotation;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _initPosition = this.transform.position;
        _initRotation = this.transform.rotation;
        _rigidBody.maxAngularVelocity = 0.5f;
        _rigidBody.maxDepenetrationVelocity = 0.5f;
        resetVelocity();
        
        _resetButton.ActionZoneEvent += args =>
        {
            if (args.InteractionT == InteractionType.Enter)
            {
                //ボールを初期座標に戻す
                resetVelocity();
                _rigidBody.useGravity = true;
                _rigidBody.freezeRotation = false;
                this.transform.SetPositionAndRotation(_initPosition,_initRotation);
            }
        };
    }
    
    /// <summary>
    /// 加速度初期化
    /// </summary>
    private void resetVelocity()
    {
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
    }
    
    /// <summary>
    /// 掴もうとしているか？
    /// </summary>
    /// <returns></returns>
    private (bool isPinching,Vector3 position) isPinchingHand(OVRHand hand)
    {
        Vector3 position = Vector3.zero;
        bool isPinching = false;

        if (   hand.GetFingerIsPinching(OVRHand.HandFinger.Index)
            || hand.GetFingerIsPinching(OVRHand.HandFinger.Middle)
            || hand.GetFingerIsPinching(OVRHand.HandFinger.Ring))
        {
            position   = hand.PointerPose.position;
            isPinching = true;
        }
        
        return (isPinching,position);
    }
    
    /// <summary>
    /// 当たった方の手の取得。
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    private (OVRHand hand , string handName) getCollisionHand(Collision other)
    {
        try
        {
            //親子関係 OVRHandPrefab/Capsules/Hand_Index1_***
            GameObject targetObject = other.transform.parent.parent.gameObject;
            OVRHand rightHand = HandsManager.Instance.RightHand;
            OVRHand leftHand  = HandsManager.Instance.LeftHand;
            if(targetObject.Equals(leftHand.gameObject))  return (leftHand, "LeftHand");
            if(targetObject.Equals(rightHand.gameObject)) return (rightHand,"RightHand");
            return (null,"None");
        }
        catch(Exception e)
        {
            //parentが無かった時のエラーをキャッチ
            return (null, "None");
        }
    }
    
    /// <summary>
    /// 触れた時
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        var collisionHand = getCollisionHand(other);
        _debugText.text = $"OnCollisionEnter Name:{collisionHand.handName}";
        if (collisionHand.hand == null) return;
        
        var result = isPinchingHand(collisionHand.hand);
        if (!result.isPinching) return;
        _rigidBody.useGravity = false;
        _rigidBody.freezeRotation = true;
        this.transform.position = result.position;
    }
    
    /// <summary>
    /// 触れている間
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionStay(Collision other)
    {
        var collisionHand = getCollisionHand(other);
        _debugText.text = $"OnCollisionStay Name:{collisionHand.handName}";
        if (collisionHand.hand == null) return;
        
        var result = isPinchingHand(collisionHand.hand);
        if (result.isPinching)
        {
            resetVelocity();
            this.transform.position = result.position;    
        }
        else
        {
            _rigidBody.useGravity = true;
        }
    }
    
    /// <summary>
    /// 離れた時
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionExit(Collision other)
    {
        var collisionHand = getCollisionHand(other);
        _debugText.text = $"OnCollisionExit Name:{collisionHand.handName}";
        if (collisionHand.hand == null) return;
      
        _rigidBody.useGravity = true;
        _rigidBody.freezeRotation = false;
    }
}
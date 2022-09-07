using UnityEngine;
using Photon.Pun;

public class NetworkObjectsController : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject beer;
    [SerializeField] GameObject head;

    void Start()
    {
        Transform rightHandTransform = GameObject.Find("RightHandAnchor").transform;
        Transform cameraTransform = Camera.main.transform;

        if (photonView.IsMine)
        {
            beer.transform.parent = rightHandTransform;
            beer.transform.localPosition = Vector3.zero;
            beer.transform.localRotation = Quaternion.identity;

            head.transform.parent = cameraTransform;
            head.transform.localPosition = Vector3.zero;
            head.transform.localRotation = Quaternion.identity;
        }
    }
}

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonInitializer : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform playerController;
    private void Start() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom() {
        Vector3 position = new Vector3(Random.Range(-10f, 10f), 1.03f, Random.Range(-10f, 10f));
        PhotonNetwork.Instantiate("NetworkObjectsController", position, Quaternion.identity);
        playerController.position = position;
    }
}

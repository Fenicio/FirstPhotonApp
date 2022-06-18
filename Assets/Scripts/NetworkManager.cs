using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnCreatedRoom() {
        Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
    }

    public void CreateRoom(string roomName) {
        PhotonNetwork.CreateRoom(roomName);
    }

    public void JoinRoom(string roomName) {
        PhotonNetwork.JoinRoom(roomName);
    }

    [PunRPC]
    public void ChangeScene(string sceneName) {
        PhotonNetwork.LoadLevel(sceneName);
    }
}

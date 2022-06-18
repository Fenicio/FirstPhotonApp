using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Menu : MonoBehaviourPunCallbacks
{
    [Header("Screens")]
    [SerializeField]
    private GameObject mainScreen;
    [SerializeField]
    private GameObject lobbyScreen;

    [Header("Main Screen")]
    [SerializeField]
    private Button createRoomButton;
    [SerializeField]
    private Button joinRoomButton;

    [Header("Lobby Screen")]
    [SerializeField]
    private TextMeshProUGUI playerListText;
    [SerializeField]
    private Button startGameButton;

    private void Start() {
        createRoomButton.interactable = false;
        joinRoomButton.interactable = false;
    }

    public override void OnConnectedToMaster() {
        createRoomButton.interactable = true;
        joinRoomButton.interactable = true;
    }

    void SetScreen(GameObject screen) {
        mainScreen.SetActive(false);
        lobbyScreen.SetActive(false);
        screen.SetActive(true);
    }

    public void onCreateRoomButton(TMP_InputField roomNameInput) {
        NetworkManager.instance.CreateRoom(roomNameInput.text);
    }

    public void OnJoinRoomButton(TMP_InputField roomNameInput) {
        NetworkManager.instance.JoinRoom(roomNameInput.text);
    }

    public void OnPlayerNameUpdated(TMP_InputField playerNameInput) {
        PhotonNetwork.NickName = playerNameInput.text;
    }

    public override void OnJoinedRoom() {
        SetScreen(lobbyScreen);
        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        UpdateLobbyUI();
    }

    [PunRPC]
    public void UpdateLobbyUI() {
        playerListText.text = "";
        // display player names
        foreach(Player p in PhotonNetwork.PlayerList) {
            playerListText.text = playerListText.text + p.NickName  + "\n";
        }

        // toggle start button
        startGameButton.interactable = PhotonNetwork.IsMasterClient;
    }

    public void OnLeaveLobbyButton() {
        PhotonNetwork.LeaveRoom();
        SetScreen(mainScreen);
    }

    [PunRPC]
    public void OnStartGameButton() {
        if (!NetworkManager.instance) { 
            Debug.Log("NetworkManager.instance is missing!");
        }
        if (!NetworkManager.instance.photonView) {
            Debug.Log("NetworkManager.instance.photonView is missing!");
        }
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Game");
    }
}

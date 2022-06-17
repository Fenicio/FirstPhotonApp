using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("Stats")]
    public bool gameEnded = false;
    public float timeToWin;
    public float invincibleDuration;

    private float hatPickupTime;

    [Header("Players")]
    public string playerPrefabLocation;
    public Vector3[] spawnPoints;
    public PlayerController[] players;
    public int playerWithHat;

    private int playersInGame;

    public static GameManager instance;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        players = new PlayerController[PhotonNetwork.PlayerList.Length];
        photonView.RPC("ImInGame", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void ImInGame() {
        playersInGame++;
        if (playersInGame == PhotonNetwork.PlayerList.Length) {
            SpawnPlayer();
        }
    }

    void SpawnPlayer() {
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation, spawnPoints[Random.Range(0, spawnPoints.Length)], Quaternion.identity);
        PlayerController playerScript = playerObj.GetComponent<PlayerController>();
        playerScript.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }

    public PlayerController GetPlayer(int playerId) {
        return players.First(x => x.id == playerId);
    }

    public PlayerController GetPlayer(GameObject playerObj) {
        return players.First(x => x.gameObject == playerObj);
    }

    public void GiveHat(int playerId, bool initialGive) {
        if (!initialGive) {
            GetPlayer(playerWithHat).setHat(false);
        }
        playerWithHat = playerId;
        GetPlayer(playerId);
        hatPickupTime = Time.time;
    }

    public bool CanGetHat() {
        return Time.time > hatPickupTime + invincibleDuration;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class GameUI : MonoBehaviour
{
    public PlayerUIContainer[] playerContainers;
    public TextMeshProUGUI winText;

    public static GameUI instance;

    private void Awake() {
        instance = this;
    }
    private void Start() {
        InitializePlayerUI();
    }

    private void Update() {
        UpdatePlayerUI();
    }

    private void UpdatePlayerUI() {
        for(int x = 0; x < GameManager.instance.players.Length; x++) {
            if (GameManager.instance.players[x] != null) {
                playerContainers[x].hatTimeSlider.value = GameManager.instance.players[x].curHatTime;
            }
        }
    }

    void InitializePlayerUI() {
        for(int x = 0; x < playerContainers.Length; x++) {
            PlayerUIContainer container = playerContainers[x];

            if (x < PhotonNetwork.PlayerList.Length) {
                container.obj.SetActive(true);
                container.nameText.text = PhotonNetwork.PlayerList[x].NickName;
                container.hatTimeSlider.maxValue = GameManager.instance.timeToWin;
            } else {
                container.obj.SetActive(false);
            }
        }
    }

    public void SetWinText(string input) {
        winText.gameObject.SetActive(true);
        winText.text = input + " wins!";
    }
}

[System.Serializable]
public class PlayerUIContainer {
    public GameObject obj;
    public TextMeshProUGUI nameText;
    public Slider hatTimeSlider;
}
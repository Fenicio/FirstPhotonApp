using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public int id;

    [Header("Info")]
    public float moveSpeed;
    public float jumpForce;
    public GameObject hatObject;

    [HideInInspector]
    public float curHatTime;

    [Header("Components")]
    public Rigidbody rig;
    public Player photonPlayer;

    private void Update() {
        Move();

        if (Input.GetKeyDown(KeyCode.Space)) {
            TryJump();
        }
    }

    [PunRPC]
    public void Initialize(Player player) {
        photonPlayer = player;
        id = player.ActorNumber;
        GameManager.instance.players[id - 1] = this;
        
        
        if (photonView.IsMine && id == 1) {
            GameManager.instance.GiveHat(id, true);
            // for now give the hat to 1st
        } else {
            rig.isKinematic = true;
        }
    }

    void Move() {
        float x = Input.GetAxis("Horizontal") * moveSpeed;
        float z = Input.GetAxis("Vertical") * moveSpeed;

        rig.velocity = new Vector3(x, rig.velocity.y, z);
    }

    void TryJump() {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, 0.7f)) {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void setHat(bool toggle) {
        hatObject.SetActive(toggle);
    }

    void OnCollisionEnter(Collision collision) {
        if (!photonView.IsMine) return;

        if (collision.gameObject.CompareTag("Player")) {
            if (GameManager.instance.GetPlayer(collision.gameObject).id == GameManager.instance.playerWithHat) {
                if (GameManager.instance.CanGetHat()) {
                    GameManager.instance.photonView.RPC("GiveHat", RpcTarget.All, id, false);
                }
            }
        }
    }
}

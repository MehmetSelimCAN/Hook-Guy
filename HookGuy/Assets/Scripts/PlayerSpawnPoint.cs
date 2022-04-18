using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnPoint : MonoBehaviour {

    private Transform finishDoor;
    private Transform startDoor;
    public static bool nextLevel = true;

    private void Awake() {
        startDoor = GameObject.Find("pfDoorStart").GetComponent<Transform>();
        if (SceneManager.GetActiveScene().name != "Final") {
            finishDoor = GameObject.Find("pfDoorFinish").GetComponent<Transform>();
        }
    }

    private void Start() {
        if (GameManager.died) {
            StartCoroutine(RespawnAnimation());
        }
        else if (GameManager.restarted) {
            transform.position = startDoor.position;
            GameManager.restarted = false;
        }
        else {
            if (nextLevel) {
                transform.position = startDoor.position;
            }
            else {
                transform.position = finishDoor.position;
            }
        }
    }

    private IEnumerator RespawnAnimation() {
        transform.position = startDoor.position;
        CharacterController2D.ChangeAnimationState("PlayerRespawn");
        yield return new WaitForSeconds(0.66f);
        GameManager.died = false;
    }
}

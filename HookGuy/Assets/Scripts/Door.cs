using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {

    private bool playerStandingOnDoor;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Z) && playerStandingOnDoor) {
            if (String.Equals(name, "pfDoorFinish") && GameManager.Instance.CanPassNextLevel()) {
                if (SceneManager.GetActiveScene().buildIndex != 15) {
                    PlayerPrefs.SetInt("LastLevel", SceneManager.GetActiveScene().buildIndex + 1);
                    PlayerSpawnPoint.nextLevel = true;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                else {
                    if (PlayerPrefs.GetInt("PointNumber") == 3) {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    }
                }
            }
            else if (String.Equals(name, "pfDoorStart") && SceneManager.GetActiveScene().buildIndex != 0/*Level 1*/) {
                PlayerSpawnPoint.nextLevel = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            playerStandingOnDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            playerStandingOnDoor = false;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDoor : MonoBehaviour {
    private bool playerStandingOnDoor;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Z) && playerStandingOnDoor) {
            if (String.Equals(name, "doorRestart")) {
                PlayerPrefs.SetInt("Star1", 0);
                PlayerPrefs.SetInt("Star2", 0);
                PlayerPrefs.SetInt("Star3", 0);
                PlayerPrefs.SetInt("PointNumber", 0);
                PlayerPrefs.SetInt("LastLevel", 1);
                SceneManager.LoadScene("MainMenu");
            }
            else if (String.Equals(name, "doorQuit")) {
                Application.Quit();
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

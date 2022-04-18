using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Experimental.Rendering.Universal;

public class LightBulb : MonoBehaviour {

    [HideInInspector] public bool isTurnOn;

    private Sprite doorOpened;
    private Sprite doorClosed;
    private Sprite lightBulbOff;
    private Sprite lightBulbOn;

    private GameObject finishDoor;

    private void Awake() {
        doorOpened = Resources.Load<Sprite>("Sprites/doorOpened");
        doorClosed = Resources.Load<Sprite>("Sprites/doorClosed");
        lightBulbOff = Resources.Load<Sprite>("Sprites/lightBulbOff");
        lightBulbOn = Resources.Load<Sprite>("Sprites/lightBulbOn");
    }

    private void Start() {
        finishDoor = GameObject.Find("pfDoorFinish");
        GetComponent<Transform>().Find("light").gameObject.SetActive(false);
    }


    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Power")) {
            isTurnOn = true;
            transform.GetComponent<SpriteRenderer>().sprite = lightBulbOn;

            if (GameManager.Instance.CanPassNextLevel()) {
                finishDoor.GetComponent<Transform>().Find("lightBulb").GetComponent<SpriteRenderer>().sprite = lightBulbOn;
                GetComponent<Transform>().Find("light").gameObject.SetActive(true);
                finishDoor.GetComponent<SpriteRenderer>().sprite = doorOpened;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Power")) {
            isTurnOn = false;
            transform.GetComponent<SpriteRenderer>().sprite = lightBulbOff;
            finishDoor.GetComponent<Transform>().Find("lightBulb").GetComponent<SpriteRenderer>().sprite = lightBulbOff;
            GetComponent<Transform>().Find("light").gameObject.SetActive(false);
            finishDoor.GetComponent<SpriteRenderer>().sprite = doorClosed;
        }
    }
}

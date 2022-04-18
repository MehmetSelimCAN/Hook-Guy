using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private GameObject[] energyBlocks;

    public static GameManager Instance;
    private SpriteRenderer finishDoorSprite;
    private SpriteRenderer finishDoorLightSprite;
    private Transform points;

    public static bool died;
    public static bool restarted;

    private void Awake() {
        Instance = this;

        energyBlocks = GameObject.FindGameObjectsWithTag("LightBulb");

        if (SceneManager.GetActiveScene().name != "Final") {
            finishDoorSprite = GameObject.Find("pfDoorFinish").transform.GetComponent<SpriteRenderer>();
            finishDoorLightSprite = GameObject.Find("pfDoorFinish").transform.Find("lightBulb").GetComponent<SpriteRenderer>();
            points = GameObject.Find("Points").transform;
            points.GetComponentInChildren<Text>().text = PlayerPrefs.GetInt("PointNumber") + " / 3";
        }
    }

    private void Start() {
        if (!(SceneManager.GetActiveScene().buildIndex == 15 && PlayerPrefs.GetInt("PointNumber") != 3)) {
            if (energyBlocks.Length == 0 && SceneManager.GetActiveScene().name != "Final") {
                finishDoorSprite.sprite = Resources.Load<Sprite>("Sprites/doorOpened");
                finishDoorLightSprite.sprite = Resources.Load<Sprite>("Sprites/lightBulbOn");
            }
        }

    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            Restart();
        }

        if (Input.GetKeyDown(KeyCode.M)) {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void CheckLastLevelDoor() {
        if (SceneManager.GetActiveScene().buildIndex == 15 && PlayerPrefs.GetInt("PointNumber") == 3) {
            finishDoorSprite.sprite = Resources.Load<Sprite>("Sprites/doorOpened");
            finishDoorLightSprite.sprite = Resources.Load<Sprite>("Sprites/lightBulbOn");
        }
    }

    public bool CanPassNextLevel() {
        if (energyBlocks.Length > 0) {
            foreach (GameObject energyBlock in energyBlocks) {
                if (!energyBlock.GetComponent<LightBulb>().isTurnOn) {
                    finishDoorSprite.sprite = Resources.Load<Sprite>("Sprites/doorClosed");
                    finishDoorLightSprite.sprite = Resources.Load<Sprite>("Sprites/lightBulbOff");
                    return false;
                }
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 15 && PlayerPrefs.GetInt("PointNumber") != 3) {
            return false;
        }
            finishDoorSprite.sprite = Resources.Load<Sprite>("Sprites/doorOpened");
            finishDoorLightSprite.sprite = Resources.Load<Sprite>("Sprites/lightBulbOn");
            return true;
    }

    public void GetPoint() {
        PlayerPrefs.SetInt("PointNumber", PlayerPrefs.GetInt("PointNumber") + 1);
        points.GetComponentInChildren<Text>().text = PlayerPrefs.GetInt("PointNumber") + " / 3";
        if (PlayerPrefs.GetInt("PointNumber") == 3) {
            CheckLastLevelDoor();
        }
    }

    private void Restart() {
        restarted = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Die() {
        died = true;
        StartCoroutine(DieAnimation());
    }

    private IEnumerator DieAnimation() {
        CharacterController2D.ChangeAnimationState("PlayerDeath");
        yield return new WaitForSeconds(0.66f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

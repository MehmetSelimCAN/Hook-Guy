using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    private Button playButton;
    private Button levelButton;
    private Button howToPlayButton;
    private Button howToPlayPage1Button;
    private Button howToPlayPage2Button;
    private Button backToMenuButton;

    private Transform mainMenu;
    private Transform levelMenu;
    private Transform howToPlayMenu;
    private Transform howToPlayPage1;
    private Transform howToPlayPage2;

    private void Awake() {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        howToPlayButton = GameObject.Find("HowToPlayButton").GetComponent<Button>();
        howToPlayPage1Button = GameObject.Find("HowToPlayPage1Button").GetComponent<Button>();
        howToPlayPage2Button = GameObject.Find("HowToPlayPage2Button").GetComponent<Button>();
        levelButton = GameObject.Find("LevelSelectButton").GetComponent<Button>();
        backToMenuButton = GameObject.Find("BackToMenuButton").GetComponent<Button>();

        mainMenu = GameObject.Find("MainMenu").transform;
        levelMenu = GameObject.Find("LevelSelectMenu").transform;
        howToPlayMenu = GameObject.Find("HowToPlayMenu").transform;
        howToPlayPage1 = GameObject.Find("HowToPlayPage1").transform;
        howToPlayPage2 = GameObject.Find("HowToPlayPage2").transform;

        levelMenu.gameObject.SetActive(false);
        howToPlayMenu.gameObject.SetActive(false);
        howToPlayPage2.gameObject.SetActive(false);
        backToMenuButton.gameObject.SetActive(false);


        playButton.onClick.AddListener(() => {
            Play();
        });

        howToPlayButton.onClick.AddListener(() => {
            HowToPlayMenu();
        });

        howToPlayPage1Button.onClick.AddListener(() => {
            HowToPlayPage1();
        });

        howToPlayPage2Button.onClick.AddListener(() => {
            HowToPlayPage2();
        });

        backToMenuButton.onClick.AddListener(() => {
            BackToMainMenu();
        });

        levelButton.onClick.AddListener(() => {
            LevelSelectMenu();
        });
    }

    private void Start() {
        if (PlayerPrefs.GetInt("LastLevel") == 0) {
            PlayerPrefs.SetInt("LastLevel", 1);
        }
    }

    private void Play() {
        SceneManager.LoadScene(PlayerPrefs.GetInt("LastLevel"));
    }

    private void LevelSelectMenu() {
        levelMenu.gameObject.SetActive(true);
        backToMenuButton.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
    }

    private void HowToPlayMenu() {
        howToPlayMenu.gameObject.SetActive(true);
        backToMenuButton.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
        howToPlayPage1.gameObject.SetActive(true);
        howToPlayPage2.gameObject.SetActive(false);
    }

    private void HowToPlayPage1() {
        howToPlayPage1.gameObject.SetActive(true);
        howToPlayPage2.gameObject.SetActive(false);
    }

    private void HowToPlayPage2() {
        howToPlayPage2.gameObject.SetActive(true);
        howToPlayPage1.gameObject.SetActive(false);
    }

    private void BackToMainMenu() {
        mainMenu.gameObject.SetActive(true);
        levelMenu.gameObject.SetActive(false);
        backToMenuButton.gameObject.SetActive(false);
        howToPlayMenu.gameObject.SetActive(false);
    }
}

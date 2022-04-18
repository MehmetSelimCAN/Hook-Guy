using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour {

    private void Awake() {
        if (PlayerPrefs.GetInt("LastLevel") == 0) {
            PlayerPrefs.SetInt("LastLevel", 1);
        }

        Transform buttonTemplate = transform.Find("levelButtonTemplate");
        buttonTemplate.gameObject.SetActive(false);

        int xIndex = 0;
        int yIndex = 0;

        for (int levelNumber = 1; levelNumber < 16; levelNumber++) {
            Transform buttonTransform = Instantiate(buttonTemplate, transform);
            buttonTransform.gameObject.SetActive(true);

            if(xIndex % 5 == 0 && xIndex != 0) {
                xIndex = 0;
                yIndex++;
            }
            xIndex++;

            float xOffsetAmount = 50f;
            float yOffsetAmount = -75f;
            buttonTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-150f + xOffsetAmount * xIndex, 100f + yOffsetAmount * yIndex);

            buttonTransform.Find("levelNumber").GetComponent<TextMeshProUGUI>().SetText(levelNumber.ToString());
            buttonTransform.name = "Level" + levelNumber.ToString();

            if (levelNumber <= PlayerPrefs.GetInt("LastLevel")) {
                buttonTransform.GetComponent<Image>().color = Color.green;
            }
            else {
                buttonTransform.GetComponent<Image>().color = Color.red;
            }

            buttonTransform.GetComponent<Button>().onClick.AddListener(() => {
                if (buttonTransform.GetComponent<Image>().color == Color.green) {
                    SceneManager.LoadScene(buttonTransform.name);
                }
            });
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {

    private void Start() {
        if (PlayerPrefs.GetInt("Star1") == 1 && name == "Star1")
            Destroy(gameObject);
        if (PlayerPrefs.GetInt("Star2") == 1 && name == "Star2")
            Destroy(gameObject);
        if (PlayerPrefs.GetInt("Star3") == 1 && name == "Star3")
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            GameManager.Instance.GetPoint();
            if (name == "Star1")
                PlayerPrefs.SetInt("Star1", 1);
            if (name == "Star2")
                PlayerPrefs.SetInt("Star2", 1);
            if (name == "Star3")
                PlayerPrefs.SetInt("Star3", 1);

            Destroy(gameObject);
        }
    }

}

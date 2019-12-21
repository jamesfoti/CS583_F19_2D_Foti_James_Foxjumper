using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour {

    public Button[] levelButtons;

    //Delete prefabs and start over!
    private void Update() {
        if (Input.GetKey("q")) {
            DeletePlayerPrefs();
        }
    }

    private void Start() {
        int levelAt = PlayerPrefs.GetInt("levelAt", 2);
        for (int i = 0; i < levelButtons.Length; i++) {
            if (i + 2 > levelAt) {
                levelButtons[i].interactable = false;
            }
        }
    }

    public void DeletePlayerPrefs() {
        PlayerPrefs.DeleteAll();
    }

    public void LoadLevelOne() {
        SceneManager.LoadScene(2);
    }
}

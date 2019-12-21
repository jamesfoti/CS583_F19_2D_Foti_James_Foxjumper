using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public PlayerController controller;
    public Animator animator;
    public static bool GameIsPaused = false;
    public static bool GameLoadedToMenu = false;
    public GameObject pauseMenuUI;
    public AudioSource gameBackgroundMuisc;


    private void Start() {
        //GameLoadedToMenu = false;
    }
    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("p")) {
            if (GameIsPaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void Resume() {
        gameBackgroundMuisc.enabled = true;
        controller.enabled = true;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause() {
        gameBackgroundMuisc.enabled = false;
        controller.enabled = false;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMainMenu() {
        GameLoadedToMenu = true;
        Time.timeScale = 1f;
        GameIsPaused = false;
        controller.enabled = true;
        SceneManager.LoadScene(0);
    }

    public void FadeToMenu() {
        animator.Play("Fade_out_quick");
        Invoke("LoadMenu", .9f);
    }

    public void QuitGame() {
        Debug.Log("QUIT");
        Application.Quit(); // the game does not actually quit inside the editor!
    }
}

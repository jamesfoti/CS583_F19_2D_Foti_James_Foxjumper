using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager gameManager;
    public Animator animator;
    public PauseMenu pauseMenu;

    public void Awake() {
        //MakeSingleton();   
    }

    public void MakeSingleton() {
        if (gameManager != null) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadGameScene() {
        SceneManager.LoadScene(1);
    }

    public void FadeOutToGameScene() {
        animator.Play("Fade_out_quick");
        Invoke("LoadGameScene", .9f);
    }

    public void RestartGame() {
        SceneManager.LoadScene(1);
    }

    public void QuitGame() {
        Debug.Log("QUIT");
        Application.Quit(); // the game does not actually quit inside the editor!
    }

}

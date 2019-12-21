using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveOnToNextLevel : MonoBehaviour {

    public PlayerUI playerUI;
    public Animator animator;
    public Animator starAnimator;
    public float delaySceneTime = .5f;
    public int nextSceneLoad;
    public GameObject playerHasWonLevelUI;

    private void Start() { 
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    public void OnTriggerEnter2D(Collider2D other) {

        // LEVEL WON!
        if (other.gameObject.tag == "Star") {
            // Move to next scene
            //SceneManager.LoadScene(nextSceneLoad);

            // Frezing player UI stats and timer
            playerUI.enabled = false;

            // Picking up and destroying gold star
            starAnimator.Play("gold_star_pickup");
            Destroy(other.gameObject, .4f);

            // Fading into level menu
            playerHasWonLevelUI.SetActive(true);
            animator.Play("Fade_out");
            Invoke("LoadLevelMenu", 6f);
           
            // Setting int for index
            if (nextSceneLoad > PlayerPrefs.GetInt("levelAt")) {
                PlayerPrefs.SetInt("levelAt", nextSceneLoad);
            }

        }
    }

    public void LoadLevelMenu() {
        //Move back to level menu
        SceneManager.LoadScene(1);
    }

    public void DestroyGameObject() {
        Destroy(gameObject);

    }
}
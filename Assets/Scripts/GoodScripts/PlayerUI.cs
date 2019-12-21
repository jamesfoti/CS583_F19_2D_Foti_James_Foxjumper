using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour {

    public static PlayerUI playerUI;

    public int gemCount = 0;
    public int cherryCount = 0;
    public int starCount = 0;
    public float currentTime = 0;

    public TextMeshProUGUI displayCherryCount;
    public TextMeshProUGUI displayGemCount;
    public TextMeshProUGUI displayStarCount;
    public TextMeshProUGUI displayTime;

    // Update is called once per frame
    void Update() {
        DisplayTime();
        displayGemCount.text = gemCount.ToString("00"); //Display gem count
        displayCherryCount.text = cherryCount.ToString("00"); //Display cherry count
        displayStarCount.text = starCount.ToString("00"); //Display star count
}

    public void DisplayTime() {
        if (PauseMenu.GameIsPaused == false) {
            currentTime += Time.deltaTime;
        }
        displayTime.text = currentTime.ToString("0000");
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Gem")) {
            Destroy(other.gameObject);
            FindObjectOfType<AudioManager>().Play("GemPickUp");
            gemCount = gemCount + 1;
        }
        if (other.gameObject.CompareTag("Cherry")) {
            Destroy(other.gameObject);
            FindObjectOfType<AudioManager>().Play("CherryPickUp");
            cherryCount = cherryCount + 1;
        }
        if (other.gameObject.CompareTag("Star")) {
            Destroy(other.gameObject);
            starCount = starCount + 1;
            FindObjectOfType<AudioManager>().Play("StarPickUp");
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour {

    private PlayerController thePlayer;

    private void Start() {
        thePlayer = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Player") {
            thePlayer.onLadder = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.name == "Player") {
            thePlayer.onLadder = false;
        }
    }

}

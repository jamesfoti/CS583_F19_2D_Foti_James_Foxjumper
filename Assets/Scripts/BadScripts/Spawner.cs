using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject obstacle;

    public float timeBtwSpawn;
    public float startTimeBtwSpawn;

    private void Update() {
        if (timeBtwSpawn <= 0) {
            Instantiate(obstacle, transform.position, Quaternion.identity);
            timeBtwSpawn = startTimeBtwSpawn;
        }
        else {
            timeBtwSpawn -= Time.deltaTime;
        }
    }

}
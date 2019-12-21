using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    private const float PLAYER_DISTANCE_SPAWN_LEVEL_PART = 200f;

    [SerializeField] private Transform levelPartStart;
    [SerializeField] private List<Transform> levelPartList;
    [SerializeField] private PlayerController player;

    private static List<Transform> listOfRandGeneratedParts;
    private Vector3 lastEndPosition;
    int count = 0;
    private float timeToDestroyPlatform = 2.5f;

    public float height;
    public float horizontalDistance;

    public GameObject killZone;

    public bool playerCrossedStartLine = false;

    private void Awake() {
        lastEndPosition = levelPartStart.Find("EndPosition").position;
        listOfRandGeneratedParts = new List<Transform>();
    }

    private void Update() {
        if (Vector3.Distance(player.GetPosition(), lastEndPosition) < PLAYER_DISTANCE_SPAWN_LEVEL_PART) {
            // Spawn another level part
            SpawnLevelPart();
        }
        DestroyPreviousLevel();

        killZone.transform.position = new Vector2(player.GetPosition().x, killZone.transform.position.y);

        if (player.GetPosition().x > levelPartStart.position.x) {
            playerCrossedStartLine = true;
        }
    }

    private void SpawnLevelPart() {
        Transform chosenLevelPart = levelPartList[Random.Range(0, levelPartList.Count)];

        Transform lastLevelPartTransform = SpawnLevelPart(chosenLevelPart, lastEndPosition).GetComponent<Transform>();
        
        lastEndPosition = lastLevelPartTransform.Find("EndPosition").position;
        lastEndPosition.y = Random.Range(0, 1);
        
        
    }

    private Transform SpawnLevelPart(Transform levelPart, Vector3 spawnPosition) {
        Transform levelPartTransform = Instantiate(levelPart, spawnPosition, Quaternion.identity);
        listOfRandGeneratedParts.Add(levelPartTransform);
        return levelPartTransform;
    }

    private void DestroyPreviousLevel() {
        if (listOfRandGeneratedParts[count] == null) {
            count++;
        }
        else if (playerCrossedStartLine) {
            Destroy(listOfRandGeneratedParts[count].gameObject, timeToDestroyPlatform);
        }
    }

}

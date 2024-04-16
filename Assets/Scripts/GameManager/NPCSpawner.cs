using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab; // Prefab of the NPC to spawn
    public int quantity = 10; // Number of NPCs to spawn
    public float spawnRadius = 5f; // Radius of the circle where NPCs will be spawned
    public float offsetRange = 1f; // Maximum offset range for randomizing spawn positions
    public GameObject floor; // Reference to the Floor GameObject
    public DifficultySO difficultySO;

    void Start()
    {
        quantity = difficultySO.Difficulty;

        // Spawn NPCs
        SpawnNPCs();
    }

    void SpawnNPCs()
    {
        // Get the center position of the floor GameObject
        Vector3 centerPosition = floor.transform.position;

        // Calculate the angle step between each NPC around the circle
        float angleStep = 360f / quantity;

        // Loop to spawn the specified quantity of NPCs
        for (int i = 0; i < quantity; i++)
        {
            // Calculate the angle for the current NPC
            float angle = i * angleStep;

            // Add a random offset to the spawn position within the offset range
            float xOffset = Random.Range(-offsetRange, offsetRange);
            float zOffset = Random.Range(-offsetRange, offsetRange);

            // Calculate the position around the circle with the offset applied
            float x = centerPosition.x + (spawnRadius + xOffset) * Mathf.Cos(angle * Mathf.Deg2Rad);
            float z = centerPosition.z + (spawnRadius + zOffset) * Mathf.Sin(angle * Mathf.Deg2Rad);

            // Spawn NPC at the calculated position
            Vector3 spawnPosition = new Vector3(x, 1, z);
            GameObject newNPC = Instantiate(npcPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
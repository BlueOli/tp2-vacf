using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public float moveSpeed = 3f;  // Movement speed of the NPC
    public float rotationSpeed = 3f;  // Rotation speed of the NPC
    public float pushForce = 10f;
    public float minDistanceToTarget = 1f;  // Minimum distance to target before stopping

    private Transform target;  // Current target of the NPC

    void Start()
    {
        // Start by randomly choosing a target (player or another NPC)
        ChooseRandomTarget();
    }

    void Update()
    {
        // If the target is null, choose a new random target
        if (target == null)
        {
            ChooseRandomTarget();
            return;
        }

        // Move towards the target
        MoveTowardsTarget();
    }

    void ChooseRandomTarget()
    {
        // Randomly choose between the player and other NPCs in the scene
        GameObject[] playerAndNPCs = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] NPCs = GameObject.FindGameObjectsWithTag("NPC");

        GameObject[] allTargets = new GameObject[playerAndNPCs.Length + NPCs.Length];
        playerAndNPCs.CopyTo(allTargets, 0);
        NPCs.CopyTo(allTargets, playerAndNPCs.Length);

        if (allTargets.Length > 0)
        {
            int randomIndex = Random.Range(0, allTargets.Length);
            target = allTargets[randomIndex].transform;
        }
    }

    void MoveTowardsTarget()
    {
        // Rotate towards the target
        Vector3 directionToTarget = target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move towards the target
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        // If close enough to the target, stop moving
        if (directionToTarget.magnitude <= minDistanceToTarget)
        {

            target = null;
        }

    }
}

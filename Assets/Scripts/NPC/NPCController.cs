using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public float moveSpeed = 3f;  // Movement speed of the NPC
    public float rotationSpeed = 3f;  // Rotation speed of the NPC
    public float pushForce = 10f;
    public float minDistanceToTarget = 1f;  // Minimum distance to target before stopping
    public float pushDistanceThreshold = 2f; // Distance threshold to trigger pushing action
    public float pushCooldown = 2f; // Cooldown period for pushing action

    public bool isGrabbed = false;
    public bool isDead = false;

    private Transform target;  // Current target of the NPC
    private float pushTimer = 0f; // Timer for push cooldown

    public GameObject centerTarget; // The center target position
    public float timeToSwitchBehavior = 8f; // Time to switch behavior in seconds
    private bool hasReachedCenter = false; // Flag to indicate if NPC has reached the center
    private float firstMoveElapsedTime = 0f; // Elapsed time since moving to center
    private float stillElapsedTime = 0f; // Elapsed time since standing still
    public float standTillTime = 6f; // Time to stop standing still
    public bool canMove = false; // Flag to know if NPCs can move

    void Start()
    {
        centerTarget = GameObject.FindGameObjectsWithTag("CenterTarget")[0];

        target = centerTarget.transform;
    }

    void Update()
    {
        stillElapsedTime += Time.deltaTime;

        if(stillElapsedTime >= standTillTime)
        {
            canMove = true;
            moveSpeed = 5f;
        }

        if(!canMove)
        {
            return;
        }


        // Increment elapsed time
        firstMoveElapsedTime += Time.deltaTime;

        // If the NPC has not reached the center yet and time threshold is not exceeded, move towards the center
        if (!hasReachedCenter || firstMoveElapsedTime <= timeToSwitchBehavior)
        {
            MoveTowardsTarget();
            CheckReachedCenter();
        }
        else
        {
            if(target == centerTarget.transform)
            {
                target = null;
                ChooseRandomTarget();
            }

            if (!isDead)
            {
                if (!isGrabbed)
                {
                    // If the target is null, choose a new random target
                    if (target == null)
                    {
                        ChooseRandomTarget();
                        return;
                    }

                    // Move towards the target
                    MoveTowardsTarget();

                    // Check distance to target for pushing action
                    CheckPushAction();

                    // Update push cooldown timer
                    UpdateCooldownTimer();
                }
            }
        }
    }

    void CheckReachedCenter()
    {
        if (Vector3.Distance(transform.position, centerTarget.transform.position) <= minDistanceToTarget)
        {
            hasReachedCenter = true;
            target = null;
            firstMoveElapsedTime = timeToSwitchBehavior;
            ChooseRandomTarget();
        }
    }

    void UpdateCooldownTimer()
    {
        if (pushTimer > 0f)
        {
            pushTimer -= Time.deltaTime;
        }
    }

    void ChooseRandomTarget()
    {
        // Randomly choose between the player and other NPCs in the scene
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] NPCs = GameObject.FindGameObjectsWithTag("NPC");

        GameObject[] allTargets = new GameObject[players.Length + NPCs.Length];
        players.CopyTo(allTargets, 0);
        NPCs.CopyTo(allTargets, players.Length);

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

        if (target.gameObject.CompareTag("NPC"))
        {
            // If target is dead, stop moving
            if (target.gameObject.GetComponent<NPCController>().isDead)
            {
                target = null;
            }
        }

        // If close enough to the target, stop moving
        if (directionToTarget.magnitude <= minDistanceToTarget)
        {
            target = null; // Set target to null to choose a new target in the next update
        }             
    }

    void CheckPushAction()
    {
        // Check if push action is on cooldown
        if (pushTimer > 0f)
        {
            return;
        }

        // Check distance to target for pushing action
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= pushDistanceThreshold)
            {
                // Perform pushing action
                PushTarget();

                // Start push cooldown
                pushTimer = pushCooldown;
            }
        }
    }

    void PushTarget()
    {
        // Perform pushing action on the target
        if (target.CompareTag("Player") || target.CompareTag("NPC"))
        {
            // Push the target away
            Rigidbody targetRb = target.GetComponent<Rigidbody>();
            if (targetRb != null)
            {
                Vector3 pushDirection = (target.position - transform.position).normalized;
                targetRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }
    }
}

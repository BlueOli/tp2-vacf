using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public float pushForce = 10f;  // Force applied when pushing or shoving

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Pushing and Shoving
        if (Input.GetKeyDown(KeyCode.F))
        {
            PushOrShove();
        }

        // Grabbing onto Objects
        if (Input.GetKeyDown(KeyCode.E))
        {
            GrabObject();
        }
    }

    void PushOrShove()
    {
        // Perform a spherecast to detect objects in front of the player
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 1f, transform.forward, 2f);

        foreach (RaycastHit hit in hits)
        {
            // Check if the hit object has a rigidbody component
            Rigidbody otherRb = hit.collider.GetComponent<Rigidbody>();
            if (otherRb != null && otherRb != rb)  // Ensure the hit object is not the player itself
            {
                // Calculate the direction from the player to the hit object
                Vector3 pushDirection = (otherRb.transform.position - transform.position).normalized;

                // Apply force to the hit object to simulate pushing or shoving
                otherRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }
    }

    void GrabObject()
    {
        // Raycast forward from the player to detect grabbable objects
        // If a grabbable object is detected, attach the player to it using a physics joint or constraint
    }
}

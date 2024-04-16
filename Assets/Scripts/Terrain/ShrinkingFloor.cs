using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingFloor : MonoBehaviour
{
    public float shrinkRate = 0.1f; // Rate at which the floor shrinks per second

    private Vector3 initialScale; // Initial scale of the floor

    void Start()
    {
        // Store the initial scale of the floor
        initialScale = transform.localScale;

        // Start the coroutine to shrink the floor over time
        StartCoroutine(ShrinkFloor());
    }

    IEnumerator ShrinkFloor()
    {
        // Continuously shrink the floor until it reaches a very small size
        while (transform.localScale.x > 1f)
        {
            // Calculate the new scale of the floor by reducing it at the shrink rate
            Vector3 newScale = transform.localScale - Vector3.one * shrinkRate * Time.deltaTime;
            newScale.y = 1;


            // Clamp the new scale to prevent it from becoming negative
            newScale = Vector3.Max(newScale, Vector3.zero);

            // Apply the new scale to the floor
            transform.localScale = newScale;

            // Wait for the next frame
            yield return null;
        }
    }
}

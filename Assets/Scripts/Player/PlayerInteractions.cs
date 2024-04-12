using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public float grabDistance = 3f;             // Maximum distance for object grabbing
    public float throwForce = 20f;               // Force applied when throwing objects
    public float pushForce = 15f;               // Force applied when pushing NPCs
    public Material highlightMaterial;          // Material for highlighting grabbable objects
    public float highlightDuration = 0.5f;      // Duration for highlighting grabbable objects

    private Rigidbody rb;
    private bool isGrabbing;
    private GameObject highlightedObject;
    private GameObject grabbedObject;
    private Material originalMaterial;

    [SerializeField]
    private GameObject handObject;

    private Camera playerCam;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isGrabbing = false;
        highlightedObject = null;
        playerCam = GetComponentInChildren<Camera>();
    }

    void Update()
    {

        #region Highlight Objects
        // Perform a raycast from the camera to detect grabbable objects
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, grabDistance))
        {
            // Check if the hit object is grabbable
            if (hit.collider.CompareTag("Grabbable"))
            {
                // Highlight the grabbable object and show a detail pop-up
                if (!isGrabbing)
                {
                    HighlightObject(hit.collider.gameObject);
                }
            }
            // Check if the hit object is an NPC
            else if (hit.collider.CompareTag("NPC"))
            {
                // Highlight the NPC and show a detail pop-up
                if (!isGrabbing)
                {
                    HighlightObject(hit.collider.gameObject);
                }
            }
            else
            {
                // Clear the highlighted object if it's not grabbable or an NPC
                ClearHighlight();
            }
        }
        else
        {
            // Clear the highlighted object if no object is hit
            ClearHighlight();
        }
        #endregion

        // Grabbing Objects & NPCs
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isGrabbing)
            {
                if(hit.collider.gameObject != null)
                {
                    GrabObject(hit.collider.gameObject);
                }
            }
            else
            {
                ReleaseObject();
            }
        }

        // Pushing NPCs
        if (Input.GetKeyDown(KeyCode.F))
        {
            PushNPC();
        }        
    }

    void HighlightObject(GameObject obj)
    {
        // Clear previous highlighted object
        ClearHighlight();

        // Highlight the new object
        highlightedObject = obj;
        Renderer renderer = highlightedObject.GetComponent<Renderer>();
        originalMaterial = renderer.material;  // Save the original material
        renderer.material = highlightMaterial;
    }

    void ClearHighlight()
    {
        if (highlightedObject != null)
        {
            Renderer renderer = highlightedObject.GetComponent<Renderer>();
            renderer.material = originalMaterial;  // Restore the original material
            highlightedObject = null;
            originalMaterial = null;
        }
    }

    void GrabObject(GameObject obj)
    {
        Rigidbody otherRb = obj.GetComponent<Rigidbody>();
        if (otherRb != null)
        {
            otherRb.isKinematic = true;  // Disable physics simulation while grabbing
            grabbedObject = obj;
            obj.transform.SetParent(handObject.transform);  // Attach the object to the player's hand
            obj.transform.localPosition = Vector3.zero;  // Reset object's position relative to the hand
            obj.transform.localRotation = Quaternion.identity;  // Reset object's rotation relative to the hand
            obj.transform.SetParent(transform);  // Attach the object to the player
            isGrabbing = true;
        }
    }

    void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            Rigidbody grabbedRb = grabbedObject.GetComponent<Rigidbody>();
            if (grabbedRb != null)
            {
                grabbedRb.isKinematic = false;  // Enable physics simulation
                grabbedObject.transform.SetParent(null);  // Detach the object from the player
                grabbedObject = null;

                Vector3 throwDirection = playerCam.transform.forward + new Vector3(0,0.3f,0);  // Add an upward component
                throwDirection.Normalize();  // Normalize the direction vector
                grabbedRb.AddForce(throwDirection * throwForce, ForceMode.Impulse);  // Throw the object forward

                isGrabbing = false;
            }
        }
    }

    void PushNPC()
    {
        // Perform a spherecast to detect NPCs in front of the player
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 1f, transform.forward, 2f);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("NPC") || hit.collider.CompareTag("Grabbable"))
            {
                Rigidbody otherRb = hit.collider.GetComponent<Rigidbody>();
                if (otherRb != null)
                {
                    // Apply force to push the NPC away from the player
                    Vector3 pushDirection = (otherRb.transform.position - transform.position).normalized;
                    otherRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
                }
            }
        }
    }
}
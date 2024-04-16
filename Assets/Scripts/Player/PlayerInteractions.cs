using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractions : MonoBehaviour
{
    public float grabDistance = 3f;             // Maximum distance for object grabbing
    public float throwForce = 20f;               // Force applied when throwing objects
    public float pushForce = 15f;               // Force applied when pushing NPCs
    public Material highlightMaterial;          // Material for highlighting grabbable objects
    public float highlightDuration = 0.5f;      // Duration for highlighting grabbable objects

    private bool isGrabbing;
    private GameObject highlightedObject;
    private GameObject grabbedObject;
    private Material originalMaterial;

    [SerializeField]
    private GameObject handObject;

    private Camera playerCam;

    private bool isPushReady = true;
    private bool isGrabReady = true;
    private float pushCooldown = 1f;
    private float grabCooldown = 2f;
    private float pushCooldownTimer = 0f;
    private float grabCooldownTimer = 0f;
    public Image pushCooldownImage;
    public Image grabCooldownImage;


    void Start()
    {
        isGrabbing = false;
        highlightedObject = null;
        playerCam = GetComponentInChildren<Camera>();

        pushCooldownImage.fillAmount = 1;
        grabCooldownImage.fillAmount = 1;
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

        if (!isGrabReady)
        {
            grabCooldownTimer += Time.deltaTime;

            if (grabCooldownTimer >= grabCooldown)
            {
                isGrabReady = true;
                grabCooldownTimer = 0f;
                grabCooldownImage.fillAmount = 1;
            }
        }

        // Pushing NPCs
        if (Input.GetKeyDown(KeyCode.F) && isPushReady)
        {
            PushNPC();
        }

        if (!isPushReady)
        {
            pushCooldownTimer += Time.deltaTime;

            // Check if push cooldown is over
            if (pushCooldownTimer >= pushCooldown)
            {
                // Reset push cooldown
                isPushReady = true;
                pushCooldownTimer = 0f;
                pushCooldownImage.fillAmount = 1;
            }
        }

        #region UI     
        if(!isPushReady)
        {
            // Update push cooldown UI
            pushCooldownImage.fillAmount = Mathf.Clamp01((pushCooldownTimer / pushCooldown));
        }

        if (!isGrabReady)
        {
            // Update throw cooldown UI
            grabCooldownImage.fillAmount = Mathf.Clamp01((grabCooldownTimer / pushCooldown));
        }
        #endregion
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
            if (obj.CompareTag("NPC"))
            {
                obj.GetComponent<NPCController>().isGrabbed = true;
                this.GetComponent<PlayerMovement>().isGrabbingNPC = true;
            }
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
                if (grabbedObject.CompareTag("NPC"))
                {
                    grabbedObject.GetComponent<NPCController>().isGrabbed = false;
                    this.GetComponent<PlayerMovement>().isGrabbingNPC = false;
                }
                grabbedObject.transform.SetParent(null);  // Detach the object from the player
                grabbedObject = null;

                Vector3 throwDirection = playerCam.transform.forward + new Vector3(0,0.3f,0);  // Add an upward component
                throwDirection.Normalize();  // Normalize the direction vector
                grabbedRb.AddForce(throwDirection * throwForce, ForceMode.Impulse);  // Throw the object forward

                isGrabbing = false;
                isGrabReady = false;
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
                    isPushReady = false;
                }
            }
        }
    }
}
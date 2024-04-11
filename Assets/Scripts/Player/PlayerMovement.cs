using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;    // Movement speed
    public float sprintSpeed = 8f;  // Sprint speed
    public float jumpForce = 5f;   // Jump force

    private Rigidbody rb;

    [SerializeField]
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, GetComponent<Collider>().bounds.extents.y + 0.1f); ;

        // Player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Apply movement
        if (moveDirection != Vector3.zero)
        {
            // Calculate movement speed based on sprinting
            float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
            Vector3 moveVelocity = transform.TransformDirection(moveDirection) * speed * Time.deltaTime;
            rb.MovePosition(rb.position + moveVelocity);
        }

        // Player jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}

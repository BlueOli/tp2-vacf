using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject pauseMenu;

    public float sensitivity = 5.0f;  // Mouse sensitivity

    private float rotationX = 0.0f;

    void Update()
    {
        if (!pauseMenu.activeSelf)
        {
            // Get mouse input for camera rotation
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            // Rotate the player GameObject horizontally based on mouse input
            transform.parent.Rotate(Vector3.up, mouseX);

            // Rotate the camera vertically based on mouse input
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Limit vertical rotation to prevent flipping
            transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
        }       
    }

    public void ChangeMouseSensitivy()
    {
        sensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
    }
}
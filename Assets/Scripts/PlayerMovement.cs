using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;        // Movement speed of the player
    public float mouseSensitivity = 2f; // Sensitivity of the mouse movement
    public float jumpForce = 5f;        // Jump force applied to the player
    public float gravity = -9.81f;      // Gravity value
    public float zoomFOV = 30f; // The field of view when zoomed in
    public float normalFOV = 90f; // The default field of view
    public float zoomSpeed = 10f; // The speed of the zoom transition
    public Camera cam;
    public Transform cameraTransform;   // Reference to the player's camera

    private CharacterController controller;  // Reference to the CharacterController component
    private Vector3 velocity;                // Store player's velocity (for gravity, jumping, etc.)
    [SerializeField] private bool isGrounded;                 // Check if the player is grounded
    private float xRotation = 0f;            // For vertical mouse movement (looking up/down)

    void Start()
    {
        // Get the CharacterController component
        controller = GetComponent<CharacterController>();

        // Lock the cursor to the game screen and hide it
        Cursor.lockState = CursorLockMode.Locked;

        cam = cameraTransform.GetComponent<Camera>();
    }

    void Update()
    {
        // Mouse look functionality (rotate the camera)
        LookAround();

        // Move the player based on input
        MovePlayer();

        // Apply gravity
        ApplyGravity();

        // Zoom in
        ZoomIn();
    }

    void ZoomIn()
    {
        // Check if the zoom button is held
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // Smoothly zoom in by reducing the FOV
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoomFOV, Time.deltaTime * zoomSpeed);
        }
        else
        {
            // Smoothly return to normal FOV when the button is released
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalFOV, Time.deltaTime * zoomSpeed);
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;

        // Move the player vertically based on velocity (gravity and jump)
        controller.Move(velocity * Time.deltaTime);
    }

    // Function to control player movement
    void MovePlayer()
    {
        // Get player input (WASD or arrow keys)
        float x = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        float z = Input.GetAxis("Vertical");   // W/S or Up/Down Arrow

        // Calculate the direction to move in relative to the player's orientation
        Vector3 move = transform.right * x + transform.forward * z;
        
        // Apply the movement
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    // Function to control camera and player rotation (look around)
    void LookAround()
    {
        // Get mouse movement input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the player around the Y axis (horizontal look)
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera around the X axis (vertical look)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp to prevent over-rotation

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}


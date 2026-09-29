using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputHandler))]
public class playercontroller : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 6.5f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravity = -19.62f;

    [Header("Look")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float minLookAngle = -80f;
    [SerializeField] private float maxLookAngle = 80f;

    private CharacterController controller;
    private PlayerInputHandler input;
    private float verticalVelocity;
    private float xRotation;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInputHandler>();
        SetCursorLocked(true);
    }

    private void Update()
    {
        // ESC se cursor free / lock toggle (testing ke liye useful)
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            SetCursorLocked(Cursor.lockState != CursorLockMode.Locked);

        if (Cursor.lockState != CursorLockMode.Locked) return;

        HandleLook();
        HandleMovement();
    }

    private void HandleLook()
    {
        // Mouse delta already per-frame hota hai, isliye Time.deltaTime se multiply nahi karte
        Vector2 look = input.Look * mouseSensitivity;

        xRotation -= look.y;
        xRotation = Mathf.Clamp(xRotation, minLookAngle, maxLookAngle);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * look.x);
    }

    private void HandleMovement()
    {
        bool grounded = controller.isGrounded;
        if (grounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        Vector2 moveInput = input.Move;
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move = Vector3.ClampMagnitude(move, 1f);

        float speed = input.SprintHeld ? sprintSpeed : walkSpeed;

        if (input.JumpPressed && grounded)
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * speed + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }

    private void SetCursorLocked(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
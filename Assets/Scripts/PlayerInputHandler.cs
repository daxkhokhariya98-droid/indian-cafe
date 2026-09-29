using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }
    public bool SprintHeld => sprintAction.IsPressed();
    public bool JumpPressed => jumpAction.WasPressedThisFrame();
    public bool InteractPressed => interactAction.WasPressedThisFrame();
    public bool DropPressed => dropAction.WasPressedThisFrame();

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private InputAction interactAction;
    private InputAction dropAction;

    private void Awake()
    {
        moveAction = new InputAction("Move", InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        moveAction.AddBinding("<Gamepad>/leftStick");

        lookAction = new InputAction("Look", InputActionType.Value, "<Mouse>/delta");

        sprintAction = new InputAction("Sprint", InputActionType.Button, "<Keyboard>/leftShift");
        jumpAction = new InputAction("Jump", InputActionType.Button, "<Keyboard>/space");
        interactAction = new InputAction("Interact", InputActionType.Button, "<Keyboard>/e");
        dropAction = new InputAction("Drop", InputActionType.Button, "<Keyboard>/q");
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        sprintAction.Enable();
        jumpAction.Enable();
        interactAction.Enable();
        dropAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        sprintAction.Disable();
        jumpAction.Disable();
        interactAction.Disable();
        dropAction.Disable();
    }

    private void Update()
    {
        Move = moveAction.ReadValue<Vector2>();
        Look = lookAction.ReadValue<Vector2>();
    }
}

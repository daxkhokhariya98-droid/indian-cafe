using UnityEngine;
using UnityEngine.InputSystem; // Added for New Input System

public class PickupSystem : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private Transform holdPosition;
    [SerializeField] private float pickupDistance = 3f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private LayerMask pickableLayer;

    private Rigidbody heldObject;

    private void Update()
    {
        // New Input System check for E key or Left Mouse Click
        if (Keyboard.current.eKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (heldObject == null)
            {
                TryPickUpObject();
            }
            else
            {
                DropObject();
            }
        }

        // New Input System check for Right Mouse Click
        if (Mouse.current.rightButton.wasPressedThisFrame && heldObject != null)
        {
            ThrowObject();
        }
    }

    private void FixedUpdate()
    {
        if (heldObject != null)
        {
            MoveObject();
        }
    }

    private void TryPickUpObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickupDistance, pickableLayer))
        {
            if (hit.rigidbody != null)
            {
                heldObject = hit.rigidbody;
                heldObject.useGravity = false;
                heldObject.linearDamping = 10; // Changed from linearDamping to drag
                heldObject.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }
    }

    private void MoveObject()
    {
        if (Vector3.Distance(heldObject.transform.position, holdPosition.position) > 0.1f)
        {
            Vector3 moveDirection = (holdPosition.position - heldObject.transform.position);
            heldObject.linearVelocity = moveDirection * moveSpeed; // Changed from linearVelocity to velocity
        }
    }

    private void DropObject()
    {
        ResetHeldObject();
    }

    private void ThrowObject()
    {
        Rigidbody tempRigid = heldObject;
        ResetHeldObject();
        tempRigid.AddForce(transform.forward * throwForce, ForceMode.Impulse);
    }

    private void ResetHeldObject()
    {
        heldObject.useGravity = true;
        heldObject.linearDamping = 1; // Changed from linearDamping to drag
        heldObject.constraints = RigidbodyConstraints.None;
        heldObject.linearVelocity = Vector3.zero; // Changed from linearVelocity to velocity
        heldObject = null;
    }
}
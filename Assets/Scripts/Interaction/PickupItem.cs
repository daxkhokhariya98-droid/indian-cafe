using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName = "Item";

    private Rigidbody rb;
    private Collider[] colliders;

    public string ItemName => itemName;
    public string InteractionPrompt => $"[E] Pick up {itemName}";

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
    }

    public void Interact(PlayerInteractor interactor)
    {
        interactor.TryPickUp(this);
    }

    public void AttachTo(Transform holdPoint)
    {
        rb.isKinematic = true;
        SetCollidersEnabled(false);

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Detach(Vector3 pushVelocity)
    {
        transform.SetParent(null);
        SetCollidersEnabled(true);

        rb.isKinematic = false;
        rb.linearVelocity = pushVelocity; // Unity 6 me velocity ka naam linearVelocity hai
    }

    public void OnHoverEnter() { }  // baad me outline/highlight yahan lagayenge
    public void OnHoverExit() { }

    private void SetCollidersEnabled(bool enabled)
    {
        foreach (Collider c in colliders)
            c.enabled = enabled;
    }
}
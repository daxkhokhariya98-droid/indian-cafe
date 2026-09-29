using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerInteractor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private InteractionPromptUI promptUI;

    [Header("Settings")]
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private float dropForwardForce = 1.5f;

    private PlayerInputHandler input;
    private IInteractable currentTarget;

    public PickupItem HeldItem { get; private set; }
    public bool IsHoldingItem => HeldItem != null;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        DetectInteractable();

        if (input.InteractPressed && IsAlive(currentTarget))
            currentTarget.Interact(this);

        if (input.DropPressed && IsHoldingItem)
            DropHeldItem();
    }

    private void DetectInteractable()
    {
        IInteractable found = null;

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer, QueryTriggerInteraction.Ignore))
        {
            found = hit.collider.GetComponentInParent<IInteractable>();
        }

        if (found == currentTarget) return;

        if (IsAlive(currentTarget))
            currentTarget.OnHoverExit();

        currentTarget = found;

        if (IsAlive(currentTarget))
        {
            currentTarget.OnHoverEnter();
            promptUI?.Show(currentTarget.InteractionPrompt);
        }
        else
        {
            promptUI?.Hide();
        }
    }

    public bool TryPickUp(PickupItem item)
    {
        if (IsHoldingItem) return false;

        HeldItem = item;
        item.AttachTo(holdPoint);
        return true;
    }

    public void DropHeldItem()
    {
        if (!IsHoldingItem) return;

        HeldItem.Detach(cameraTransform.forward * dropForwardForce);
        HeldItem = null;
    }

    // Baad me cooking stations item ko consume karenge, tab ye use hoga
    public PickupItem ReleaseHeldItem()
    {
        PickupItem item = HeldItem;
        HeldItem = null;
        return item;
    }

    // Unity objects destroy hone par interface null check fail ho sakta hai, isliye ye helper
    private static bool IsAlive(IInteractable target)
    {
        if (target == null) return false;
        if (target is Object unityObject) return unityObject != null;
        return true;
    }
}
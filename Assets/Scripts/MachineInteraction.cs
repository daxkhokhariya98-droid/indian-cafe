using UnityEngine;

public class MachineInteraction : MonoBehaviour
{
    [Header("References")]
    public MatchaMachine matchaMachine;
    public GameObject machineCanvas; 
    public float interactionDistance = 3f;
    public Transform playerTransform; 

    void Start()
    {
        if (matchaMachine == null) matchaMachine = GetComponent<MatchaMachine>();
        if (machineCanvas != null) machineCanvas.SetActive(false);
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(playerTransform.position, transform.position);

        if (distance <= interactionDistance)
        {
            if (machineCanvas != null && !machineCanvas.activeSelf)
                machineCanvas.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
                TryRefillFromHands();

            if (Input.GetKeyDown(KeyCode.G))
                TryCraftMatcha();
        }
        else
        {
            if (machineCanvas != null && machineCanvas.activeSelf)
                machineCanvas.SetActive(false);
        }
    }

    void TryRefillFromHands()
    {
        PickupSystem pickupSystem = playerTransform.GetComponent<PickupSystem>();
        if (pickupSystem != null)
        {
            GameObject heldObj = pickupSystem.GetHeldItem();
            if (heldObj != null)
            {
                IngredientPacket packet = heldObj.GetComponent<IngredientPacket>();
                if (packet != null)
                {
                    matchaMachine.RefillMachine(packet);
                    if (packet.currentAmount <= 0) pickupSystem.DropItem();
                }
            }
        }
    }

    void TryCraftMatcha()
    {
        if (matchaMachine != null)
        {
            PickupSystem pickupSystem = playerTransform.GetComponent<PickupSystem>();
            Transform holdPt = (pickupSystem != null) ? pickupSystem.holdPoint : null;

            // MakeMatchaLatte ko holdPoint bhejenge taaki cup wahan spawn ho sake
            matchaMachine.MakeMatchaLatte(holdPt);
        }
    }
}
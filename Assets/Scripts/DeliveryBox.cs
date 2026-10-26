using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class DeliveryBox : MonoBehaviour
{
    [Header("Box Contents")]
    [SerializeField] private string itemID; // e.g., "MatchaLatte" or "Dango"
    [SerializeField] private int itemCount = 5;

    public string ItemID => itemID;
    public int ItemCount => itemCount;

    private void Start()
    {
        // Ensure it's set up for the pickup system we made earlier
        gameObject.layer = LayerMask.NameToLayer("Pickable");
    }
}
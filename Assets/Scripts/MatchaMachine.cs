using UnityEngine;
using TMPro; // Yeh missing tha

public class MatchaMachine : MonoBehaviour
{
    [Header("Machine Fill Levels (Max 100%)")]
    public float matchaAmount = 0f;
    public float milkAmount = 0f;
    public float sweetenerAmount = 0f;

    [Header("UI Text Displays on Machine")]
    public TextMeshProUGUI matchaText;
    public TextMeshProUGUI milkText;
    public TextMeshProUGUI sweetenerText;

    [Header("Crafting Settings")]
    public GameObject matchaLattePrefab; 
    public Transform spawnPoint; 

    void Update()
    {
        if (matchaText) matchaText.text = "Matcha: " + Mathf.RoundToInt(matchaAmount) + "%";
        if (milkText) milkText.text = "Milk: " + Mathf.RoundToInt(milkAmount) + "%";
        if (sweetenerText) sweetenerText.text = "Sweetener: " + Mathf.RoundToInt(sweetenerAmount) + "%";
    }

    public void RefillMachine(IngredientPacket packet)
    {
        if (packet == null) return;

        if (packet.type == IngredientType.MatchaPowder)
        {
            float spaceNeeded = 100f - matchaAmount;
            if (spaceNeeded > 0)
                matchaAmount += packet.TransferIngredient(spaceNeeded);
        }
        else if (packet.type == IngredientType.Milk)
        {
            float spaceNeeded = 100f - milkAmount;
            if (spaceNeeded > 0)
                milkAmount += packet.TransferIngredient(spaceNeeded);
        }
        else if (packet.type == IngredientType.Sweetener)
        {
            float spaceNeeded = 100f - sweetenerAmount;
            if (spaceNeeded > 0)
                sweetenerAmount += packet.TransferIngredient(spaceNeeded);
        }
    }

    public bool MakeMatchaLatte(Transform holdPoint)
    {
        if (matchaAmount >= 25f && milkAmount >= 25f && sweetenerAmount >= 25f)
        {
            matchaAmount -= 25f;
            milkAmount -= 25f;
            sweetenerAmount -= 25f;

            if (matchaLattePrefab != null)
            {
                if (holdPoint != null && holdPoint.childCount == 0)
                {
                    GameObject latte = Instantiate(matchaLattePrefab, holdPoint.position, holdPoint.rotation);
                    latte.transform.SetParent(holdPoint);
                    latte.transform.localPosition = Vector3.zero;
                    latte.transform.localRotation = Quaternion.identity;

                    Rigidbody rb = latte.GetComponent<Rigidbody>();
                    if (rb != null) { rb.isKinematic = true; rb.useGravity = false; }
                }
                else if (spawnPoint != null)
                {
                    Instantiate(matchaLattePrefab, spawnPoint.position, spawnPoint.rotation);
                }
            }

            Debug.Log("Matcha Latte Crafted & Spawned! ☕");
            return true;
        }
        else
        {
            Debug.Log("Ingredients shortage in machine!");
            return false;
        }
    }
}
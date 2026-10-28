using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Player Economy")]
    public int playerMoney = 1000; // Starting money
    public TextMeshProUGUI moneyText;

    [Header("Item Prices")]
    public int matchaPrice = 45;
    public int milkPrice = 35;
    public int sweetenerPrice = 35;

    [Header("Item Prefabs")]
    public GameObject matchaPrefab;
    public GameObject milkPrefab;
    public GameObject sweetenerPrefab;

    [Header("Delivery Point")]
    public Transform deliverySpawnPoint; // Shop ke baahar jahan delivery box/item drop hoga

    private void Start()
    {
        UpdateMoneyUI();
    }

    public void BuyMatchaPowder()
    {
        if (playerMoney >= matchaPrice)
        {
            playerMoney -= matchaPrice;
            UpdateMoneyUI();
            SpawnItem(matchaPrefab);
            Debug.Log("Matcha Powder ordered and delivered!");
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    public void BuyMilk()
    {
        if (playerMoney >= milkPrice)
        {
            playerMoney -= milkPrice;
            UpdateMoneyUI();
            SpawnItem(milkPrefab);
            Debug.Log("Milk ordered and delivered!");
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    public void BuySweetener()
    {
        if (playerMoney >= sweetenerPrice)
        {
            playerMoney -= sweetenerPrice;
            UpdateMoneyUI();
            SpawnItem(sweetenerPrefab);
            Debug.Log("Sweetener ordered and delivered!");
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    private void SpawnItem(GameObject itemPrefab)
    {
        if (itemPrefab != null && deliverySpawnPoint != null)
        {
            // Thoda sa random offset taaki saare item ek ke upar ek spawn na ho
            Vector3 spawnPosition = deliverySpawnPoint.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.5f, Random.Range(-0.2f, 0.2f));
            Instantiate(itemPrefab, spawnPosition, deliverySpawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Prefab ya Delivery Spawn Point missing hai Inspector mein!");
        }
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "₹ " + playerMoney;
        }
    }
}
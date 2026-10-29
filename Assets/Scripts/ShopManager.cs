using UnityEngine;
using System.Collections;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Item Prices")]
    public int matchaPrice = 45;
    public int milkPrice = 35;
    public int sweetenerPrice = 35;

    [Header("Item Prefabs")]
    public GameObject matchaPrefab;
    public GameObject milkPrefab;
    public GameObject sweetenerPrefab;

    [Header("Delivery Settings")]
    public Transform deliverySpawnPoint; // Shop ke baahar jahan delivery item drop hoga
    public float deliveryDelay = 30f;    // 30 seconds delay delivery ke liye

    public void BuyMatchaPowder()
    {
        // EconomyManager ke through balance check aur deduct hoga
        if (EconomyManager.Instance != null && EconomyManager.Instance.SpendMoney(matchaPrice))
        {
            StartCoroutine(SpawnDeliveryItemWithDelay(matchaPrefab));
            Debug.Log("Matcha Powder ordered! Will arrive in 30 seconds.");
        }
        else
        {
            Debug.Log("Not enough money or EconomyManager missing!");
        }
    }

    public void BuyMilk()
    {
        if (EconomyManager.Instance != null && EconomyManager.Instance.SpendMoney(milkPrice))
        {
            StartCoroutine(SpawnDeliveryItemWithDelay(milkPrefab));
            Debug.Log("Milk ordered! Will arrive in 30 seconds.");
        }
        else
        {
            Debug.Log("Not enough money or EconomyManager missing!");
        }
    }

    public void BuySweetener()
    {
        if (EconomyManager.Instance != null && EconomyManager.Instance.SpendMoney(sweetenerPrice))
        {
            StartCoroutine(SpawnDeliveryItemWithDelay(sweetenerPrefab));
            Debug.Log("Sweetener ordered! Will arrive in 30 seconds.");
        }
        else
        {
            Debug.Log("Not enough money or EconomyManager missing!");
        }
    }

    private IEnumerator SpawnDeliveryItemWithDelay(GameObject itemPrefab)
    {
        // 30 seconds ka wait karega
        yield return new WaitForSeconds(deliveryDelay);

        if (itemPrefab != null && deliverySpawnPoint != null)
        {
            // Thoda sa random offset taaki items ek ke upar ek spawn na ho
            Vector3 spawnPosition = deliverySpawnPoint.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.5f, Random.Range(-0.2f, 0.2f));
            Instantiate(itemPrefab, spawnPosition, deliverySpawnPoint.rotation);
            Debug.Log("Item Delivered at Shop Entrance!");
        }
        else
        {
            Debug.LogWarning("Prefab ya Delivery Spawn Point missing hai Inspector mein!");
        }
    }
}
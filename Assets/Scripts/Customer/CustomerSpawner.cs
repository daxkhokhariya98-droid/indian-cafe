using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] private Transform entryPoint;
    [SerializeField] private Transform counterPoint;
    [SerializeField] private Transform[] seatPoints;
    [SerializeField] private Transform exitPoint;

    [Header("Spawning")]
    [SerializeField] private GameObject[] customerPrefabs;
    [SerializeField] private float minSpawnInterval = 5f;
    [SerializeField] private float maxSpawnInterval = 12f;
    [SerializeField] private int maxActiveCustomers = 4;

    private readonly List<CustomerAI> activeCustomers = new();
    private readonly HashSet<Transform> occupiedSeats = new();
    private float spawnTimer;

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f && activeCustomers.Count < maxActiveCustomers)
        {
            SpawnCustomer();
            spawnTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    private void SpawnCustomer()
    {
        Transform freeSeat = GetFreeSeat();
        if (freeSeat == null) return; // cafe full hai, spawn skip

        GameObject prefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];
        GameObject obj = Instantiate(prefab, entryPoint.position, entryPoint.rotation);

        CustomerAI customer = obj.GetComponent<CustomerAI>();
        customer.Setup(counterPoint, freeSeat, exitPoint);

        occupiedSeats.Add(freeSeat);
        activeCustomers.Add(customer);

        customer.OnCustomerLeft += HandleCustomerLeft;
        customer.OnOrderExpired += HandleOrderExpired;
    }

    private Transform GetFreeSeat()
    {
        foreach (Transform seat in seatPoints)
        {
            if (!occupiedSeats.Contains(seat))
                return seat;
        }
        return null;
    }

    private void HandleCustomerLeft(CustomerAI customer)
    {
        CleanupCustomer(customer);
    }

    private void HandleOrderExpired(CustomerAI customer)
    {
        // TODO: baad me "angry customer" feedback / tip penalty yaha add karenge
    }
    private readonly Dictionary<CustomerAI, Transform> customerSeats = new();

    // SpawnCustomer me, occupiedSeats.Add ke saath ye bhi add karo:
    // customerSeats[customer] = freeSeat;

    private void CleanupCustomer(CustomerAI customer)
    {
        if (customerSeats.TryGetValue(customer, out Transform seat))
        {
            occupiedSeats.Remove(seat);
            customerSeats.Remove(customer);
        }
        activeCustomers.Remove(customer);
        customer.OnCustomerLeft -= HandleCustomerLeft;
        customer.OnOrderExpired -= HandleOrderExpired;
        Destroy(customer.gameObject, 0.1f);
    }
}
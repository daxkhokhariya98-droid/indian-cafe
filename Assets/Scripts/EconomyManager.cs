using UnityEngine;
using TMPro;

public class EconomyManager : MonoBehaviour
{
    // Singleton pattern taaki ise kisi bhi script se easily access kiya ja sake
    public static EconomyManager Instance;

    [Header("Player Bank Balance")]
    public int currentBalance = 1000; // Starting money 1000 se start hoga

    [Header("UI Reference")]
    public TextMeshProUGUI balanceDisplayTexts; // Saare UI texts jahan balance dikhana hai

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    // Paise add karne ke liye (jaise customer se income)
    public void AddMoney(int amount)
    {
        currentBalance += amount;
        UpdateUI();
        Debug.Log("Added ₹" + amount + ". New Balance: ₹" + currentBalance);
    }

    // Paise deduct karne ke liye (jaise shop se item buy karna ya bills pay karna)
    public bool SpendMoney(int amount)
    {
        if (currentBalance >= amount)
        {
            currentBalance -= amount;
            UpdateUI();
            Debug.Log("Spent ₹" + amount + ". Remaining Balance: ₹" + currentBalance);
            return true; // Transaction successful
        }
        else
        {
            Debug.Log("Not enough balance!");
            return false; // Transaction failed
        }
    }

    private void UpdateUI()
    {
        if (balanceDisplayTexts != null)
        {
            balanceDisplayTexts.text = "₹ " + currentBalance;
        }
    }
}
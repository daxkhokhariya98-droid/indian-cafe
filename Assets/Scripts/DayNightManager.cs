using UnityEngine;
using TMPro;

public class DayNightManager : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI timeAndDateText; // PC Header ka "Time & Date Text"

    [Header("Time Settings")]
    public float dayDurationInSeconds = 120f; // 1 full day kitne real-world seconds mein hoga

    private float timer = 0f;
    private int currentHour = 8;  // Morning 8:00 AM se start
    private int currentMinute = 0;
    private int dayIndex = 0;

    // Months ki list January 1 se start karne ke liye
    private string[] monthNames = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
    private int currentMonthIndex = 0; // January
    private int currentDayOfMonth = 1;  // 1st

    void Update()
    {
        timer += Time.deltaTime;

        // In-game minutes update
        if (timer >= (dayDurationInSeconds / 1440f)) // 1440 minutes in a day
        {
            timer = 0f;
            currentMinute++;

            if (currentMinute >= 60)
            {
                currentMinute = 0;
                currentHour++;

                if (currentHour >= 24)
                {
                    currentHour = 0;
                    AdvanceToNextDay();
                }
            }

            UpdateUI();
        }
    }

    void AdvanceToNextDay()
    {
        currentDayOfMonth++;

        // Basic Month transition (Har month 30 days ka simple logic)
        if (currentDayOfMonth > 30)
        {
            currentDayOfMonth = 1;
            currentMonthIndex++;

            if (currentMonthIndex >= monthNames.Length)
            {
                currentMonthIndex = 0; // Reset to January
            }
        }
    }

    void UpdateUI()
    {
        if (timeAndDateText != null)
        {
            string ampm = currentHour >= 12 ? "PM" : "AM";
            int displayHour = currentHour % 12;
            if (displayHour == 0) displayHour = 12;

            // Example Format: "08:30 AM | Jan 01"
            timeAndDateText.text = string.Format("{0:D2}:{1:D2} {2} | {3} {4:D2}",
                displayHour, currentMinute, ampm, monthNames[currentMonthIndex], currentDayOfMonth);
        }
    }
}
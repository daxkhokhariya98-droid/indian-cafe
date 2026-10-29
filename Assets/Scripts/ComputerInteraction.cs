using UnityEngine;
using TMPro;

public class ComputerInteraction : MonoBehaviour
{
    [Header("UI References")]
    public GameObject computerUI; // PC Canvas / MonitorFrame
    public TextMeshProUGUI interactPromptText; // "Press [E] to Access Computer"

    [Header("Settings")]
    public float interactionDistance = 3f;
    public Transform playerTransform;

    private bool isInRange = false;
    private bool isUsingComputer = false;

    void Start()
    {
        // Agar player transform assign nahi kiya, toh auto-find karne ki koshish karega
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
            else if (Camera.main != null)
            {
                playerTransform = Camera.main.transform;
            }
        }

        if (computerUI != null) computerUI.SetActive(false);
        if (interactPromptText != null) interactPromptText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(playerTransform.position, transform.position);

        if (distance <= interactionDistance)
        {
            isInRange = true;
            if (!isUsingComputer && interactPromptText != null)
            {
                interactPromptText.gameObject.SetActive(true);
                interactPromptText.text = "Press [E] to Access Computer";
            }
        }
        else
        {
            isInRange = false;
            if (interactPromptText != null) interactPromptText.gameObject.SetActive(false);

            // Distance se baahar jane par PC auto-close ho jaye
            if (isUsingComputer)
            {
                ClosePC();
            }
        }

        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("[E] Pressed near computer!");
            ToggleComputer();
        }
    }

    public void ToggleComputer()
    {
        isUsingComputer = !isUsingComputer;
        Debug.Log("ToggleComputer called! New State: " + isUsingComputer);
        ApplyState();
    }

    // UI Red Button ke OnClick() par ye call hoga
    public void ClosePC()
    {
        isUsingComputer = false;
        Debug.Log("ClosePC called!");
        ApplyState();
    }

    private void ApplyState()
    {
        if (computerUI != null)
        {
            computerUI.SetActive(isUsingComputer);
            Debug.Log("Computer UI Active state set to: " + isUsingComputer);
        }
        else
        {
            Debug.LogWarning("Computer UI Panel reference missing hai Inspector mein!");
        }

        if (isUsingComputer)
        {
            if (interactPromptText != null) interactPromptText.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
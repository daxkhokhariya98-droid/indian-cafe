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
            ToggleComputer();
        }
    }

    public void ToggleComputer()
    {
        isUsingComputer = !isUsingComputer;
        ApplyState();
    }

    // UI Red Button ke OnClick() par ye call hoga
    public void ClosePC()
    {
        isUsingComputer = false;
        ApplyState();
    }

    private void ApplyState()
    {
        if (computerUI != null)
        {
            computerUI.SetActive(isUsingComputer);
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
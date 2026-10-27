using UnityEngine;
using UnityEngine.InputSystem;

public class JukeboxInteractable : MonoBehaviour
{
    [Header("UI Panel Reference")]
    [SerializeField] private GameObject jukeboxUI; 

    [Header("Interaction Settings")]
    [SerializeField] private float interactionDistance = 3f;
    private Transform playerTransform;
    private bool isPlayerNearby = false;
    private bool isUIOpen = false;

    private void Start()
    {
        // Player ko tag se dhundhte hain
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // Safety check: Agar yeh script galti se Canvas par lag gayi hai, toh poora canvas disable mat karo
        if (jukeboxUI != null && jukeboxUI != this.gameObject)
        {
            jukeboxUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= interactionDistance)
        {
            isPlayerNearby = true;

            // 'E' key press check (New Input System)
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                ToggleJukeboxUI();
            }
        }
        else
        {
            isPlayerNearby = false;
            if (isUIOpen)
            {
                CloseJukeboxUI();
            }
        }
    }

    private void ToggleJukeboxUI()
    {
        isUIOpen = !isUIOpen;
        if (jukeboxUI != null && jukeboxUI != this.gameObject)
        {
            jukeboxUI.SetActive(isUIOpen);
        }

        if (isUIOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void CloseJukeboxUI()
    {
        isUIOpen = false;
        if (jukeboxUI != null && jukeboxUI != this.gameObject)
        {
            jukeboxUI.SetActive(false);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
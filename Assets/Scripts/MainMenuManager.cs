using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Name to Load")]
    [SerializeField] private string gameSceneName = "Level 1";

    [Header("Settings Panel (Optional)")]
    [SerializeField] private GameObject settingsPanel;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip buttonClickSound;

    private void Start()
    {
        // Cursor hamesha menu me visible aur unlocked rahega
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Safety Check: Sirf tabhi band kare jab settingsPanel assigned ho, 
        // woh khud yeh object na ho, aur woh poora Canvas ya BackgroundPanel na ho!
        if (settingsPanel != null && settingsPanel != this.gameObject && settingsPanel.name != "Canvas" && settingsPanel.name != "BackgroundPanel")
        {
            settingsPanel.SetActive(false);
        }
    }

    public void PlayGame()
    {
        PlaySound();
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenSettings()
    {
        PlaySound();
        if (settingsPanel != null && settingsPanel != this.gameObject)
        {
            settingsPanel.SetActive(true);
        }
    }

    public void CloseSettings()
    {
        PlaySound();
        if (settingsPanel != null && settingsPanel != this.gameObject)
        {
            settingsPanel.SetActive(false);
        }
    }

    public void QuitGame()
    {
        PlaySound();
        Debug.Log("Game is quitting...");
        Application.Quit();
    }

    private void PlaySound()
    {
        if (buttonClickSound != null)
        {
            // PlayClipAtPoint ensures the sound plays even when the scene changes
            AudioSource.PlayClipAtPoint(buttonClickSound, Camera.main.transform.position);
        }
    }
}
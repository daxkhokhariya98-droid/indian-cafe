using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Name to Load")]
    [SerializeField] private string gameSceneName = "Level 1";

    [Header("Settings Panel (Optional)")]
    [SerializeField] private GameObject settingsPanel;

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
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenSettings()
    {
        if (settingsPanel != null && settingsPanel != this.gameObject)
        {
            settingsPanel.SetActive(true);
        }
    }

    public void CloseSettings()
    {
        if (settingsPanel != null && settingsPanel != this.gameObject)
        {
            settingsPanel.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Game is quitting...");
        Application.Quit();
    }
}
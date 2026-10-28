using UnityEngine;

public class ComputerWindowManager : MonoBehaviour
{
    [Header("All Computer Windows")]
    public GameObject shopWindow;
    public GameObject upgradesWindow;
    public GameObject billsWindow;
    public GameObject musicWindow;

    // Sabhi windows ko band karke sirf targeted window kholne ke liye
    public void OpenShopWindow()
    {
        CloseAllWindows();
        if (shopWindow != null) shopWindow.SetActive(true);
    }

    public void OpenUpgradesWindow()
    {
        CloseAllWindows();
        if (upgradesWindow != null) upgradesWindow.SetActive(true);
    }

    public void OpenBillsWindow()
    {
        CloseAllWindows();
        if (billsWindow != null) billsWindow.SetActive(true);
    }

    public void OpenMusicWindow()
    {
        CloseAllWindows();
        if (musicWindow != null) musicWindow.SetActive(true);
    }

    public void CloseAllWindows()
    {
        if (shopWindow != null) shopWindow.SetActive(false);
        if (upgradesWindow != null) upgradesWindow.SetActive(false);
        if (billsWindow != null) billsWindow.SetActive(false);
        if (musicWindow != null) musicWindow.SetActive(false);
    }
}
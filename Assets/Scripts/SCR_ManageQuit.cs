using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_ManageQuit : MonoBehaviour {


    public GameObject quitPanel;

    private void Start()
    {
        quitPanel.SetActive(false);
    }

    public void OpenQuitPanel()
    {
        quitPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CancelQuit()
    {
        quitPanel.SetActive(false);
    }
}

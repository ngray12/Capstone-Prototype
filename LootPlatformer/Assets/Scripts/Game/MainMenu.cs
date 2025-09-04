using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject creditsMenu;
    public GameObject htpMenu;

    public void ShowCredits()
    {
        creditsMenu.SetActive(true);
    }
    public void CloseCredits()
    {
        creditsMenu.SetActive(false);
    }
    
    public void OpenHTP()
    {
        htpMenu.SetActive(true);
    }

    public void CloseHTP()
    {
        htpMenu.SetActive(false);
    }
    
    public void PlayGame()
    {
        GameManager.Instance.LoadLevelSelect();
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}

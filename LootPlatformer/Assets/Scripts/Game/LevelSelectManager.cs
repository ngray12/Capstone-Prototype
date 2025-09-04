using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
   public void GoToMainMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadLevel2() 
    {
        SceneManager.LoadScene(3);
    }
    public void LoadLevel3() 
    {
        SceneManager.LoadScene(4);
    }
    public void LoadLevel4() 
    {
        SceneManager.LoadScene(5);
    }
    public void LoadLevel5() 
    {
        SceneManager.LoadScene(6);
    }







}

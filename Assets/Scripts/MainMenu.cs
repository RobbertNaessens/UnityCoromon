using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void quitGame()
    {
        Application.Quit();
    }

    public void playGame()
    {
        CreatePlatform.DeleteIndex();
        SceneManager.LoadScene("UitlegScene");       
    }
}

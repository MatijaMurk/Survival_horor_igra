using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
public void ExitButton()
    {
        Application.Quit();
        print("Exit Successful");
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Game");
    }
}

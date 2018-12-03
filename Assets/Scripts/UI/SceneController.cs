using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    private int sceneNum;

    void Awake()
    {
        sceneNum = SceneManager.GetActiveScene().buildIndex;
    }

	public void StartGame()
    {
        Debug.Log("asdf");
        SceneManager.LoadScene(1);
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(sceneNum + 1);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(sceneNum);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToMenu();
        }
    }
}

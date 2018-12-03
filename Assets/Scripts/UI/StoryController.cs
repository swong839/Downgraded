using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryController : MonoBehaviour {

    private int textIndex;
    private int m_sceneIndex;

    [SerializeField]
    private string[] story;

    [SerializeField]
    private GameObject completeObject;

    [SerializeField]
    private Text text;

    [SerializeField]
    private GameObject next;

    void Awake()
    {
        if (completeObject != null)
        {
            completeObject.SetActive(false);
        }
        textIndex = 0;
        m_sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

	// Use this for initialization
	void Start () {

	}
	
	IEnumerator showStory()
    {
        for (int i = 0; i < story.Length; i++)
        {
            for (int j = 0; j < story[i].Length; j++)
            {
                text.text += story[i][j];
                yield return new WaitForSeconds(0.01f);
            }
            text.text += '\n';
            text.text += '\n';
            yield return new WaitForSeconds(1.5f);
        }
        if (next != null)
        {
            next.SetActive(true);
        }
    }

    public void startStory()
    {
        completeObject.SetActive(true);
        StartCoroutine("showStory");
    }

    public void skip()
    {
        StopAllCoroutines();
        goToGame();
    }

    public void goToGame()
    {
        SceneManager.LoadScene(m_sceneIndex + 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {

    [SerializeField]
    private string tutorial;

    [SerializeField]
    private Text text;

    private bool used;

	void OnTriggerEnter2D(Collider2D col)
    {
        if (!used && col.CompareTag("Player"))
        {
            used = true;
            StartCoroutine("startType");
        }
    }

    IEnumerator startType()
    {
        for (int i = 0; i < tutorial.Length; i++)
        {
            text.text += tutorial[i];
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(3f);
        text.text = "";
    }
}

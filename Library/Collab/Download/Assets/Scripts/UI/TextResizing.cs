using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextResizing : MonoBehaviour
{

    public float scale; //value from 0 to 1

    private void Awake()
    {
        StartCoroutine(resize());
    }

    //public void resize()
    IEnumerator resize()
    {
        yield return new WaitForEndOfFrame();
        Text myText = gameObject.GetComponent<Text>();
        myText.fontSize = (int)(gameObject.GetComponent<RectTransform>().rect.height * scale);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelmetStatus : MonoBehaviour {

    private bool m_half;

    public Sprite half_helmet;

    public bool Half
    {
        get { return m_half; }
    }

    public void halfHealth()
    {
        m_half = true;
        gameObject.GetComponent<Image>().sprite = half_helmet;
        Debug.Log("switched image");
    }
}

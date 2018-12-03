using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour {

    [SerializeField]
    private GameObject credits;

    [SerializeField]
    private GameObject main;

    public void ShowCredits()
    {
        main.SetActive(false);
        credits.SetActive(true);
        Debug.Log(credits.activeSelf);
    }

    public void ExitCredits()
    {
        credits.SetActive(false);
        main.SetActive(true);
    }
}

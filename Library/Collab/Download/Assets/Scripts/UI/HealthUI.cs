using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {

    public static event Delegates.IntDelegate HealthUIUpdateEvent;

    [SerializeField]
    private GameObject unit;

    [SerializeField]
    private int health;

    [SerializeField]
    private GameObject[] healthList;

    [SerializeField]
    private HelmetStatus status;
    //private HorizontalLayoutGroup layout;

    private void OnEnable()
    {
        PlayerManager.PlayerManagerUpdateHealthEvent += UpdateHealth;
        PlayerManager.PlayerSetUpHealth += SetUpHealth;
    }

    private void OnDisable()
    {
        PlayerManager.PlayerManagerUpdateHealthEvent -= UpdateHealth;
        PlayerManager.PlayerSetUpHealth -= SetUpHealth;
    }

    void Awake()
    {
        health = 10;
        healthList = new GameObject[health];
        //layout = gameObject.GetComponent<HorizontalLayoutGroup>();
    }

    // Use this for initialization
    void Start()
    {
        SetUpHealth(health);
    }

    void SetUpHealth(int num)
    {
        health = 10;
        for (int i = 0; i < health / 2; i++)
        {
            healthList[i] = Instantiate(unit, this.transform);
        }
    }

    void UpdateHealth(int num)
    {
        while (health > num)
        {
            //layout.spacing -= 50;
            Debug.Log(num);
            int index = ((health + 1) / 2) - 1;
            Debug.Log(index);
            
            status = healthList[index].GetComponent<HelmetStatus>();
            if (!status.Half)
            {
                status.halfHealth();
            }
            else
            {
                Debug.Log("destroying");
                Destroy(healthList[index]);
            }
            health -= 1;
        }
    }
}

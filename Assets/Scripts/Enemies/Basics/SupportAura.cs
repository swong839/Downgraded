﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportAura : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<BasicEnemy>().buff();
        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<BasicEnemy>().unbuff();
        }
    }
}

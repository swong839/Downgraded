using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SFXManager : MonoBehaviour {

    private static SFXManager m_instance;
    
    public static SFXManager Instance
    {
        get { return m_instance; }
    }

    [SerializeField]
    private AudioClass[] SFX;

	void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_instance = this;
        }
        foreach (AudioClass each in SFX)
        {
            each.source = gameObject.AddComponent<AudioSource>();
            each.source.clip = each.clip;
            each.source.volume = each.volume;
            each.source.loop = each.loop;
            each.source.pitch = each.pitch;
        }
    }

    public void Play(string name)
    {
        AudioClass s = Array.Find(SFX, item => item.name == name);

        if (s != null)
        {
            s.source.Play();
        }
    }
}

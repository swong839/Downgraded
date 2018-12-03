using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioClass{

    [SerializeField]
    private string m_name;

    [SerializeField]
    private AudioClip m_clip;

    [SerializeField]
    [Range(0f, 1f)]
    private float m_volume;

    [SerializeField]
    [Range(0f, 1.5f)]
    private float m_pitch;

    [HideInInspector]
    private AudioSource m_source;

    [SerializeField]
    private bool m_loop;

    public string name
    {
        get { return m_name; }
    }

    public AudioClip clip
    {
        get { return m_clip; }
    }

    public float volume
    {
        get { return m_volume; }
    }

    public float pitch
    {
        get { return m_pitch; }
    }

    public AudioSource source
    {
        set { m_source = value; }
        get { return m_source; }
    }

    public bool loop
    {
        get { return m_loop; }
    }
}

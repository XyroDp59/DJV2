using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SphereCollider))]
public class SoundEmitter : MonoBehaviour
{
    [SerializeField] private UnityEvent onPlay;
    
    private AudioSource m_Source;
    private SphereCollider m_Trigger;
    private float volume;
    private bool m_IsPlaying;

    // Start is called before the first frame update
    void Awake()
    {
        m_Source = GetComponent<AudioSource>();
        m_Trigger = GetComponent<SphereCollider>();
        m_Trigger.isTrigger = true;
        m_Trigger.radius = 0f;
    }

    public void PlaySound(float volume)
    {
        if (m_IsPlaying) { return; }
        StartCoroutine(PlaySoundCoroutine(volume));
    }

    IEnumerator PlaySoundCoroutine(float volume)
    {
        m_IsPlaying = true;
        this.volume = volume;
        m_Source.volume = volume;
        onPlay.Invoke();
        yield return new WaitForSeconds(m_Source.clip.length);
        m_IsPlaying=false;
        this.volume = 0;
    }

    public float GetSoundVolume()
    {
        if (m_IsPlaying) return volume;
        return 0;
    }
}

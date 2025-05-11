using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    [SerializeField] private UnityEvent onPlay;
    [SerializeField] private AudioSource source;
    
    private float volume;
    private bool m_IsPlaying;

    private float floorCoeff = 1f;

    private void Awake()
    {
        if(source == null) source = GetComponent<AudioSource>();
    }

    public void PlaySound(float volume, float duration)
    {
        if (m_IsPlaying) { return; }
        StartCoroutine(PlaySoundCoroutine(volume * floorCoeff, duration));
    }

    public void PlaySound(float volume)
    {
        PlaySound(volume, source.clip.length);
    }

    public void SetFloorCoeff(float floorCoeff)
    {
        this.floorCoeff = floorCoeff;
    }

    IEnumerator PlaySoundCoroutine(float volume, float duration)
    {
        m_IsPlaying = true;
        this.volume = volume;
        //m_Source.volume = volume;
        onPlay.Invoke();
        yield return new WaitForSeconds(duration);
        m_IsPlaying=false;
        this.volume = 0;
    }

    public float GetSoundVolume()
    {
        if (m_IsPlaying) return volume;
        return 0;
    }
}

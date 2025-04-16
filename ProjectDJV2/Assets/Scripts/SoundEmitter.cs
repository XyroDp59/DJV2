using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    private AudioSource m_Source;
    private SphereCollider m_Trigger;
    [SerializeField] private float volume2RadiusRatio = 1.0f;

    // Start is called before the first frame update
    void Awake()
    {
        m_Source = GetComponent<AudioSource>();
        m_Trigger = GetComponent<SphereCollider>();
    }

    public void PlaySound(float volume)
    {
        StartCoroutine(PlaySoundCoroutine(volume));
    }

    IEnumerator PlaySoundCoroutine(float volume)
    {
        m_Trigger.enabled = true;
        m_Trigger.radius = volume * volume2RadiusRatio;
        yield return new WaitForSeconds(m_Source.clip.length);
        m_Trigger.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy;
        if (other.TryGetComponent<Enemy>(out enemy))
        {
            enemy.AddToInterestPoints(transform);
        }
    }
}

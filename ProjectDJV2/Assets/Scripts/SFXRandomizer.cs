using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXRandomizer : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;

    public void playOneShot()
    {
        GetComponent<AudioSource>().PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
}

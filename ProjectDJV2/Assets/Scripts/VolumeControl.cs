using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    // Options
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider soundtrackSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private AudioMixer output;
    
    private bool _isSFXClicked;
    
    public void OnMasterVolumeChange()
    {
        float newVolume = masterSlider.value > 0 ? 20f * Mathf.Log10(masterSlider.value) : -80f;
        output.SetFloat("MasterVolume", newVolume );
    }

    public void OnSoundtrackVolumeChange()
    {
        float newVolume = soundtrackSlider.value > 0 ? 20f * Mathf.Log10(soundtrackSlider.value) : -80f;
        output.SetFloat("SoundtrackVolume", newVolume);
    }

    public void OnSFXVolumeChange()
    {
        float newVolume = sfxSlider.value > 0 ? 20f * Mathf.Log10(sfxSlider.value) : -80f;
        output.SetFloat("SFXVolume", newVolume);
    }
}

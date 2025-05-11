using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMeter : MonoBehaviour
{
    [SerializeField] private SoundEmitter soundEmitter;
    [SerializeField] private GameObject needle;

    private float _currentVolume = 0;
    private float _previousVolume = -1;
    private Quaternion _rotationVolume;
    private Quaternion _rotationStart;
    private float _animationTime;
    
    // Notes pour l'interpolation
    // Max Pivot rotation angle Z :  85
    // Min Pivot rotation angle Z :  -85
    // Max Emitter noise : 12
    // Min Emitter noise : 0
    
    private void Update()
    {
        _currentVolume = soundEmitter.GetSoundVolume() * soundEmitter.GetFloorCoeff();
        if (Math.Abs(_currentVolume - _previousVolume) > 0.0001f)
        {
            float ratio = _currentVolume / 12f;
            float angleVolume = Mathf.Lerp(85, -85, ratio);
        
            _rotationVolume = Quaternion.Euler(0, 0, angleVolume);
            
            _rotationStart = needle.transform.rotation;

            _animationTime = 0;
        }
        
        _animationTime += Time.deltaTime;
        needle.transform.rotation = Quaternion.Lerp(_rotationStart, _rotationVolume, _animationTime);

        _previousVolume = _currentVolume;
    }
}

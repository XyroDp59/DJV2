using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] float soundCoefficient;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out SoundEmitter s))
        {
            s.SetFloorCoeff(soundCoefficient);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out SoundEmitter s))
        {
            s.SetFloorCoeff(1);
        }
    }
}

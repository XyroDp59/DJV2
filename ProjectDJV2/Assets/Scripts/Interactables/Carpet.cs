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
            Debug.Log(s.gameObject.name);
            s.SetFloorCoeff(soundCoefficient);
        }
    }
}

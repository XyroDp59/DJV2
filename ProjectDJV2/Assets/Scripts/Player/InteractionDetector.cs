using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] PlayerController p;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out IInteractable i)) { 
            p.SetInteractableObject(i);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IInteractable i))
        {
            p.SetInteractableObject(null);
        }
    }
}

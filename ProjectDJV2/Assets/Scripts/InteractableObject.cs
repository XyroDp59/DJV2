using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SoundEmitter))]
public class InteractableObject : MonoBehaviour
{
    [SerializeField] public UnityEvent OnInteract;
    [SerializeField] protected SoundEmitter emitter;

    public void Interact()
    {
        OnInteract.Invoke();
    }

    protected void Awake()
    {
        
    }
}

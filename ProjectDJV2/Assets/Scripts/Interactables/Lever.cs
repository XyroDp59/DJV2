using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SoundEmitter))]
public class Lever : MonoBehaviour, IInteractable
{
    [SerializeField] public UnityEvent OnInteract;
    protected SoundEmitter emitter;

    public void Interact(PlayerController player)
    {
        OnInteract.Invoke();
    }

    protected void Awake()
    {
        emitter = GetComponent<SoundEmitter>();
    }
}

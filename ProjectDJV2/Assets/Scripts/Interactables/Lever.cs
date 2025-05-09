using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SoundEmitter))]
public class Lever : MonoBehaviour, IInteractable
{
    [SerializeField] public UnityEvent OnInteract;
    protected SoundEmitter emitter;

    public void Interact(PlayerController player)
    {
        Debug.Log("Interact");
        OnInteract.Invoke();
    }

    protected void Awake()
    {
        emitter = GetComponent<SoundEmitter>();
    }
}

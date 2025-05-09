using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class GrabbableObject : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject ThrowIndicator;
    [SerializeField] private float throwVelocity;
    [SerializeField] private float thrownVolume;
    [SerializeField] private float grabbedVolume;

    [SerializeField] public UnityEvent OnInteract;
    SoundEmitter emitter;

    public bool isThrown = false;
    Rigidbody rb;

    private void Awake()
    {
        emitter = GetComponent<SoundEmitter>();
        rb = GetComponent<Rigidbody>();
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
    }

    public void Interact(PlayerController player)
    {
        OnInteract.Invoke();
        OnGrab(player);
    }



    private void OnGrab(PlayerController player)
    {

        rb.useGravity = false;
        rb.isKinematic = true;
        transform.parent = player.transform;
        transform.localPosition = Vector3.up * (player.transform.localScale.y*1.2f + transform.localScale.y) ;
        player.SetItemGrabbed(this);
        emitter.PlaySound(grabbedVolume);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isThrown)
        {
            isThrown = false;
            emitter.PlaySound(thrownVolume);
            Debug.Log("BONK");
        }
    }

}

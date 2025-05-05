using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GrabbableObject : InteractableObject
{
    [SerializeField] GameObject ThrowIndicator;
    [SerializeField] private float throwVelocity;
    [SerializeField] private float thrownVolume;
    [SerializeField] private float grabbedVolume;


    private bool isThrown = false;
    Rigidbody rb;

    private new void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }


    public void Interact(PlayerController player)
    {
        OnGrab(player);
    }



    private void OnGrab(PlayerController player)
    {
        transform.parent = player.transform;
        transform.localPosition = Vector3.up * (player.transform.localScale.y + transform.localScale.y) ;
        ThrowIndicator.SetActive(true);
        emitter.PlaySound(grabbedVolume);
    }

    public void Throw()
    {
        rb.velocity = (transform.parent.forward + Vector3.up).normalized * throwVelocity;
        transform.parent = null;
        isThrown = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isThrown)
        {
            isThrown = false;
            emitter.PlaySound(thrownVolume);
        }
    }

}

using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(SoundEmitter))]
public class PlayerController : MonoBehaviour
{
    private PlayerControls controls;
    private Vector2 moveInput;

    public UnityEvent OnInteractEvent;
    private SoundEmitter soundEmitter;
    private Rigidbody rb;

    [Header("Interaction")]
    [SerializeField] float interactionRange = 1.5f;
    [SerializeField] ParticleSystem throwIndicator;
    [SerializeField] float throwVelocity = 3f;
    private GrabbableObject currentGrabbedItem = null;
    private IInteractable nearActivable = null;

    [Header("Movement")]
    [SerializeField] float crouchSpeed = 1f;
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float runningSpeed = 5f;
    private float currentSpeed;

    [SerializeField] float maxStamina = 2f;
    private bool isRunning = false;
    private float stamina = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        controls = new PlayerControls();
        soundEmitter = GetComponent<SoundEmitter>();
        rb = GetComponent<Rigidbody>();
    }

    #region Input System
    private void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Gameplay.Interact.performed += ctx => OnInteract();

        controls.Gameplay.Crouch.performed += ctx => currentSpeed = crouchSpeed;
        controls.Gameplay.Crouch.canceled += ctx => currentSpeed = walkSpeed;

        controls.Gameplay.Sprint.performed += ctx => isRunning = true;
        controls.Gameplay.Sprint.canceled += ctx => { currentSpeed = walkSpeed; isRunning = false; };
        currentSpeed = walkSpeed;
    }
    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
    #endregion
    #region Interactions
    private void OnInteract()
    {
        if (currentGrabbedItem != null)
        {
            Throw();
            return;
        }
        if (nearActivable == null) return;

        nearActivable.Interact(this);
        OnInteractEvent.Invoke();
    }

    public void SetInteractableObject(IInteractable i)
    {
        nearActivable = i;
    }

    public void SetItemGrabbed(GrabbableObject grabbableObject)
    {
        throwIndicator.gameObject.SetActive(!(grabbableObject == null));
        currentGrabbedItem = grabbableObject;
    }

    public void Throw()
    {
        currentGrabbedItem.GetRigidbody().useGravity = true;
        currentGrabbedItem.GetRigidbody().isKinematic = false;
        currentGrabbedItem.transform.parent = null;
        currentGrabbedItem.isThrown = true;
        currentGrabbedItem.GetRigidbody().velocity = (transform.forward + Vector3.up).normalized * throwVelocity;

        SetItemGrabbed(null);
    }
    #endregion

    private void Update()
    {
        //Move player and play sounds
        Vector3 dir = new Vector3(moveInput.x, 0, moveInput.y);
        rb.velocity = dir * currentSpeed ;
        if(moveInput != Vector2.zero)
        {
           soundEmitter.PlaySound(currentSpeed);
           Quaternion targetRotation = Quaternion.LookRotation(dir);
            rb.angularVelocity *= 0;
            transform.rotation = targetRotation;
        }

        //Stamina
        if (isRunning)
        {
            if (stamina <= 0f) return;
            currentSpeed = runningSpeed;
            stamina -= Time.deltaTime;
            if (stamina <= 0) { stamina = -maxStamina; isRunning = false; currentSpeed = walkSpeed; }
        }
        else
            stamina = Mathf.Clamp(stamina + Time.deltaTime, -maxStamina, maxStamina);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * interactionRange);
    }
}

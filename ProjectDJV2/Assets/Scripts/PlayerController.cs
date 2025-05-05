using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SoundEmitter))]
public class PlayerController : MonoBehaviour
{
    private PlayerControls controls;
    private Vector2 moveInput;

    private GrabbableObject currentGrabbedItem;
    private InteractableObject nearActivable;

    public UnityEvent OnInteractEvent;
    private SoundEmitter soundEmitter;
    private Rigidbody rb;


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


    private void OnInteract()
    {
        if (currentGrabbedItem != null)
        {
            currentGrabbedItem.Throw();
            return;
        }
        if (nearActivable == null) return;

        nearActivable.Interact();
        OnInteractEvent.Invoke();
    }

    private void Update()
    {
        rb.velocity = new Vector3(moveInput.x,0,moveInput.y) * currentSpeed ;
        if(moveInput != Vector2.zero)
        {
           soundEmitter.PlaySound(currentSpeed);
        }

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
}
